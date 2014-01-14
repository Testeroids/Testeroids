namespace Testeroids.TriangulationEngine
{
    using System;
    using System.Collections;
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

            var triangulatedName = this.triangulationValues
                                       .Aggregate(
                                                  string.Format("{0} - Triangulated : ", this.TestName.Name),
                                                  (s,
                                                   tuple) => string.Format("{0} {1} = {2}", s, tuple.Item1.Name, ToStringRepresentation(tuple)));

            this.TestName.Name = triangulatedName;
            this.TestName.FullName = this.triangulationValues
                                         .Aggregate(
                                                    string.Format("{0}_Triangulated", this.TestName.FullName),
                                                    (s,
                                                     tuple) => string.Format("{0}_{1}_Is_{2}", s, tuple.Item1.Name, ToStringRepresentation(tuple)));
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

            var contextSpecificationBase = (IContextSpecification)this.Fixture;
            contextSpecificationBase.BaseSetUp();
            return base.RunTest();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the provided type is not a special case of IEnumerable which should not be considered as an enumerable. eg. <see cref="string"/>
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        private static bool IsSpecialEnumerable(Type propertyType)
        {
            return propertyType == typeof(string);
        }

        private static string ToStringRepresentation(Tuple<PropertyInfo, object> triangulatedValue)
        {
            string representation;
            var propertyType = triangulatedValue.Item1.PropertyType;
            if (!IsSpecialEnumerable(propertyType) && !propertyType.FindInterfaces((type,
                                                                                    criteria) => type == typeof(IEnumerable), null).Any())
            {
                representation = triangulatedValue.Item2.ToString();
            }
            else
            {
                var propertyValues = ((IEnumerable)triangulatedValue.Item2).Cast<object>();

                representation = string.Format("{{ {0} }}", string.Join(", ", propertyValues));
            }

            return representation;
        }

        #endregion
    }
}