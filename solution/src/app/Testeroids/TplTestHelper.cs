namespace Testeroids
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    /// <summary>
    /// Allows access to the test <see cref="TaskScheduler"/> associated with the current test fixture.
    /// </summary>
    public static class TplTestHelper
    {
        #region Properties

        /// <summary>
        /// Gets the active test task scheduler.
        /// </summary>
        private static TplTestPlatformHelper.TestTaskScheduler TestTaskScheduler
        {
            get
            {
                return (TplTestPlatformHelper.TestTaskScheduler)TaskScheduler.Default;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Test if the given <param name="task">task</param> was executed (completed).
        /// </summary>
        /// <param name="task">The task to test.</param>
        /// <returns><c>true</c> if the task was queued and executed.</returns>
        /// <remarks>
        /// TODO : Improvement possible is to have a fluent interface with something like TaskAssert.WasTaskExecuted().Before(Task) ... (?)
        /// </remarks>
        [PublicAPI]
        public static bool WasTaskExecuted(Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException();
            }

            return task.IsCompleted && TestTaskScheduler.HistoricQueue.Contains(task);
        }

        /// <summary>
        /// Test if the given <paramref name="task">task</paramref> was executed (completed) in the first position of the queue.
        /// </summary>
        /// <param name="task">The task to test.</param>
        /// <returns><c>true</c> if the task was queued in the first position and executed.</returns>
        [PublicAPI]
        public static bool WasTaskExecutedFirst(Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException();
            }

            return task.IsCompleted && TestTaskScheduler.HistoricQueue.FirstOrDefault() == task;
        }

        /// <summary>
        /// Test if the first task <param name="taskBefore">task</param> was executed (completed) before the second task.
        /// </summary>
        /// <param name="taskBefore">The task which should have been executed first.</param>
        /// <param name="taskAfter">The task which should have been executed after <paramref name="taskBefore" /> .</param>
        /// <returns><c>true</c> if <paramref name="taskBefore"/> was queued and executed before <paramref name="taskAfter"/>.</returns>
        [PublicAPI]
        public static bool WasTaskExecutedFirstComparedTo(
            Task taskBefore,
            Task taskAfter)
        {
            if (!WasTaskExecuted(taskBefore))
            {
                return false;
            }

            if (!WasTaskExecuted(taskAfter))
            {
                return true;
            }

            return TestTaskScheduler.HistoricQueue.IndexOf(taskBefore) < TestTaskScheduler.HistoricQueue.IndexOf(taskAfter);
        }

        /// <summary>
        /// Determines whether TPL tasks will be run on a given <see cref="IContextSpecification"/>. The <paramref name="contextSpecification"/> instance needs to have the <see cref="TplContextAspectAttribute"/> aspect applied to it.
        /// </summary>
        /// <param name="contextSpecification">
        /// The context specification to inspect.
        /// </param>
        /// <returns>
        /// The value defined in <see cref="TplContextAspectAttribute.ExecuteTplTasks"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">The <paramref name="contextSpecification"/> instance does not have <see cref="TplContextAspectAttribute"/> applied to it.</exception>
        public static bool WillExecuteTplTasksOn(IContextSpecification contextSpecification)
        {
            var tplContextAspectAttribute =
                contextSpecification.GetType()
                                    .GetCustomAttributes(typeof(TplContextAspectAttribute), true)
                                    .Cast<TplContextAspectAttribute>()
                                    .Single();

            return tplContextAspectAttribute.ExecuteTplTasks;
        }

        #endregion
    }
}