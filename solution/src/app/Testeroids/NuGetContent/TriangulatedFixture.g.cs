// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TriangulatedFixture.g.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Tests.TesteroidsAddins
{
    using System;

    using JetBrains.Annotations;

    using NUnit.Core;
    using NUnit.Core.Extensibility;

    using Testeroids.TriangulationEngine;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [NUnitAddin(Description = "Testeroids Triangulation Engine")]
    public class TriangulatedFixture : Attribute, 
                                       IAddin, 
                                       ISuiteBuilder
    {
        #region Public Methods and Operators

        public Test BuildFrom(Type type)
        {
            return new TriangulatedTestSuiteBuilder(type);
        }

        public bool CanBuildFrom([NotNull] Type type)
        {
            bool isOk;

            if (type.IsAbstract)
            {
                isOk = false;
            }
            else
            {
                isOk = false;
                while (type.BaseType != typeof(object) && !isOk)
                {
                    isOk = NUnit.Core.Reflect.HasAttribute(type, "Testeroids.Tests.TesteroidsAddins.TriangulatedFixture", true);
                    var baseType = type.BaseType;
                    if (baseType != null)
                    {
                        type = baseType;
                    }
                }
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