// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Testeroids" file="MockNotVerifiedException.cs">
//   © 2012-2014 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Exception thrown when a setup for a mock is detected which has not been verified.
    /// </summary>
    public class MockNotVerifiedException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MockNotVerifiedException"/> class.
        /// </summary>
        /// <param name="unverifiedMember">
        /// The unverified member.
        /// </param>
        public MockNotVerifiedException(MemberInfo unverifiedMember)
            : base(
                string.Format(
                    unverifiedMember.MemberType == MemberTypes.Property
                        ? "The {0} property mock for the {1} type was set up but not verified."
                        : "The {0} method mock for the {1} type was set up but not verified.", 
                    unverifiedMember, 
                    unverifiedMember.DeclaringType))
        {
        }

        #endregion
    }
}