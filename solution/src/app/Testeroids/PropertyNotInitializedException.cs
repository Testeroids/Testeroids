// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyNotInitializedException.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;

    /// <summary>
    /// Exception is thrown when code tries to call the get method on a property which has not been set
    /// </summary>
    public class PropertyNotInitializedException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyNotInitializedException"/> class. 
        /// </summary>
        /// <param name="propertyName">
        /// Full name of the property which hasn't been set before get was accessed
        /// </param>
        public PropertyNotInitializedException(string propertyName)
            : base("Property " + propertyName + "'s get method was called before the property was set")
        {
            this.PropertyName = propertyName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the full name of the property which hasn't been set before get was accessed
        /// </summary>
        public string PropertyName { get; private set; }

        #endregion
    }
}