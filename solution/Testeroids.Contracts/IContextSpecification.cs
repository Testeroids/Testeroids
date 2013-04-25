// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Testeroids" file="IContextSpecification.cs">
//   � 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Contracts
{
    using System.ComponentModel;

    using NUnit.Framework;

    /// <summary>
    ///   Base class for implementing the AAA pattern.
    /// </summary>
    public interface IContextSpecification
    {
        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether there are prerequisite tests running.
        /// </summary>
        bool ArePrerequisiteTestsRunning { get; }

        #endregion

        /// <summary>
        ///   Sets up the test (calls Testeroids.ContextSpecificationBase.EstablishContext() followed by Testeroids.ContextSpecificationBase.InitializeSubjectUnderTest()).
        /// </summary>
        [SetUp]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void BaseSetUp();
    }
}