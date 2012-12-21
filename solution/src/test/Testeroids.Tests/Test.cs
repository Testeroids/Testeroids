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

        #region Public Properties

        public ICalculator Calculator
        {
            get { return this.calculator; }
        }

        #endregion

        #region Public Methods and Operators

        public int Sum(int a,
                       int b)
        {
            return this.Calculator.Sum(a, b);
        }

        #endregion
    }
}