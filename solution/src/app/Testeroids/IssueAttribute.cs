// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IssueAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    ///   IssueAttribute is used to annotate a test method with the bug-tracking issue number that gave origin to the test.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    public class IssueAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="IssueAttribute" /> class. Creates an <see cref="IssueAttribute" /> that annotates a test method with a given issue number.
        /// </summary>
        /// <param name="issueNumber"> The bug-tracking issue number. </param>
        public IssueAttribute(string issueNumber)
        {
            this.IssueNumber = issueNumber;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the bug-tracking issue number.
        /// </summary>
        [PublicAPI]
        public string IssueNumber { get; private set; }

        #endregion
    }
}