namespace Testeroids.Rx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;

    using Microsoft.Reactive.Testing;

    internal class MockObserver<T> : ITestableObserver<T>
    {
        #region Fields

        private readonly long absoluteEndTime;

        private readonly List<Recorded<Notification<T>>> messages;

        private readonly Microsoft.Reactive.Testing.TestScheduler scheduler;

        #endregion

        #region Constructors and Destructors

        public MockObserver(Microsoft.Reactive.Testing.TestScheduler scheduler,
                            long absoluteEndTime)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException("scheduler");
            }

            this.scheduler = scheduler;
            this.absoluteEndTime = absoluteEndTime;
            this.messages = new List<Recorded<Notification<T>>>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets recorded timestamped notification messages received by the observer.
        /// </summary>
        public IList<Recorded<Notification<T>>> Messages
        {
            get
            {
                return this.messages.Where(m => m.Time <= this.absoluteEndTime).ToArray();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns <c>true</c> if an error or a completed event has already been observed.
        /// </summary>
        internal bool IsEnded
        {
            get
            {
                return this.messages.Any(m => m.Value.Kind == NotificationKind.OnError || m.Value.Kind == NotificationKind.OnCompleted);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// </summary>
        public void OnCompleted()
        {
            this.messages.Add(new Recorded<Notification<T>>(this.scheduler.Clock, Notification.CreateOnCompleted<T>()));
        }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
            this.messages.Add(new Recorded<Notification<T>>(this.scheduler.Clock, Notification.CreateOnError<T>(error)));
        }

        /// <summary>
        /// Provides the observer with new data.
        /// </summary>
        /// <param name="value">The current notification information.</param>
        public void OnNext(T value)
        {
            this.messages.Add(new Recorded<Notification<T>>(this.scheduler.Clock, Notification.CreateOnNext(value)));
        }

        #endregion
    }
}