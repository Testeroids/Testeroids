// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICalculator.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Tests
{
    public interface ICalculator
    {
        #region Public Properties

        int Radix { get; set; }

        #endregion

        #region Public Methods and Operators

        int Sum(int a, int b);

        void Clear();

        #endregion
    }
}