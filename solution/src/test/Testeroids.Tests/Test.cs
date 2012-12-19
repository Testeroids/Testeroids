namespace Testeroids.Tests
{
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

        #region Public Methods and Operators

        public int Sum(int a,
                       int b)
        {
            return this.calculator.Sum(a, b);
        }

        #endregion
    }
}