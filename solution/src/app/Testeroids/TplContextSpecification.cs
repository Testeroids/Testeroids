// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TplContextSpecification.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    /// <summary>
    /// Enhances the <see cref="ContextSpecification{TSubjectUnderTest}"/> to add support for TPL testing,
    /// namely by providing a serial task scheduler which records the order of executed tasks.
    /// </summary>
    /// <typeparam name="TSubjectUnderTest">The type of the subject under test.</typeparam>
    [Obsolete("This class is no longer supported. Please use ContextSpecification<> instead, and apply the TplContextAspectAttribute to the test fixture.")]
    public abstract class TplContextSpecification<TSubjectUnderTest> : ContextSpecification<TSubjectUnderTest>
        where TSubjectUnderTest : class
    {
        #region Fields

        /// <summary>
        /// The scheduler which will be responsible for recording the order of the queuing of the tasks.
        /// </summary>
        private TplTestPlatformHelper.TestTaskScheduler testTaskScheduler;

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

            this.testTaskScheduler = new TplTestPlatformHelper.TestTaskScheduler(this.ExecuteTplTasks);

            TplTestPlatformHelper.SetDefaultScheduler(this.testTaskScheduler);
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
        [PublicAPI]
        protected bool WasTaskExecuted(Task task)
        {
            return TplTestHelper.WasTaskExecuted(task);
        }

        /// <summary>
        /// Test if the given <paramref name="task">task</paramref> was executed (completed) in the first position of the queue.
        /// </summary>
        /// <param name="task">The task to test.</param>
        /// <returns><c>true</c> if the task was queued in the first position and executed.</returns>
        [PublicAPI]
        protected bool WasTaskExecutedFirst(Task task)
        {
            return TplTestHelper.WasTaskExecutedFirst(task);
        }

        /// <summary>
        /// Test if the first task <param name="taskBefore">task</param> was executed (completed) before the second task.
        /// </summary>
        /// <param name="taskBefore">The task which should have been executed first.</param>
        /// <param name="taskAfter">The task which should have been executed after <paramref name="taskBefore" /> .</param>
        /// <returns><c>true</c> if <paramref name="taskBefore"/> was queued and executed before <paramref name="taskAfter"/>.</returns>
        [PublicAPI]
        protected bool WasTaskExecutedFirstComparedTo(
            Task taskBefore, 
            Task taskAfter)
        {
            return TplTestHelper.WasTaskExecutedFirstComparedTo(taskBefore, taskAfter);
        }

        #endregion
    }
}