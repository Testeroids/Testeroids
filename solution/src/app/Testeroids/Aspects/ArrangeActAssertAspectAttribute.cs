// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrangeActAssertAspectAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Aspects
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using Microsoft.Reactive.Testing;

    using NUnit.Framework;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Advices;
    using PostSharp.Extensibility;

    using Testeroids.Aspects.Attributes;

    /// <summary>
    ///   <see cref="ArrangeActAssertAspectAttribute" /> provides behavior that is necessary for a better integration of AAA syntax with the unit testing framework.
    ///   Specifically, it injects calls to prerequisite tests (marked with <see cref="PrerequisiteAttribute"/>) and to the Because() method into each test in each test fixture.
    ///   It also handles assert failures so that a failing test marked as <see cref="PrerequisiteAttribute"/> is flagged as such in the exception message.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public class ArrangeActAssertAspectAttribute : InstanceLevelAspect
    {
        #region Fields

        /// <summary>
        ///   Field bound at runtime to a delegate of the method <c>Because</c> .
        /// </summary>
        [NotNull]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", 
            Justification = "Reviewed. PostSharp requires this to be public.")]
        [ImportMember("OnBecauseRequested", IsRequired = true)]
        [UsedImplicitly]
        public Action OnBecauseRequestedMethod;

        /// <summary>
        ///   Field bound at runtime to a delegate of the method <c>RunPrerequisiteTests</c> .
        /// </summary>
        [NotNull]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", 
            Justification = "Reviewed. PostSharp requires this to be public.")]
        [ImportMember("RunPrerequisiteTests", IsRequired = true)]
        [UsedImplicitly]
        public Action RunPrerequisiteTestsMethod;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ArrangeActAssertAspectAttribute" /> class.
        /// </summary>
        public ArrangeActAssertAspectAttribute()
        {
            this.AttributeTargetMemberAttributes = MulticastAttributes.Public | MulticastAttributes.Instance;
            this.AttributeTargetElements = MulticastTargets.Class;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Checks whether the type is a candidate for the aspect to apply to it.
        /// </summary>
        /// <param name="type">the type to check if it needs the attribute.</param>
        /// <returns><c>true</c> if the attribute needs to be applied to the type; <c>false</c> otherwise.</returns>
        public override bool CompileTimeValidate(Type type)
        {
            return typeof(IContextSpecification).IsAssignableFrom(type) && base.CompileTimeValidate(type);
        }

        /// <summary>
        ///   The method executed when exception occurred in standard test execution.
        /// </summary>
        /// <param name="args"> The Method Execution Args. </param>
        [OnMethodExceptionAdvice(Master = @"OnStandardTestMethodEntry")]
        public void OnException(MethodExecutionArgs args)
        {
            if (!(args.Exception is AssertionException))
            {
                return;
            }

            if (!args.Exception.Message.TrimStart().StartsWith("Expected"))
            {
                return;
            }

            var message = string.Format("{0}.{1}\r\n{2}", args.Instance.GetType().Name, args.Method.Name, args.Exception.Message);

            if (args.Method.IsDefined(typeof(PrerequisiteAttribute), false))
            {
                message = string.Format("Prerequisite failed: {0}", message);
                args.Exception = new PrerequisiteFailureException(message, args.Exception);
                args.FlowBehavior = FlowBehavior.ThrowException;
            }
            else
            {
                args.FlowBehavior = FlowBehavior.RethrowException;
            }
        }

        /// <summary>
        ///   Executed when <see cref="Testeroids.Aspects.Attributes.ExceptionResilientAttribute"/> is set on a test method.
        /// </summary>
        /// <param name="args"> The Method interception args. </param>
        [OnMethodInvokeAdvice]
        [MethodPointcut(@"SelectExceptionResilientTestMethods")]
        public void OnExceptionResilientTestMethodEntry(MethodInterceptionArgs args)
        {
            try
            {
                this.OnTestMethodEntry(
                    (IContextSpecification)args.Instance, 
                    args.Method, 
                    this.OnBecauseRequestedMethod);
            }
            catch (Exception e)
            {
                // we don't care about exceptions right now
                var exceptionResilientAttribute =
                    args.Method.GetCustomAttributes(typeof(ExceptionResilientAttribute), true)
                        .Cast<ExceptionResilientAttribute>()
                        .Single();
                var exceptionTypeToIgnore = exceptionResilientAttribute.ExceptionTypeToIgnore;

                if (exceptionTypeToIgnore != null && !exceptionTypeToIgnore.IsInstanceOfType(e))
                {
                    throw;
                }
            }
            finally
            {
                // Actual assertion.
                args.Proceed();

                // a test method has a void return type, but the documentation states that the Returnvalue must be set
                args.ReturnValue = null;
            }
        }

        /// <summary>
        ///   The method called when a method is marked with test.
        /// </summary>
        /// <param name="args"> The method execution args. </param>
        [OnMethodEntryAdvice]
        [MethodPointcut(@"SelectTestMethods")]
        [DebuggerNonUserCode]
        public void OnStandardTestMethodEntry(MethodExecutionArgs args)
        {
            this.OnTestMethodEntry((IContextSpecification)args.Instance, args.Method, this.OnBecauseRequestedMethod);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Select the test methods marked with <see cref="Testeroids.Aspects.Attributes.ExceptionResilientAttribute"/>.
        /// </summary>
        /// <param name="type"> The test fixture type to investigate. </param>
        /// <returns> The list of test method marked with <see cref="Testeroids.Aspects.Attributes.ExceptionResilientAttribute"/>. </returns>
        [UsedImplicitly]
        private static IEnumerable<MethodBase> SelectExceptionResilientTestMethods(Type type)
        {
            const BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public;

            var selectTestMethods = from methodInfo in type.GetMethods(BindingFlags)
                                    where methodInfo.IsDefined(typeof(TestAttribute), false) &&
                                          methodInfo.IsDefined(typeof(ExceptionResilientAttribute), true)
                                    select methodInfo;
            return selectTestMethods;
        }

        /// <summary>
        ///   Select the test methods marked with <see cref="TestAttribute"/>, but not marked with <see cref="Testeroids.Aspects.Attributes.DoNotCallBecauseMethodAttribute"/> or <see cref="Testeroids.Aspects.Attributes.ExceptionResilientAttribute"/>.
        /// </summary>
        /// <param name="type"> The test fixture type to investigate. </param>
        /// <returns> The list of test methods which match the prerequisites. </returns>
        [UsedImplicitly]
        private static IEnumerable<MethodBase> SelectTestMethods(Type type)
        {
            const BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public;

            var selectTestMethods = from methodInfo in type.GetMethods(BindingFlags)
                                    where
                                        methodInfo.IsDefined(typeof(TestAttribute), false) &&
                                        !methodInfo.IsDefined(typeof(DoNotCallBecauseMethodAttribute), true) &&
                                        !methodInfo.IsDefined(typeof(ExceptionResilientAttribute), true)
                                    select methodInfo;
            return selectTestMethods;
        }

        /// <summary>
        ///   Method executed when entering a test method.
        /// </summary>
        /// <param name="instance"> The instance of the context specification. </param>
        /// <param name="methodInfo"> The test method. </param>
        /// <param name="onBecauseRequestedAction"> The because method. </param>
        private void OnTestMethodEntry(
            IContextSpecification instance, 
            MethodBase methodInfo, 
            Action onBecauseRequestedAction)
        {
            var isRunningInTheContextOfAnotherTest = instance.ArePrerequisiteTestsRunning;

            if (isRunningInTheContextOfAnotherTest)
            {
                return;
            }

            var isRunningPrerequisite = methodInfo.IsDefined(typeof(PrerequisiteAttribute), true);

            onBecauseRequestedAction();

            if (!isRunningPrerequisite)
            {
                this.RunPrerequisiteTestsMethod();
            }

            // Handle FaultedTaskExpectedExceptionAttribute and FaultedTaskExceptionResilientAttribute
            var taskAttributes = methodInfo.GetCustomAttributes(typeof(FaultedTaskExceptionAttribute), false);
            var exceptionAttribute = taskAttributes.OfType<FaultedTaskExpectedExceptionAttribute>().FirstOrDefault();
            if (exceptionAttribute != null)
            {
                Assert.IsTrue(instance.UnhandledExceptions.Any(o => o.GetType() == exceptionAttribute.ExpectedException));
            }
            else
            {
                var exceptionResilientAttribute = taskAttributes.OfType<FaultedTaskExceptionResilientAttribute>().FirstOrDefault();
                if (exceptionResilientAttribute != null)
                {
                    // if we protect ourselves against some exceptions but not the ones which were thrown.
                    foreach (var unhandledException in instance.UnhandledExceptions)
                    {
                        if (unhandledException.GetType() != exceptionResilientAttribute.Exception)
                        {
                            throw new UnexpectedUnhandledException(unhandledException);
                        }
                    }
                }
                else
                {
                    // if we don't protect ourselves against any exception
                    foreach (var unhandledException in instance.UnhandledExceptions)
                    {
                        throw new UnexpectedUnhandledException(unhandledException);
                    }
                }
            }           
        }

        #endregion
    }
}