// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Testeroids" file="ContextSpecificationBase.cs">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using NUnit.Framework;

    /// <summary>
    ///   Base class for implementing the AAA pattern.
    /// </summary>
    public abstract class ContextSpecificationBase : IContextSpecification
    {
        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether there are prerequisite tests running.
        /// </summary>
        public bool ArePrerequisiteTestsRunning { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The base set up.
        /// </summary>
        [SetUp]
        public virtual void BaseSetUp()
        {
            this.PreTestSetUp();
            this.EstablishContext();
            this.InitializeSubjectUnderTest();
        }

        /// <summary>
        ///   The base tear down.
        /// </summary>
        [TearDown]
        public virtual void BaseTearDown()
        {
            this.DisposeContext();
        }

        #endregion

        #region Methods

        /// <summary>
        ///   This will be called by the ArrangeActAssertAspectAttribute aspect.
        /// </summary>
        protected internal abstract void Because();

        /// <summary>
        ///   Called to dispose all unmanaged resources used by the test.
        /// </summary>
        [DebuggerNonUserCode]
        protected virtual void DisposeContext()
        {
        }

        /// <summary>
        ///   The arrange part.
        /// </summary>
        [DebuggerNonUserCode]
        protected virtual void EstablishContext()
        {
        }

        /// <summary>
        ///   The initialize subject under test.
        /// </summary>
        protected abstract void InitializeSubjectUnderTest();

        /// <summary>
        /// This test is meant for internal library use only.
        /// </summary>
        [DebuggerNonUserCode]
        protected virtual void PreTestSetUp()
        {
        }

        /// <summary>
        ///   This will be called by the ArrangeActAssertAspectAttribute aspect.
        /// </summary>
        protected void RunPrerequisiteTests()
        {
            this.ArePrerequisiteTestsRunning = true;

            try
            {
                var prerequisiteTestsToRun =
                    this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(
                        m => m.IsDefined(typeof(PrerequisiteAttribute), true));

                foreach (var prerequisiteTest in prerequisiteTestsToRun)
                {
                    prerequisiteTest.Invoke(
                        this, 
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, 
                        null, 
                        null, 
                        CultureInfo.InvariantCulture);
                }
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
            finally
            {
                this.ArePrerequisiteTestsRunning = false;
            }
        }

        #endregion
    }
}