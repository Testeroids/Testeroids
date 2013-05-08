// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuiteBuildersTests.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Tests
{
    using System.Threading.Tasks;

    using Moq;

    using NUnit.Framework;

    using Testeroids.Mocking;
    using Testeroids.Tests.TesteroidsAddins;
    using Testeroids.TriangulationEngine;

    public class SuiteBuildersTests
    {
        public abstract class given_instantiated_Sut : ContextSpecification<Test>
        {
            #region Context

            private IMock<ICalculator> InjectedCalculatorMock { get; set; }

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

            [TriangulatedFixture]
            public abstract class when_Sum_is_called_with_triangulation_on_abstract_Context : given_instantiated_Sut
            {
                #region Context

                [TriangulationValues(5, 6)]
                protected int SpecifiedOperand1 { get; private set; }

                private int Result { get; set; }

                private int SpecifiedOperand2 { get; set; }

                protected abstract int EstablishSpecifiedOperand2();

                protected override void EstablishContext()
                {
                    base.EstablishContext();

                    this.SpecifiedOperand2 = this.EstablishSpecifiedOperand2();

                    this.CheckSetupsAreMatchedWithVerifyCalls = true;
                }

                protected override sealed void Because()
                {
                    this.Result = this.Sut.Sum(this.SpecifiedOperand1, this.SpecifiedOperand2);
                }

                protected override void DisposeContext()
                {
                    this.SpecifiedOperand1 = 0;
                    base.DisposeContext();
                }

                #endregion

                public abstract class with_returned_result : when_Sum_is_called_with_triangulation_on_abstract_Context
                {
                    #region Context

                    private int ReturnedSum { get; set; }

                    private int EstablishReturnedSum()
                    {
                        // Return an erroneous value, just to certify that we are returning the value which is handed out by the mock
                        return this.SpecifiedOperand1 + this.SpecifiedOperand2;
                    }

                    protected override void EstablishContext()
                    {
                        base.EstablishContext();
                        this.ReturnedSum = this.EstablishReturnedSum();
                        this.InjectedCalculatorMock
                            .Setup(o => o.Sum(It.IsAny<int>(), It.IsAny<int>()))
                            .Returns(this.ReturnedSum)
                            .DontEnforceSetupVerification()
                            .EnforceUsage();
                    }

                    #endregion

                    public class with_SpecifiedOperand2_equal_to_7 : with_returned_result
                    {
                        #region Context

                        protected override sealed int EstablishSpecifiedOperand2()
                        {
                            return 7;
                        }

                        #endregion
                    }

                    public class with_SpecifiedOperand2_equal_to_minus_7 : with_returned_result
                    {
                        #region Context

                        protected override sealed int EstablishSpecifiedOperand2()
                        {
                            return -7;
                        }

                        #endregion
                    }

                    [Test]
                    public void then_Result_matches_ReturnedSum()
                    {
                        Assert.AreEqual(this.ReturnedSum, this.Result);
                    }
                }
            }

            [TriangulatedFixture]
            [TplContextAspect]
            public abstract class when_Sum_is_called_with_triangulation_on_abstract_Context_and_TplContextAspect : TestSpecs.given_instantiated_Sut
            {
                #region Context

                [TriangulationValues(5, 6)]
                protected int SpecifiedOperand1 { get; private set; }

                private int Result { get; set; }

                private int SpecifiedOperand2 { get; set; }

                protected abstract int EstablishSpecifiedOperand2();

                protected override void EstablishContext()
                {
                    base.EstablishContext();

                    this.SpecifiedOperand2 = this.EstablishSpecifiedOperand2();

                    this.CheckSetupsAreMatchedWithVerifyCalls = true;
                }

                protected override sealed void Because()
                {
                    this.Result = this.Sut.Sum(this.SpecifiedOperand1, this.SpecifiedOperand2);
                }

                protected override void DisposeContext()
                {
                    this.SpecifiedOperand1 = 0;
                    base.DisposeContext();
                }

                #endregion

                public abstract class with_returned_result : when_Sum_is_called_with_triangulation_on_abstract_Context_and_TplContextAspect
                {
                    #region Context

                    private int ReturnedSum { get; set; }

                    private int EstablishReturnedSum()
                    {
                        // Return an erroneous value, just to certify that we are returning the value which is handed out by the mock
                        return this.SpecifiedOperand1 + this.SpecifiedOperand2;
                    }

                    protected override void EstablishContext()
                    {
                        base.EstablishContext();

                        this.SpecifiedOperand1 = 1;

                        this.ReturnedSum = this.EstablishReturnedSum();
                        this.InjectedCalculatorMock
                            .Setup(o => o.Sum(It.IsAny<int>(), It.IsAny<int>()))
                            .Returns(this.ReturnedSum)
                            .DontEnforceSetupVerification()
                            .EnforceUsage();
                    }

                    #endregion

                    public class with_SpecifiedOperand2_equal_to_7 : with_returned_result
                    {
                        #region Context

                        protected override sealed int EstablishSpecifiedOperand2()
                        {
                            return 7;
                        }

                        #endregion
                    }

                    /// <remarks>
                    ///  HACK : we use the type's name in string representation because TestTaskScheduler is internal to Testeroids.
                    /// </remarks>
                    [Test]
                    public void then_Default_TaskScheduler_is_of_type_TestTaskScheduler()
                    {
                        Assert.AreEqual("TestTaskScheduler", TaskScheduler.Default.GetType().Name);
                    }
                }
            }
        }
    }
}