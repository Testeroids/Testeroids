namespace Testeroids
{
    using System;

    using Testeroids.Aspects;

    /// <summary>
    ///   Implements the base class to define a Context/Specification style test fixture to test a class constructor.
    /// </summary>
    /// <typeparam name="TSubjectUnderTest"> Type of the class which constructor is to be tested. </typeparam>
    public abstract class SubjectInstantiationContextSpecification<TSubjectUnderTest> : ContextSpecificationBase
        where TSubjectUnderTest : class
    {
        #region Properties

        /// <summary>
        ///   Gets the subject under test.
        /// </summary>
        protected TSubjectUnderTest Sut { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///   Instantiates the Subject Under Test.
        /// </summary>
        protected internal override void Because()
        {
            this.InstantiateSut();
        }

        /// <summary>
        ///   The method being tested. It instantiates the <see cref="Sut"/>.
        /// </summary>
        /// <returns> The instance of TSubjectUnderTest. </returns>
        protected abstract TSubjectUnderTest BecauseSutIsCreated();

        /// <summary>
        ///   Performs additional initialization after the subject under test has been created.
        /// </summary>
        protected override sealed void InitializeSubjectUnderTest()
        {
        }

        /// <summary>
        /// Instantiates the Subject Under Test.
        /// </summary>
        private void InstantiateSut()
        {
            this.Sut = this.BecauseSutIsCreated();
            GC.SuppressFinalize(this.Sut);
        }

        #endregion
    }
}