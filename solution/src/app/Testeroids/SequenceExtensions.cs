namespace Testeroids
{
    using System;
    using System.Linq.Expressions;

    using Moq;
    using Moq.Language;

    /// <summary>
    /// Helper for sequencing return values in the same method.
    /// </summary>
    public static class SequenceExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Return a sequence of values, once per call.
        /// </summary>
        public static ISetupSequentialResult<TResult> SetupSequence<TMock, TResult>(this IMock<TMock> mock, Expression<Func<TMock, TResult>> expression) where TMock : class
        {
            return Mock.Get(mock.Object).SetupSequence(expression);
        }

        #endregion
    }
}