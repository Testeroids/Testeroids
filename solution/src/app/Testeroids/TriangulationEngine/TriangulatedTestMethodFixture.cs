namespace Testeroids.TriangulationEngine
{
    using System;

    using NUnit.Core;

    /// <summary>
    /// The NUnit fixture representing triangulated contexts.
    /// </summary>
    public class TriangulatedTestMethodFixture : NUnitTestFixture
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TriangulatedTestMethodFixture"/> class.
        /// </summary>
        /// <param name="declaringType">
        /// The type on top of which the triangulated context is built.
        /// </param>
        public TriangulatedTestMethodFixture(Type declaringType)
            : base(declaringType)
        {
            var baseType = declaringType.BaseType;
            if (baseType != null)
            {
                this.Parent = new TriangulatedTestMethodFixture(baseType);
            }
        }

        #endregion
    }
}