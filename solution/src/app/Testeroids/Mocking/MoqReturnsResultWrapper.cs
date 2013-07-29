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

    internal class MoqReturnsResultWrapper<T> : Moq.Language.Flow.IReturnsResult<T>, 
                                                IVerifiesInternals
        where T : class
    {
        #region Fields

        private readonly IReturnsResult<T> wrappedReturnsResult;

        #endregion

        #region Constructors and Destructors

        public MoqReturnsResultWrapper(
            LambdaExpression expression, 
            IReturnsResult<T> returnsResult, 
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
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        ICallbackResult ICallback.Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            return this.wrappedReturnsResult.Callback(action);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises(
            Action<T> eventExpression, 
            EventArgs args)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, args);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises(
            Action<T> eventExpression, 
            Func<EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises(
            Action<T> eventExpression, 
            params object[] args)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, args);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1>(
            Action<T> eventExpression, 
            Func<T1, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2>(
            Action<T> eventExpression, 
            Func<T1, T2, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            Action<T> eventExpression, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, EventArgs> func)
        {
            return this.wrappedReturnsResult.Raises(eventExpression, func);
        }

        /// <inheritdoc/>
        IVerifies IRaise<T>.Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            Action<T> eventExpression, 
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