// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextSpecification.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;

    /// <summary>
    ///   Implements the base class to define a Context/Specification style test fixture to test a class method.
    /// </summary>
    /// <typeparam name="TSubjectUnderTest"> The type of the subject under test. </typeparam>
    public abstract class ContextSpecification<TSubjectUnderTest> : ContextSpecificationBase
        where TSubjectUnderTest : class
    {
        #region Properties

        /// <summary>
        ///   Gets the instance of the subject under test.
        /// </summary>
        protected TSubjectUnderTest Sut { get; private set; }

        /// <summary>
        ///   Called to dispose all unmanaged resources used by the test. Additionally, it disposes the <see cref="Sut"/> if it is <see cref="IDisposable"/>.
        /// </summary>
        protected override void DisposeContext()
        {
            base.DisposeContext();

            var disposableSut = this.Sut as IDisposable;
            if (disposableSut != null)
            {
                disposableSut.Dispose();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Called to allow the derived class to instantiate the subject under test (<see cref="Sut"/>).
        /// </summary>
        /// <returns> Returns the subject under test </returns>
        protected abstract TSubjectUnderTest CreateSubjectUnderTest();

        /// <summary>
        ///   Takes care of creating (through <see cref="CreateSubjectUnderTest"/>) and initializing the subject under test.
        /// </summary>
        protected override void InitializeSubjectUnderTest()
        {
            this.Sut = this.CreateSubjectUnderTest();
        }

        #endregion
    }
}