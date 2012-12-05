// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailAbstractTestFixtureWithoutTestFixtureAspectAttribute.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
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

    using Testeroids.Aspects.Attributes;

    /// <summary>
    ///   Test that if a test fixture has no tests, it must be declared <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/>.
    /// </summary>
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(ArrangeActAssertAspectAttribute))]
    [MulticastAttributeUsage(MulticastTargets.Class, 
        TargetTypeAttributes = MulticastAttributes.AnyScope | MulticastAttributes.AnyVisibility | MulticastAttributes.NonAbstract | MulticastAttributes.Managed, 
        AllowMultiple = false, Inheritance = MulticastInheritance.None)]
    public class FailAbstractTestFixtureWithoutTestFixtureAspectAttribute : InstanceLevelAspect
    {
        #region Public Methods and Operators

        /// <summary>
        ///   The validation method executed at compilation time.
        /// </summary>
        /// <param name="type"> The test class type. </param>
        /// <returns> false if the given class is an abstractTestFixture and does not contain at least one <see cref="TestFixtureAttribute"/> or <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/>, true otherwise. </returns>
        public override bool CompileTimeValidate(Type type)
        {
            if (TypeInvestigationService.IsAbstractTestFixture(type) && !TypeInvestigationService.GetAllTestFixtureTypes(type).Any())
            {
                // Check if IgnoreMissingNestedTextFixtures is set on AbstractTestFixtureAttribute
                var abstractTextFixtureAttr = (AbstractTestFixtureAttribute)type.GetCustomAttributes(typeof(AbstractTestFixtureAttribute), false).FirstOrDefault();

                if (abstractTextFixtureAttr != null && !abstractTextFixtureAttr.IgnoreMissingNestedTextFixtures)
                {
                    return ErrorService.RaiseError(this.GetType(), type, string.Format("{0} does not contain any TestFixture or AbstractTestFixture. AbstractTestFixture must contain testfixture", type.Name));
                }
            }

            return base.CompileTimeValidate(type);
        }

        #endregion
    }
}