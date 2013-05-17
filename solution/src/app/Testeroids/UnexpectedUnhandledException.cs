// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnexpectedUnhandledException.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids.Aspects
{
    using System;

    public class UnexpectedUnhandledException : Exception
    {
        #region Constructors and Destructors

        public UnexpectedUnhandledException(Exception unhandledException)
            : base("An exception which was not expected was thrown and not handled.\r\n\r\n" + unhandledException.Message, unhandledException)
        {
        }

        #endregion
    }
}