// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MakeEmptyTestsInconclusiveAspectAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Aspects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using JetBrains.Annotations;

    using NUnit.Framework;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Advices;
    using PostSharp.Aspects.Dependencies;
    using PostSharp.Extensibility;

    /// <summary>
    ///   Test if a test do something or is empty.
    /// </summary>
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(ArrangeActAssertAspectAttribute))]
    [MulticastAttributeUsage(MulticastTargets.Class, 
        TargetTypeAttributes = MulticastAttributes.AnyScope | MulticastAttributes.AnyVisibility | MulticastAttributes.NonAbstract | MulticastAttributes.Managed, 
        AllowMultiple = false, Inheritance = MulticastInheritance.None)]
    public class MakeEmptyTestsInconclusiveAspectAttribute : InstanceLevelAspect
    {
        #region Public Methods and Operators

        /// <summary>
        /// Checks if the passed class contains any nested classes marked with <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/> or <see cref="TestFixtureAttribute"/>.
        /// </summary>
        /// <param name="type">
        /// The type of the class to check.
        /// </param>
        /// <returns>
        /// <c>true</c> if the passed class contains any any nested classes marked with <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/> or <see cref="TestFixtureAttribute"/>, <c>false</c> otherwise.
        /// </returns>
        public override bool CompileTimeValidate(Type type)
        {
            // Use reflection to find <see cref="TestFixtureAttribute"/>, instead of binding to the NUnit type directly.
            // This prevents versioning issues with PostSharp when applying the aspect at compile-time.
            return TypeInvestigationService.IsContextSpecification(type) && base.CompileTimeValidate(type);
        }

        /// <summary>
        /// The on success.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        [OnMethodSuccessAdvice]
        [MethodPointcut(@"SelectEmptyTestMethods")]
        public void OnSuccess(MethodExecutionArgs args)
        {
            Assert.Inconclusive("The following test doesn't seem to be implemented : {0}.", args.Method.Name);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Get intermediate language of passed method.
        /// </summary>
        /// <param name="methodInfo"> The method info. </param>
        /// <returns> The intermediate language as a byte array. </returns>
        /// <exception cref="ArgumentException">Thrown if methodBody is null.</exception>
        private static IEnumerable<byte> GetIntermediateLanguageFromMethodInfoBase(MethodBase methodInfo)
        {
            var methodBody = methodInfo.GetMethodBody();
            if (methodBody == null)
            {
                throw new ArgumentException("Method Body is null");
            }

            return methodBody.GetILAsByteArray();
        }

        /// <summary>
        /// Enumerates the methods that are empty test of a class.
        /// </summary>
        /// <param name="type">
        /// The class whose test methods must be enumerated.
        /// </param>
        /// <returns>
        /// The list of methods that are empty test.
        /// </returns>
        [UsedImplicitly]
        private static IEnumerable<MethodBase> SelectEmptyTestMethods(Type type)
        {
            const BindingFlags BindingFlags =
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly |
                System.Reflection.BindingFlags.Public;

            var selectEmptyTestMethods =
                type.GetMethods(BindingFlags)
                    .Where(info => (GetIntermediateLanguageFromMethodInfoBase(info).Count() <= 2) &&
                                   info.IsDefined(typeof(TestAttribute), false) &&
                                   !info.IsDefined(typeof(ExpectedExceptionAttribute), true) && 
                                   !info.IsDefined(typeof(FaultedTaskExpectedExceptionAttribute), true));

            return selectEmptyTestMethods;
        }

        #endregion
    }
}