namespace Testeroids.Mocking
{
    using System;
    using System.Linq.Expressions;

    using Moq.Language;
    using Moq.Language.Flow;

    internal class MoqSetupSetterWrapper<TMock> : ISetup<TMock>, IVerifiesInternals
        where TMock : class
    {
        private ISetup<TMock> setupSet;


        public MoqSetupSetterWrapper(ISetup<TMock> setupSet,
                                     Tuple<object, string> setterExpressionWithDescription,
                                     TesteroidsMock<TMock> testeroidsMock)
        {
            this.setupSet = setupSet;
            this.Expression = setterExpressionWithDescription;
            this.TesteroidsMock = testeroidsMock;
        }

        protected MoqSetupSetterWrapper()
        {
            throw new NotImplementedException();
        }

        public ICallbackResult Callback(Action action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T>(Action<T> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2>(Action<T1, T2> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            return this.setupSet.Callback(action);
        }

        public ICallbackResult Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            return this.setupSet.Callback(action);
        }

        public IThrowsResult Throws(Exception exception)
        {
            return this.setupSet.Throws(exception);
        }

        public IThrowsResult Throws<TException>() where TException : Exception, new()
        {
            return this.setupSet.Throws<TException>();
        }

        public IVerifies AtMostOnce()
        {
            return this.setupSet.AtMostOnce();
        }

        public IVerifies AtMost(int callCount)
        {
            return this.setupSet.AtMost(callCount);
        }

        public void Verifiable()
        {
            this.setupSet.Verifiable();
        }

        public void Verifiable(string failMessage)
        {
            this.setupSet.Verifiable(failMessage);
        }

        public IVerifies Raises(Action<TMock> eventExpression,
                                EventArgs args)
        {
            return this.setupSet.Raises(eventExpression, args);
        }

        public IVerifies Raises(Action<TMock> eventExpression,
                                Func<EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises(Action<TMock> eventExpression,
                                params object[] args)
        {
            return this.setupSet.Raises(eventExpression, args);
        }

        public IVerifies Raises<T1>(Action<TMock> eventExpression,
                                    Func<T1, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2>(Action<TMock> eventExpression,
                                        Func<T1, T2, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3>(Action<TMock> eventExpression,
                                            Func<T1, T2, T3, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4>(Action<TMock> eventExpression,
                                                Func<T1, T2, T3, T4, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5>(Action<TMock> eventExpression,
                                                    Func<T1, T2, T3, T4, T5, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6>(Action<TMock> eventExpression,
                                                        Func<T1, T2, T3, T4, T5, T6, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7>(Action<TMock> eventExpression,
                                                            Func<T1, T2, T3, T4, T5, T6, T7, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8>(Action<TMock> eventExpression,
                                                                Func<T1, T2, T3, T4, T5, T6, T7, T8, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<TMock> eventExpression,
                                                                    Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<TMock> eventExpression,
                                                                         Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<TMock> eventExpression,
                                                                              Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<TMock> eventExpression,
                                                                                   Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<TMock> eventExpression,
                                                                                        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<TMock> eventExpression,
                                                                                             Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<TMock> eventExpression,
                                                                                                  Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<TMock> eventExpression,
                                                                                                       Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, EventArgs> func)
        {
            return this.setupSet.Raises(eventExpression, func);
        }

        public object Expression { get; set; }

        public IVerifiedMock TesteroidsMock { get; set; }
    }

    internal class MoqSetupSetterWrapper<TMock, TProperty> : MoqSetupSetterWrapper<TMock>, 
                                ISetupSetter<TMock, TProperty>
                                                  
        where TMock : class
    {
        public MoqSetupSetterWrapper(ISetupSetter<TMock, TProperty> setupSetter,
                                     Action<TMock> setterExpression,
                                     TesteroidsMock<TMock> testeroidsMock)
        {
            throw new NotImplementedException();
        }

        public ICallbackResult Callback(Action<TProperty> action)
        {
            throw new NotImplementedException();
        }
    }
}