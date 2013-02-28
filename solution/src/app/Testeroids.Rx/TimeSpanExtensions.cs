// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanExtensions.cs" company="Liebherr International AG">
//   © 2012-2013 Liebherr. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Liebherr.Lioba.Tests.Common
{
    using System;

    public static class TimeSpanExtensions
    {
        private static readonly TimeSpan OneTick = TimeSpan.FromTicks(1);

        public static TimeSpan JustBefore(this TimeSpan value)
        {
            return value.Subtract(OneTick);
        }

        public static TimeSpan JustAfter(this TimeSpan value)
        {
            return value.Add(OneTick);
        }

        public static TimeSpan After(this TimeSpan value, TimeSpan valueToAdd)
        {
            return value.Add(valueToAdd);
        }

        public static TimeSpan Times(this TimeSpan value, long factor)
        {
            return TimeSpan.FromTicks(value.Ticks * factor);
        }
    }
}