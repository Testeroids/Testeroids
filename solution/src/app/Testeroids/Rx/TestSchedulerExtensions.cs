// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSchedulerExtensions.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Rx
{
    using System;
    using System.Reactive.Concurrency;

    using JetBrains.Annotations;

    using Microsoft.Reactive.Testing;

    /// <summary>
    /// Defines extensions which facilitate the usage of <see cref="TestScheduler"/>.
    /// </summary>
    public static class TestSchedulerExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates the <see cref="IObservable{T}"/> to consume and subscribe to it.
        /// Waits for the <see cref="VirtualTimeSchedulerBase{TAbsolute,TRelative}.Clock"/> to reach the end time or the scheduler to be canceled.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="scheduler">The target scheduler.</param>
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
        public static ITestableObserver<T> Consume<T>(
            [NotNull] this TestScheduler scheduler, 
            Func<IObservable<T>> create, 
            TimeSpan absoluteEndTime)
        {
            return scheduler.Consume(create, absoluteEndTime.Ticks);
        }

        /// <summary>
        /// Schedules an action to be executed at dueTime.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param><param name="action">Action to be executed.</param>
        /// <returns>
        /// The disposable object used to cancel the scheduled action (best effort).
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is null.</exception>
        public static IDisposable ScheduleRelative(
            [NotNull] this VirtualTimeScheduler<long, long> scheduler, 
            TimeSpan dueTime, 
            Action action)
        {
            return scheduler.ScheduleRelative(dueTime.Ticks, action);
        }

        /// <summary>
        /// Advances the scheduler's clock by the specified relative time.
        /// </summary>
        /// <param name="scheduler">The target scheduler.</param>
        /// <param name="time">Relative time to advance the scheduler's clock by.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="time"/> is negative.</exception>
        public static void Sleep(
            [NotNull] this VirtualTimeScheduler<long, long> scheduler, 
            TimeSpan time)
        {
            scheduler.Sleep(time.Ticks);
        }

        /// <summary>
        /// Waits for Scheduler events until we are canceled or the <paramref name="absoluteEndTime"/> has been reached.
        /// </summary>
        /// <param name="scheduler">The target scheduler.</param>
        /// <param name="absoluteEndTime">
        /// The time at which the scheduler should stop.
        /// The Scheduler moves in time as he execute scheduled tasks.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="absoluteEndTime"/> is negative.</exception>
        public static void StartWaitingUntil(
            [NotNull] this TestScheduler scheduler, 
            TimeSpan absoluteEndTime)
        {
            scheduler.StartWaitingUntil(absoluteEndTime.Ticks);
        }

        #endregion
    }
}