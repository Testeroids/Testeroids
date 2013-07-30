// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoqCallbackResultWrapper.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Mocking
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

    using Moq.Language;
    using Moq.Language.Flow;

    internal class MoqCallbackResultWrapper : ICallbackResult, 
                                              IVerifiesInternals
    {
        #region Fields

        private readonly ICallbackResult wrappedCallbackResult;

        #endregion

        #region Constructors and Destructors

        public MoqCallbackResultWrapper(
            LambdaExpression expression, 
            ICallbackResult wrappedCallbackResult, 
            IVerifiedMock testeroidsMock)
        {
            this.wrappedCallbackResult = wrappedCallbackResult;
            this.Expression = expression;
            this.TesteroidsMock = testeroidsMock;
        }

        #endregion

        #region Public Properties

        public LambdaExpression Expression { get; private set; }

        public IVerifiedMock TesteroidsMock { get; private set; }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("To verify this condition, use the overload to Verify that receives Times.AtMost(callCount).")]
        IVerifies IOccurrence.AtMost(int callCount)
        {
            return this.wrappedCallbackResult.AtMost(callCount);
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("To verify this condition, use the overload to Verify that receives Times.AtMostOnce().")]
        IVerifies IOccurrence.AtMostOnce()
        {
            return this.wrappedCallbackResult.AtMostOnce();
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws(Exception exception)
        {
            var throwsResult = this.wrappedCallbackResult.Throws(exception);
            return new MoqThrowsResult(this.Expression, throwsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws<TException>()
        {
            var throwsResult = this.wrappedCallbackResult.Throws<TException>();
            return new MoqThrowsResult(this.Expression, throwsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        void IVerifies.Verifiable()
        {
            this.wrappedCallbackResult.Verifiable();
        }

        /// <inheritdoc/>
        void IVerifies.Verifiable(string failMessage)
        {
            this.wrappedCallbackResult.Verifiable(failMessage);
        }

        #endregion
    }
}