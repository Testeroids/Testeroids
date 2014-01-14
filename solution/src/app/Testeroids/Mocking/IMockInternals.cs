namespace Testeroids.Mocking
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Implements an internal contract which allows for additional features on <see cref="ITesteroidsMock"/> objects.
    /// </summary>
    internal interface IMockInternals
    {
        #region Public Properties

        /// <summary>
        /// Gets a list of all the methods which were set up and a <see cref="bool"/> indicating if the given method was verified through a call to <see cref="ITesteroidsMock.Verify"/>.
        /// </summary>
        IEnumerable<Tuple<MemberInfo, bool>> VerifiedSetups { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Reset the counts of all the method calls done previously.
        /// </summary>
        void ResetAllCallCounts();

        #endregion
    }
}