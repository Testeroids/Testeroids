// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailTestWithoutTestFixtureAspectAttribute.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Aspects
{
    using System;
    using System.Reflection;

    using NUnit.Framework;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Dependencies;
    using PostSharp.Extensibility;

    /// <summary>
    ///   Test that a class marked with <see cref="TestFixtureAttribute"/> contains methods marked with <see cref="TestAttribute"/>.
    /// </summary>
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(ArrangeActAssertAspectAttribute))]
    [MulticastAttributeUsage(MulticastTargets.Method, 
        TargetTypeAttributes = MulticastAttributes.AnyScope | MulticastAttributes.AnyVisibility | MulticastAttributes.NonAbstract | MulticastAttributes.Managed, 
        AllowMultiple = false, Inheritance = MulticastInheritance.None)]
    public class FailTestWithoutTestFixtureAspectAttribute : MethodLevelAspect
    {
        #region Public Methods and Operators

        /// <summary>
        /// Checks if the given method is marked with <see cref="TestAttribute"/> but is not included in a class marked with <see cref="TestFixtureAttribute"/> or <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/>.
        /// </summary>
        /// <param name="method">
        /// The method to check. 
        /// </param>
        /// <returns> 
        /// false if the given method is marked with <see cref="TestAttribute"/> and is not included in a class marked with <see cref="TestFixtureAttribute"/> or <see cref="Testeroids.Aspects.Attributes.AbstractTestFixtureAttribute"/>. 
        /// </returns>
        public override bool CompileTimeValidate(MethodBase method)
        {
            if (TypeInvestigationService.IsTestMethod(method))
            {
                if ((method.DeclaringType != null) && !(TypeInvestigationService.IsTestFixture(method.DeclaringType) || TypeInvestigationService.IsAbstractTestFixture(method.DeclaringType)))
                {
                    return ErrorService.RaiseError(method.DeclaringType, method, string.Format("{0} is marked with Test attribute, but is not included in a class marked with AbstractTestFixture or textFixture", method.Name));
                }
            }

            return true;
        }

        #endregion
    }
}