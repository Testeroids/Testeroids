﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestConcurrencyAbstractionLayer.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;
    using System.Reactive.Concurrency;

    using Testeroids.Rx;

    /// <summary>
    /// Test version of the <see cref="IConcurrencyAbstractionLayer"/> service, which relies on virtual time rather than on real timers and <see cref="System.Threading.Thread.Sleep(int)"/>.
    /// It leverages the <see cref="Microsoft.Reactive.Testing.TestScheduler"/> used in a test fixture in order to schedule all events in the same container.
    /// </summary>
    internal class TestConcurrencyAbstractionLayer : IConcurrencyAbstractionLayer
    {
        #region Fields

        /// <summary>
        /// The concurrency abstraction layer instance to use when a <see cref="Microsoft.Reactive.Testing.TestScheduler"/> is not being used.
        /// </summary>
        private readonly IConcurrencyAbstractionLayer defaultConcurrencyAbstractionLayer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestConcurrencyAbstractionLayer"/> class.
        /// </summary>
        /// <param name="defaultConcurrencyAbstractionLayer">
        /// The concurrency abstraction layer instance to use when a <see cref="Microsoft.Reactive.Testing.TestScheduler"/> is not being used.
        /// </param>
        public TestConcurrencyAbstractionLayer(IConcurrencyAbstractionLayer defaultConcurrencyAbstractionLayer)
        {
            this.defaultConcurrencyAbstractionLayer = defaultConcurrencyAbstractionLayer;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether long-running scheduling is supported.
        /// </summary>
        public bool SupportsLongRunning
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the function which can return the <see cref="Microsoft.Reactive.Testing.TestScheduler"/> instance used in the tests.
        /// </summary>
        internal Func<Microsoft.Reactive.Testing.TestScheduler> GetTestScheduler { get; set; }

        /// <summary>
        /// Gets a value indicating whether the default scheduler will be used.
        /// </summary>
        internal bool UseDefaultScheduler
        {
            get
            {
                return this.GetTestScheduler == null;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Queues a method for execution.
        /// </summary>
        /// <param name="action">Method to execute.</param>
        /// <param name="state">State to pass to the method.</param>
        /// <remarks>This implementation schedules the <paramref name="action"/> for the next virtual clock cycle.</remarks>
        /// <returns>
        /// Disposable object that can be used to cancel the queued method.
        /// </returns>
        public IDisposable QueueUserWorkItem(
            Action<object> action, 
            object state)
        {
            if (this.UseDefaultScheduler)
            {
                return this.defaultConcurrencyAbstractionLayer.QueueUserWorkItem(action, state);
            }

            return this.GetTestScheduler().Schedule(() => action(state));
        }

        /// <summary>
        /// Blocking sleep operation.
        /// </summary>
        /// <remarks>This implementation merely advances the virtual clock.</remarks>
        /// <param name="timeout">Time to sleep.</param>
        public void Sleep(TimeSpan timeout)
        {
            if (this.UseDefaultScheduler)
            {
                this.defaultConcurrencyAbstractionLayer.Sleep(timeout);
            }

            this.GetTestScheduler().Sleep(timeout);
        }

        /// <summary>
        /// Queues a method for periodic execution based on the specified period.
        /// </summary>
        /// <param name="action">Method to execute; should be safe for reentrancy.</param><param name="period">Period for running the method periodically.</param>
        /// <returns>
        /// Disposable object that can be used to stop the timer.
        /// </returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        public IDisposable StartPeriodicTimer(
            Action action, 
            TimeSpan period)
        {
            if (this.UseDefaultScheduler)
            {
                return this.defaultConcurrencyAbstractionLayer.StartPeriodicTimer(action, period);
            }

            return this.GetTestScheduler().SchedulePeriodic(period, action);
        }

        /// <summary>
        /// Starts a new stopwatch object.
        /// </summary>
        /// <remarks>This implementation relies on the <see cref="TestScheduler"/> to provide an <see cref="IStopwatch"/> instance.</remarks>
        /// <returns>
        /// New stopwatch object; started at the time of the request.
        /// </returns>
        public IStopwatch StartStopwatch()
        {
            if (this.UseDefaultScheduler)
            {
                return this.defaultConcurrencyAbstractionLayer.StartStopwatch();
            }

            return this.GetTestScheduler().AsStopwatchProvider().StartStopwatch();
        }

        /// <summary>
        /// Starts a new long-running thread.
        /// </summary>
        /// <param name="action">Method to execute.</param><param name="state">State to pass to the method.</param>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        public void StartThread(
            Action<object> action, 
            object state)
        {
            if (this.UseDefaultScheduler)
            {
                this.defaultConcurrencyAbstractionLayer.StartThread(action, state);
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Queues a method for execution at the specified relative time.
        /// </summary>
        /// <remarks>This implementation schedules the <paramref name="action"/> on the <see cref="TestScheduler"/>.</remarks>
        /// <param name="action">Method to execute.</param><param name="state">State to pass to the method.</param><param name="dueTime">Time to execute the method on.</param>
        /// <returns>
        /// Disposable object that can be used to stop the timer.
        /// </returns>
        public IDisposable StartTimer(
            Action<object> action, 
            object state, 
            TimeSpan dueTime)
        {
            if (this.UseDefaultScheduler)
            {
                return this.defaultConcurrencyAbstractionLayer.StartTimer(action, state, dueTime);
            }

            return this.GetTestScheduler().ScheduleRelative(TestConcurrencyAbstractionLayer.Normalize(dueTime).Ticks, () => action(state));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Ensures that a <paramref name="dueTime"/> is not negative.
        /// </summary>
        /// <param name="dueTime">
        /// The time value to be normalized.
        /// </param>
        /// <returns>
        /// The <see cref="TimeSpan"/> value trimmed at zero.
        /// </returns>
        private static TimeSpan Normalize(TimeSpan dueTime)
        {
            if (dueTime < TimeSpan.Zero)
            {
                return TimeSpan.Zero;
            }

            return dueTime;
        }

        #endregion
    }
}