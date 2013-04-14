// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TplTestPlatformHelper.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
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
    /// Contains test implementations for TPL schedulers.
    /// </summary>
    internal static class TplTestPlatformHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// Uses reflection to set the default scheduler to use for any newly started task.
        /// </summary>
        /// <param name="taskScheduler">The <see cref="TaskScheduler"/> to use by default.</param>
        public static void SetDefaultScheduler(TaskScheduler taskScheduler)
        {
            var taskSchedulerType = typeof(TaskScheduler);
            var defaultTaskSchedulerField = taskSchedulerType.GetField("s_defaultTaskScheduler", BindingFlags.SetField | BindingFlags.Static | BindingFlags.NonPublic);
            Debug.Assert(defaultTaskSchedulerField != null, "Could not find the TaskScheduler.s_defaultTaskScheduler field. We are assuming this implementation aspect of the .NET Framework to be able to unit test TPL.");
            defaultTaskSchedulerField.SetValue(null, taskScheduler);
        }

        #endregion

        /// <summary>
        /// Provides a task scheduler which informs the developer that the tests are not running in a TPL-enabled test fixture.
        /// </summary>
        internal sealed class InvalidTaskScheduler : TaskScheduler
        {
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
                throw new InvalidOperationException(string.Format("A TPL task was queued even though the test fixture does not have the {0} applied. Please apply the aspect to the test fixture.", typeof(TplContextAspectAttribute).Name));
            }

            /// <summary>Runs the provided Task synchronously on the current thread.</summary>
            /// <param name="task">The task to be executed.</param>
            /// <param name="taskWasPreviouslyQueued">Whether the Task was previously queued to the scheduler.</param>
            /// <returns>True if the Task was successfully executed; otherwise, false.</returns>
            protected override bool TryExecuteTaskInline(
                Task task, 
                bool taskWasPreviouslyQueued)
            {
                this.QueueTask(task);

                return true;
            }

            #endregion
        }

        /// <summary>
        /// Provides a task scheduler that runs tasks only on the current thread, and records the order in which <see cref="Task"/>s were queued using <see cref="QueueTask"/>.
        /// </summary>
        internal sealed class TestTaskScheduler : TaskScheduler
        {
            #region Fields

            /// <summary>
            /// Setting this field to <c>false</c> will prevent any queued task from starting.
            /// </summary>
            private readonly bool executeTplTasks;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="TestTaskScheduler"/> class. 
            /// </summary>
            /// <param name="executeTplTasks">
            /// Setting this field to <c>false</c> will prevent any queued task from starting.
            /// </param>
            public TestTaskScheduler(bool executeTplTasks)
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
                if (taskWasPreviouslyQueued)
                {
                    throw new NotImplementedException("We didn't know we could get there ;)");
                }

                this.QueueTask(task);

                return true;
            }

            #endregion
        }

        public static TaskScheduler GetDefaultScheduler()
        {
            var taskSchedulerType = typeof(TaskScheduler);
            var defaultTaskSchedulerField = taskSchedulerType.GetField("s_defaultTaskScheduler", BindingFlags.GetField | BindingFlags.Static | BindingFlags.NonPublic);
            Debug.Assert(defaultTaskSchedulerField != null, "Could not find the TaskScheduler.s_defaultTaskScheduler field. We are assuming this implementation aspect of the .NET Framework to be able to unit test TPL.");
            return (TaskScheduler)defaultTaskSchedulerField.GetValue(null);
        }

        public static bool IsFaultedTaskHandled(Task task)
        {
            // task.m_contingentProperties.m_exceptionsHolder.m_isHandled
            // HACK: we should be able to dramatically improve performances: When finalized, TaskExceptionHolder (a private member down the chain of a Task) throws the static event TaskScheduler.UnobservedTaskException. Unfortunately, for some reason I could not get this event to get fired. therefore, I had to resort to reflection in order to fail only unobserved tasks. :(
            // Note : this resource helped. Read the comments around m_contingentProperties and AddException(): http://www.dotnetframework.org/default.aspx/4@0/4@0/untmp/DEVDIV_TFS/Dev10/Releases/RTMRel/ndp/clr/src/BCL/System/Threading/Tasks/Task@cs/1305376/Task@cs
            var contingentProperties = task.GetType().GetField("m_contingentProperties", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(task);
            bool isHandled = false;
            if (contingentProperties != null)
            {
                var contingentPropertiesType = contingentProperties.GetType();
                // in .net 4.0, the m_exceptionsHolder field is public and requires the BindingFlags.Public flag ! (public volatile TaskExceptionHolder m_exceptionsHolder;) but not on .net 4.5 (internal volatile TaskExceptionHolder m_exceptionsHolder;)
                var exceptionsHolderFieldInfo = contingentPropertiesType.GetType().GetField("m_exceptionsHolder", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (exceptionsHolderFieldInfo != null)
                {
                    var exceptionsHolder = exceptionsHolderFieldInfo.GetValue(contingentProperties);
                    if (exceptionsHolder != null)
                    {
                        isHandled = (bool)exceptionsHolder.GetType().GetField("m_isHandled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(exceptionsHolder);
                    }
                }
            }
            return isHandled;
        }
    }
}