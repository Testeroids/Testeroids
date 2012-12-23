// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Testeroids" file="MockNotVerifiedException.cs">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids
{
    using System;
    using System.Reflection;

    public class MockNotVerifiedException : Exception
    {
        #region Constructors and Destructors

        public MockNotVerifiedException(MemberInfo unverifiedMember)
            : base(string.Format(
                (unverifiedMember.MemberType == MemberTypes.Property
                     ? "The {0} property mock for the {1} type was set up but not verified."
                     : "The {0} method mock for the {1} type was set up but not verified."), unverifiedMember, unverifiedMember.DeclaringType))
        {
        }

        #endregion
    }
}