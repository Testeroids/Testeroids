// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Testeroids" file="IMockInternals.cs">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Mocking
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Implements an internal contract which allows for additional features on <see cref="IMock"/> objects.
    /// </summary>
    internal interface IMockInternals
    {
        #region Public Properties

        /// <summary>
        /// Gets a list of all the methods which were set up and a <see cref="bool"/> indicating if the given method was verified through a call to <see cref="IMock.Verify"/>.
        /// </summary>
        IEnumerable<Tuple<MemberInfo, bool>> VerifiedSetups { get; }

        #endregion
    }
}