// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnNextNotification.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Rx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reactive;

    /// <summary>
    /// Represents an OnNext notification to an observer.
    /// </summary>
    /// <typeparam name="T">The type of the elements received by the observer.</typeparam>
    /// <remarks>
    /// Takes the implementation in <see cref="Notification{T}"/> and supplements <see cref="Equals(Notification{T})"/> it so that it handles array values correctly.
    /// </remarks>
    [DebuggerDisplay("OnNext({Value})")]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class OnNextNotification<T> : Notification<T>
    {
        #region Fields

        /// <summary>
        /// The value passed to <see cref="IObserver{T}.OnNext"/>.
        /// </summary>
        private readonly T value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OnNextNotification{T}"/> class. 
        /// Constructs a notification of a new value.
        /// </summary>
        /// <param name="value">
        /// The value passed to <see cref="IObserver{T}.OnNext"/>.
        /// </param>
        public OnNextNotification(T value)
        {
            this.value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns null.
        /// </summary>
        public override Exception Exception
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Returns true.
        /// </summary>
        public override bool HasValue
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns NotificationKind.OnNext.
        /// </summary>
        public override NotificationKind Kind
        {
            get
            {
                return NotificationKind.OnNext;
            }
        }

        /// <summary>
        /// Returns the value of an OnNext notification.
        /// </summary>
        public override T Value
        {
            get
            {
                return this.value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Invokes the observer's method corresponding to the notification.
        /// </summary>
        /// <param name="observer">Observer to invoke the notification on.</param>
        public override void Accept(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException("observer");
            }

            observer.OnNext(this.Value);
        }

        /// <summary>
        /// Invokes the observer's method corresponding to the notification and returns the produced result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned from the observer's notification handlers.</typeparam><param name="observer">Observer to invoke the notification on.</param>
        /// <returns>
        /// Result produced by the observation.
        /// </returns>
        public override TResult Accept<TResult>(IObserver<T, TResult> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException("observer");
            }

            return observer.OnNext(this.Value);
        }

        /// <summary>
        /// Invokes the delegate corresponding to the notification.
        /// </summary>
        /// <param name="onNext">Delegate to invoke for an OnNext notification.</param><param name="onError">Delegate to invoke for an OnError notification.</param>
        /// <param name="onCompleted">Delegate to invoke for an OnCompleted notification.</param>
        public override void Accept(
            Action<T> onNext, 
            Action<Exception> onError, 
            Action onCompleted)
        {
            if (onNext == null)
            {
                throw new ArgumentNullException("onNext");
            }

            if (onError == null)
            {
                throw new ArgumentNullException("onError");
            }

            if (onCompleted == null)
            {
                throw new ArgumentNullException("onCompleted");
            }

            onNext(this.Value);
        }

        /// <summary>
        /// Invokes the delegate corresponding to the notification and returns the produced result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned from the notification handler delegates.</typeparam><param name="onNext">Delegate to invoke for an OnNext notification.</param><param name="onError">Delegate to invoke for an OnError notification.</param><param name="onCompleted">Delegate to invoke for an OnCompleted notification.</param>
        /// <returns>
        /// Result produced by the observation.
        /// </returns>
        public override TResult Accept<TResult>(
            Func<T, TResult> onNext, 
            Func<Exception, TResult> onError, 
            Func<TResult> onCompleted)
        {
            if (onNext == null)
            {
                throw new ArgumentNullException("onNext");
            }

            if (onError == null)
            {
                throw new ArgumentNullException("onError");
            }

            if (onCompleted == null)
            {
                throw new ArgumentNullException("onCompleted");
            }

            return onNext(this.Value);
        }

        /// <summary>
        /// Determines whether the current Notification&lt;T&gt; object has the same observer message payload as a specified Notification&lt;T&gt; value.
        /// </summary>
        /// <param name="other">An object to compare to the current Notification&lt;T&gt; object.</param>
        /// <returns>
        /// true if both Notification&lt;T&gt; objects have the same observer message payload; otherwise, false.
        /// </returns>
        /// <remarks>
        /// Equality of Notification&lt;T&gt; objects is based on the equality of the observer message payload they represent, including the notification Kind and the Value or Exception (if any).
        ///             This means two Notification&lt;T&gt; objects can be equal even though they don't represent the same observer method call, but have the same Kind and have equal parameters passed to the observer method.
        ///             In case one wants to determine whether two Notification&lt;T&gt; objects represent the same observer method call, use Object.ReferenceEquals identity equality instead.
        /// </remarks>
        public override bool Equals(Notification<T> other)
        {
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (object.ReferenceEquals(other, null) || other.Kind != NotificationKind.OnNext)
            {
                return false;
            }

            var enumerable = this.Value as IEnumerable;
            if (enumerable != null)
            {
                return enumerable.Cast<object>().SequenceEqual(((IEnumerable)other.Value).Cast<object>());
            }

            return EqualityComparer<T>.Default.Equals(this.Value, other.Value);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(this.Value);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.CurrentCulture, 
                "OnNext({0})", 
                new[] { (object)this.Value });
        }

        #endregion
    }
}