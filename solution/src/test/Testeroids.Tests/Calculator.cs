namespace Testeroids.Tests
{
    using System.Threading.Tasks;

    internal class Calculator : ICalculator
    {
        #region Constructors and Destructors

        public Calculator()
        {
            this.Radix = 10;
        }

        #endregion

        #region Public Properties

        public int Radix { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public int Sum(int a,
                       int b)
        {
            return a + b;
        }

        public Task<int> SumAsync(int a,
                                  int b)
        {
            var taskCompletionSource = new TaskCompletionSource<int>();
            taskCompletionSource.SetResult(a + b);
            return taskCompletionSource.Task;
        }

        #endregion
    }
}