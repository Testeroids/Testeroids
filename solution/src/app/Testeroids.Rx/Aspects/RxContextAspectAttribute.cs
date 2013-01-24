// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RxContextAspectAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Rx.Aspects
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using JetBrains.Annotations;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Advices;
    using PostSharp.Extensibility;
    using PostSharp.Reflection;

    /// <summary>
    /// Aspect which adds support for Rx testing.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RxContextAspectAttribute : InstanceLevelAspect
    {
        #region Fields

        /// <summary>
        /// The <see cref="TargetRxContext"/> property on the target class.
        /// </summary>
        [NotNull]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", 
            Justification = "Reviewed. PostSharp requires this to be public.")]
        [ImportMember("RxContext", IsRequired = false, Order = ImportMemberOrder.AfterIntroductions)]
        [UsedImplicitly]
        public Property<RxContext> TargetRxContext;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RxContextAspectAttribute"/> class.
        /// </summary>
        public RxContextAspectAttribute()
        {
            this.AttributeTargetMemberAttributes = MulticastAttributes.Public | MulticastAttributes.Instance;
            this.AttributeTargetElements = MulticastTargets.Class;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="RxContext"/> for the target test fixture. This is only introduced in the case where the target type does not define it.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.Ignore, Visibility = Visibility.Family)]
        public RxContext RxContext { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Replaces the <see cref="ContextSpecificationBase.PreTestSetUp"/> method to instantiate the <see cref="RxContext"/>.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, IsVirtual = true, Visibility = Visibility.Family)]
        public void PreTestSetUp()
        {
            this.TargetRxContext.Set(new RxContext());
        }

        #endregion
    }
}