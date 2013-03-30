// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Testeroids" file="MockNotVerifiedException.cs">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;
    using System.Linq.Expressions;
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
        public MockNotVerifiedException(object unverifiedMember)
            : base(GenerateMessage(unverifiedMember))
        {
        }

        private static string GenerateMessage(object expression)
        {
            var unverifiedMember = expression as MemberInfo;
            string generatedMessage = null;
            if (unverifiedMember != null)
            {
                generatedMessage = string.Format(unverifiedMember.MemberType == MemberTypes.Property
                                                     ? "The {0} property mock for the {1} type was set up but not verified."
                                                     : "The {0} method mock for the {1} type was set up but not verified.", unverifiedMember, unverifiedMember.DeclaringType);
            }
            else
            {
                // do your thing.
                var action = expression as Tuple<object, string>;
                if (action != null)
                {
                    generatedMessage = string.Format("The property setter mocked for the following setup could not be verified : \r\n\r\n{0}", action.Item2);
                }
                else
                {
                    generatedMessage = "A member mock was set up but not verified.";
                }
            }
            return generatedMessage;
        }

        #endregion
    }
}