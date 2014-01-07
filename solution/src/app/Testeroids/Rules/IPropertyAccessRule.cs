namespace Testeroids.Rules
{
    using System.Reflection;

    /// <summary>
    ///   Defines the contract for the rule verifying access on properties.
    /// </summary>
    public interface IPropertyAccessRule : IInstanceLevelRule
    {
        #region Public Methods and Operators

        /// <summary>
        /// Verifies if access on property getter is allowed.
        /// </summary>
        /// <param name="propertyInfo">
        /// The info of the property on which the getter has been called.
        /// </param>
        void OnPropertyGet(PropertyInfo propertyInfo);

        /// <summary>
        /// Verifies if access on property setter is allowed.
        /// </summary>
        /// <param name="propertyInfo">
        /// The info of the property on which the setter has been called.
        /// </param>
        void OnPropertySet(PropertyInfo propertyInfo);

        #endregion
    }
}