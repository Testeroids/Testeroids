namespace Testeroids.Rules
{
    using System;

    using Testeroids.Aspects;

    /// <summary>
    /// Abstract rule that can be enforced by <see cref="EnforceInstanceLevelRulesAspectAttribute"/>.
    /// </summary>
    [Serializable]
    public abstract class InstanceLevelRule : Rule,
                                              IInstanceLevelRule
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
        public virtual bool CompileTimeValidate(Type type)
        {
            return true;
        }

        #endregion
    }
}