// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultedTaskExceptionResilientAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids.Aspects
{
    using System;

    public class FaultedTaskExceptionResilientAttribute : FaultedTaskExceptionAttribute
    {
        #region Constructors and Destructors

        public FaultedTaskExceptionResilientAttribute(Type exception)
        {
            this.Exception = exception;
        }

        #endregion

        #region Public Properties

        public Type Exception { get; set; }

        #endregion
    }
}