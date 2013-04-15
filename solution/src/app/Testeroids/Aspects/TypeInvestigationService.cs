// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeInvestigationService.cs" company="Testeroids">
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

    using Testeroids.Aspects.Attributes;

    /// <summary>
    ///   Responsible for checking the nature of a type.
    /// </summary>
    public static class TypeInvestigationService
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Returns the list of types marked either with <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/> nested in passed type.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to investigate. </param>
        /// <returns> the list of nested types marked either with <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/>. </returns>
        [PublicAPI]
        public static IEnumerable<Type> GetAbstractTestFixtureTypes(Type classTypeToInvestigate)
        {
            return classTypeToInvestigate.GetNestedTypes().Where(IsAbstractTestFixture);
        }

        /// <summary>
        ///   Returns the list of types nested in passed type.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to investigate. </param>
        /// <returns> the list of nested types. </returns>
        [PublicAPI]
        public static IEnumerable<Type> GetAllNestedTypes(Type classTypeToInvestigate)
        {
            return classTypeToInvestigate.GetNestedTypes();
        }

        /// <summary>
        ///   Returns the list of types marked either with <see cref="TestFixtureAttribute"/> or <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/> nested in passed type.
        /// </summary>
        /// <param name="typeToInvestigate"> The type to investigate. </param>
        /// <returns> the list of nested types marked either with <see cref="TestFixtureAttribute"/> or <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/>. </returns>
        [PublicAPI]
        public static IEnumerable<Type> GetAllTestFixtureTypes(Type typeToInvestigate)
        {
            return typeToInvestigate.GetNestedTypes().Where(y => IsTestFixture(y) || IsAbstractTestFixture(y));
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
        ///   Returns the list of types marked either with <see cref="TestFixtureAttribute"/> nested in passed type.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to investigate. </param>
        /// <returns> the list of nested types marked either with <see cref="TestFixtureAttribute"/>. </returns>
        [PublicAPI]
        public static IEnumerable<Type> GetConcreteTestFixtureTypes(Type classTypeToInvestigate)
        {
            return classTypeToInvestigate.GetNestedTypes().Where(IsTestFixture);
        }

        /// <summary>
        ///   Returns all methods of the type having a Test and a ExceptionResilient Attributes.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to investigate. </param>
        /// <returns> The list of methods having Test and ExceptionResilient Attribute. </returns>
        [PublicAPI]
        public static IEnumerable<MethodInfo> GetExceptionResilientTestMethods(Type classTypeToInvestigate)
        {
            return classTypeToInvestigate.GetMethods().Where(y => IsTestMethod(y) && IsExceptionResilientTestMethod(y));
        }

        /// <summary>
        ///   Returns all methods of the type having a <see cref="TestAttribute"/> and a <see cref="ExpectedExceptionAttribute"/>s.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to investigate. </param>
        /// <returns> The list of methods having <see cref="TestAttribute"/> and <see cref="ExpectedExceptionAttribute"/>. </returns>
        [PublicAPI]
        public static IEnumerable<MethodInfo> GetExpectedExceptionTestMethods(Type classTypeToInvestigate)
        {
            return classTypeToInvestigate.GetMethods().Where(y => IsTestMethod(y) && IsExpectedExceptionTestMethod(y));
        }

        /// <summary>
        ///   Returns the list of test non marked with Prerequisite Attribute.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type to check </param>
        /// <returns> the list of test non marked with Prerequisite Attribute. </returns>
        [PublicAPI]
        public static IEnumerable<MethodInfo> GetNonPrerequisiteTestMethods(Type classTypeToInvestigate)
        {
            return classTypeToInvestigate.GetMethods().Where(y => IsTestMethod(y) && !IsPrerequisiteTestMethod(y));
        }

        /// <summary>
        ///   Test if type is an abstract test fixture.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type To Investigate. </param>
        /// <returns> <c>true</c> if type is an <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/>, <c>false</c> otherwise. </returns>
        public static bool IsAbstractTestFixture(Type classTypeToInvestigate)
        {
            return classTypeToInvestigate.IsAbstract && typeof(IContextSpecification).IsAssignableFrom(classTypeToInvestigate);
        }

        /// <summary>
        ///   Test if injected type is a test fixture.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type To Investigate. </param>
        /// <returns> <c>true</c> if injected type is a TextFixture, <c>false</c> otherwise. </returns>
        [PublicAPI]
        public static bool IsConcreteTestFixture(Type classTypeToInvestigate)
        {
            return !classTypeToInvestigate.IsAbstract && typeof(IContextSpecification).IsAssignableFrom(classTypeToInvestigate);
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
        ///   Test if injected type is a test fixture or an abstract test fixture.
        /// </summary>
        /// <param name="classTypeToInvestigate"> The type To Investigate. </param>
        /// <returns> <c>true</c> if injected type is a <see cref="TestFixtureAttribute"/> or an <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/>, <c>false</c> otherwise. </returns>
        [PublicAPI]
        public static bool IsTestFixture(Type classTypeToInvestigate)
        {
            return IsAbstractTestFixture(classTypeToInvestigate) || IsConcreteTestFixture(classTypeToInvestigate);
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