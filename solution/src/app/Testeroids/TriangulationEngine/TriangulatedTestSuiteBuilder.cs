namespace Testeroids.TriangulationEngine
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using NUnit.Core;
    using NUnit.Framework;

    /// <summary>
    /// The test suite builder for triangulated tests. Will be responsible for the creation of the combinations of triangulated properties' values.
    /// </summary>
    public class TriangulatedTestSuiteBuilder : TestSuite, 
                                                ITplContextFix
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TriangulatedTestSuiteBuilder"/> class.
        /// </summary>
        /// <param name="fixtureType">
        /// The type of the fixture containing the test methods one which the triangulation should be applied.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public TriangulatedTestSuiteBuilder(Type fixtureType)
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

                var contextType = fixtureType;
                var triangulatedProperties = Enumerable.Empty<PropertyInfo>();
                while (contextType != typeof(object))
                {
                    triangulatedProperties = contextType
                        .FindMembers(
                            MemberTypes.Property, 
                            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, 
                            (info, 
                             criteria) => info.IsDefined(typeof(TriangulationValuesAttribute), false), 
                            null)
                        .Cast<PropertyInfo>()
                        .Concat(triangulatedProperties);
                    contextType = contextType.BaseType;
                }

                var possibleValuesForProperties = new Dictionary<PropertyInfo, object[]>();

                foreach (var property in triangulatedProperties.Where(o => o.CanWrite))
                {
                    var values = property.GetCustomAttributes(typeof(TriangulationValuesAttribute), false).Cast<TriangulationValuesAttribute>().Single().TriangulationValues;
                    possibleValuesForProperties.Add(property, values);
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

        #region Public Methods and Operators

        /// <summary>
        /// Adds support for the TPL contexts as done by the <see cref="TplContextAspectAttribute"/>.
        /// </summary>
        /// <remarks>
        /// The mechanism is sub-optimal at the moment, because it duplicates the code of the <see cref="TplContextAspectAttribute"/>. eventually, we should make the code re-usable.
        /// </remarks>
        public void AddTplSupport()
        {
            var customAttributes = this.FixtureType.GetCustomAttributes(true);
            var tplContextAspect = customAttributes.OfType<TplContextAspectAttribute>().FirstOrDefault();
            if (tplContextAspect != null)
            {
                var testTaskScheduler = new TplTestPlatformHelper.TestTaskScheduler(tplContextAspect.ExecuteTplTasks);

                TplTestPlatformHelper.SetDefaultScheduler(testTaskScheduler);
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
            Dictionary<PropertyInfo, object[]> possibleValuesForProperties, 
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

            var possibleValues = possibleValuesForProperties[currentProperty];

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