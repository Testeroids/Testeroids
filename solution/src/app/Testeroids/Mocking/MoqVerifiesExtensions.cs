// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoqSetupWrapped.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids.Mocking
{
    public static class MoqVerifiesExtensions
    {        
        public static Moq.Language.IVerifies DontEnforceSetupVerification(this Moq.Language.IVerifies verifies)
        {
            var testeroidsSetup = (Mocking.IVerifiesInternals)verifies;
            var expression = testeroidsSetup.Expression;
            var testeroidsMock = testeroidsSetup.TesteroidsMock;
            testeroidsMock.UnregisterSetupForVerification(expression);
            return verifies;
        }

        public static Moq.Language.IVerifies EnforceUsage(this Moq.Language.IVerifies verifies)
        {
            verifies.Verifiable();
            return verifies;
        }
    }
}