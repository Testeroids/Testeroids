// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextSpecification.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    /// <summary>
    ///   Derives from <see cref="ContextSpecificationBase" /> to provide strong typed access to the subject under test.
    /// </summary>
    /// <typeparam name="TSubjectUnderTest"> The type of the subject under test. </typeparam>
    public abstract class ContextSpecification<TSubjectUnderTest> : ContextSpecificationBase
    {
        #region Properties

        /// <summary>
        ///   Gets the instance of the subject under test.
        /// </summary>
        protected TSubjectUnderTest Sut { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///   Called to allow the derived class to instantiate the subject under test.
        /// </summary>
        /// <returns> Returns the subject under test </returns>
        protected abstract TSubjectUnderTest CreateSubjectUnderTest();

        /// <summary>
        ///   Takes care of creating (through CreateSubjectUnderTest()) and initializing the subject under test.
        /// </summary>
        protected override void InitializeSubjectUnderTest()
        {
            this.Sut = this.CreateSubjectUnderTest();
        }

        #endregion
    }
}