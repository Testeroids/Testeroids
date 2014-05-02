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
    ///   <see cref="InstrumentTestsAspect" /> provides behavior that is necessary for a better integration of AAA syntax with the unit testing framework.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [MulticastAttributeUsage(Inheritance = MulticastInheritance.Strict)]
    public class InstrumentTestsAspect : InstanceLevelAspect
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
        ///   Executed for methods which are not marked with <see cref="ExpectedExceptionAttribute"/>, but are in the context of a test marked with <see cref="ExpectedExceptionAttribute"/>.
        /// </summary>
        /// <param name="args"> The Method interception args. </param>
        [OnMethodEntryAdvice]
        [MethodPointcut(@"SelectExceptionResilientTestMethods")]
        [DebuggerNonUserCode]
        public void OnExceptionResilientTestMethodEntry(MethodExecutionArgs args)
        {
            ((IContextSpecification)args.Instance).OnTestMethodCalled(args.Method, true);
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
            ((IContextSpecification)args.Instance).OnTestMethodCalled(args.Method, false);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Select the remaining test methods contained in a fixture where one test method is marked <see cref="ExpectedExceptionAttribute"/>.
        /// </summary>
        /// <param name="type"> The test fixture type to investigate. </param>
        /// <returns> The list of remaining test methods contained in a fixture where one test method is marked <see cref="ExpectedExceptionAttribute"/>. </returns>
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

            return Enumerable.Empty<MethodBase>();
        }

        /// <summary>
        ///   Select the test methods marked with <see cref="TestAttribute"/>, but not considered exception-resilient.
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
