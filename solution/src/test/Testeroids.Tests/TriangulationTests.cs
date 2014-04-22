namespace Testeroids.Tests
// ReSharper disable InconsistentNaming
// ReSharper disable AccessToStaticMemberViaDerivedType
// ReSharper disable SealedMemberInSealedClass
{
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using Testeroids.Mocking;

    public class TriangulationTests
    {
        public abstract class given_instantiated_Sut : ContextSpecification<Test>
        {
            #region Context

            private ITesteroidsMock<ICalculator> InjectedCalculatorMock { get; set; }

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

            [TestFixture(0)]
            [TestFixture(1)]
            public class when_Sum_is_called : given_instantiated_Sut
            {
                #region Context

                private int CurrentTestSet { get; set; }

                private int Result { get; set; }

                private int ReturnedSum { get; set; }

                private int SpecifiedOperand1 { get; set; }

                private int SpecifiedOperand2 { get; set; }

                protected override void EstablishContext()
                {
                    base.EstablishContext();

                    var testSets = new[]
                                   {
                                       new { SpecifiedOperand1 = 10, SpecifiedOperand2 = 7 },
                                       new { SpecifiedOperand1 = 10, SpecifiedOperand2 = -7 },
                                       new { SpecifiedOperand1 = 10, SpecifiedOperand2 = 8 }
                                   };
                    var currentTestSet = testSets[this.CurrentTestSet];

                    this.SpecifiedOperand1 = currentTestSet.SpecifiedOperand1;
                    this.SpecifiedOperand2 = currentTestSet.SpecifiedOperand2;

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

                public when_Sum_is_called(int testSet)
                {
                    this.CurrentTestSet = testSet;
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

            [TestFixture(new[] { 1, 2 })]
            [TestFixture(new[] { 3, 4, 5 })]
            public class when_Clear_is_called_with_triangulation_on_arrays : SubjectInstantiationContextSpecification<Test>
            {
                #region Context

                private int[] TriangulatedArray { get; set; }

                protected override void EstablishContext()
                {
                    base.EstablishContext();

                    this.CheckSetupsAreMatchedWithVerifyCalls = true;
                }

                /// <summary>
                ///   The method being tested. It instantiates the <see cref="Sut"/>.
                /// </summary>
                /// <returns> The instance of TSubjectUnderTest. </returns>
                protected override Test BecauseSutIsCreated()
                {
                    return new Test(this.MockRepository.CreateMock<ICalculator>().Object);
                }

                public when_Clear_is_called_with_triangulation_on_arrays(int[] array)
                {
                    this.TriangulatedArray = array;
                }

                #endregion

                [Test]
                public void then_TriangulatedArray_is_not_empty()
                {
                    CollectionAssert.IsNotEmpty(this.TriangulatedArray);
                }
            }

            [TestFixture(new[] { 1, 2 })]
            [TestFixture(new[] { 3, 4, 5 })]
            public class when_Clear_is_called_with_triangulation_on_enumerables : SubjectInstantiationContextSpecification<Test>
            {
                #region Context

                private IEnumerable<int> TriangulatedEnumerable { get; set; }

                protected override void EstablishContext()
                {
                    base.EstablishContext();

                    this.CheckSetupsAreMatchedWithVerifyCalls = true;
                }

                public when_Clear_is_called_with_triangulation_on_enumerables(int[] array)
                {
                    this.TriangulatedEnumerable = array;
                }

                #endregion

                /// <summary>
                ///   The method being tested. It instantiates the <see cref="Sut"/>.
                /// </summary>
                /// <returns> The instance of TSubjectUnderTest. </returns>
                protected override Test BecauseSutIsCreated()
                {
                    return new Test(this.MockRepository.CreateMock<ICalculator>().Object);
                }

                /// <summary>
                /// there should be 2 tests with that name !
                /// </summary>
                [Test]
                public void then_TriangulatedEnumerable_is_not_empty()
                {
                    CollectionAssert.IsNotEmpty(this.TriangulatedEnumerable);
                }
            }

            [TestFixture(5)]
            [TestFixture(6)]
            public abstract class when_Sum_is_called_with_triangulation_on_abstract_Context : given_instantiated_Sut
            {
                #region Context

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

                protected when_Sum_is_called_with_triangulation_on_abstract_Context(int operand1)
                {
                    this.SpecifiedOperand1 = operand1;
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

                    protected with_returned_result(int operand1)
                        : base(operand1)
                    {
                    }

                    #endregion

                    public class with_SpecifiedOperand2_equal_to_7 : with_returned_result
                    {
                        #region Context

                        protected override sealed int EstablishSpecifiedOperand2()
                        {
                            return 7;
                        }

                        public with_SpecifiedOperand2_equal_to_7(int operand1)
                            : base(operand1)
                        {
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

                        public with_SpecifiedOperand2_equal_to_minus_7(int operand1)
                            : base(operand1)
                        {
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
        }
    }
}