namespace Testeroids.Mocking
{
    using System;
    using System.Linq.Expressions;

    using Moq.Language;
    using Moq.Language.Flow;

    internal class MoqReturnsThrowsWrapper<TMock, TResult> : Moq.Language.Flow.IReturnsThrows<TMock, TResult>
        where TMock : class
    {
        #region Fields

        private readonly LambdaExpression expression;

        private readonly IVerifiedMock testeroidsMock;

        private readonly IReturnsThrows<TMock, TResult> wrappedReturnsThrows;

        #endregion

        #region Constructors and Destructors

        public MoqReturnsThrowsWrapper(
            LambdaExpression expression,
            IReturnsThrows<TMock, TResult> returnsThrows,
            IVerifiedMock testeroidsMock)
        {
            this.expression = expression;
            this.wrappedReturnsThrows = returnsThrows;
            this.testeroidsMock = testeroidsMock;
        }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.CallBase()
        {
            var returnsResult = this.wrappedReturnsThrows.CallBase();
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns(TResult value)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(value);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns(Func<TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1>(Func<T1, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2>(Func<T1, T2, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3>(Func<T1, T2, T3, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4>(Func<T1, T2, T3, T4, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturns<TMock, TResult>.Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> valueFunction)
        {
            var returnsResult = this.wrappedReturnsThrows.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.expression, returnsResult, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws(Exception exception)
        {
            var returnsThrows = this.wrappedReturnsThrows.Throws(exception);
            return new MoqThrowsResultWrapper(this.expression, returnsThrows, this.testeroidsMock);
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws<TException>()
        {
            var returnsThrows = this.wrappedReturnsThrows.Throws<TException>();
            return new MoqThrowsResultWrapper(this.expression, returnsThrows, this.testeroidsMock);
        }

        #endregion
    }
}