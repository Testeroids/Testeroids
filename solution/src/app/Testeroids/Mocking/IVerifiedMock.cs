namespace Testeroids.Mocking
{
    using System.Linq.Expressions;

    internal interface IVerifiedMock
    {
        /// <summary>
        /// Unregisters the specified expression in order to ignore it in the sanity check which makes sure there is a call verification unit test for each setup.
        /// </summary>
        /// <param name="expression"></param>
        void UnregisterSetupForVerification(LambdaExpression expression);
    }
}