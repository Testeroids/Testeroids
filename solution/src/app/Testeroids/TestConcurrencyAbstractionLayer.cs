// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestConcurrencyAbstractionLayer.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;
    using System.Diagnostics;
    using System.Reactive.Concurrency;

    using Microsoft.Reactive.Testing;

    /// <summary>
    /// Test version of the <see cref="IConcurrencyAbstractionLayer"/> service, which relies on virtual time rather than on real timers and <see cref="System.Threading.Thread.Sleep(int)"/>.
    /// It leverages the <see cref="TestScheduler"/> used in a test fixture in order to schedule all events in the same container.
    /// </summary>
    public class TestConcurrencyAbstractionLayer : IConcurrencyAbstractionLayer
    {
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
        internal Func<TestScheduler> GetTestScheduler { get; set; }

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
            this.PerformSanityCheck();

            return this.GetTestScheduler().Schedule(() => action(state));
        }

        /// <summary>
        /// Blocking sleep operation.
        /// </summary>
        /// <remarks>This implementation merely advances the virtual clock.</remarks>
        /// <param name="timeout">Time to sleep.</param>
        public void Sleep(TimeSpan timeout)
        {
            this.PerformSanityCheck();

            this.GetTestScheduler().Sleep(timeout.Ticks);
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
            this.PerformSanityCheck();

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
            this.PerformSanityCheck();

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
            this.PerformSanityCheck();

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
            this.PerformSanityCheck();

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

        /// <summary>
        /// Perform a sanity check to warn the developer if there is a missing aspect.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when an Rx scheduler is used outside the scope of an Rx test (<see cref="GetTestScheduler"/> is not defined).
        /// </exception>
        private void PerformSanityCheck()
        {
            if (this.UseDefaultScheduler)
            {
                const string Message = "An Rx query was executed even though the test fixture is not marked with [RxContextAspect] or [RxTestSchedulerAspect]. Please apply the correct aspect to the test fixture so that schedulers do not work in a multi-threaded fashion.";

                Debugger.Log(0, "Testeroids", Message);
                Debugger.Break();

                throw new InvalidOperationException(Message);
            }
        }

        #endregion
    }
}