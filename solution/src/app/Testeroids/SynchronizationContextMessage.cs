// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynchronizationContextMessage.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    /// <summary>
    /// Represents a message passed through the <see cref="TestSynchronizationContext"/>.
    /// </summary>
    public class SynchronizationContextMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizationContextMessage"/> class.
        /// </summary>
        /// <param name="method">
        /// The message dispatch method.
        /// </param>
        /// <param name="now">
        /// The current time (expressed in ticks).
        /// </param>
        /// <param name="state">
        /// The state passed to the callback.
        /// </param>
        public SynchronizationContextMessage(
            MessageDispatchMethod method, 
            long now, 
            object state)
        {
            this.Method = method;
            this.Timestamp = now;
            this.State = state;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the method.
        /// </summary>
        public MessageDispatchMethod Method { get; private set; }

        /// <summary>
        /// Gets the state passed to the callback.
        /// </summary>
        public object State { get; private set; }

        /// <summary>
        /// Gets the current time (expressed in ticks).
        /// </summary>
        public long Timestamp { get; private set; }

        #endregion
    }
}