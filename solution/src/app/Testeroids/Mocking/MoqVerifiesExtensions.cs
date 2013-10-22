namespace Testeroids.Mocking
{
    using Moq.Language;

    public static class MoqVerifiesExtensions
    {
        #region Public Methods and Operators

        public static IVerifies DontEnforceSetupVerification(this IVerifies verifies)
        {
            var testeroidsSetup = (Mocking.IVerifiesInternals)verifies;
            var expression = testeroidsSetup.Expression;
            var testeroidsMock = testeroidsSetup.TesteroidsMock;
            testeroidsMock.UnregisterSetupForVerification(expression);
            return verifies;
        }

        public static IVerifies EnforceUsage(this IVerifies verifies)
        {
            verifies.Verifiable();
            return verifies;
        }

        #endregion
    }
}