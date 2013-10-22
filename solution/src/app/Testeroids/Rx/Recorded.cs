namespace Testeroids.Rx
{
    using System;
    using System.Reactive;

    using Microsoft.Reactive.Testing;

    /// <summary>
    /// Defines Helper methods to use when testing Rx's <see cref="IObservable{T}"/> 
    /// </summary>
    public static class Recorded
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnCompleted"/> kind.
        /// </summary>
        /// <typeparam name="T">Type of the notifications.</typeparam>
        /// <param name="time">Virtual time the value was produced on.</param>
        /// <returns>
        /// A new <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnCompleted"/> kind.
        /// </returns>
        public static Recorded<Notification<T>> OnCompleted<T>(long time)
        {
            return new Recorded<Notification<T>>(time, Notification.CreateOnCompleted<T>());
        }

        /// <summary>
        /// Creates a <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnCompleted"/> kind.
        /// </summary>
        /// <typeparam name="T">Type of the notifications.</typeparam>
        /// <param name="time">Virtual time the value was produced on.</param>
        /// <returns>
        /// A new <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnCompleted"/> kind.
        /// </returns>
        public static Recorded<Notification<T>> OnCompleted<T>(TimeSpan time)
        {
            return Recorded.OnCompleted<T>(time.Ticks);
        }

        /// <summary>
        /// Creates a <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnError"/> kind.
        /// </summary>
        /// <typeparam name="T">Type of the notifications.</typeparam>
        /// <param name="time">Virtual time the value was produced on.</param>
        /// <param name="error">The exception to record.</param>
        /// <returns>
        /// A new <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnError"/> kind.
        /// </returns>
        public static Recorded<Notification<T>> OnError<T>(
            long time, 
            Exception error)
        {
            return new Recorded<Notification<T>>(time, Notification.CreateOnError<T>(error));
        }

        /// <summary>
        /// Creates a <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnError"/> kind.
        /// </summary>
        /// <typeparam name="T">Type of the notifications.</typeparam>
        /// <param name="time">Virtual time the value was produced on.</param>
        /// <param name="error">The exception to record.</param>
        /// <returns>
        /// A new <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnError"/> kind.
        /// </returns>
        public static Recorded<Notification<T>> OnError<T>(
            TimeSpan time, 
            Exception error)
        {
            return Recorded.OnError<T>(time.Ticks, error);
        }

        /// <summary>
        /// Creates a <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnNext"/> kind.
        /// </summary>
        /// <typeparam name="T">Type of the notifications.</typeparam>
        /// <param name="time">Virtual time the value was produced on.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A new <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnNext"/> kind.
        /// </returns>
        public static Recorded<Notification<T>> OnNext<T>(
            long time, 
            T value)
        {
            return new Recorded<Notification<T>>(time, new OnNextNotification<T>(value));
        }

        /// <summary>
        /// Creates a <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnNext"/> kind.
        /// </summary>
        /// <typeparam name="T">Type of the notifications.</typeparam>
        /// <param name="time">Virtual time the value was produced on.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A new <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnNext"/> kind.
        /// </returns>
        public static Recorded<Notification<T>> OnNext<T>(
            TimeSpan time, 
            T value)
        {
            return Recorded.OnNext(time.Ticks, value);
        }

        /// <summary>
        /// Creates the a <see cref="Recorded{Notification}"/> with <see cref="NotificationKind.OnNext"/> kind
        /// converting the passed <see cref="IMock{T}"/> to be <see cref="IEquatable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the notifications.</typeparam>
        /// <param name="time">Virtual time the value was produced on.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A new <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnNext"/> kind.
        /// </returns>
        public static Recorded<Notification<T>> OnNext<T>(
            long time, 
            IMock<T> value)
            where T : class
        {
            // We are using the Equals implementation of Recorded to test which calls Equals on the mocks
            return OnNext(time, value.AsEquatable().Object);
        }

        /// <summary>
        /// Creates the a <see cref="Recorded{Notification}"/> with <see cref="NotificationKind.OnNext"/> kind
        /// converting the passed <see cref="IMock{T}"/> to be <see cref="IEquatable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the notifications.</typeparam>
        /// <param name="time">Virtual time the value was produced on.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A new <see cref="Recorded{Notification}"/> instance with <see cref="NotificationKind.OnNext"/> kind.
        /// </returns>
        public static Recorded<Notification<T>> OnNext<T>(
            TimeSpan time, 
            IMock<T> value)
            where T : class
        {
            return OnNext(time.Ticks, value);
        }

        /// <summary>
        /// Creates a new subscription object with the given virtual subscription and unsubscription time.
        /// </summary>
        /// <returns>
        /// An instance of a <see cref="Microsoft.Reactive.Testing.Subscription"/> object, which can be used to compare with actual results on <see cref="ITestableObservable{T}"/>.
        /// </returns>
        public static Subscription Subscription()
        {
            return new Subscription();
        }

        /// <summary>
        /// Creates a new subscription object with the given virtual subscription and unsubscription time.
        /// </summary>
        /// <param name="subscribeTime">Virtual time at which the subscription occurred.</param>
        /// <returns>
        /// An instance of a <see cref="Microsoft.Reactive.Testing.Subscription"/> object, which can be used to compare with actual results on <see cref="ITestableObservable{T}"/>.
        /// </returns>
        public static Subscription Subscription(TimeSpan subscribeTime)
        {
            return new Subscription(subscribeTime.Ticks);
        }

        /// <summary>
        /// Creates a new subscription object with the given virtual subscription and unsubscription time.
        /// </summary>
        /// <param name="subscribeTimeAsTicks">Virtual time at which the subscription occurred.</param>
        /// <returns>
        /// An instance of a <see cref="Microsoft.Reactive.Testing.Subscription"/> object, which can be used to compare with actual results on <see cref="ITestableObservable{T}"/>.
        /// </returns>
        public static Subscription Subscription(
            long subscribeTimeAsTicks)
        {
            return new Subscription(subscribeTimeAsTicks);
        }

        /// <summary>
        /// Creates a new subscription object with the given virtual subscription and unsubscription time.
        /// </summary>
        /// <param name="subscribeTime">Virtual time at which the subscription occurred.</param>
        /// <param name="unsubscribeTime">Virtual time at which the unsubscription occurred.</param>
        /// <returns>
        /// An instance of a <see cref="Microsoft.Reactive.Testing.Subscription"/> object, which can be used to compare with actual results on <see cref="ITestableObservable{T}"/>.
        /// </returns>
        public static Subscription Subscription(
            TimeSpan subscribeTime, 
            TimeSpan unsubscribeTime)
        {
            return new Subscription(subscribeTime.Ticks, unsubscribeTime.Ticks);
        }

        /// <summary>
        /// Creates a new subscription object with the given virtual subscription and unsubscription time.
        /// </summary>
        /// <param name="subscribeTimeAsTicks">Virtual time at which the subscription occurred.</param>
        /// <param name="unsubscribeTimeAsTicks">Virtual time at which the unsubscription occurred.</param>
        /// <returns>
        /// An instance of a <see cref="Microsoft.Reactive.Testing.Subscription"/> object, which can be used to compare with actual results on <see cref="ITestableObservable{T}"/>.
        /// </returns>
        public static Subscription Subscription(
            long subscribeTimeAsTicks, 
            long unsubscribeTimeAsTicks)
        {
            return new Subscription(subscribeTimeAsTicks, unsubscribeTimeAsTicks);
        }

        #endregion
    }
}