namespace Testeroids.TriangulationEngine
{
    using System;

    using NUnit.Core.Extensibility;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [NUnitAddin(Description = "Testeroid Triangulation Engine")]
    public class TriangulatedFixture : Attribute, 
                                       NUnit.Core.Extensibility.IAddin, 
                                       ISuiteBuilder
    {
        #region Public Methods and Operators

        public NUnit.Core.Test BuildFrom(Type type)
        {
            return new SuiteTestBuilder(type);
        }

        public bool CanBuildFrom(Type type)
        {
            bool isOk;

            if (type.IsAbstract)
            {
                isOk = false;
            }
            else
            {
                isOk = NUnit.Core.Reflect.HasAttribute(type, "Testeroids.TriangulatedFixture", true);
            }

            return isOk;
        }

        public bool Install(IExtensionHost host)
        {
            var testCaseBuilders = host.GetExtensionPoint("SuiteBuilders");

            testCaseBuilders.Install(this);

            return true;
        }

        #endregion
    }
}