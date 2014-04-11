namespace Testeroids.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// Rule which verifies that no Get method is accessed before
    /// the Set method has been called. This is to make sure that injected values are always established before they are used in a
    /// child test class.
    /// </summary>
    [Serializable]
    public class ProhibitGetOnNotInitializedPropertyRule : InstanceLevelRule,
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
        /// Initializes the aspect instance. This method is invoked when all system elements of the aspect (like member imports)
        ///               have completed.
        /// </summary>
        public override void Initialize()
        {
            this.propertySetList = new List<string>();
        }

        /// <summary>
        /// If the property's set method has not been called the method will throw an <see cref="PropertyNotInitializedException"/>.
        /// </summary>
        /// <param name="propertyInfo">
        /// The info of the property on which the getter has been called.
        /// </param>
        /// <exception cref="PropertyNotInitializedException">
        /// When the property's set method has not been called before.
        /// </exception>
        public void OnPropertyGet(PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetSetMethod(true) == null
                || this.propertySetList.Contains(propertyInfo.Name))
            {
                return;
            }

            Debug.Assert(propertyInfo.DeclaringType != null, "propertyInfo.DeclaringType != null");
            throw new PropertyNotInitializedException(string.Format("{0}.{1}", propertyInfo.DeclaringType.FullName, propertyInfo.Name));
        }

        /// <summary>
        /// Adds the property to the list of properties where set has been accessed.
        /// </summary>
        /// <param name="propertyInfo">
        /// The info of the property on which the setter has been called.
        /// </param>
        public void OnPropertySet(PropertyInfo propertyInfo)
        {
            if (!this.propertySetList.Contains(propertyInfo.Name))
            {
                this.propertySetList.Add(propertyInfo.Name);
            }
        }

        #endregion
    }
}