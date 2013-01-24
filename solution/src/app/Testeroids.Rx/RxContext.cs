// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RxContext.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Rx
{
    using System;
    using System.Reactive.Concurrency;

    using Microsoft.Reactive.Testing;

    using RxSchedulers.Switch;

    /// <summary>
    /// Implements functionality which is useful for a <see cref="IContextSpecification"/> implementation in what relates to Reactive Extensions,
    /// namely configuring the <see cref="SchedulerSwitch"/> for test purposes, and lazy instantiation of <see cref="TestScheduler"/>s.
    /// </summary>
    [Serializable]
    public class RxContext
    {
        #region Fields

        /// <summary>
        /// The cache of <see cref="TestScheduler"/>s for the current test.
        /// </summary>
        private readonly TestScheduler[] testSchedulers = new TestScheduler[6];

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RxContext"/> class.
        /// </summary>
        public RxContext()
        {
            var immediateScheduler = System.Reactive.Concurrency.ImmediateScheduler.Instance;

            SchedulerSwitch.GetCurrentThreadScheduler = () => immediateScheduler;
            SchedulerSwitch.GetDispatcherScheduler = () => immediateScheduler;
            SchedulerSwitch.GetImmediateScheduler = () => immediateScheduler;
            SchedulerSwitch.GetNewThreadScheduler = () => immediateScheduler;
            SchedulerSwitch.GetTaskPoolScheduler = () => immediateScheduler;
            SchedulerSwitch.GetThreadPoolScheduler = () => immediateScheduler;
        }

        #endregion

        #region Enums

        /// <summary>
        /// The scheduler type.
        /// </summary>
        private enum SchedulerType
        {
            /// <summary>
            /// Used as an internal reference for the <see cref="System.Reactive.Concurrency.CurrentThreadScheduler"/> in the <see cref="testSchedulers"/> array.
            /// </summary>
            CurrentThread, 

            /// <summary>
            /// Used as an internal reference for the <see cref="System.Reactive.Concurrency.DispatcherScheduler"/> in the <see cref="testSchedulers"/> array.
            /// </summary>
            Dispatcher, 

            /// <summary>
            /// Used as an internal reference for the <see cref="System.Reactive.Concurrency.ImmediateScheduler"/> in the <see cref="testSchedulers"/> array.
            /// </summary>
            Immediate, 

            /// <summary>
            /// Used as an internal reference for the <see cref="System.Reactive.Concurrency.NewThreadScheduler"/> in the <see cref="testSchedulers"/> array.
            /// </summary>
            NewThread, 

            /// <summary>
            /// Used as an internal reference for the <see cref="System.Reactive.Concurrency.TaskPoolScheduler"/> in the <see cref="testSchedulers"/> array.
            /// </summary>
            TaskPool, 

            /// <summary>
            /// Used as an internal reference for the <see cref="ThreadPoolScheduler"/> in the <see cref="testSchedulers"/> array.
            /// </summary>
            ThreadPool
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="TestScheduler"/> that will act as a stand-in for <see cref="CurrentThreadScheduler"/>.
        /// </summary>
        public TestScheduler CurrentThreadScheduler
        {
            get { return this.RetrieveTestScheduler(RxContext.SchedulerType.CurrentThread); }
        }

        /// <summary>
        /// Gets the <see cref="TestScheduler"/> that will act as a stand-in for <see cref="DispatcherScheduler"/>.
        /// </summary>
        public TestScheduler DispatcherScheduler
        {
            get { return this.RetrieveTestScheduler(RxContext.SchedulerType.Dispatcher); }
        }

        /// <summary>
        /// Gets the <see cref="TestScheduler"/> that will act as a stand-in for <see cref="ImmediateScheduler"/>.
        /// </summary>
        public TestScheduler ImmediateScheduler
        {
            get { return this.RetrieveTestScheduler(RxContext.SchedulerType.Immediate); }
        }

        /// <summary>
        /// Gets the <see cref="TestScheduler"/> that will act as a stand-in for <see cref="NewThreadScheduler"/>.
        /// </summary>
        public TestScheduler NewThreadScheduler
        {
            get { return this.RetrieveTestScheduler(RxContext.SchedulerType.NewThread); }
        }

        /// <summary>
        /// Gets the <see cref="TestScheduler"/> that will act as a stand-in for <see cref="TaskPoolScheduler"/>.
        /// </summary>
        public TestScheduler TaskPoolScheduler
        {
            get { return this.RetrieveTestScheduler(RxContext.SchedulerType.TaskPool); }
        }

        /// <summary>
        /// Gets the <see cref="TestScheduler"/> that will act as a stand-in for <see cref="ThreadPoolScheduler"/>.
        /// </summary>
        public TestScheduler ThreadPoolScheduler
        {
            get { return this.RetrieveTestScheduler(RxContext.SchedulerType.ThreadPool); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves a given <see cref="TestScheduler"/> (and instantiates it, if required).
        /// </summary>
        /// <param name="type">
        /// The type of the test scheduler.
        /// </param>
        /// <returns>
        /// An instance of the <see cref="TestScheduler"/> reserved for the usage as a scheduler of a <param name="type">specified type</param>.
        /// </returns>
        private TestScheduler RetrieveTestScheduler(SchedulerType type)
        {
            var index = (int)type;
            var retrieveTestScheduler = this.testSchedulers[index] ?? (this.testSchedulers[index] = new TestScheduler());
            return retrieveTestScheduler;
        }

        #endregion
    }
}