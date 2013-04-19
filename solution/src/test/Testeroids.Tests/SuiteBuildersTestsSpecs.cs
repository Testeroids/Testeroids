// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuiteBuildersTestsSpecs.cs" company="Testeroids">
//   © 2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using NUnit.Framework;

    public class SuiteBuildersTestsSpecs
    {
        public abstract class given_instantiated_Sut : ContextSpecification<SuiteTestBuilder>
        {
            protected override SuiteTestBuilder CreateSubjectUnderTest()
            {
                return new SuiteTestBuilder(typeof(Test));
            }

            [TestFixture]
            public class when_DiscoverTestMethods_is_called : given_instantiated_Sut
            {
                #region Context

                /// <remarks> Please, as the property's type, mention explicitly the <see cref="System.Type"/> returned by methodName.</remarks>
                private IEnumerable<TriangulatedTestMethod> Result { get; set; }

                protected Dictionary<PropertyInfo, TriangulatedValuesInformation> SpecifiedPossibleValuesForProperties { get; private set; }

                protected MethodInfo SpecifiedMethod { get; private set; }

                protected override void EstablishContext()
                {
                    base.EstablishContext();
                    this.SpecifiedPossibleValuesForProperties = new Dictionary<PropertyInfo, TriangulatedValuesInformation>();
                    TriangulatedValuesInformation values = new TriangulatedValuesInformation(new object[] { 10, 5 });
                    var prop1 = this.MockRepository.CreateMock<PropertyInfo>();
                    prop1.SetupGet(o => o.Name).Returns("Property1");
                    this.SpecifiedPossibleValuesForProperties.Add(prop1.Object, values);

                    values = new TriangulatedValuesInformation(new object[] { 7, -7, 8 });
                    var prop2 = this.MockRepository.CreateMock<PropertyInfo>();
                    prop2.SetupGet(o => o.Name).Returns("Property2");
                    this.SpecifiedPossibleValuesForProperties.Add(prop2.Object, values);
                    this.SpecifiedMethod = null;
                }

                protected override void Because()
                {                    
                    this.Result = this.Sut.DiscoverTestMethods(this.SpecifiedPossibleValuesForProperties, this.SpecifiedMethod);
                }

                #endregion

                [Test]
                public void then_Result_contains_6_elements()
                {
                    Assert.AreEqual(6, this.Result.Count());
                }
            }
        }
    }       
}