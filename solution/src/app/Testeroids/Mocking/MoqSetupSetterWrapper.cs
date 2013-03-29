namespace Testeroids.Mocking
{
    using System;
    using System.Linq.Expressions;

    using Moq.Language;
    using Moq.Language.Flow;

    internal class MoqSetupSetterWrapper<TMock, TResult> : ISetupSetter<TMock, TResult>, IVerifiesInternals
        where TMock : class
    {
        public MoqSetupSetterWrapper(ISetupSetter<TMock, TResult> setupSetter,
                                     Action<TMock> setterExpression,
                                     TesteroidsMock<TMock> testeroidsMock)
        {
            throw new NotImplementedException();
        }      

        public LambdaExpression Expression { get; set; }

        public IVerifiedMock TesteroidsMock { get; set; }

        public ICallbackResult Callback(Action<TResult> action)
        {
            throw new NotImplementedException();
        }

        public IThrowsResult Throws(Exception exception)
        {
            throw new NotImplementedException();
        }

        public IThrowsResult Throws<TException>() where TException : Exception, new()
        {
            throw new NotImplementedException();
        }

        public IVerifies AtMostOnce()
        {
            throw new NotImplementedException();
        }

        public IVerifies AtMost(int callCount)
        {
            throw new NotImplementedException();
        }

        public void Verifiable()
        {
            throw new NotImplementedException();
        }

        public void Verifiable(string failMessage)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises(Action<TMock> eventExpression,
                                EventArgs args)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises(Action<TMock> eventExpression,
                                Func<EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises(Action<TMock> eventExpression,
                                params object[] args)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1>(Action<TMock> eventExpression,
                                    Func<T1, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2>(Action<TMock> eventExpression,
                                        Func<T1, T2, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3>(Action<TMock> eventExpression,
                                            Func<T1, T2, T3, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4>(Action<TMock> eventExpression,
                                                Func<T1, T2, T3, T4, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4, T5>(Action<TMock> eventExpression,
                                                    Func<T1, T2, T3, T4, T5, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6>(Action<TMock> eventExpression,
                                                        Func<T1, T2, T3, T4, T5, T6, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7>(Action<TMock> eventExpression,
                                                            Func<T1, T2, T3, T4, T5, T6, T7, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8>(Action<TMock> eventExpression,
                                                                Func<T1, T2, T3, T4, T5, T6, T7, T8, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<TMock> eventExpression,
                                                                    Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<TMock> eventExpression,
                                                                         Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<TMock> eventExpression,
                                                                              Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<TMock> eventExpression,
                                                                                   Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<TMock> eventExpression,
                                                                                        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<TMock> eventExpression,
                                                                                             Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<TMock> eventExpression,
                                                                                                  Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, EventArgs> func)
        {
            throw new NotImplementedException();
        }

        public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<TMock> eventExpression,
                                                                                                       Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, EventArgs> func)
        {
            throw new NotImplementedException();
        }
    }
}