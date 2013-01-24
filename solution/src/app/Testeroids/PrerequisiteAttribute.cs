// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrerequisiteAttribute.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;

    /// <summary>
    ///   Marks the test as a prerequisite to the other tests in the same fixture
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PrerequisiteAttribute : Attribute
    {
    }
}