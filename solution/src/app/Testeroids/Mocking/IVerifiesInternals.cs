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