// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Test.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Tests
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    public class Test
    {
        #region Fields

        private readonly ICalculator calculator;

        #endregion

        #region Constructors and Destructors

        public Test(ICalculator calculator)
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

        public Task<int> FailingSumAsync()
        {
            var failingSumAsync = this.SumAsync(4, 5).ContinueWith(task =>
                                                                       {
                                                                           throw new NotImplementedException(string.Format("If everything went fine, the result should have been {0}, but it didn't and an exception was thrown. Therefore, you should see it in the runner :)", task.Result));
                                                                           return 0;
                                                                       });
            return failingSumAsync;
        }

        public void FireAndForgetFailingTask()
        {
            this.FailingSumAsync();
        }

        public void FireForgetAndSwallowFailingTask()
        {
            this.SwallowedFailingSumAsync();
        }

        public IObservable<int> ReturnObserver()
        {
            return Observable.Create<int>((observer, 
                                           token) =>
                                          Task<IDisposable>.Factory.StartNew(() => { throw new TestException(); }));
        }

        public int Sum(int a, 
                       int b)
        {
            return this.Calculator.Sum(a, b);
        }

        public Task<int> SumAsync(int a, 
                                  int b)
        {
            return new TaskFactory<int>().StartNew(() => a + b);
        }

        public Task<int> SwallowedFailingSumAsync()
        {
            var failingSumAsync = this.SumAsync(4, 5).ContinueWith(task =>
                                                                       {
                                                                           throw new NotImplementedException(string.Format("If everything went fine, the result should have been {0}, but it didn't and an exception was thrown. Therefore, you should see it in the runner :)", task.Result));
                                                                           return 0;
                                                                       });
            try
            {
                failingSumAsync.Wait();
            }
            catch (Exception)
            {
                // Swallow
            }

            return failingSumAsync;
        }

        #endregion
    }
}