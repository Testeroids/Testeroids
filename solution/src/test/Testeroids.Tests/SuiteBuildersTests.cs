// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuiteBuildersTests.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Tests
{
    using System;

    using Moq;

    using NUnit.Core.Extensibility;
    using NUnit.Framework;

    using Testeroids.TriangulationEngine;

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
            [NUnitAddins.TriangulatedFixture]
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
}