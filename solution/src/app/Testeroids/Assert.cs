// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Assert.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
// ReSharper disable RedundantNameQualifier
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    ///   The Assert class extends the <see cref="NUnit.Framework.Assert" /> to make it aware of <see cref="Nullable{T}" /> values. When the value is null, it means the Act part of the test has not been run.
    /// </summary>
    public class Assert : NUnit.Framework.Assert
    {
        #region Public Methods and Operators

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public static void AreEqual(
            int expected, 
            int? actual)
        {
            NUnit.Framework.Assert.IsNotNull(actual, "The test did not produce a value. Are you sure the Because method was executed?");

            NUnit.Framework.Assert.AreEqual(expected, actual.Value);
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public static void AreNotEqual(
            int expected, 
            int? actual)
        {
            NUnit.Framework.Assert.IsNotNull(actual, "The test did not produce a value. Are you sure the Because method was executed?");

            NUnit.Framework.Assert.AreNotEqual(expected, actual.Value);
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public static void Greater(
            int expected, 
            int? actual)
        {
            NUnit.Framework.Assert.IsNotNull(actual, "The test did not produce a value. Are you sure the Because method was executed?");

            NUnit.Framework.Assert.Greater(expected, actual.Value);
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public static void GreaterOrEqual(
            int expected, 
            int? actual)
        {
            NUnit.Framework.Assert.IsNotNull(actual, "The test did not produce a value. Are you sure the Because method was executed?");

            NUnit.Framework.Assert.GreaterOrEqual(expected, actual.Value);
        }

        /// <summary>
        ///   Asserts that a condition is false. If the condition is true the method throws an <see
        ///    cref="NUnit.Framework.AssertionException" /> .
        /// </summary>
        /// <param name="condition"> The evaluated condition </param>
        public static void IsFalse(bool? condition)
        {
            NUnit.Framework.Assert.IsNotNull(condition, "The test did not produce a value. Are you sure the Because method was executed?");

            NUnit.Framework.Assert.IsFalse(condition.Value);
        }

        /// <summary>
        ///   Asserts that a condition is true. If the condition is true the method throws an <see
        ///    cref="NUnit.Framework.AssertionException" /> .
        /// </summary>
        /// <param name="condition"> The evaluated condition </param>
        public static void IsTrue(bool? condition)
        {
            NUnit.Framework.Assert.IsNotNull(condition, "The test did not produce a value. Are you sure the Because method was executed?");

            NUnit.Framework.Assert.IsTrue(condition.Value);
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public static void Less(
            int expected, 
            int? actual)
        {
            NUnit.Framework.Assert.IsNotNull(actual, "The test did not produce a value. Are you sure the Because method was executed?");

            NUnit.Framework.Assert.Less(expected, actual.Value);
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        public static void LessOrEqual(
            int expected, 
            int? actual)
        {
            NUnit.Framework.Assert.IsNotNull(actual, "The test did not produce a value. Are you sure the Because method was executed?");

            NUnit.Framework.Assert.LessOrEqual(expected, actual.Value);
        }

        #endregion
    }
}