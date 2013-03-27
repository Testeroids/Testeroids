// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuiteBuildersTests.cs" company="Testeroids">
//   © 2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Moq;

    using NUnit.Core;
    using NUnit.Core.Extensibility;
    using NUnit.Framework;

    using Testeroids.Aspects.Attributes;

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
            [AbstractTestFixture]
            public abstract class when_Sum_is_called : given_instantiated_Sut
            {
                #region Context

                private int Result { get; set; }

                private int ReturnedSum { get; set; }

                [TriangulationValues(10)]
                private int SpecifiedOperand1 { get; set; }

                [TriangulationValues(7, -7)]
                private int SpecifiedOperand2 { get; set; }

                protected abstract int EstablishSpecifiedOperand1();

                protected abstract int EstablishSpecifiedOperand2();

                protected abstract int EstablishReturnedSum();

                protected override void EstablishContext()
                {
                    base.EstablishContext();

                    this.SpecifiedOperand1 = this.EstablishSpecifiedOperand1();
                    this.SpecifiedOperand2 = this.EstablishSpecifiedOperand2();

                    this.ReturnedSum = this.EstablishReturnedSum();

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

                [TestFixture]
                [TriangulatedFixture]
                public class with_SpecifiedOperand1_equal_to_10_and_SpecifiedOperand2_equal_to_7 : when_Sum_is_called
                {
                    #region Context

                    protected override int EstablishSpecifiedOperand1()
                    {
                        return 10;
                    }

                    protected override int EstablishSpecifiedOperand2()
                    {
                        return 7;
                    }

                    protected override int EstablishReturnedSum()
                    {
                        // Return an erroneous value, just to certify that we are returning the value which is handed out by the mock
                        return int.MaxValue;
                    }

                    #endregion
                }

                [TestFixture]
                public class with_SpecifiedOperand1_equal_to_10_and_SpecifiedOperand2_equal_to_minus_7 : when_Sum_is_called
                {
                    #region Context

                    protected override int EstablishSpecifiedOperand1()
                    {
                        return 10;
                    }

                    protected override int EstablishSpecifiedOperand2()
                    {
                        return -7;
                    }

                    protected override int EstablishReturnedSum()
                    {
                        return 3;
                    }

                    #endregion
                }

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
                public void then_Result_matches_ReturnedSum()
                {
                    Assert.AreEqual(this.ReturnedSum, this.Result);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [NUnitAddin(Description = "Hello World Plugin")]
    public class TriangulatedFixture : Attribute, NUnit.Core.Extensibility.IAddin, ISuiteBuilder
    {
        public bool Install(IExtensionHost host)
        {
            IExtensionPoint testCaseBuilders = host.GetExtensionPoint("SuiteBuilders");

            testCaseBuilders.Install(this); //this implements both interfaces

            return true;

        }       

        public bool CanBuildFrom(Type type)
        {
            bool isOk;

            if (type.IsAbstract)
            {
                isOk =  false;
            }
            else
            {                
                isOk = NUnit.Core.Reflect.HasAttribute(type, "Testeroids.Tests.TriangulatedFixture", true);
            }

            return isOk;
        }

        public NUnit.Core.Test BuildFrom(Type type)
        {
            return new SuiteTestBuilder(type);
        }
    }

    public class SuiteTestBuilder : TestSuite
    {

        public SuiteTestBuilder(Type fixtureType)
            : base(fixtureType)
        {
            this.Fixture = Reflect.Construct(fixtureType);

            foreach (MethodInfo method in fixtureType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (method.Name.StartsWith("then_"))
                {
                    var nUnitTestMethod = new TriangulatedTestMethod(method);                    

                    // TODO : We can probably use multiple constructs of the fixture, call some methods on it to parametrize it with triangulation values, and call this.Add multiple times to add multiple Tests to the suite.                    
                    this.Add(nUnitTestMethod);
                }
            }
        }
    }

    public class TriangulatedTestMethod : NUnitTestMethod
    {
        public TriangulatedTestMethod(MethodInfo methodInfo)
            : base(methodInfo)
        {
        }

        public override TestResult RunTest()
        {
            return base.RunTest();
        }
    }

    internal class TriangulationValuesAttribute : Attribute
    {
        private readonly object[] triangulationValues;

        public TriangulationValuesAttribute(params object[] triangulationValues)
        {
            this.triangulationValues = triangulationValues;            
        }
    }
}