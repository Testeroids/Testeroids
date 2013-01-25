// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailPrivateFieldCalledInNestedClassAspectAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Aspects
{
    using System;
    using System.Linq;
    using System.Reflection;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Dependencies;
    using PostSharp.Extensibility;

    /// <summary>
    ///   Test that private fields are not called in nested classes.
    /// </summary>
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(ArrangeActAssertAspectAttribute))]
    [MulticastAttributeUsage(MulticastTargets.Class, 
        TargetTypeAttributes = MulticastAttributes.AnyScope | MulticastAttributes.AnyVisibility | MulticastAttributes.NonAbstract | MulticastAttributes.Managed, 
        AllowMultiple = false, Inheritance = MulticastInheritance.None)]
    public class FailPrivateFieldCalledInNestedClassAspectAttribute : InstanceLevelAspect
    {
        #region Public Methods and Operators

        /// <summary>
        ///   The compile time validate.
        /// </summary>
        /// <param name="type"> The class to be checked. </param>
        /// <returns> The System.Boolean. </returns>
        public override bool CompileTimeValidate(Type type)
        {
            try
            {
                const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Static;
                if (type.IsAbstract)
                {
                    var fields = type.GetFields(Flags).Where(f => f.IsPrivate).ToList();
                    if (fields.Any())
                    {
                        var field = fields.First();
                        return ErrorService.RaiseError(this.GetType(), type, field.Name + " should be a protected property.\r\n");
                    }
                }
            }
            catch (Exception e)
            {
                return ErrorService.RaiseError(this.GetType(), type, e.Message);
            }

            return base.CompileTimeValidate(type);
        }

        #endregion
    }
}