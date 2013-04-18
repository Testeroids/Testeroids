// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProhibitSetOnInitializedPropertyAspectAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Aspects
{
    using System;
    using System.Collections.Generic;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Advices;
    using PostSharp.Extensibility;
    using PostSharp.Reflection;

    /// <summary>
    /// Aspect which is applied to the <see cref="ContextSpecificationBase"/> type to verify that no Get method is accessed before
    /// the Set method has been called. This is to make sure that injected values are always established before they are used in a
    /// child test class.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [MulticastAttributeUsage(MulticastTargets.Class, 
        TargetTypeAttributes = MulticastAttributes.AnyScope | MulticastAttributes.AnyVisibility | MulticastAttributes.NonAbstract | MulticastAttributes.Managed, 
        AllowMultiple = false, Inheritance = MulticastInheritance.Multicast)]
    public class ProhibitSetOnInitializedPropertyAspectAttribute : InstanceLevelAspect
    {
        #region Public Properties

        /// <summary>
        /// Gets the list containing the names of the properties where the Set method has been accessed
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.Ignore, Visibility = Visibility.Family)]
        public List<string> PropertySetList { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Method invoked <i>instead</i> of the <c>Set</c> semantic of the field or property to which the current aspect is applied,
        ///               i.e. when the value of this field or property is changed.
        /// If the property's set method has already been called the method will throw an <see cref="PropertyAlreadyInitializedException"/>.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        /// <exception cref="PropertyNotInitializedException">
        /// When the property's set method has already been called before.
        /// </exception>
        [OnLocationSetValueAdvice]
        [MulticastPointcut(Targets = MulticastTargets.Property, Attributes = MulticastAttributes.AnyVisibility | MulticastAttributes.Instance)]
        public void OnPropertySet(LocationInterceptionArgs args)
        {
            if (args.Location.PropertyInfo.GetSetMethod(true) != null &&
                !this.PropertySetList.Contains(args.LocationName))
            {
                throw new PropertyAlreadyInitializedException(args.LocationFullName);
            }

            this.PropertySetList.Add(args.LocationName);

            args.ProceedSetValue();
        }

        /// <summary>
        /// Initializes the aspect instance. This method is invoked when all system elements of the aspect (like member imports)
        ///               have completed.
        /// </summary>
        public override void RuntimeInitializeInstance()
        {
            base.RuntimeInitializeInstance();

            this.PropertySetList = new List<string>();
        }

        #endregion
    }
}