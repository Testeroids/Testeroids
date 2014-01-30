namespace Testeroids.Aspects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using JetBrains.Annotations;

    using NUnit.Framework;

    using Testeroids.Aspects.Attributes;

    /// <summary>
    ///   Responsible for checking the nature of a type.
    /// </summary>
    public static class TypeInvestigationService
    {
        #region Constants

        /// <summary>
        /// The <see cref="BindingFlags"/> used to find test methods in a given <see cref="Type"/>.
        /// </summary>
        internal const BindingFlags TestMethodBindingFlags = BindingFlags.Instance | BindingFlags.Public;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns the list of abstract, nested types implementing <see cref="IContextSpecification"/>  in passed type.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to investigate. </param>
        /// <returns> the list of abstract, nested types implementing <see cref="IContextSpecification"/>. </returns>
        [PublicAPI]
        public static IEnumerable<Type> GetAbstractContextSpecificationTypes(Type classTypeToInvestigate)
        {
            return GetAllNestedTypes(classTypeToInvestigate).Where(IsAbstractContextSpecification);
        }

        /// <summary>
        ///   Returns the list of nested types implementing <see cref="IContextSpecification"/> in passed type.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to investigate. </param>
        /// <returns> the list of nested types implementing <see cref="IContextSpecification"/>. </returns>
        [PublicAPI]
        public static IEnumerable<Type> GetAllContextSpecificationTypes(Type classTypeToInvestigate)
        {
            return GetAllNestedTypes(classTypeToInvestigate).Where(y => IsContextSpecification(y) || IsAbstractContextSpecification(y));
        }

        /// <summary>
        ///   Returns the list of types nested in passed type.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to investigate. </param>
        /// <returns> the list of nested types. </returns>
        [PublicAPI]
        public static IEnumerable<Type> GetAllNestedTypes(Type classTypeToInvestigate)
        {
            return classTypeToInvestigate.GetNestedTypes()
                                         .SelectMany(nestedType => new[] { nestedType }.Concat(GetAllNestedTypes(nestedType)));
        }

        /// <summary>
        ///   Returns all methods of the type having a Test Attribute.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to investigate. </param>
        /// <returns> The list of methods having Test Attribute. </returns>
        [PublicAPI]
        public static IEnumerable<MethodInfo> GetAllTestMethods(Type classTypeToInvestigate)
        {
            return classTypeToInvestigate.GetMethods().Where(IsTestMethod);
        }

        /// <summary>
        ///   Returns the list of nested types implementing <see cref="IContextSpecification"/> in passed type.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to investigate. </param>
        /// <returns> the list of nested types implementing <see cref="IContextSpecification"/>. </returns>
        [PublicAPI]
        public static IEnumerable<Type> GetConcreteContextSpecificationTypes(Type classTypeToInvestigate)
        {
            return classTypeToInvestigate.GetNestedTypes().Where(IsContextSpecification);
        }

        /// <summary>
        ///   Returns all methods of the type having a Test and a ExceptionResilient Attributes.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to investigate. </param>
        /// <returns> The list of methods having Test and ExceptionResilient Attribute. </returns>
        [PublicAPI]
        public static IEnumerable<MethodInfo> GetExceptionResilientTestMethods(Type classTypeToInvestigate)
        {
            return GetTestMethods(classTypeToInvestigate, false).Where(IsExceptionResilientTestMethod);
        }

        /// <summary>
        ///   Returns all methods of the type having a <see cref="TestAttribute"/> and a <see cref="ExpectedExceptionAttribute"/>s.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to investigate. </param>
        /// <returns> The list of methods having <see cref="TestAttribute"/> and <see cref="ExpectedExceptionAttribute"/>. </returns>
        [PublicAPI]
        public static IEnumerable<MethodInfo> GetExpectedExceptionTestMethods(Type classTypeToInvestigate)
        {
            return GetTestMethods(classTypeToInvestigate, false).Where(IsExpectedExceptionTestMethod);
        }

        /// <summary>
        ///   Returns the list of test non marked with Prerequisite Attribute.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to check </param>
        /// <returns> the list of test non marked with Prerequisite Attribute. </returns>
        [PublicAPI]
        public static IEnumerable<MethodInfo> GetNonPrerequisiteTestMethods(Type classTypeToInvestigate)
        {
            return GetTestMethods(classTypeToInvestigate, false).Where(y => !IsPrerequisiteTestMethod(y));
        }

        /// <summary>
        /// Selects all test methods in a given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// The type to inspect.
        /// </param>
        /// <param name="flattenHierarchy">
        /// Specifies that tests from parent fixtures should be returned.
        /// </param>
        /// <returns>
        /// A list of all the test methods in the specified <paramref name="type"/>.
        /// </returns>
        public static IEnumerable<MethodInfo> GetTestMethods(Type type,
                                                             bool flattenHierarchy)
        {
            var testMethods =
                from method in type.GetMethods(TestMethodBindingFlags | (flattenHierarchy
                                                                             ? BindingFlags.FlattenHierarchy
                                                                             : BindingFlags.Default))
                where IsTestMethod(method) &&
                      !method.IsDefined(typeof(DoNotCallBecauseMethodAttribute), false)
                select method;

            if (!flattenHierarchy)
            {
                return from testMethod in testMethods
                       where testMethod.DeclaringType == type
                       select testMethod;
            }

            return testMethods;
        }

        /// <summary>
        ///   Test if type is an abstract context specification.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type To Investigate. </param>
        /// <returns> <c>true</c> if type is an abstract <see cref="IContextSpecification"/>, <c>false</c> otherwise. </returns>
        [PublicAPI]
        public static bool IsAbstractContextSpecification(Type classTypeToInvestigate)
        {
            return classTypeToInvestigate.IsAbstract && typeof(IContextSpecification).IsAssignableFrom(classTypeToInvestigate);
        }

        /// <summary>
        ///   Test if injected type is a context specification.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type To Investigate. </param>
        /// <returns> <c>true</c> if injected type is a concrete <see cref="IContextSpecification"/>, <c>false</c> otherwise. </returns>
        [PublicAPI]
        public static bool IsConcreteContextSpecification(Type classTypeToInvestigate)
        {
            return !classTypeToInvestigate.IsAbstract && typeof(IContextSpecification).IsAssignableFrom(classTypeToInvestigate);
        }

        /// <summary>
        ///   Test if injected type is a context specification test fixture.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type To Investigate. </param>
        /// <returns> <c>true</c> if injected type is an <see cref="IContextSpecification"/>, <c>false</c> otherwise. </returns>
        [PublicAPI]
        public static bool IsContextSpecification(Type classTypeToInvestigate)
        {
            return IsAbstractContextSpecification(classTypeToInvestigate) || IsConcreteContextSpecification(classTypeToInvestigate);
        }

        /// <summary>
        ///   Checks if a test method has the ExceptionResilient attribute.
        /// </summary>
        /// <param name="method"> The method to check. </param>
        /// <returns> <c>true</c> if the test method has the ExceptionResilient attribute, <c>false</c> otherwise. </returns>
        [PublicAPI]
        public static bool IsExceptionResilientTestMethod(MethodBase method)
        {
            return method.IsDefined(typeof(ExceptionResilientAttribute), false);
        }

        /// <summary>
        ///   Checks if a test method has the <see cref="ExpectedExceptionAttribute"/>.
        /// </summary>
        /// <param name="method"> The method to check. </param>
        /// <returns> <c>true</c> if the test method has the <see cref="ExpectedExceptionAttribute"/>, <c>false</c> otherwise. </returns>
        [PublicAPI]
        public static bool IsExpectedExceptionTestMethod(MethodBase method)
        {
            return method.IsDefined(typeof(ExpectedExceptionAttribute), false);
        }

        /// <summary>
        ///   Checks if a test method has the <see cref="PrerequisiteAttribute"/>.
        /// </summary>
        /// <param name="method"> The method to check. </param>
        /// <returns> <c>true</c> if the test method has the <see cref="PrerequisiteAttribute"/>, <c>false</c> otherwise. </returns>
        [PublicAPI]
        public static bool IsPrerequisiteTestMethod(MethodBase method)
        {
            return method.IsDefined(typeof(PrerequisiteAttribute), false);
        }

        /// <summary>
        ///   Checks if a method has the <see cref="TestAttribute"/>.
        /// </summary>
        /// <param name="method"> The method to check. </param>
        /// <returns> <c>true</c> if the test method has the Test attribute, <c>false</c> otherwise. </returns>
        [PublicAPI]
        public static bool IsTestMethod(MethodBase method)
        {
            return method.IsDefined(typeof(TestAttribute), false);
        }

        #endregion
    }
}