// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalUnitTestAspectsAssemblyInfo.cs" company="Liebherr International AG">
//   © 2012 Liebherr. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using PostSharp.Extensibility;

using Testeroids.Aspects;

[assembly: ArrangeActAssertAspect(AttributeTargetMemberAttributes = MulticastAttributes.Public | MulticastAttributes.Instance, AttributeTargetElements = MulticastTargets.Class)]
[assembly: MakeEmptyTestsInconclusiveAspect]
[assembly: FailAbstractTestFixtureWithoutTestFixtureAspect]
[assembly: FailTestFixtureWithoutTestAspect]
[assembly: FailTestWithoutTestFixtureAspect]
[assembly: FailTestsOnAbstractClassesNonMarkedAbstractTestFixtureAspect]
[assembly: CategorizeUnitTestFixturesAspect]
[assembly: FailNotCalledBaseEstablishContextAspect]