// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailTestFixtureWithoutTestAspectAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Aspects
{
    using System;
    using System.Linq;

    using NUnit.Framework;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Dependencies;
    using PostSharp.Extensibility;

    /// <summary>
    ///   Test that a class marked with <see cref="TestFixtureAttribute"/> contains methods marked with <see cref="TestAttribute"/>.
    /// </summary>
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(ArrangeActAssertAspectAttribute))]
    [MulticastAttributeUsage(MulticastTargets.Class, 
        TargetTypeAttributes = MulticastAttributes.AnyScope | MulticastAttributes.AnyVisibility | MulticastAttributes.NonAbstract | MulticastAttributes.Managed, 
        AllowMultiple = false, Inheritance = MulticastInheritance.None)]
    public class FailTestFixtureWithoutTestAspectAttribute : InstanceLevelAspect
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Checks if the given class is marked as <see cref="TestFixtureAttribute"/> and does not contain at least one <see cref="TestAttribute"/> method.
        /// </summary>
        /// <param name="type"> The test class type. </param>
        /// <returns> false if the given class is marked as <see cref="TestFixtureAttribute"/> and does not contain at least one <see cref="TestAttribute"/> method, true otherwise. </returns>
        public override bool CompileTimeValidate(Type type)
        {
            if (TypeInvestigationService.IsConcreteTestFixture(type) && !TypeInvestigationService.GetAllTestMethods(type).Any())
            {
                return ErrorService.RaiseError(this.GetType(), type, string.Format("{0} does not contain any tests. TestFixture must contain tests", type.Name));
            }

            return base.CompileTimeValidate(type);
        }

        #endregion
    }
}