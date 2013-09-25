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
    using Testeroids.Tests.TesteroidsAddins;
    using Testeroids.TriangulationEngine;

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
            [TriangulatedFixture]
            public abstract class when_Sum_is_called : given_instantiated_Sut
            {
                #region Context

                private Task<int> Result { get; set; }

                [TriangulationValues(10, -1)]
                private int SpecifiedOperand1 { get; set; }

                [TriangulationValues(999, -100000)]
                private int SpecifiedOperand2 { get; set; }

                protected override sealed void Because()
                {
                    this.Result = this.Sut.Sum(this.SpecifiedOperand1, this.SpecifiedOperand2);
                }

                #endregion

                public class with_returned_result : when_Sum_is_called
                {
                    #region Context

                    private int ReturnedSum { get; set; }

                    protected override void EstablishContext()
                    {
                        base.EstablishContext();

                        this.ReturnedSum = this.SpecifiedOperand1 + this.SpecifiedOperand2;
                        this.InjectedCalculatorMock
                            .Setup(o => o.Sum(It.IsAny<int>(), It.IsAny<int>()))
                            .Returns(this.ReturnedSum)
                            .DontEnforceSetupVerification()
                            .EnforceUsage();
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