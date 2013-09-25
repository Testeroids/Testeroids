// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreadSafeTestSynchronizationContextGateway.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// A gateway that dispatches calls to the correct <see cref="testSynchronizationContext"/> for the current thread (allowing for parallel running of tests).
    /// </summary>
    public class ThreadSafeTestSynchronizationContextGateway : SynchronizationContext
    {
        #region Fields

        /// <summary>
        /// The test synchronization context for the current thread.
        /// </summary>
        private ThreadLocal<TestSynchronizationContext> testSynchronizationContext;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the recorded messages (sent through <see cref="Post"/> or <see cref="Send"/>).
        /// </summary>
        /// <exception cref="InvalidOperationException">The TestSynchronizationContext is not initialized for this thread.</exception>
        public IList<SynchronizationContextMessage> RecordedMessages
        {
            get
            {
                if (this.testSynchronizationContext.IsValueCreated)
                {
                    throw new InvalidOperationException("The TestSynchronizationContext is not initialized for this thread!");
                }

                return this.testSynchronizationContext.Value.RecordedMessages;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Initializes the <see cref="TestSynchronizationContext"/> for this thread (normally called for each test).
        /// </summary>
        public void Initialize()
        {
            this.testSynchronizationContext = new ThreadLocal<TestSynchronizationContext>(() => new TestSynchronizationContext());
        }

        /// <summary>
        /// When overridden in a derived class, dispatches an asynchronous message to a synchronization context.
        /// </summary>
        /// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback"/> delegate to call.</param>
        /// <param name="state">The object passed to the delegate.</param>
        public override void Post(
            SendOrPostCallback d, 
            object state)
        {
            this.testSynchronizationContext.Value.Post(d, state);
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
            this.testSynchronizationContext.Value.Send(d, state);
        }

        /// <summary>
        /// Tears down the <see cref="testSynchronizationContext"/> for the current thread.
        /// </summary>
        public void Teardown()
        {
            this.testSynchronizationContext = new ThreadLocal<TestSynchronizationContext>();
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
            return this.testSynchronizationContext.Value.Wait(waitHandles, waitAll, millisecondsTimeout);
        }

        #endregion
    }
}