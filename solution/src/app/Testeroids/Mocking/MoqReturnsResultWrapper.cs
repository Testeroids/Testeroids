// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoqReturnsResultWrapper.cs" company="Testeroids">
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

    internal class MoqReturnsResultWrapper<TMock> : Moq.Language.Flow.IReturnsResult<TMock>, 
                                                    IVerifiesInternals
        where TMock : class
    {
        #region Fields

        private readonly IReturnsResult<TMock> wrappedReturnsResult;

        #endregion

        #region Constructors and Destructors

        public MoqReturnsResultWrapper(
            LambdaExpression expression, 
            IReturnsResult<TMock> returnsResult, 
            IVerifiedMock testeroidsMock)
        {
            this.Expression = expression;
            this.wrappedReturnsResult = returnsResult;
            this.TesteroidsMock = testeroidsMock;
        }

        #endregion

        #region Public Properties

        public LambdaExpression Expression { get; private set; }

        public IVerifiedMock TesteroidsMock { get; private set; }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("To verify this condition, use the overload to Verify that receives Times.AtMost(callCount).")]
        IVerifies IOccurrence.AtMost(int callCount)
        {
            return this.wrappedReturnsResult.AtMost(callCount);
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("To verify this condition, use the overload to Verify that receives Times.AtMostOnce().")]
        IVerifies IOccurrence.AtMostOnce()
        {
            return this.wrappedReturnsResult.AtMostOnce();
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback(Action action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1>(Action<T1> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2>(Action<T1, T2> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            var callbackResult = this.wrappedReturnsResult.Callback(action);
            return new MoqCallbackResultWrapper(this.Expression, callbackResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises(
            Action<TMock> eventExpression, 
            EventArgs args)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, args);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises(
            Action<TMock> eventExpression, 
            Func<EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises(
            Action<TMock> eventExpression, 
            params object[] args)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, args);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1>(
            Action<TMock> eventExpression, 
            Func<T1, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2>(
            Action<TMock> eventExpression, 
            Func<T1, T2, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4, T5>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, T5, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4, T5, T6>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4, T5, T6, T7>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4, T5, T6, T7, T8>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<TMock>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            Action<TMock> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        void IVerifies.Verifiable()
        {
            this.wrappedReturnsResult.Verifiable();
        }

        /// <inheritdoc/>
        void IVerifies.Verifiable(string failMessage)
        {
            this.wrappedReturnsResult.Verifiable(failMessage);
        }

        #endregion
    }
}