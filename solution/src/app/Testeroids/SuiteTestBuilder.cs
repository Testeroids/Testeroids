namespace Testeroids
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using NUnit.Core;
    using NUnit.Framework;

    public class SuiteTestBuilder : TestSuite
    {
        #region Constructors and Destructors

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public SuiteTestBuilder(Type fixtureType)
            : base(fixtureType)
        {
            this.Parent = new TriangulatedTestMethodFixture(fixtureType);
            foreach (var method in fixtureType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                // Let's ignore methods which are not decorated with the TestAttribute attribute.
                if (!method.GetCustomAttributes(typeof(TestAttribute), true).Any())
                {
                    continue;
                }

                var triangulatedProperties = method.DeclaringType.FindMembers(
                    MemberTypes.Property, 
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, 
                    (info, 
                     criteria) => info.IsDefined(typeof(TriangulationValuesAttribute), false), 
                    null).Cast<PropertyInfo>();

                var possibleValuesForProperties = new Dictionary<PropertyInfo, TriangulatedValuesInformation>();

                foreach (var property in triangulatedProperties)
                {
                    var values = property.GetCustomAttributes(typeof(TriangulationValuesAttribute), false).Cast<TriangulationValuesAttribute>().Single().TriangulationValues;
                    var valuesInfo = new TriangulatedValuesInformation(values);
                    possibleValuesForProperties.Add(property, valuesInfo);
                }

                var propertyInfos = possibleValuesForProperties.Keys.ToArray();

                var triangulationValues = new List<Tuple<PropertyInfo, object>>(propertyInfos.Length);
                var triangulatedSets = new Collection<IList<Tuple<PropertyInfo, object>>>();
                this.BuildTriangulationSet(triangulationValues, possibleValuesForProperties, propertyInfos, triangulatedSets);

                foreach (var triangulatedSet in triangulatedSets)
                {
                    this.Add(new TriangulatedTestMethod(method, triangulatedSet));
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Recursively builds the triangulation sets and stores them in the specified <paramref name="triangulatedSets"/> accumulator.
        /// </summary>
        /// <param name="triangulationValues">The accumulator for the values of the previously visited properties in need of triangulation.</param>
        /// <param name="possibleValuesForProperties">The different possible values for the properties in need of triangulation. i.e. : the triangulated values.</param>
        /// <param name="triangulatedProperties">The properties in need of triangulation</param>
        /// <param name="triangulatedSets">Serves as an accumulator and operation output for the outcome of the recursion.</param>
        private void BuildTriangulationSet(
            IList<Tuple<PropertyInfo, object>> triangulationValues, 
            IDictionary<PropertyInfo, TriangulatedValuesInformation> possibleValuesForProperties, 
            IList<PropertyInfo> triangulatedProperties, 
            ICollection<IList<Tuple<PropertyInfo, object>>> triangulatedSets)
        {
            // if the recursion is complete, add the just built set of tiangulated values - representing a complete list of values for all the triangulated properties - to the triangulatedSets.
            if (triangulationValues.Count == triangulatedProperties.Count)
            {
                triangulatedSets.Add(triangulationValues);
                return;
            }

            // find the current property:
            var currentProperty = triangulatedProperties[triangulationValues.Count];

            var possibleValues = possibleValuesForProperties[currentProperty].Values;

            for (var index = 0; index < possibleValues.Length; index++)
            {
                var possibleValue = possibleValues[index];

                // Memory optimization to avoid wasting a list at each recursion:
                // Create n-1 different lists, with n being the number of possible values for the current property.
                // Recycle the one which was passed as argument, as the *last* list to be processed (so as to leave the other lists unaffected)
                var newtriangulationSet = index < possibleValues.Length
                                              ? triangulationValues.ToList()
                                              : triangulationValues;

                // for a given property, we its possible values (one at each iteration of the current for loop.)
                newtriangulationSet.Add(new Tuple<PropertyInfo, object>(currentProperty, possibleValue));

                // ... and we continue the recursion
                this.BuildTriangulationSet(newtriangulationSet, possibleValuesForProperties, triangulatedProperties, triangulatedSets);
            }
        }

        #endregion
    }
}