namespace Testeroids.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// Rule which verifies that no Set method is called a second time.
    /// Currently not functional as EstablishContext method is called multiple times.
    /// </summary>
    [Serializable]
    public class ProhibitSetOnInitializedPropertyRule : InstanceLevelRule,
                                                        IPropertyAccessRule
    {
        #region Fields

        /// <summary>
        /// The list containing the names of the properties where the Set method has been accessed
        /// </summary>
        private List<string> propertySetList;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Initializes the rule instance.
        /// </summary>
        public override void Initialize()
        {
            this.propertySetList = new List<string>();
        }

        /// <summary>
        /// This rules doesn't control getter on properties.
        /// </summary>
        /// <param name="propertyInfo">
        /// The info of the property on which the getter was called.
        /// </param>
        public void OnPropertyGet(PropertyInfo propertyInfo)
        {
        }

        /// <summary>
        /// If the property's set method has already been called the method will throw an <see cref="PropertyAlreadyInitializedException"/>.
        /// </summary>
        /// <param name="propertyInfo">
        /// The info of the property on which the setter was called.
        /// </param>
        /// <exception cref="PropertyAlreadyInitializedException">
        /// When the property's set method has already been called before.
        /// </exception>
        public void OnPropertySet(PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetSetMethod(true) != null &&
                propertyInfo.DeclaringType != typeof(ContextSpecificationBase) &&
                this.propertySetList.Contains(propertyInfo.Name))
            {
                Debug.Assert(propertyInfo.DeclaringType != null, "propertyInfo.DeclaringType != null");
                throw new PropertyAlreadyInitializedException(string.Format("{0}.{1}", propertyInfo.DeclaringType.FullName, propertyInfo.Name));
            }

            this.propertySetList.Add(propertyInfo.Name);
        }

        #endregion
    }
}