// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVerifiesInternals.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Mocking
{
    using System.Linq.Expressions;

    internal interface IVerifiesInternals
    {
        #region Public Properties

        LambdaExpression Expression { get; }

        IVerifiedMock TesteroidsMock { get; }

        #endregion
    }
}