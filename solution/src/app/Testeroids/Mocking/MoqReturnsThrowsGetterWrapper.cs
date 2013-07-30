// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoqReturnsThrowsGetterWrapper.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Mocking
{
    using System;
    using System.Linq.Expressions;

    using Moq.Language;
    using Moq.Language.Flow;

    internal class MoqReturnsThrowsGetterWrapper<TMock, TResult> : Moq.Language.Flow.IReturnsThrowsGetter<TMock, TResult>
        where TMock : class
    {
        #region Fields

        private readonly LambdaExpression expression;

        private readonly IVerifiedMock testeroidsMock;

        private readonly IReturnsThrowsGetter<TMock, TResult> wrappedReturnsThrows;

        #endregion

        #region Constructors and Destructors

        public MoqReturnsThrowsGetterWrapper(
            LambdaExpression expression, 
            IReturnsThrowsGetter<TMock, TResult> wrappedReturnsThrows, 
            IVerifiedMock testeroidsMock)
        {
            this.expression = expression;
            this.wrappedReturnsThrows = wrappedReturnsThrows;
            this.testeroidsMock = testeroidsMock;
        }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturnsGetter<TMock, TResult>.Returns(TResult value)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(value);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturnsGetter<TMock, TResult>.Returns(Func<TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws(Exception exception)
        {
            var returnsThrows = this.wrappedReturnsThrows.Throws(exception);
            return new MoqThrowsResult(this.expression, returnsThrows, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws<TException>()
        {
            var returnsThrows = this.wrappedReturnsThrows.Throws<TException>();
            return new MoqThrowsResult(this.expression, returnsThrows, this.testeroidsMock);
        }

        #endregion
    }
}