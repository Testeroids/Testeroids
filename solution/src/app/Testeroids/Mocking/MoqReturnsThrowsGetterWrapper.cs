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

        private readonly IReturnsThrowsGetter<TMock, TResult> wrappedReturnsThrowsGetter;

        #endregion

        #region Constructors and Destructors

        public MoqReturnsThrowsGetterWrapper(
            LambdaExpression expression,
            IReturnsThrowsGetter<TMock, TResult> returnsThrowsGetter,
            IVerifiedMock testeroidsMock)
        {
            this.expression = expression;
            this.wrappedReturnsThrowsGetter = returnsThrowsGetter;
            this.testeroidsMock = testeroidsMock;
        }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturnsGetter<TMock, TResult>.CallBase()
        {
            var returnsResult = this.wrappedReturnsThrowsGetter.CallBase();
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturnsGetter<TMock, TResult>.Returns(TResult value)
        {
            var returnsResult = this.wrappedReturnsThrowsGetter.Returns(value);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturnsGetter<TMock, TResult>.Returns(Func<TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrowsGetter.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws(Exception exception)
        {
            var returnsThrows = this.wrappedReturnsThrowsGetter.Throws(exception);
            return new MoqThrowsResultWrapper(this.expression, returnsThrows, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws<TException>()
        {
            var returnsThrows = this.wrappedReturnsThrowsGetter.Throws<TException>();
            return new MoqThrowsResultWrapper(this.expression, returnsThrows, this.testeroidsMock);
        }

        #endregion
    }
}