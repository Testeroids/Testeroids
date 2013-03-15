// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestScheduler.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Rx
{
    using System;
    using System.Linq;
    using System.Reactive.Concurrency;

    using Microsoft.Reactive.Testing;

    /// <summary>
    /// Extended version of Rx's TestScheduler to be able to test methods which return an <see cref="IObservable{T}"/>.
    /// In comparison to the original test scheduler's <see cref="VirtualTimeSchedulerBase{TAbsolute,TRelative}.Start"/> method, 
    /// <see cref="StartWaitingUntil(long)"/> doesn't complete when no items are scheduled anymore but when the Scheduler has been canceled
    /// or the <see cref="VirtualTimeSchedulerBase{TAbsolute,TRelative}.Clock"/> has reached the specified end time.
    /// </summary>
    public class TestScheduler : Microsoft.Reactive.Testing.TestScheduler
    {
        #region Fields

        /// <summary>
        /// The synchronization primitive for access to the scheduler queue.
        /// </summary>
        private readonly object schedulerQueueLockObject = new object();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates the <see cref="IObservable{T}"/> to consume and subscribe to it.
        /// Waits for the <see cref="VirtualTimeSchedulerBase{TAbsolute,TRelative}.Clock"/> to reach the end time or the scheduler to be canceled.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="create">
        /// The factory method which will create the <see cref="IObservable{T}"/> to consume.
        /// </param>
        /// <param name="absoluteEndTime">
        /// The time at which the scheduler should stop.
        /// The scheduler moves in time as it executes scheduled tasks.
        /// </param>
        /// <returns>
        /// The <see cref="ITestableObserver{T}"/> which has been used to consume the <see cref="IObservable{T}"/>.
        /// </returns>
        public ITestableObserver<T> Consume<T>(
            Func<IObservable<T>> create, 
            TimeSpan absoluteEndTime)
        {
            return this.Consume(create, absoluteEndTime.Ticks);
        }

        /// <summary>
        /// Creates the <see cref="IObservable{T}"/> to consume and subscribe to it.
        /// Waits for the <see cref="VirtualTimeSchedulerBase{TAbsolute,TRelative}.Clock"/> to reach the end time or the scheduler to be canceled.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="create">
        /// The factory method which will create the <see cref="IObservable{T}"/> to consume.
        /// </param>
        /// <param name="absoluteEndTime">
        /// The time at which the scheduler should stop.
        /// The scheduler moves in time as it executes scheduled tasks.</param>
        /// <returns>
        /// The <see cref="ITestableObserver{T}"/> which has been used to consume the <see cref="IObservable{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="absoluteEndTime"/> is negative.</exception>
        public ITestableObserver<T> Consume<T>(
            Func<IObservable<T>> create, 
            long absoluteEndTime = long.MaxValue)
        {
            if (this.Comparer.Compare(absoluteEndTime, 0) < 0)
            {
                throw new ArgumentOutOfRangeException("absoluteEndTime");
            }

            var observer = (MockObserver<T>)this.CreateObserver<T>(absoluteEndTime);
            var source = create();

            if (observer.Messages.Any())
            {
                this.Clock = System.Math.Min(absoluteEndTime, observer.Messages.Max(x => x.Time));
            }

            using (source.Subscribe(observer))
            {
                if (!observer.IsEnded)
                {
                    this.StartWaitingUntil(absoluteEndTime);
                }

                return observer;
            }
        }

        /// <summary>
        /// Creates an observer that records received notification messages and timestamps those.
        /// </summary>
        /// <typeparam name="T">The element type of the observer being created.</typeparam>
        /// <returns>
        /// Observer that can be used to assert the timing of received notifications.
        /// </returns>
        public new ITestableObserver<T> CreateObserver<T>()
        {
            return this.CreateObserver<T>(long.MaxValue);
        }

        /// <summary>
        /// Creates an observer that records received notification messages and timestamps those.
        /// </summary>
        /// <param name="absoluteEndTime">
        /// The time at which the scheduler should stop.
        /// </param>
        /// <typeparam name="T">The element type of the observer being created.</typeparam>
        /// <returns>
        /// Observer that can be used to assert the timing of received notifications.
        /// </returns>
        public ITestableObserver<T> CreateObserver<T>(long absoluteEndTime)
        {
            return new MockObserver<T>(this, absoluteEndTime);
        }

        /// <summary>
        /// Schedules an action to be executed at the virtual time specified in <paramref name="dueTime"/>.
        /// </summary>
        /// <typeparam name="TState">
        /// The type of the state passed to the scheduled action.
        /// </typeparam>
        /// <param name="state">
        /// State passed to the action to be executed.
        /// </param>
        /// <param name="dueTime">
        /// Absolute virtual time at which to execute the action.
        /// </param>
        /// <param name="action">
        /// Action to be executed.
        /// </param>
        /// <returns>
        /// Disposable object used to cancel the scheduled action (best effort).
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// Thrown when <paramref name="action"/> is null.
        /// </exception>
        public override IDisposable ScheduleAbsolute<TState>(
            TState state, 
            long dueTime, 
            Func<IScheduler, TState, IDisposable> action)
        {
            lock (this.schedulerQueueLockObject)
            {
// ReSharper disable RedundantTypeArgumentsOfMethod
                var returnValue = base.ScheduleAbsolute<TState>(state, dueTime, action);

// ReSharper restore RedundantTypeArgumentsOfMethod
                return returnValue;
            }
        }

        /// <summary>
        /// Waits for Scheduler events until we are canceled or there are no more events.
        /// </summary>
        public void StartWaiting()
        {
            this.StartWaitingUntil(TimeSpan.MaxValue);
        }

        /// <summary>
        /// Waits for Scheduler events until we are canceled or the <paramref name="absoluteEndTime"/> has been reached.
        /// </summary>
        /// <param name="absoluteEndTime">
        /// The time at which the scheduler should stop.
        /// The Scheduler moves in time as he execute scheduled tasks.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="absoluteEndTime"/> is negative.</exception>
        public void StartWaitingUntil(TimeSpan absoluteEndTime)
        {
            this.StartWaitingUntil(absoluteEndTime.Ticks);
        }

        /// <summary>
        /// Waits for Scheduler events until we are canceled or the <paramref name="absoluteEndTime"/> has been reached.
        /// </summary>
        /// <param name="absoluteEndTime">
        /// The time at which the scheduler should stop.
        /// The Scheduler moves in time as he execute scheduled tasks.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="absoluteEndTime"/> is negative.</exception>
        public void StartWaitingUntil(long absoluteEndTime)
        {
            if (this.Comparer.Compare(absoluteEndTime, 0) < 0)
            {
                throw new ArgumentOutOfRangeException("absoluteEndTime");
            }

            for (;;)
            {
                IScheduledItem<long> next;

                lock (this.schedulerQueueLockObject)
                {
                    next = this.GetNext();
                }

                if (next == null)
                {
                    return;
                }

                if (this.Comparer.Compare(next.DueTime, this.Clock) > 0)
                {
                    this.Clock = next.DueTime;

                    if (this.Clock > absoluteEndTime)
                    {
                        return;
                    }
                }

                next.Invoke();
            }
        }

        #endregion
    }
}