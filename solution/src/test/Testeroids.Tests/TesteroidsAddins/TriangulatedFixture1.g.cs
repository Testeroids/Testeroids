// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TriangulatedFixture1.g.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids.Tests.TesteroidsAddins
{
    using System;

    using NUnit.Core.Extensibility;

    using Testeroids.TriangulationEngine;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [NUnitAddin(Description = "Testeroids Triangulation Engine")]
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
                isOk = NUnit.Core.Reflect.HasAttribute(type, "Testeroids.Tests.TesteroidsAddins.TriangulatedFixture", true);
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