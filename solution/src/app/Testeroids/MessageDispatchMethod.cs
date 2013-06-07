// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageDispatchMethod.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System.Threading;

    /// <summary>
    /// The message dispatch method used in <see cref="SynchronizationContext"/>.
    /// </summary>
    public enum MessageDispatchMethod
    {
        /// <summary>
        /// A method was invoked using <see cref="SynchronizationContext.Post"/>.
        /// </summary>
        Post, 

        /// <summary>
        /// A method was invoked using <see cref="SynchronizationContext.Send"/>.
        /// </summary>
        Send
    }
}