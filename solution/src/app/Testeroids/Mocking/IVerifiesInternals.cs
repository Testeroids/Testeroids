namespace Testeroids.Mocking
{
    using System.Linq.Expressions;

    internal interface IVerifiesInternals
    {
        LambdaExpression Expression { get; set; }

        IVerifiedMock TesteroidsMock { get; set; }        
    }
}