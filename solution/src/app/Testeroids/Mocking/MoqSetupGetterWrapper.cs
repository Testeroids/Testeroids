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

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1>(Action<T1> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2>(Action<T1, T2> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            return this.wrappedSetup.Callback(action);
        }

        public Moq.Language.Flow.IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            return this.wrappedSetup.Callback(action);
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

        public IReturnsResult<T> Returns<T1>(Func<T1, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2>(Func<T1, T2, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3>(Func<T1, T2, T3, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4>(Func<T1, T2, T3, T4, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T, TResult>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> valueFunction)
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
            this.Verifiable(failMessage);
        }

        #endregion
    }
}