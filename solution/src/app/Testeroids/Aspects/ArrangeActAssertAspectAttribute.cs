namespace Testeroids.Aspects
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using JetBrains.Annotations;

    using NUnit.Framework;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Advices;
    using PostSharp.Extensibility;

    /// <summary>
    ///   <see cref="InvokeTestsAspect" /> provides behavior that is necessary for a better integration of AAA syntax with the unit testing framework.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [MulticastAttributeUsage(Inheritance = MulticastInheritance.Strict)]
    public class InvokeTestsAspect : InstanceLevelAspect
    {
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
        ///   Executed when <see cref="Testeroids.Aspects.Attributes.ExceptionResilientAttribute"/> is set on a test method.
        /// </summary>
        /// <param name="args"> The Method interception args. </param>
        [OnMethodInvokeAdvice]
        [MethodPointcut(@"SelectExceptionResilientTestMethods")]
        public void OnExceptionResilientTestMethodEntry(MethodInterceptionArgs args)
        {
            ((IContextSpecification)args.Instance).Act(args.Method, true);
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
            ((IContextSpecification)args.Instance).Act(args.Method, false);
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
            var testMethodsInContext = TypeInvestigationService.GetAllContextSpecificationTypes(type)
                                                               .SelectMany(nestedType => TypeInvestigationService.GetTestMethods(nestedType, true));
            var expectedExceptionTestMethodsInContext = TypeInvestigationService.GetTestMethods(type, true)
                                                                                .Concat(testMethodsInContext)
                                                                                .Where(TypeInvestigationService.IsExpectedExceptionTestMethod)
                                                                                .ToArray();

            // If there is any test method marked with ExpectedExceptionAttribute, then all other act as if marked with ExceptionResilientAttribute
            if (expectedExceptionTestMethodsInContext.Any())
            {
                var testMethods = TypeInvestigationService.GetTestMethods(type, false);
                return testMethods.Except(expectedExceptionTestMethodsInContext).ToArray();
            }

            // Otherwise, take only the ones actually marked with ExceptionResilientAttribute
            return TypeInvestigationService.GetExceptionResilientTestMethods(type);
        }

        /// <summary>
        ///   Select the test methods marked with <see cref="TestAttribute"/>, but not marked with <see cref="Testeroids.Aspects.Attributes.DoNotCallBecauseMethodAttribute"/> or <see cref="Testeroids.Aspects.Attributes.ExceptionResilientAttribute"/>.
        /// </summary>
        /// <param name="type"> The test fixture type to investigate. </param>
        /// <returns> The list of test methods which match the prerequisites. </returns>
        [UsedImplicitly]
        private static IEnumerable<MethodBase> SelectTestMethods(Type type)
        {
            var testMethods = TypeInvestigationService.GetTestMethods(type, false);

            return testMethods.Except(SelectExceptionResilientTestMethods(type));
        }

        #endregion
    }
}
