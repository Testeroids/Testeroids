namespace Testeroids
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using JetBrains.Annotations;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Advices;
    using PostSharp.Reflection;

    /// <summary>
    /// Aspect which adds support for TPL testing.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TplContextAspectAttribute : InstanceLevelAspect
    {
        #region Fields

        /// <summary>
        /// The <see cref="ContextSpecificationBase.BaseTestFixtureSetUp"/> method on the target class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Postsharp needs it to public")]
        [UsedImplicitly]
        [NotNull]
        [ImportMember("BaseTestFixtureSetUp", IsRequired = true, Order = ImportMemberOrder.BeforeIntroductions)]
        public Action BaseTestFixtureSetUpMethod;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TplContextAspectAttribute"/> class.
        /// </summary>
        public TplContextAspectAttribute()
        {
            this.ExecuteTplTasks = true;
        }

        #endregion

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

        #region Public Methods and Operators

        /// <summary>
        /// Replaces the <see cref="ContextSpecificationBase.PreTestSetUp"/> method to set the <see cref="TplTestPlatformHelper.TestTaskScheduler"/> as the default scheduler.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, IsVirtual = true, Visibility = Visibility.Public)]
        public void BaseTestFixtureSetUp()
        {
            this.BaseTestFixtureSetUpMethod();

            var testTaskScheduler = new TplTestPlatformHelper.TestTaskScheduler(this.ExecuteTplTasks);

            TplTestPlatformHelper.SetDefaultScheduler(testTaskScheduler);
        }

        #endregion
    }
}