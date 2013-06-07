// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TplTest.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Tests
{
    using System.Threading;
    using System.Threading.Tasks;

    public class TplTest
    {
        #region Fields

        private readonly ICalculator calculator;

        #endregion

        #region Constructors and Destructors

        public TplTest(ICalculator calculator)
        {
            this.calculator = calculator;
        }

        #endregion

        #region Public Properties

        public ICalculator Calculator
        {
            get
            {
                return this.calculator;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Clear()
        {
            this.Calculator.Clear();
        }

        public Task<int> Sum(
            int a, 
            int b)
        {
            return Task<int>.Factory
                            .StartNew(() => this.Calculator.Sum(a, b))
                            .ContinueWith(t => t.Result, CancellationToken.None, TaskContinuationOptions.AttachedToParent, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
    }
}