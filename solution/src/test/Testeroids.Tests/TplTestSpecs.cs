// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TplTestSpecs.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Tests
// ReSharper disable InconsistentNaming
// ReSharper disable AccessToStaticMemberViaDerivedType
// ReSharper disable SealedMemberInSealedClass
{
    using System.Threading.Tasks;

    using Moq;

    using NUnit.Framework;

    using Testeroids.Mocking;

    public abstract class TplTestSpecs
    {
        public abstract class given_instantiated_Sut : ContextSpecification<TplTest>
        {
            #region Context

            protected IMock<ICalculator> InjectedCalculatorMock { get; private set; }

            protected override void EstablishContext()
            {
                base.EstablishContext();

                this.InjectedCalculatorMock = this.MockRepository.CreateMock<ICalculator>();
            }

            protected override TplTest CreateSubjectUnderTest()
            {
                return new TplTest(this.InjectedCalculatorMock.Object);
            }

            #endregion

            [TplContextAspect(ExecuteTplTasks = true)]
            public abstract class when_Sum_is_called : given_instantiated_Sut
            {
                #region Context

                private Task<int> Result { get; set; }

                private int SpecifiedOperand1 { get; set; }

                private int SpecifiedOperand2 { get; set; }

                protected abstract int EstablishSpecifiedOperand1();

                protected abstract int EstablishSpecifiedOperand2();

                protected override void EstablishContext()
                {
                    base.EstablishContext();

                    this.SpecifiedOperand1 = this.EstablishSpecifiedOperand1();
                    this.SpecifiedOperand2 = this.EstablishSpecifiedOperand2();
                }

                protected override sealed void Because()
                {
                    this.Result = this.Sut.Sum(this.SpecifiedOperand1, this.SpecifiedOperand2);
                }

                #endregion

                public abstract class with_returned_result : when_Sum_is_called
                {
                    #region Context

                    private int ReturnedSum { get; set; }

                    protected abstract int EstablishReturnedSum();

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

                    public class with_SpecifiedOperand1_equal_to_10_and_SpecifiedOperand2_equal_to_7 : with_returned_result
                    {
                        #region Context

                        protected override sealed int EstablishSpecifiedOperand1()
                        {
                            return 10;
                        }

                        protected override sealed int EstablishSpecifiedOperand2()
                        {
                            return 7;
                        }

                        protected override sealed int EstablishReturnedSum()
                        {
                            // Return an erroneous value, just to certify that we are returning the value which is handed out by the mock
                            return int.MaxValue;
                        }

                        #endregion
                    }

                    public class with_SpecifiedOperand1_equal_to_10_and_SpecifiedOperand2_equal_to_minus_7 : with_returned_result
                    {
                        #region Context

                        protected override sealed int EstablishSpecifiedOperand1()
                        {
                            return 10;
                        }

                        protected override sealed int EstablishSpecifiedOperand2()
                        {
                            return -7;
                        }

                        protected override sealed int EstablishReturnedSum()
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
                    public void then_Result_status_matches_RanToCompletion()
                    {
                        Assert.AreEqual(TaskStatus.RanToCompletion, this.Result.Status);
                    }

                    [Test]
                    public void then_Result_matches_ReturnedSum()
                    {
                        Assert.AreEqual(this.ReturnedSum, this.Result.Result);
                    }
                }

                public class with_thrown_TestException : when_Sum_is_called
                {
                    #region Context

                    protected override sealed int EstablishSpecifiedOperand1()
                    {
                        // irrelevant
                        return 0;
                    }

                    protected override sealed int EstablishSpecifiedOperand2()
                    {
                        // irrelevant
                        return 0;
                    }

                    protected override void EstablishContext()
                    {
                        base.EstablishContext();

                        this.InjectedCalculatorMock
                            .Setup(o => o.Sum(It.IsAny<int>(), It.IsAny<int>()))
                            .Throws<TestException>();
                    }

                    #endregion

                    [Test]
                    public void then_Result_is_faulted()
                    {
                        Assert.AreEqual(TaskStatus.Faulted, this.Result.Status);
                    }

                    [Test]
                    public void then_Result_contains_TestException()
                    {
                        Assert.IsInstanceOf<TestException>(this.Result.Exception.Flatten().InnerException);
                    }

                    [Test]
                    public void then_Sum_is_called_once_on_InjectedCalculatorMock()
                    {
                        this.InjectedCalculatorMock.Verify(o => o.Sum(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
                    }
                }
            }
        }
    }
}