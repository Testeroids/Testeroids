// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubjectInstantiationContextSpecification.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
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
        ///   This will be called by the <see cref="ArrangeActAssertAspectAttribute"/> aspect. Performs the "Act" part, or the logic which is to be tested (in this case, executing the constructor of the <typeparamref name="TSubjectUnderTest"/>.
        /// </summary>
        protected internal override void Because()
        {
            this.Sut = this.BecauseSutIsCreated();
        }

        /// <summary>
        ///   The method being tested. It instantiates the <see cref="Sut"/>.
        /// </summary>
        /// <returns> The instance of TSubjectUnderTest. </returns>
        protected abstract TSubjectUnderTest BecauseSutIsCreated();

        /// <summary>
        ///   Performs additional initialization after the subject under test has been created. Not used in this scenario.
        /// </summary>
        protected override sealed void InitializeSubjectUnderTest()
        {
        }

        #endregion
    }
}