namespace Testeroids
{
    using System;

    using NUnit.Core;

    public class TriangulatedTestMethodFixture : NUnitTestFixture
    {
        #region Constructors and Destructors

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