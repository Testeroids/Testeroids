namespace Testeroids.Mocking
{
    using System.Linq.Expressions;

    internal interface IVerifiesInternals
    {
        object Expression { get; set; }

        IVerifiedMock TesteroidsMock { get; set; }        
    }
}