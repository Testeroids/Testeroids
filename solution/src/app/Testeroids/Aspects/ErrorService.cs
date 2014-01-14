namespace Testeroids.Aspects
{
    using System;
    using System.Reflection;

    using PostSharp;
    using PostSharp.Extensibility;

    /// <summary>
    ///   Manage the error returned by Aspects CompileTimeValidate method.
    /// </summary>
    public static class ErrorService
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Raise the error returned by Aspects CompileTimeValidate method.
        /// </summary>
        /// <param name="aspectType"> The Aspect Class Type that raise the error. </param>
        /// <param name="verifiedClassType"> The class Type given to the CompileTimeValidate method. </param>
        /// <param name="message"> The message to give explaining the error. </param>
        /// <returns> always false as an error has occurred. </returns>
        public static bool RaiseError(
            Type aspectType,
            Type verifiedClassType,
            string message)
        {
            message = string.Concat(aspectType.Name, " : \r\n\r\n ", message);
            Message.Write(MessageLocation.Of(verifiedClassType), SeverityType.Error, aspectType.Name, string.Format(message, verifiedClassType.Name));

            return false;
        }

        /// <summary>
        ///   Raise the error returned by Aspects CompileTimeValidate method.
        /// </summary>
        /// <param name="aspectType"> The Aspect Class Type that raise the error. </param>
        /// <param name="verifiedMethod"> The class Type given to the CompileTimeValidate method. </param>
        /// <param name="message"> The message to give explaining the error. </param>
        /// <returns> always false as an error has occurred. </returns>
        public static bool RaiseError(
            Type aspectType,
            MethodBase verifiedMethod,
            string message)
        {
            message = string.Concat(aspectType.Name, " : \r\n\r\n ", message);
            Message.Write(MessageLocation.Of(verifiedMethod), SeverityType.Error, aspectType.Name, string.Format(message, verifiedMethod.Name));

            return false;
        }

        #endregion
    }
}