namespace Testeroids.Rx
{
    using System;

    /// <summary>
    /// Extensions methods for improved syntax for expressing <see cref="TimeSpan"/> arithmetic.
    /// </summary>
    public static class TimeSpanExtensions
    {
        #region Static Fields

        /// <summary>
        /// Represents a single clock tick.
        /// </summary>
        private static readonly TimeSpan OneTick = TimeSpan.FromTicks(1);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds a <see cref="TimeSpan"/> to the target time span.
        /// </summary>
        /// <param name="value">
        /// The target <see cref="TimeSpan"/>.
        /// </param>
        /// <param name="valueToAdd">
        /// The <see cref="TimeSpan"/> to add.
        /// </param>
        /// <returns>
        /// The resulting <see cref="TimeSpan"/>.
        /// </returns>
        public static TimeSpan After(
            this TimeSpan value, 
            TimeSpan valueToAdd)
        {
            return value.Add(valueToAdd);
        }

        /// <summary>
        /// Returns the <see cref="TimeSpan"/> representing the moment just after the target <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="value">
        /// The target <see cref="TimeSpan"/>.
        /// </param>
        /// <returns>
        /// The <see cref="TimeSpan"/> representing the moment just after the target <see cref="TimeSpan"/>.
        /// </returns>
        public static TimeSpan JustAfter(this TimeSpan value)
        {
            return value.Add(OneTick);
        }

        /// <summary>
        /// Returns the <see cref="TimeSpan"/> representing the moment just before the target <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="value">
        /// The target <see cref="TimeSpan"/>.
        /// </param>
        /// <returns>
        /// The <see cref="TimeSpan"/> representing the moment just before the target <see cref="TimeSpan"/>.
        /// </returns>
        public static TimeSpan JustBefore(this TimeSpan value)
        {
            return value.Subtract(OneTick);
        }

        /// <summary>
        /// Returns the <paramref name="value"/> multiplied by <paramref name="factor"/>.
        /// </summary>
        /// <param name="value">
        /// The target <see cref="TimeSpan"/>.
        /// </param>
        /// <returns>
        /// The <see cref="TimeSpan"/> representing the moment just after the target <see cref="TimeSpan"/>.
        /// </returns>
        public static TimeSpan Times(
            this TimeSpan value, 
            long factor)
        {
            return TimeSpan.FromTicks(value.Ticks * factor);
        }

        #endregion
    }
}