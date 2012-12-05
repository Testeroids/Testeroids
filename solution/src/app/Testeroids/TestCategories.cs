// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestCategories.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using NUnit.Framework;

    /// <summary>
    ///   The test categories (used in <see cref="CategoryAttribute" /> ).
    /// </summary>
    public static class TestCategories
    {
        #region Constants

        /// <summary>
        ///   Category used for integration tests.
        /// </summary>
        public const string Integration = "Integration Tests";

        /// <summary>
        ///   Category used for unit tests meant to be run manually.
        /// </summary>
        public const string Manual = "Manual Tests";

        #endregion
    }
}