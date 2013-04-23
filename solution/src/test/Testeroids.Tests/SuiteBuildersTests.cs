// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuiteBuildersTests.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using Moq;

    using NUnit.Core;
    using NUnit.Core.Extensibility;
    using NUnit.Framework;

    public class SuiteBuildersTests
    {
        public abstract class given_instantiated_Sut : ContextSpecification<Test>
        {
            #region Context

            protected IMock<ICalculator> InjectedCalculatorMock { get; private set; }

            protected override void EstablishContext()
            {
                base.EstablishContext();

                this.InjectedCalculatorMock = this.MockRepository.CreateMock<ICalculator>();
            }

            protected override Test CreateSubjectUnderTest()
            {
                return new Test(this.InjectedCalculatorMock.Object);
            }

            #endregion

            /// <summary>
            /// TODO: This class has 2 test fixtures which require a simpler triangulation mechanism.
            /// </summary>
            [TestFixture]
            [TriangulatedFixture]
            public class when_Sum_is_called : given_instantiated_Sut
            {
                #region Context

                private int Result { get; set; }

                private int ReturnedSum { get; set; }

                [TriangulationValues(10)]
                private int SpecifiedOperand1 { get; set; }

                [TriangulationValues(7, -7, 8)]
                private int SpecifiedOperand2 { get; set; }

                protected override void EstablishContext()
                {
                    base.EstablishContext();

                    this.ReturnedSum = int.MaxValue;

                    this.InjectedCalculatorMock
                        .Setup(o => o.Sum(It.IsAny<int>(), It.IsAny<int>()))
                        .Returns(this.ReturnedSum)
                        .Verifiable();
                }

                protected override sealed void Because()
                {
                    this.Result = this.Sut.Sum(this.SpecifiedOperand1, this.SpecifiedOperand2);
                }

                #endregion

                [Test]
                public void then_Sum_is_called_once_on_InjectedTestMock()
                {
                    this.InjectedCalculatorMock.Verify(o => o.Sum(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
                }

                [Test]
                public void then_Sum_is_called_once_on_InjectedTestMock_passing_SpecifiedOperand1()
                {
                    this.InjectedCalculatorMock.Verify(o => o.Sum(this.SpecifiedOperand1, It.IsAny<int>()), Times.Once());
                }

                [Test]
                public void then_Sum_is_called_once_on_InjectedTestMock_passing_SpecifiedOperand2()
                {
                    this.InjectedCalculatorMock.Verify(o => o.Sum(It.IsAny<int>(), this.SpecifiedOperand2), Times.Once());
                }

                [Test]
                public void then_SpecifiedOperand1_matches_10()
                {
                    Assert.AreEqual(this.SpecifiedOperand1, 10);
                }

                ///// <summary>
                ///// This one will fail in some cases because SpecifiedOperand2 is being triangulated with -7 and 7 !
                ///// </summary>
                ////  [Test]
                ////  public void then_SpecifiedOperand2_matches_minus7()
                ////  {
                ////      Assert.AreEqual(this.SpecifiedOperand2, -7);
                ////  }
                [Test]
                public void then_Result_matches_ReturnedSum()
                {
                    Assert.AreEqual(this.ReturnedSum, this.Result);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [NUnitAddin(Description = "Hello World Plugin")]
    public class TriangulatedFixture : Attribute, 
                                       NUnit.Core.Extensibility.IAddin, 
                                       ISuiteBuilder
    {
        #region Public Methods and Operators

        public NUnit.Core.Test BuildFrom(Type type)
        {
            return new SuiteTestBuilder(type);
        }

        public bool CanBuildFrom(Type type)
        {
            bool isOk;

            if (type.IsAbstract)
            {
                isOk = false;
            }
            else
            {
                isOk = NUnit.Core.Reflect.HasAttribute(type, "Testeroids.Tests.TriangulatedFixture", true);
            }

            return isOk;
        }

        public bool Install(IExtensionHost host)
        {
            var testCaseBuilders = host.GetExtensionPoint("SuiteBuilders");

            testCaseBuilders.Install(this);

            return true;
        }

        #endregion
    }

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

    public class TriangulatedValuesInformation
    {
        #region Constructors and Destructors

        public TriangulatedValuesInformation(object[] values)
        {
            this.Values = values;
            this.CurrentlyProcessedValueIndex = 0;
        }

        #endregion

        #region Public Properties

        public int CurrentlyProcessedValueIndex { get; set; }

        public object[] Values { get; private set; }

        #endregion
    }

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
                triangulationValue.Item1.SetValue(this.Fixture, triangulationValue.Item2, null);

// Find out when is this.Fixture instantiates, and by reflection: use triangulationValue.Item1 ( the PropertyInfo ) to override the property's value before running the tests.
            }

            var contextSpecificationBase = this.Fixture as ContextSpecificationBase;
            contextSpecificationBase.BaseSetUp();
            return base.RunTest();
        }

        #endregion
    }

    public class TriangulatedTestMethodFixture : NUnitTestFixture
    {
        #region Constructors and Destructors

        public TriangulatedTestMethodFixture(Type declaringType)
            : base(declaringType)
        {
            var baseType = declaringType.BaseType;
            if (baseType != null)
            {
                this.Parent = new TriangulatedTestMethodFixture(baseType);
            }
        }

        #endregion
    }

    internal class TriangulationValuesAttribute : Attribute
    {
        #region Constructors and Destructors

        public TriangulationValuesAttribute(params object[] triangulationValues)
        {
            this.TriangulationValues = triangulationValues;
        }

        #endregion

        #region Public Properties

        public object[] TriangulationValues { get; private set; }

        #endregion
    }
}