namespace Testeroids.Rules
{
    using System;

    using Testeroids.Aspects;

    /// <summary>
    ///   Defines the contract for a rule that can be enforced by <see cref="EnforceInstanceLevelRulesAspectAttribute"/>.
    /// </summary>
    public interface IInstanceLevelRule : IRule
    {
        #region Public Methods and Operators

        /// <summary>
        /// Validates if the <paramref name="type"/> conforms to the rule.
        /// Raises Errors if not.
        /// </summary>
        /// <param name="type">
        /// The type to validate.
        /// </param>
        /// <returns>
        /// <c>true</c> if the rule should be applied on this type.
        /// </returns>
        bool CompileTimeValidate(Type type);

        #endregion
    }
}