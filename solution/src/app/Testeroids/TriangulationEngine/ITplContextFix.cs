// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITplContextFix.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.TriangulationEngine
{
    /// <summary>
    /// The contract for tests containers which support making up for <see cref="TplContextAspectAttribute"/> which would be applied to declared test fixtures, but eaten by applying add-ins and building new test suites out of those declared test fixtures.
    /// </summary>
    public interface ITplContextFix
    {
        #region Public Methods and Operators

        /// <summary>
        /// Adds support for the TPL contexts as done by the <see cref="TplContextAspectAttribute"/>.
        /// </summary>
        /// <remarks>
        /// The mechanism is sub-optimal at the moment, because it duplicates the code of the <see cref="TplContextAspectAttribute"/>. eventually, we should make the code re-usable.
        /// </remarks>
        void AddTplSupport();

        #endregion
    }
}