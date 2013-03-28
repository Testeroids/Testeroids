namespace Testeroids.Mocking
{
    using System;
    using System.Linq.Expressions;

    using Moq.Language.Flow;

    internal class MoqSetupGetterWrapper<T, TResult> : Moq.Language.Flow.ISetupGetter<T, TResult>, 
                                                       IVerifiesInternals
        where T : class
    {
        #region Fields

        private readonly ISetupGetter<T, TResult> wrappedSetup;

        #endregion

        #region Constructors and Destructors

        public MoqSetupGetterWrapper(Moq.Language.Flow.ISetupGetter<T, TResult> setup, 
                                     Expression<Func<T, TResult>> expression, 
                                     TesteroidsMock<T> testeroidsMock)
        {
            this.wrappedSetup = setup;
            this.Expression = expression;
            this.TesteroidsMock = testeroidsMock;
        }

        #endregion

        #region Public Properties

        public LambdaExpression Expression { get; set; }

        public IVerifiedMock TesteroidsMock { get; set; }

        #endregion

        #region Public Methods and Operators

        public IReturnsThrowsGetter<T, TResult> Callback(Action action)
        {
            var returnsThrows = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsGetterWrapper<T, TResult>(returnsThrows);
        }      

        public IReturnsResult<T> Returns(TResult value)
        {
            var returnsResult = this.wrappedSetup.Returns(value);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns(Func<TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }       

        public IThrowsResult Throws(Exception exception)
        {
            return this.wrappedSetup.Throws(exception);
        }

        public IThrowsResult Throws<TException>() where TException : Exception, new()
        {
            return this.wrappedSetup.Throws<TException>();
        }

        public void Verifiable()
        {
            this.wrappedSetup.Verifiable();
        }

        public void Verifiable(string failMessage)
        {
            this.wrappedSetup.Verifiable(failMessage);
        }

        #endregion
    }
}