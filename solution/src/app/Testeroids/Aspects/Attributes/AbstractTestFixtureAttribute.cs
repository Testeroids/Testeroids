// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractTestFixtureAttribute.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Aspects.Attributes
{
    using System;

    using PostSharp.Aspects;

    /// <summary>
    ///   Marks an abstract class as allowed to contain test methods. Only works on abstract classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AbstractTestFixtureAttribute : TypeLevelAspect
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="FailAbstractTestFixtureWithoutTestFixtureAspectAttribute"/> should ignore missing nested text fixtures.
        /// </summary>
        public bool IgnoreMissingNestedTextFixtures { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The compile time validate.
        /// </summary>
        /// <param name="type"> The class type to check for abstraction. </param>
        /// <returns> true if the given class type is abstract, . </returns>
        /// <exception cref="NotSupportedException">Thrown if the class is not abstract.</exception>
        public override bool CompileTimeValidate(Type type)
        {
            var compileTimeValidate = type.IsAbstract;
            return compileTimeValidate || ErrorService.RaiseError(this.GetType(), type, string.Format("The '{0}' class cannot be applied on non-abstract classes.\r\nPlease mark the '{1}' class as abstract.\r\n", typeof(AbstractTestFixtureAttribute).Name, type.Name));
        }

        #endregion
    }
}