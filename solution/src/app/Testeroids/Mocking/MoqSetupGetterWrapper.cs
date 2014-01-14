namespace Testeroids.Mocking
{
    using System;
    using System.Linq.Expressions;

    using Moq.Language;
    using Moq.Language.Flow;

    internal class MoqSetupGetterWrapper<TMock, TProperty> : IVerifiesInternals,
                                                             Moq.Language.Flow.ISetupGetter<TMock, TProperty>
        where TMock : class
    {
        #region Fields

        private readonly ISetupGetter<TMock, TProperty> wrappedSetupGetter;

        #endregion

        #region Constructors and Destructors

        public MoqSetupGetterWrapper(
            ISetupGetter<TMock, TProperty> setupGetter,
            LambdaExpression expression,
            IVerifiedMock testeroidsMock)
        {
            this.Expression = expression;
            this.TesteroidsMock = testeroidsMock;
            this.wrappedSetupGetter = setupGetter;
        }

        #endregion

        #region Public Properties

        public LambdaExpression Expression { get; private set; }

        public IVerifiedMock TesteroidsMock { get; private set; }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturnsGetter<TMock, TProperty>.CallBase()
        {
            var returnsResult = this.wrappedSetupGetter.CallBase();
            return new MoqReturnsResultWrapper<TMock>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        IReturnsThrowsGetter<TMock, TProperty> ICallbackGetter<TMock, TProperty>.Callback(Action action)
        {
            var returnsThrowsGetter = this.wrappedSetupGetter.Callback(action);
            return new MoqReturnsThrowsGetterWrapper<TMock, TProperty>(this.Expression, returnsThrowsGetter, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturnsGetter<TMock, TProperty>.Returns(TProperty value)
        {
            var returnsResult = this.wrappedSetupGetter.Returns(value);
            return new MoqReturnsResultWrapper<TMock>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        IReturnsResult<TMock> IReturnsGetter<TMock, TProperty>.Returns(Func<TProperty> valueFunction)
        {
            var returnsResult = this.wrappedSetupGetter.Returns(valueFunction);
            return new MoqReturnsResultWrapper<TMock>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws(Exception exception)
        {
            var returnsThrows = this.wrappedSetupGetter.Throws(exception);
            return new MoqThrowsResultWrapper(this.Expression, returnsThrows, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws<TException>()
        {
            var returnsThrows = this.wrappedSetupGetter.Throws<TException>();
            return new MoqThrowsResultWrapper(this.Expression, returnsThrows, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        void IVerifies.Verifiable()
        {
            this.wrappedSetupGetter.Verifiable();
        }

        /// <inheritdoc/>
        void IVerifies.Verifiable(string failMessage)
        {
            this.wrappedSetupGetter.Verifiable(failMessage);
        }

        #endregion
    }
}