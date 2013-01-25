// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionResilientAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Aspects.Attributes
{
    using System;

    /// <summary>
    ///   Meant for advanced scenarios. This attribute works with the <see cref="ArrangeActAssertAspectAttribute" /> aspect to define tests which we do not want to fail because an exception has been thrown.
    /// </summary>
    /// <remarks>
    ///   Use carefully, since throwing an exception within the Because() would jump directly to the assertion./>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ExceptionResilientAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ExceptionResilientAttribute" /> class. Ignore all exceptions.
        /// </summary>
        public ExceptionResilientAttribute()
        {
            this.ExceptionTypeToIgnore = null;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ExceptionResilientAttribute" /> class.
        /// </summary>
        /// <param name="exceptionTypeToIgnore"> Exception type to ignore in tests. </param>
        public ExceptionResilientAttribute(Type exceptionTypeToIgnore)
        {
            this.ExceptionTypeToIgnore = exceptionTypeToIgnore;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the exception type to ignore.
        /// </summary>
        public Type ExceptionTypeToIgnore { get; private set; }

        #endregion
    }
}