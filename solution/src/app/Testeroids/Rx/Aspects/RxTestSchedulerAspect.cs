namespace Testeroids.Rx.Aspects
{
    using System;
    using System.Reactive.Concurrency;
    using System.Reactive.PlatformServices;
    using System.Reflection;
    using System.Threading;

    using JetBrains.Annotations;

    using RxSchedulers.Switch;

    /// <summary>
    /// Aspect which adds support for Rx testing configuring the <see cref="RxSchedulers.Switch.SchedulerSwitch"/> for test purposes with the <see cref="TestScheduler"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [Serializable]
    public class RxTestSchedulerAspect : TestFixtureSetupAttributeBase
    {
        #region Constructors and Destructors

        public RxTestSchedulerAspect()
        {
            this.Order = 20;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Called by the framework to register the attribute with the context for setup/teardown notifications.
        /// </summary>
        /// <param name="contextSpecification">
        /// The target context specification instance.
        /// </param>
        public override void Register(IContextSpecification contextSpecification)
        {
            contextSpecification.SetupTasks.Add(this.SetupTestScheduler);
            contextSpecification.TeardownTasks.Add(this.TeardownTestScheduler);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the TestScheduler property on the target <paramref name="contextSpecification"/>.
        /// </summary>
        /// <param name="contextSpecification">
        /// The target context specification instance.
        /// </param>
        /// <param name="testScheduler">
        /// The test scheduler to apply to <paramref name="contextSpecification"/>.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when this aspect is applied to a test fixture not defining a TestScheduler property.
        /// </exception>
        private static void SetTestScheduler(IContextSpecification contextSpecification,
                                             [CanBeNull] TestScheduler testScheduler)
        {
            var testSchedulerPropertyInfo = contextSpecification.GetType().GetProperty("TestScheduler", BindingFlags.Instance | BindingFlags.NonPublic);

            if (testSchedulerPropertyInfo == null)
            {
                throw new InvalidOperationException("RxTestSchedulerAspect was applied but the target test fixture does not contain a TestScheduler property.");
            }

            Helper.SetPrivateProperty(contextSpecification, "TestScheduler", testScheduler);
        }

        /// <summary>
        /// Replaces the <see cref="ContextSpecificationBase.PreTestFixtureSetUp"/> method to instantiate the <see cref="TestScheduler"/>
        /// and configure the <see cref="RxSchedulers.Switch.SchedulerSwitch"/>.
        /// </summary>
        private void SetupTestScheduler(IContextSpecification contextSpecification)
        {
            Func<IScheduler> unassignedGuardScheduler = () => { throw new InvalidOperationException("Please assign a scheduler to the respective SchedulerSwitch property. No scheduler is currently assigned."); };

            SchedulerSwitch.GetCurrentThreadScheduler = unassignedGuardScheduler;
            SchedulerSwitch.GetDispatcherScheduler = unassignedGuardScheduler;
            SchedulerSwitch.GetImmediateScheduler = unassignedGuardScheduler;
            SchedulerSwitch.GetNewThreadScheduler = unassignedGuardScheduler;
            SchedulerSwitch.GetTaskPoolScheduler = unassignedGuardScheduler;
            SchedulerSwitch.GetThreadPoolScheduler = unassignedGuardScheduler;

            // Replace the default IConcurrencyAbstractionLayer through a specialized PlatformEnlightenmentProvider, 
            // in order to be able to leverage our TestScheduler to introduce virtual time everywhere.
            var testPlatformEnlightenmentProvider = (TestPlatformEnlightenmentProvider)PlatformEnlightenmentProvider.Current;
            var testScheduler = new ThreadLocal<TestScheduler>(() => new TestScheduler());
            testPlatformEnlightenmentProvider.GetTestScheduler = () => testScheduler.Value;

            SetTestScheduler(contextSpecification, testScheduler.Value);
        }

        /// <summary>
        /// Clear the TestScheduler property and all schedulers in <see cref="SchedulerSwitch"/>.
        /// </summary>
        private void TeardownTestScheduler(IContextSpecification contextSpecification)
        {
            SetTestScheduler(contextSpecification, null);

            SchedulerSwitch.GetCurrentThreadScheduler = null;
            SchedulerSwitch.GetDispatcherScheduler = null;
            SchedulerSwitch.GetImmediateScheduler = null;
            SchedulerSwitch.GetNewThreadScheduler = null;
            SchedulerSwitch.GetTaskPoolScheduler = null;
            SchedulerSwitch.GetThreadPoolScheduler = null;

            var testPlatformEnlightenmentProvider = (TestPlatformEnlightenmentProvider)PlatformEnlightenmentProvider.Current;
            testPlatformEnlightenmentProvider.GetTestScheduler = null;
        }

        #endregion
    }
}