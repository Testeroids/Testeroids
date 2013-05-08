// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TriangulatedTestMethod.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.TriangulationEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using NUnit.Core;

    /// <summary>
    /// Represents an occurence of the combination of a test for which one or more properties have been triangulated.
    /// </summary>
    public class TriangulatedTestMethod : NUnitTestMethod
    {
        #region Fields

        /// <summary>
        /// A list of the properties on which triangulation should be applied, along with their possible values.
        /// </summary>
        private readonly IList<Tuple<PropertyInfo, object>> triangulationValues;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TriangulatedTestMethod"/> class.
        /// </summary>
        /// <param name="methodInfo">
        /// The <see cref="MethodInfo"/> on top of which the triangulated test must be built.
        /// </param>
        /// <param name="triangulationValues">
        /// A list of the properties on which triangulation should be applied, along with their possible values.
        /// </param>
        public TriangulatedTestMethod(MethodInfo methodInfo, 
                                      IList<Tuple<PropertyInfo, object>> triangulationValues)
            : base(methodInfo)
        {
            this.triangulationValues = triangulationValues;

            var triangulatedName = this.triangulationValues.Aggregate(this.TestName.Name + " - Triangulated : ", (s, 
                                                                                                                  tuple) => string.Format("{0} {1} = {2}", s, tuple.Item1.Name, tuple.Item2.ToString()));
            this.TestName.Name = triangulatedName;
            this.TestName.FullName = this.triangulationValues.Aggregate(this.TestName.FullName + "Triangulated", (s, 
                                                                                                                  tuple) => string.Format("{0}_{1}Is{2}", s, tuple.Item1.Name, tuple.Item2.ToString()));
        }

        #endregion

        #region Public Methods and Operators

        public override TestResult RunTest()
        {
            var tplContextFix = this.Parent as ITplContextFix;
            if (tplContextFix != null)
            {
                tplContextFix.AddTplSupport();
            }

            foreach (var triangulationValue in this.triangulationValues)
            {
                var setMethod = triangulationValue.Item1.GetSetMethod(true);
                setMethod.Invoke(this.Fixture, new[] { triangulationValue.Item2 });
            }

            var contextSpecificationBase = this.Fixture as IContextSpecification;
            contextSpecificationBase.BaseSetUp();
            return base.RunTest();
        }

        #endregion
    }
}