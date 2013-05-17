// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultedTaskExpectedExceptionAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids.Aspects
{
    using System;

    public class FaultedTaskExpectedExceptionAttribute : FaultedTaskExceptionAttribute
    {
        #region Constructors and Destructors

        public FaultedTaskExpectedExceptionAttribute(Type expectedException)
        {
            this.ExpectedException = expectedException;
            if (!expectedException.IsSubclassOf(typeof(Exception)))
            {
                throw new NotSupportedException("Only types inherited from Exception are supported");
            }
        }

        #endregion

        #region Public Properties

        public Type ExpectedException { get; set; }

        #endregion
    }
}