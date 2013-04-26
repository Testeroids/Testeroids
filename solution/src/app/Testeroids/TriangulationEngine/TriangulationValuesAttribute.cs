namespace Testeroids.TriangulationEngine
{
    using System;

    public class TriangulationValuesAttribute : Attribute
    {
        #region Constructors and Destructors

        public TriangulationValuesAttribute(params object[] triangulationValues)
        {
            this.TriangulationValues = triangulationValues;
        }

        #endregion

        #region Public Properties

        public object[] TriangulationValues { get; private set; }

        #endregion
    }
}