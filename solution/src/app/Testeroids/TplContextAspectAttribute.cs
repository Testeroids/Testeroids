// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TplContextAspectAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;

    using PostSharp.Aspects;

    /// <summary>
    /// Aspect which adds support for TPL testing.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TplContextAspectAttribute : InstanceLevelAspect
    {
        #region Fields

        /// <summary>
        /// The scheduler which will be responsible for recording the order of the queuing of the tasks.
        /// </summary>
        private TplTestPlatformHelper.TestTaskScheduler testTaskScheduler;

        #endregion

        /// <summary>
        /// Initializes the aspect instance. This method is invoked when all system elements of the aspect (like member imports)
        ///               have completed.
        /// </summary>
        public override void RuntimeInitializeInstance()
        {
            base.RuntimeInitializeInstance();

            this.testTaskScheduler = new TplTestPlatformHelper.TestTaskScheduler(this.ExecuteTplTasks);

            TplTestPlatformHelper.SetDefaultScheduler(this.testTaskScheduler);
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether queued tasks will be executes.
        /// Setting this property to <c>false</c> will prevent any queued task from starting.
        /// </summary>
        /// <remarks>
        /// This could be extended in the future to be a predicate which allows more fine-grained control over the tasks which should not be started.
        /// </remarks>
        public bool ExecuteTplTasks { get; set; }

        #endregion
    }
}