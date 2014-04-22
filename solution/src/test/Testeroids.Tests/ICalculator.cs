namespace Testeroids.Tests
{
    using System.Threading.Tasks;

    public interface ICalculator
    {
        #region Public Properties

        int Radix { get; set; }

        #endregion

        #region Public Methods and Operators

        void Clear();

        int Sum(int a,
                int b);

        Task<int> SumAsync(int a,
                           int b);

        #endregion
    }
}