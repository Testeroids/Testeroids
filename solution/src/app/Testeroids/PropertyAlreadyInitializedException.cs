// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyAlreadyInitializedException.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Exception is thrown when code tries to call the get method on a property which has not been set
    /// </summary>
    public class PropertyAlreadyInitializedException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAlreadyInitializedException"/> class. 
        /// </summary>
        /// <param name="propertyName">
        /// Full name of the property which hasn't been set before get was accessed
        /// </param>
        public PropertyAlreadyInitializedException(string propertyName)
            : base(string.Format("Property {0} was already set.", propertyName))
        {
            this.PropertyName = propertyName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the full name of the property which hasn't been set before get was accessed
        /// </summary>
        [PublicAPI]
        public string PropertyName { get; private set; }

        #endregion
    }
}