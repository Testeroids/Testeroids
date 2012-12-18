// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TplContextSpecification.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Enhances the <see cref="ContextSpecification{TSubjectUnderTest}"/> to add support for TPL testing,
    /// namely by providing a serial task scheduler which records the order of executed tasks.
    /// </summary>
    /// <typeparam name="TSubjectUnderTest">The type of the subject under test.</typeparam>
    public abstract class TplContextSpecification<TSubjectUnderTest> : ContextSpecification<TSubjectUnderTest>
        where TSubjectUnderTest : class
    {
        #region Fields

        /// <summary>
        /// The scheduler which will be responsible for recording the order of the queuing of the tasks.
        /// </summary>
        private CurrentThreadTaskScheduler testTaskScheduler;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether queued tasks will be executes.
        /// Setting this property to <c>false</c> will prevent any queued task from starting.
        /// </summary>
        /// <remarks>
        /// This could be extended in the future to be a predicate which allows more fine-grained control over the tasks which should not be started.
        /// </remarks>
        private bool ExecuteTplTasks { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///   The arrange part. Enhances the base class by adding a custom test scheduler.
        /// </summary>
        protected override void EstablishContext()
        {
            base.EstablishContext();

            this.ExecuteTplTasks = this.EstablishExecuteTplTasks();

            this.testTaskScheduler = new CurrentThreadTaskScheduler(this.ExecuteTplTasks);

            SetDefaultScheduler(this.testTaskScheduler);
        }

        /// <summary>
        /// Establishes the behavior of the <see cref="TaskScheduler"/> in which tested tasks are run.
        /// </summary>
        /// <returns>Returning <c>false</c> will prevent any queued task from starting and will allow testing the impact on execution
        /// of subsequent tasks (e.g. <see cref="Task.ContinueWith(System.Action{System.Threading.Tasks.Task})"/>, <see cref="Task.Wait()"/>).
        /// It will fail any non-asynchrony related tests.
        /// </returns>
        protected abstract bool EstablishExecuteTplTasks();

        /// <summary>
        /// Test if the given <param name="task">task</param> was executed (completed).
        /// </summary>
        /// <param name="task">The task to test.</param>
        /// <returns><c>true</c> if the task was queued and executed.</returns>
        /// <remarks>
        /// TODO : Improvement possible is to have a fluent interface with something like TaskAssert.WasTaskExecuted().Before(Task) ... (?)
        /// </remarks>
        protected bool WasTaskExecuted(Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException();
            }

            return task.IsCompleted && this.testTaskScheduler.HistoricQueue.Contains(task);
        }

        /// <summary>
        /// Test if the given <paramref name="task">task</paramref> was executed (completed) in the first position of the queue.
        /// </summary>
        /// <param name="task">The task to test.</param>
        /// <returns><c>true</c> if the task was queued in the first position and executed.</returns>
        protected bool WasTaskExecutedFirst(Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException();
            }

            return task.IsCompleted && this.testTaskScheduler.HistoricQueue.FirstOrDefault() == task;
        }

        /// <summary>
        /// Test if the first task <param name="taskBefore">task</param> was executed (completed) before the second task.
        /// </summary>
        /// <param name="taskBefore">The task which should have been executed first.</param>
        /// <param name="taskAfter">The task which should have been executed after <paramref name="taskBefore" /> .</param>
        /// <returns><c>true</c> if <paramref name="taskBefore"/> was queued and executed before <paramref name="taskAfter"/>.</returns>
        protected bool WasTaskExecutedFirstComparedTo(
            Task taskBefore,
            Task taskAfter)
        {
            if (!this.WasTaskExecuted(taskBefore))
            {
                return false;
            }

            if (!this.WasTaskExecuted(taskAfter))
            {
                return true;
            }

            return this.testTaskScheduler.HistoricQueue.IndexOf(taskBefore) < this.testTaskScheduler.HistoricQueue.IndexOf(taskAfter);
        }

        /// <summary>
        /// Uses reflection to set the default scheduler to use for any newly started task.
        /// </summary>
        /// <param name="taskScheduler">The <see cref="TaskScheduler"/> to use by default.</param>
        private static void SetDefaultScheduler(TaskScheduler taskScheduler)
        {
            var taskSchedulerType = typeof(TaskScheduler);
            var defaultTaskSchedulerField = taskSchedulerType.GetField("s_defaultTaskScheduler", BindingFlags.SetField | BindingFlags.Static | BindingFlags.NonPublic);
            Debug.Assert(defaultTaskSchedulerField != null, "Could not find the TaskScheduler.s_defaultTaskScheduler field. We are assuming this implementation aspect of the .NET Framework to be able to unit test TPL.");
            defaultTaskSchedulerField.SetValue(null, taskScheduler);
        }

        #endregion

        /// <summary>
        /// Provides a task scheduler that runs tasks only on the current thread, and records the order in which <see cref="Task"/>s were queued using <see cref="QueueTask"/>.
        /// </summary>
        private sealed class CurrentThreadTaskScheduler : TaskScheduler
        {
            #region Fields

            /// <summary>
            /// Setting this field to <c>false</c> will prevent any queued task from starting.
            /// </summary>
            private readonly bool executeTplTasks;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="CurrentThreadTaskScheduler"/> class. 
            /// </summary>
            /// <param name="executeTplTasks">
            /// Setting this field to <c>false</c> will prevent any queued task from starting.
            /// </param>
            public CurrentThreadTaskScheduler(bool executeTplTasks)
            {
                this.HistoricQueue = new List<Task>();
                this.executeTplTasks = executeTplTasks;
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets the maximum degree of parallelism for this scheduler.
            /// </summary>
            /// <returns>Always 1, in order to prevent concurrency.</returns>
            public override int MaximumConcurrencyLevel
            {
                get
                {
                    return 1;
                }
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets a list representing the queue of the <see cref="Task"/>s.
            /// </summary>
            internal IList<Task> HistoricQueue { get; private set; }

            #endregion

            #region Methods

            /// <summary>Gets the Tasks currently scheduled to this scheduler.</summary>
            /// <returns>An empty enumerable, as Tasks are never queued, only executed.</returns>
            protected override IEnumerable<Task> GetScheduledTasks()
            {
                return Enumerable.Empty<Task>();
            }

            /// <summary>Runs the provided Task synchronously on the current thread.</summary>
            /// <param name="task">The task to be executed.</param>
            protected override void QueueTask(Task task)
            {
                if (this.executeTplTasks)
                {
                    this.TryExecuteTask(task);
                }

                this.HistoricQueue.Add(task);
            }

            /// <summary>Runs the provided Task synchronously on the current thread.</summary>
            /// <param name="task">The task to be executed.</param>
            /// <param name="taskWasPreviouslyQueued">Whether the Task was previously queued to the scheduler.</param>
            /// <returns>True if the Task was successfully executed; otherwise, false.</returns>
            protected override bool TryExecuteTaskInline(
                Task task,
                bool taskWasPreviouslyQueued)
            {
                throw new NotImplementedException("We didn't know we could get there ;)");

//// var success = !this.executeTplTasks || this.TryExecuteTask(task);
                //// this.HistoricQueue.Add(task);
                //// return success;
            }

            #endregion
        }
    }
}