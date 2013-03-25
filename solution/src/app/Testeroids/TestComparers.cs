// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestComparers.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System.Collections;

    using NUnit.Framework;

    /// <summary>
    /// A set of comparers which are useful for test scenarios (e.g. <see cref="CollectionAssert"/>).
    /// </summary>
    public static class TestComparers
    {
        #region Public Properties

        /// <summary>
        /// Gets an <see cref="IComparer"/> which compares objects based on reference identity.
        /// </summary>
        public static IComparer ReferenceEquality
        {
            get
            {
                return new ReferenceEqualityComparer();
            }
        }

        #endregion

        /// <summary>
        /// An <see cref="IComparer"/> implementation which compares objects based on reference identity.
        /// </summary>
        private class ReferenceEqualityComparer : IComparer
        {
            #region Public Methods and Operators

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, same reference, or greater than the other.
            /// </summary>
            /// <returns>
            /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero <paramref name="x"/>'s hash code is less than <paramref name="y"/>'s. Zero <paramref name="x"/> is same reference as <paramref name="y"/>. Greater than zero <paramref name="x"/>'s hash code is greater than <paramref name="y"/>'s. 
            /// </returns>
            /// <param name="x">The first object to compare. </param><param name="y">The second object to compare. </param><exception cref="T:System.ArgumentException">Neither <paramref name="x"/> nor <paramref name="y"/> implements the <see cref="T:System.IComparable"/> interface.-or- <paramref name="x"/> and <paramref name="y"/> are of different types and neither one can handle comparisons with the other. </exception><filterpriority>2</filterpriority>
            public int Compare(
                object x, 
                object y)
            {
                return object.ReferenceEquals(x, y)
                           ? x.GetHashCode().CompareTo(y.GetHashCode())
                           : 0;
            }

            #endregion
        }
    }
}