namespace Testeroids.Mocking
{
    using System;
    using System.Linq.Expressions;

    using Moq.Language;
    using Moq.Language.Flow;

    internal class MoqReturnsResultWrapper<T, TResult> : Moq.Language.Flow.IReturnsResult<T>, IVerifiesInternals
        where T : class
    {
        public LambdaExpression Expression { get; set; }

        public Moq.Language.Flow.IReturnsResult<T> ReturnsResult { get; set; }

        public IVerifiedMock TesteroidsMock { get; set; }

        public MoqReturnsResultWrapper(LambdaExpression expression,
                                       Moq.Language.Flow.IReturnsResult<T> returnsResult,
                                       IVerifiedMock testeroidsMock)
        {
            this.Expression = expression;
            this.ReturnsResult = returnsResult;
            this.TesteroidsMock = testeroidsMock;
        }

        #region Callback

        public ICallbackResult Callback(Action action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1>(Action<T1> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2>(Action<T1, T2> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            return this.ReturnsResult.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            return this.ReturnsResult.Callback(action);
        } 
        #endregion

        public IVerifies AtMostOnce()
        {
            return this.ReturnsResult.AtMostOnce();
        }

        public IVerifies AtMost(int callCount)
        {
            return this.ReturnsResult.AtMost(callCount);
        }

        #region Raises
        public IVerifies Raises(Action<T> eventExpression,
                                EventArgs args)
        {
            return this.ReturnsResult.Raises(eventExpression, args);
        }

        public IVerifies Raises(Action<T> eventExpression,
                                Func<EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises(Action<T> eventExpression,
                                params object[] args)
        {
            return this.ReturnsResult.Raises(eventExpression, args);
        }

        public IVerifies Raises<T1>(Action<T> eventExpression,
                                    Func<T1, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2>(Action<T> eventExpression,
                                        Func<T1, T2, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3>(Action<T> eventExpression,
                                            Func<T1, T2, T3, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4>(Action<T> eventExpression,
                                                Func<T1, T2, T3, T4, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5>(Action<T> eventExpression,
                                                    Func<T1, T2, T3, T4, T5, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6>(Action<T> eventExpression,
                                                        Func<T1, T2, T3, T4, T5, T6, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7>(Action<T> eventExpression,
                                                            Func<T1, T2, T3, T4, T5, T6, T7, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T> eventExpression,
                                                                Func<T1, T2, T3, T4, T5, T6, T7, T8, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T> eventExpression,
                                                                    Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T> eventExpression,
                                                                         Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T> eventExpression,
                                                                              Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T> eventExpression,
                                                                                   Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T> eventExpression,
                                                                                        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T> eventExpression,
                                                                                             Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T> eventExpression,
                                                                                                  Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T> eventExpression,
                                                                                                       Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, EventArgs> func)
        {
            return this.ReturnsResult.Raises(eventExpression, func);
        } 
        #endregion

        public void Verifiable()
        {
            this.ReturnsResult.Verifiable();            
        }

        public void Verifiable(string failMessage)
        {
            this.ReturnsResult.Verifiable(failMessage);
        }
    }
}