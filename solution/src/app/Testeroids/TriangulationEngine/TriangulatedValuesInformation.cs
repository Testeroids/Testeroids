namespace Testeroids.TriangulationEngine
{
    public class TriangulatedValuesInformation
    {
        #region Constructors and Destructors

        public TriangulatedValuesInformation(object[] values)
        {
            this.Values = values;
            this.CurrentlyProcessedValueIndex = 0;
        }

        #endregion

        #region Public Properties

        public int CurrentlyProcessedValueIndex { get; set; }

        public object[] Values { get; private set; }

        #endregion
    }
}