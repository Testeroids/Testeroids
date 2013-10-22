namespace Testeroids.Rx
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;

    using JetBrains.Annotations;

    using Microsoft.Reactive.Testing;

    /// <summary>
    /// Defines extensions which facilitate the usage of <see cref="ITestableObserver{T}"/>.
    /// </summary>
    public static class TestableObserverExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Counts all messages of <see cref="NotificationKind.OnNext"/> kind.
        /// </summary>
        /// <typeparam name="T">Type of the notifications.</typeparam>
        /// <param name="messages">The list containing the messages to count.</param>
        /// <returns>Number of messages with <see cref="NotificationKind.OnNext"/> kind.</returns>
        public static int CountOnNext<T>([NotNull] this IEnumerable<Recorded<Notification<T>>> messages)
        {
            return messages.Count(msg => msg.Value.Kind == NotificationKind.OnNext);
        }

        #endregion
    }
}