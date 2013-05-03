namespace Testeroids.TriangulationEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using NUnit.Core;

    public class TriangulatedTestMethod : NUnitTestMethod
    {
        #region Fields

        private readonly IEnumerable<Tuple<PropertyInfo, object>> triangulationValues;

        #endregion

        #region Constructors and Destructors

        public TriangulatedTestMethod(MethodInfo methodInfo, 
                                      IEnumerable<Tuple<PropertyInfo, object>> triangulationValues)
            : base(methodInfo)
        {
            this.triangulationValues = triangulationValues;
            var testFixture = new TriangulatedTestMethodFixture(this.Method.DeclaringType);
            this.Parent = testFixture;

            var triangulatedName = triangulationValues.Aggregate(this.TestName.Name + " - Triangulated : ", (s, 
                                                                                                             tuple) => string.Format("{0} {1} = {2}", s, tuple.Item1.Name, tuple.Item2.ToString()));
            this.TestName.Name = triangulatedName;
            this.TestName.FullName = triangulationValues.Aggregate(this.TestName.FullName + "Triangulated", (s, 
                                                                                                             tuple) => string.Format("{0}_{1}Is{2}", s, tuple.Item1.Name, tuple.Item2.ToString()));
        }

        #endregion

        #region Public Methods and Operators

        public override TestResult RunTest()
        {
            foreach (var triangulationValue in this.triangulationValues)
            {
                var setMethod = triangulationValue.Item1.GetSetMethod(true);
                setMethod.Invoke(this.Fixture, new[] { triangulationValue.Item2 });
                // triangulationValue.Item1.SetValue(this.Fixture, triangulationValue.Item2, null);

// Find out when is this.Fixture instantiates, and by reflection: use triangulationValue.Item1 ( the PropertyInfo ) to override the property's value before running the tests.
            }

            var contextSpecificationBase = this.Fixture as IContextSpecification;
            contextSpecificationBase.BaseSetUp();
            return base.RunTest();
        }

        #endregion
    }
}