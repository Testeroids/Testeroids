namespace Testeroids
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Testeroids.Aspects;

    /// <summary>
    ///   Base class for implementing the AAA pattern.
    /// </summary>
    public interface IContextSpecification
    {
        #region Public Properties

        /// <summary>
        ///     Gets the list of tasks to be executed during context setup.
        /// </summary>
        IList<Action<IContextSpecification>> SetupTasks { get; }

        /// <summary>
        ///     Gets the list of tasks to be executed during context teardown.
        /// </summary>
        IList<Action<IContextSpecification>> TeardownTasks { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// This will be called by the <see cref="InstrumentTestsAspect"/> aspect. Performs the "OnTestMethodCalled" part, responsible for reporting the behavior found during the Act part.
        /// </summary>
        /// <param name="testMethodInfo">
        /// The <see cref="MethodBase"/> instance that describes the executing test.
        /// </param>
        /// <param name="isExceptionResilient">
        /// <c>true</c> if the test is set to ignore some exception. <c>false</c> otherwise.
        /// </param>
        void OnTestMethodCalled(MethodBase testMethodInfo,
                                bool isExceptionResilient);

        #endregion
    }
}