// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoqSetupWrapper.cs" company="Testeroids">
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

    internal class MoqSetupWrapper<T> : IVerifiesInternals, 
                                        Moq.Language.Flow.ISetup<T>
        where T : class
    {
        #region Fields

        private readonly ISetup<T> wrappedSetup;

        #endregion

        #region Constructors and Destructors

        public MoqSetupWrapper(
            ISetup<T> setup, 
            LambdaExpression expression, 
            IVerifiedMock testeroidsMock)
        {
            this.Expression = expression;
            this.TesteroidsMock = testeroidsMock;
            this.wrappedSetup = setup;
        }

        #endregion

        #region Public Properties

        public LambdaExpression Expression { get; private set; }

        public IVerifiedMock TesteroidsMock { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("To verify this condition, use the overload to Verify that receives Times.AtMost(callCount).")]
        public IVerifies AtMost(int callCount)
        {
            return this.wrappedSetup.AtMost(callCount);
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("To verify this condition, use the overload to Verify that receives Times.AtMostOnce().")]
        public IVerifies AtMostOnce()
        {
            return this.wrappedSetup.AtMostOnce();
        }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback(Action action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1>(Action<T1> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2>(Action<T1, T2> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises(
            Action<T> eventExpression, 
            EventArgs args)
        {
            return this.wrappedSetup.Raises(eventExpression, args);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises(
            Action<T> eventExpression, 
            Func<EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises(
            Action<T> eventExpression, 
            params object[] args)
        {
            return this.wrappedSetup.Raises(eventExpression, args);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1>(
            Action<T> eventExpression, 
            Func<T1, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2>(
            Action<T> eventExpression, 
            Func<T1, T2, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, EventArgs> func)
        {
            return this.wrappedSetup.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws(Exception exception)
        {
            return this.wrappedSetup.Throws(exception);
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws<TException>()
        {
            return this.wrappedSetup.Throws(new TException());
        }

        /// <inheritdoc/>
        void IVerifies.Verifiable()
        {
            this.wrappedSetup.Verifiable();
        }

        /// <inheritdoc/>
        void IVerifies.Verifiable(string failMessage)
        {
            this.wrappedSetup.Verifiable(failMessage);
        }

        #endregion
    }

    internal class MoqSetupWrapper<T, TResult> : Moq.Language.Flow.ISetup<T, TResult>, 
                                                 IVerifiesInternals
        where T : class
    {
        #region Fields

        private readonly ISetup<T, TResult> wrappedSetup;

        #endregion

        #region Constructors and Destructors

        public MoqSetupWrapper(
            ISetup<T, TResult> setup, 
            Expression<Func<T, TResult>> expression, 
            IVerifiedMock testeroidsMock)
        {
            this.wrappedSetup = setup;
            this.Expression = expression;
            this.TesteroidsMock = testeroidsMock;
        }

        #endregion

        #region Public Properties

        public LambdaExpression Expression { get; private set; }

        public IVerifiedMock TesteroidsMock { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback(Action action)
        {
            var returnsThrows = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, returnsThrows, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1>(Action<T1> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2>(Action<T1, T2> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            var callbackResult = this.wrappedSetup.Callback(action);
            return new MoqReturnsThrowsWrapper<T, TResult>(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns(TResult value)
        {
            var returnsResult = this.wrappedSetup.Returns(value);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns(Func<TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1>(Func<T1, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2>(Func<T1, T2, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3>(Func<T1, T2, T3, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4>(Func<T1, T2, T3, T4, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> valueFunction)
        {
            var returnsResult = this.wrappedSetup.Returns(valueFunction);
            return new MoqReturnsResultWrapper<T>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws(Exception exception)
        {
            var returnsThrows = this.wrappedSetup.Throws(exception);
            return new MoqThrowsResultWrapper(this.Expression, returnsThrows, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        IThrowsResult IThrows.Throws<TException>()
        {
            var returnsThrows = this.wrappedSetup.Throws<TException>();
            return new MoqThrowsResultWrapper(this.Expression, returnsThrows, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        void IVerifies.Verifiable()
        {
            this.wrappedSetup.Verifiable();
        }

        /// <inheritdoc/>
        void IVerifies.Verifiable(string failMessage)
        {
            this.wrappedSetup.Verifiable(failMessage);
        }

        #endregion
    }
}