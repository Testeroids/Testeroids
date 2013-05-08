// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TriangulationValuesAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids.TriangulationEngine
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// The attribute used to decorate properties in test specs in order to apply triangulation to it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    public class TriangulationValuesAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TriangulationValuesAttribute"/> class.
        /// </summary>
        /// <param name="triangulationValues">
        /// The values which were defined to use for the triangulation.
        /// </param>
        public TriangulationValuesAttribute(params object[] triangulationValues)
        {
            this.TriangulationValues = triangulationValues;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the values which were defined to use for the triangulation.
        /// </summary>
        public object[] TriangulationValues { get; private set; }

        #endregion
    }
}