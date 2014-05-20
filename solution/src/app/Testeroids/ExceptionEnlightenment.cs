namespace Testeroids
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// The default exception enlightenment, which will use <c>ExceptionDispatchInfo</c> if possible, falling back on <c>Exception.PrepForRemoting</c>, with a final fallback on <see cref="P:System.Exception.Data"/>.
    /// </summary>
    /// <remarks>
    /// NOTE: Copied from Nito.AsyncEx in order to avoid unnecessary dependencies. This will no longer be necessary once we move to .NET Framework 4.5.
    /// </remarks>
    internal sealed class ExceptionEnlightenment
    {
        #region Static Fields

        /// <summary>
        /// A delegate that will call <c>ExceptionDispatchInfo.Capture</c> followed by <c>ExceptionDispatchInfo.Throw</c>, or <c>null</c> if the <c>ExceptionDispatchInfo</c> type does not exist.
        /// 
        /// </summary>
        private static readonly Action<Exception> CaptureAndThrow;

        /// <summary>
        /// A delegate that will call <c>Exception.PrepForRemoting</c>, or <c>null</c> if the method does not exist. This member is always <c>null</c> if <see cref="F:CaptureAndThrow"/> is non-<c>null</c>.
        /// 
        /// </summary>
        private static readonly Action<Exception> PrepForRemoting;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Examines the current runtime and initializes the static delegates appropriately.
        /// 
        /// </summary>
        static ExceptionEnlightenment()
        {
            var exceptionType = typeof(Exception);
            var exceptionDispatchInfoType = Type.GetType("System.Runtime.ExceptionServices.ExceptionDispatchInfo");
            if (exceptionDispatchInfoType != null)
            {
                try
                {
                    var parameterExpression = Expression.Parameter(exceptionType, "exception");
                    CaptureAndThrow = Expression.Lambda<Action<Exception>>(Expression.Call(Expression.Call(exceptionDispatchInfoType, "Capture", null, new Expression[] { parameterExpression }), "Throw", null, new Expression[0]), new[] { parameterExpression })
                                                .Compile();
                }
                catch (InvalidOperationException)
                {
                }
                catch (ArgumentException)
                {
                }
            }
            if (CaptureAndThrow != null)
            {
                return;
            }
            var method = TryGetMethod(exceptionType, "PrepForRemoting", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method == null)
            {
                return;
            }
            try
            {
                var parameterExpression = Expression.Parameter(exceptionType, "exception");
                PrepForRemoting = Expression.Lambda<Action<Exception>>(Expression.Call(parameterExpression, method), new[] { parameterExpression }).Compile();
            }
            catch (ArgumentException)
            {
            }
        }

        #endregion

        #region Public Methods and Operators

        public static Exception PrepareForRethrow(Exception exception)
        {
            if (CaptureAndThrow != null)
            {
                CaptureAndThrow(exception);
            }
            else if (PrepForRemoting != null)
            {
                PrepForRemoting(exception);
            }
            else
            {
                TryAddStackTrace(exception);
            }
            return exception;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attempts to add the original stack trace to the <see cref="P:System.Exception.Data"/> collection.
        /// 
        /// </summary>
        /// <param name="exception">The exception. May not be <c>null</c>.</param>
        /// <returns>
        /// <c>true</c> if the stack trace was successfully saved; <c>false</c> otherwise.
        /// </returns>
        private static void TryAddStackTrace(Exception exception)
        {
            try
            {
                exception.Data.Add("Original stack trace", exception.StackTrace);
            }
            catch (ArgumentException)
            {
            }
            catch (NotSupportedException)
            {
            }
        }

        /// <summary>
        /// Attempts to look up a method for a type, handling vexing exceptions.
        /// 
        /// </summary>
        /// <param name="type">The type on which to look up the method.</param><param name="name">The method to look up.</param><param name="flags">The binding flags used when searching for the method.</param>
        private static MethodInfo TryGetMethod(Type type,
                                               string name,
                                               BindingFlags flags)
        {
            try
            {
                return type.GetMethod(name, flags);
            }
            catch (AmbiguousMatchException)
            {
                return null;
            }
        }

        #endregion
    }
}