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
    ///   Tests whether an abstract test fixture contains child test fixtures (if not, then no tests are run and that constitutes a user error).
    /// </summary>
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(ArrangeActAssertAspectAttribute))]
    [MulticastAttributeUsage(Inheritance = MulticastInheritance.Strict)]
    public class MakeEmptyTestsInconclusiveAspectAttribute : InstanceLevelAspect
    {
        #region Public Methods and Operators

        /// <summary>
        /// Checks if the passed class contains any nested test fixture classes.
        /// </summary>
        /// <param name="type">
        /// The type of the class to check.
        /// </param>
        /// <returns>
        /// <c>true</c> if the passed class contains any any nested test fixture classes, <c>false</c> otherwise.
        /// </returns>
        public override bool CompileTimeValidate(Type type)
        {
            // Use reflection to find <see cref="TestFixtureAttribute"/>, instead of binding to the NUnit type directly.
            // This prevents versioning issues with PostSharp when applying the aspect at compile-time.
            return TypeInvestigationService.IsContextSpecification(type) && base.CompileTimeValidate(type);
        }

        /// <summary>
        /// Marks the test as inconclusive.
        /// </summary>
        /// <param name="args">
        /// The advice arguments.
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
            var selectEmptyTestMethods =
                from testMethod in TypeInvestigationService.GetTestMethods(type, false)
                where !testMethod.IsAbstract &&
                      !TypeInvestigationService.IsExpectedExceptionTestMethod(testMethod) &&
                      GetIntermediateLanguageFromMethodInfoBase(testMethod).Count() <= 2
                select testMethod;

            return selectEmptyTestMethods;
        }

        #endregion
    }
}
