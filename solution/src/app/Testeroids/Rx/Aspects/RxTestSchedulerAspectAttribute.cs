// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Testeroids" file="RxTestSchedulerAspect.cs">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Rx.Aspects
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reactive.PlatformServices;
    using System.Threading;

    using JetBrains.Annotations;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Advices;
    using PostSharp.Extensibility;
    using PostSharp.Reflection;

    using RxSchedulers.Switch;

    /// <summary>
    /// Aspect which adds support for Rx testing configuring the <see cref="RxSchedulers.Switch.SchedulerSwitch"/> for test purposes with the <see cref="TestScheduler"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [Serializable]
    public class RxTestSchedulerAspectAttribute : InstanceLevelAspect
    {
        #region Fields

        /// <summary>
        /// The <see cref="ContextSpecificationBase.BaseTearDown"/> method on the target class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Postsharp needs it to public")]
        [UsedImplicitly]
        [NotNull]
        [ImportMember("BaseTearDown", IsRequired = true, Order = ImportMemberOrder.BeforeIntroductions)]
        public Action BaseTearDownMethod;

        /// <summary>
        /// The <see cref="ContextSpecificationBase.BaseTearDown"/> method on the target class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Postsharp needs it to public")]
        [UsedImplicitly]
        [NotNull]
        [ImportMember("PreTestSetUp", IsRequired = true, Order = ImportMemberOrder.BeforeIntroductions)]
        public Action PreTestSetUpMethod;

        /// <summary>
        /// The <see cref="TestScheduler"/> property on the target class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Postsharp needs it to public")]
        [NotNull]
        [ImportMember("TestScheduler", IsRequired = true, Order = ImportMemberOrder.BeforeIntroductions)]
        [UsedImplicitly]
        public Property<TestScheduler> TestSchedulerProperty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RxTestSchedulerAspectAttribute"/> class.
        /// </summary>
        public RxTestSchedulerAspectAttribute()
        {
            this.AttributeTargetMemberAttributes = MulticastAttributes.Public | MulticastAttributes.Instance;
            this.AttributeTargetElements = MulticastTargets.Class;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Clear the <see cref="TestSchedulerProperty"/> and all schedulers in <see cref="SchedulerSwitch"/>.
        /// </summary>
        [IntroduceMember(IsVirtual = true, OverrideAction = MemberOverrideAction.OverrideOrFail, Visibility = Visibility.Public)]
        public void BaseTearDown()
        {
            this.BaseTearDownMethod();

            this.TestSchedulerProperty.Set(null);

            SchedulerSwitch.GetCurrentThreadScheduler = null;
            SchedulerSwitch.GetDispatcherScheduler = null;
            SchedulerSwitch.GetImmediateScheduler = null;
            SchedulerSwitch.GetNewThreadScheduler = null;
            SchedulerSwitch.GetTaskPoolScheduler = null;
            SchedulerSwitch.GetThreadPoolScheduler = null;

            var testPlatformEnlightenmentProvider = (TestPlatformEnlightenmentProvider)PlatformEnlightenmentProvider.Current;
            testPlatformEnlightenmentProvider.GetTestScheduler = null;
        }

        /// <summary>
        /// Replaces the <see cref="ContextSpecificationBase.PreTestSetUp"/> method to instantiate the <see cref="TestScheduler"/>
        /// and configure the <see cref="RxSchedulers.Switch.SchedulerSwitch"/>.
        /// </summary>
        [IntroduceMember(IsVirtual = true, OverrideAction = MemberOverrideAction.OverrideOrFail, Visibility = Visibility.Family)]
        public void PreTestSetUp()
        {
            var testScheduler = new ThreadLocal<TestScheduler>(() => new TestScheduler());
            SchedulerSwitch.GetCurrentThreadScheduler = () => testScheduler.Value;
            SchedulerSwitch.GetDispatcherScheduler = () => testScheduler.Value;
            SchedulerSwitch.GetImmediateScheduler = () => testScheduler.Value;
            SchedulerSwitch.GetNewThreadScheduler = () => testScheduler.Value;
            SchedulerSwitch.GetTaskPoolScheduler = () => testScheduler.Value;
            SchedulerSwitch.GetThreadPoolScheduler = () => testScheduler.Value;

            // Replace the default IConcurrencyAbstractionLayer through a specialized PlatformEnlightenmentProvider, 
            // in order to be able to leverage our TestScheduler to introduce virtual time everywhere.
            var testPlatformEnlightenmentProvider = (TestPlatformEnlightenmentProvider)PlatformEnlightenmentProvider.Current;
            testPlatformEnlightenmentProvider.GetTestScheduler = () => testScheduler.Value;

            this.TestSchedulerProperty.Set(testScheduler.Value);

            this.PreTestSetUpMethod();
        }

        #endregion
    }
}