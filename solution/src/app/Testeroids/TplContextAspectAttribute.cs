// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TplContextAspectAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Advices;
    using PostSharp.Extensibility;
    using PostSharp.Reflection;

    /// <summary>
    /// Aspect which adds support for TPL testing.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [MulticastAttributeUsage(MulticastTargets.Class, Inheritance = MulticastInheritance.Multicast, PersistMetaData = true)]
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

        /// <summary>
        /// The <see cref="ContextSpecificationBase.OnExitingBecause"/> method on the target class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Postsharp needs it to public")]
        [UsedImplicitly]
        [NotNull]
        [ImportMember("OnExitingBecause", IsRequired = true, Order = ImportMemberOrder.BeforeIntroductions)]
        public Action OnExitingBecauseMethod;      

        /// <summary>
        /// The <see cref="ContextSpecificationBase.BaseSetUp"/> method on the target class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Postsharp needs it to public")]
        [UsedImplicitly]
        [NotNull]
        [ImportMember("BaseSetUp", IsRequired = true, Order = ImportMemberOrder.BeforeIntroductions)]
        public Action BaseSetUpMethod;        

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

        /// <remarks>FIXME: Right now, this method doesn't do anything : it is supposed to handle the <see cref="TplTestPlatformHelper.TestTaskScheduler.UnobservedTaskException"/> event, but it never gets raised. We could obviously get rid of it and speed up PostSharp weaving process, but better even would be to fix it so this code actually works and the event handler gets called whenever a task is being disposed/finalized and its exceptions have not been observed !</remarks>
        private void TestTaskSchedulerOnUnobservedTaskException(object sender,
                                                                UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            throw new NotImplementedException("If we get there, it's good, that means we can fix a hack in the BaseTearDown method !!!");
        }

        /// <summary>
        /// Replaces the <see cref="ContextSpecificationBase.PreTestSetUp"/> method to set the <see cref="TplTestPlatformHelper.TestTaskScheduler"/> as the default scheduler.
        /// </summary>
        /// <remarks>FIXME: Right now, this method doesn't do anything : it is supposed to register to the <see cref="TplTestPlatformHelper.TestTaskScheduler.UnobservedTaskException"/> event, but it never gets raised. We could obviously get rid of it and speed up PostSharp weaving process, but better even would be to fix it so this code actually works and the event handler gets called whenever a task is being disposed/finalized and its exceptions have not been observed !</remarks>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, IsVirtual = true, Visibility = Visibility.Public)]
        public void BaseSetUp()
        {
            this.BaseSetUpMethod();

            // FIXME: beware : we never unregister from the event here! it might lead to a memory leak, but I wanted to make sure it is registered explicitly for THIS test (making sure it was loaded in the correct AppDomain)
            TplTestPlatformHelper.TestTaskScheduler.UnobservedTaskException += TestTaskSchedulerOnUnobservedTaskException;

        }

        /// <summary>
        /// Replaces the <see cref="ContextSpecificationBase.PreTestSetUp"/> method to set the <see cref="TplTestPlatformHelper.TestTaskScheduler"/> as the default scheduler.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, IsVirtual = true, Visibility = Visibility.Public)]
        public void OnExitingBecause()
        {
            this.OnExitingBecauseMethod();

            var testTaskScheduler = (TplTestPlatformHelper.TestTaskScheduler)TplTestPlatformHelper.GetDefaultScheduler();
            foreach (var task in testTaskScheduler.HistoricQueue)
            {
                // task.m_contingentProperties.m_exceptionsHolder.m_isHandled
                // HACK: we should be able to dramatically improve performances: When finalized, TaskExceptionHolder (a private member down the chain of a Task) throws the static event TaskScheduler.UnobservedTaskException. Unfortunately, for some reason I could not get this event to get fired. therefore, I had to resort to reflection in order to fail only unobserved tasks. :(
                var contingentProperties = task.GetType().GetField("m_contingentProperties", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(task);
                if (contingentProperties != null)
                {
                    // throw new Exception("contingentProperties.ToString() =" + contingentProperties.ToString());
                    var contingentPropertiesType = contingentProperties.GetType();
                    // throw new Exception("contingentPropertiesType.Name =" + contingentPropertiesType.Name);
                    var exceptionsHolderFieldInfo = contingentPropertiesType.GetField("m_exceptionsHolder", BindingFlags.Instance | BindingFlags.NonPublic);
                    throw new Exception("exceptionsHolderFieldInfo.Name =" + exceptionsHolderFieldInfo.ToString());
                    if (exceptionsHolderFieldInfo != null)
                    {
                        throw new Exception("exceptionsHolderFieldInfo.Name =" + exceptionsHolderFieldInfo.Name);
                        var exceptionsHolder = exceptionsHolderFieldInfo.GetValue(contingentProperties);
                        if (exceptionsHolder != null)
                        {
                            throw new Exception("exceptionsHolder.ToString() =" + exceptionsHolder.ToString());
                            var isHandled = (bool)exceptionsHolder.GetType().GetField("m_isHandled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(exceptionsHolder);
                            throw new Exception("isHandled =" + isHandled.ToString());

                            if (!isHandled)
                            {
                                // we'll throw the exceptions one after the other. therefore, we won't have aggregate exceptions, but only the internal ones.
                                task.Exception.Handle(exception =>
                                    {
                                        ((IContextSpecification)this.Instance).UnhandledExceptions.Add(exception);
                                        return true;
                                    });
                            }
                        }
                    }
                }              
            }
        }

        #endregion
    }
}