namespace Testeroids
{
    using System.Reflection;

    using Testeroids.Aspects;

    /// <summary>
    ///   Base class for implementing the AAA pattern.
    /// </summary>
    public interface IContextSpecification
    {
        #region Public Methods and Operators

        /// <summary>
        /// This will be called by the <see cref="InvokeTestsAspect"/> aspect. Performs the "Act" part, or the logic which is to be tested.
        /// </summary>
        /// <param name="testMethodInfo">
        /// The <see cref="MethodBase"/> instance that describes the executing test.
        /// </param>
        /// <param name="isExceptionResilient">
        /// <c>true</c> if the test is set to ignore some exception. <c>false</c> otherwise.
        /// </param>
        void Act(MethodBase testMethodInfo,
                 bool isExceptionResilient);

        /// <summary>
        ///   Sets up the test (calls <see cref="Testeroids.ContextSpecificationBase.EstablishContext"/> followed by <see cref="Testeroids.ContextSpecificationBase.InitializeSubjectUnderTest" />).
        /// </summary>
        void BaseSetUp();

        #endregion
    }
}
