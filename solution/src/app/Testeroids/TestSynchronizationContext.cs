// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSynchronizationContext.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// The synchronization context used in the testing context.
    /// </summary>
    internal class TestSynchronizationContext : SynchronizationContext
    {
        #region Fields

        /// <summary>
        /// The recorded messages (sent through <see cref="Post"/> or <see cref="Send"/>).
        /// </summary>
        private readonly IList<SynchronizationContextMessage> recordedMessages = new List<SynchronizationContextMessage>();

        /// <summary>
        /// The current virtual time, expressed in ticks.
        /// </summary>
        private long clock;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the recorded messages (sent through <see cref="Post"/> or <see cref="Send"/>).
        /// </summary>
        public IList<SynchronizationContextMessage> RecordedMessages
        {
            get
            {
                return this.recordedMessages;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// When overridden in a derived class, dispatches an asynchronous message to a synchronization context.
        /// </summary>
        /// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback"/> delegate to call.</param>
        /// <param name="state">The object passed to the delegate.</param>
        public override void Post(
            SendOrPostCallback d, 
            object state)
        {
            this.RecordedMessages.Add(new SynchronizationContextMessage(MessageDispatchMethod.Post, this.clock++, state));

            d(state);
        }

        /// <summary>
        /// When overridden in a derived class, dispatches a synchronous message to a synchronization context.
        /// </summary>
        /// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback"/> delegate to call.</param>
        /// <param name="state">The object passed to the delegate.</param>
        public override void Send(
            SendOrPostCallback d, 
            object state)
        {
            this.RecordedMessages.Add(new SynchronizationContextMessage(MessageDispatchMethod.Send, this.clock++, state));

            d(state);
        }

        /// <summary>
        /// Waits for any or all the elements in the specified array to receive a signal.
        /// </summary>
        /// <returns>
        /// The array index of the object that satisfied the wait.
        /// </returns>
        /// <param name="waitHandles">An array of type <see cref="T:System.IntPtr"/> that contains the native operating system handles.</param>
        /// <param name="waitAll">true to wait for all handles; false to wait for any handle. </param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite"/> (-1) to wait indefinitely.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="waitHandles"/> is null.
        /// </exception>
        public override int Wait(
            IntPtr[] waitHandles, 
            bool waitAll, 
            int millisecondsTimeout)
        {
            try
            {
                return base.Wait(waitHandles, waitAll, millisecondsTimeout);
            }
            finally
            {
                this.clock += millisecondsTimeout * TimeSpan.TicksPerMillisecond;
            }
        }

        #endregion
    }
}