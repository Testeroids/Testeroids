namespace Testeroids
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static class TaskExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Helper method waits on tasks and tries to replicate the handling of exceptions that is achieved with the "await"
        ///     keyword (not wrapping it in an AggregateException).
        /// </summary>
        /// <param name="result">The task to wait on.</param>
        /// <param name="contextSpecification">The current test fixture.</param>
        public static void Wait(
            this Task result,
            IContextSpecification contextSpecification)
        {
            if (!TplTestHelper.WillExecuteTplTasksOn(contextSpecification))
            {
                return;
            }

            // Emulate the "await" exception handling behavior
            try
            {
                result.Wait(CancellationToken.None);
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
        }

        #endregion
    }
}