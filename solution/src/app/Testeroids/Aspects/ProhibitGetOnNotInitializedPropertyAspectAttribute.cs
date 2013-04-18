// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProhibitGetOnNotInitializedPropertyAspectAttribute.cs" company="Testeroids">
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
    public class ProhibitGetOnNotInitializedPropertyAspectAttribute : InstanceLevelAspect
    {
        #region Fields

        /// <summary>
        /// The list containing the names of the properties where the Set method has been accessed
        /// </summary>
        private List<string> propertySetList;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Method invoked <i>instead</i> of the <c>Get</c> semantic of property to which the current aspect is applied,
        ///               i.e. when the value of this field or property is retrieved.
        /// If the property's set method has not been called the method will throw an <see cref="PropertyNotInitializedException"/>.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        /// <exception cref="PropertyNotInitializedException">
        /// When the property's set method has not been called before.
        /// </exception>
        [OnLocationGetValueAdvice]
        [MulticastPointcut(Targets = MulticastTargets.Property, Attributes = MulticastAttributes.AnyVisibility | MulticastAttributes.Instance)]
        public void OnPropertyGet(LocationInterceptionArgs args)
        {
            if (args.Location.PropertyInfo.GetSetMethod(true) != null &&
                !this.propertySetList.Contains(args.LocationName))
            {
                throw new PropertyNotInitializedException(args.LocationFullName);
            }

            args.ProceedGetValue();
        }

        /// <summary>
        /// Method invoked <i>instead</i> of the <c>Set</c> semantic of the field or property to which the current aspect is applied,
        ///               i.e. when the value of this field or property is changed.
        /// Adds the property to the list of properties where set has been accessed.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        [OnLocationSetValueAdvice(Master = "OnPropertyGet")]
        public void OnPropertySet(LocationInterceptionArgs args)
        {
            if (!this.propertySetList.Contains(args.LocationName))
            {
                this.propertySetList.Add(args.LocationName);
            }

            args.ProceedSetValue();
        }

        /// <summary>
        /// Initializes the aspect instance. This method is invoked when all system elements of the aspect (like member imports)
        ///               have completed.
        /// </summary>
        public override void RuntimeInitializeInstance()
        {
            base.RuntimeInitializeInstance();

            this.propertySetList = new List<string>();
        }

        #endregion
    }
}