// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoqThrowsResult.cs" company="Testeroids">
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

    internal class MoqThrowsResult : IThrowsResult, 
                                     IVerifiesInternals
    {
        #region Fields

        private readonly IThrowsResult throwsResult;

        #endregion

        #region Constructors and Destructors

        public MoqThrowsResult(
            LambdaExpression expression, 
            IThrowsResult throwsResult, 
            IVerifiedMock testeroidsMock)
        {
            this.Expression = expression;
            this.throwsResult = throwsResult;
            this.TesteroidsMock = testeroidsMock;
        }

        #endregion

        #region Public Properties

        public LambdaExpression Expression { get; set; }

        public IVerifiedMock TesteroidsMock { get; set; }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc/>
        [Obsolete("To verify this condition, use the overload to Verify that receives Times.AtMost(callCount).")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        IVerifies IOccurrence.AtMost(int callCount)
        {
            return this.throwsResult.AtMost(callCount);
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("To verify this condition, use the overload to Verify that receives Times.AtMostOnce().")]
        IVerifies IOccurrence.AtMostOnce()
        {
            return this.throwsResult.AtMostOnce();
        }

        /// <inheritdoc/>
        void IVerifies.Verifiable()
        {
            this.throwsResult.Verifiable();
        }

        /// <inheritdoc/>
        void IVerifies.Verifiable(string failMessage)
        {
            this.throwsResult.Verifiable(failMessage);
        }

        #endregion
    }
}