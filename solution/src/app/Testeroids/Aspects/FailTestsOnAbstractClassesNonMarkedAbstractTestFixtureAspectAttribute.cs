// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailTestsOnAbstractClassesNonMarkedAbstractTestFixtureAspectAttribute.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids.Aspects
{
    using System;
    using System.Linq;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Dependencies;
    using PostSharp.Extensibility;

    using Testeroids.Aspects.Attributes;

    /// <summary>
    /// Aspect to check if an abstract classes containing test is marked with <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/>.
    /// </summary>
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(ArrangeActAssertAspectAttribute))]
    [MulticastAttributeUsage(MulticastTargets.Class,
        TargetTypeAttributes = MulticastAttributes.AnyScope | MulticastAttributes.AnyVisibility | MulticastAttributes.NonAbstract | MulticastAttributes.Managed,
        AllowMultiple = false, Inheritance = MulticastInheritance.None)]
    public class FailTestsOnAbstractClassesNonMarkedAbstractTestFixtureAspectAttribute : InstanceLevelAspect
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FailTestsOnAbstractClassesNonMarkedAbstractTestFixtureAspectAttribute"/> class.
        /// </summary>
        public FailTestsOnAbstractClassesNonMarkedAbstractTestFixtureAspectAttribute()
        {
            this.AttributeTargetTypeAttributes = MulticastAttributes.Abstract;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The compile time validate.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// <c>true</c> if the aspect is to be applied to the <paramref name="type"/> type.
        /// </returns>
        public override bool CompileTimeValidate(Type type)
        {
            if (type.IsAbstract && TypeInvestigationService.GetNonPrerequisiteTestMethods(type).Any())
            {
                if (!type.IsDefined(typeof(AbstractTestFixtureAttribute), false))
                {
                    ErrorService.RaiseError(this.GetType(), type, string.Format("{0} : Abstract test class must be marked as AbstractTestFixture if they contains tests: {1}", this.GetType().Name, type.FullName));
                }
            }

            return base.CompileTimeValidate(type);
        }

        #endregion
    }
}