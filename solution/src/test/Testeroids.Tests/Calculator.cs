namespace Testeroids.Tests
{
    internal class Calculator : ICalculator
    {
        #region Public Methods and Operators

        public int Sum(int a,
                       int b)
        {
            return a + b;
        }

        #endregion
    }
}