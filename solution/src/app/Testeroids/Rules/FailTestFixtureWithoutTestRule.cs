namespace Testeroids.Rules
{
    using System;
    using System.Linq;

    using NUnit.Framework;

    using Testeroids.Aspects;

    /// <summary>
    ///   Test that a class marked with <see cref="TestFixtureAttribute"/> contains methods marked with <see cref="TestAttribute"/>.
    /// </summary>
    [Serializable]
    public class FailTestFixtureWithoutTestRule : InstanceLevelRule
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Checks if the given class is marked as <see cref="TestFixtureAttribute"/> and does not contain at least one <see cref="TestAttribute"/> method.
        /// </summary>
        /// <param name="type"> The test class type. </param>
        /// <returns> false if the given class is marked as <see cref="TestFixtureAttribute"/> and does not contain at least one <see cref="TestAttribute"/> method, true otherwise. </returns>
        public override bool CompileTimeValidate(Type type)
        {
            if (TypeInvestigationService.IsConcreteContextSpecification(type) && !TypeInvestigationService.GetAllTestMethods(type).Any())
            {
                return ErrorService.RaiseError(this.GetType(), type, string.Format("{0} does not contain any tests. Concrete ContextSpecifications must contain tests", type.Name));
            }

            return true;
        }

        #endregion
    }
}