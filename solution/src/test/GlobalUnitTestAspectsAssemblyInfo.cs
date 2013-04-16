﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalUnitTestAspectsAssemblyInfo.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using PostSharp.Extensibility;

using Testeroids.Aspects;

[assembly: ArrangeActAssertAspect(AttributeTargetMemberAttributes = MulticastAttributes.Public | MulticastAttributes.Instance, AttributeTargetElements = MulticastTargets.Class)]
[assembly: MakeEmptyTestsInconclusiveAspect]
[assembly: FailTestFixtureWithoutTestAspect]
[assembly: CategorizeUnitTestFixturesAspect]
[assembly: FailNotCalledBaseEstablishContextAspect]