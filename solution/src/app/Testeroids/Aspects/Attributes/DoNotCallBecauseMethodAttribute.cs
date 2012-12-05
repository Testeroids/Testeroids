// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoNotCallBecauseMethodAttribute.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Aspects.Attributes
{
    using System;

    /// <summary>
    ///   Meant for advanced scenarios. This attribute works with the <see cref="ArrangeActAssertAspectAttribute" /> aspect to define tests in which we do not want the aspect to inject Because() calls automatically.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DoNotCallBecauseMethodAttribute : Attribute
    {
    }
}