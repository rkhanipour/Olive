﻿using System;

namespace Olive
{
    partial class OliveExtensions
    {
        const int HALF_CIRCLE_DEGREES = 180;

        /// <summary>
        /// Rounds this value.
        /// </summary>
        public static double Round(this double @this, int digits) =>
            (double)Math.Round((decimal)@this, digits, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Rounds this value.
        /// </summary>
        public static decimal Round(this decimal @this, int digits) =>
            Math.Round(@this, digits, MidpointRounding.AwayFromZero);

        /// <summary>
        /// In mathematics and computer science, truncation is the term for limiting the number of digits right of the decimal point, by discarding the least significant ones.
        /// Note that in some cases, truncating would yield the same result as rounding, but truncation does not round up or round down the digits; it merely cuts off at the specified digit.
        /// </summary>
        public static double Truncate(this double @this, int places)
        {
            var multiplier = Math.Pow(10, (double)places);

            if (@this > 0)
                return Math.Floor(@this * multiplier) / multiplier;
            else
                return Math.Ceiling(@this * multiplier) / multiplier;
        }

        public static string ToString(this double? @this, string format) => ($"{{0:{format}}}").FormatWith(@this);

        public static string ToString(this decimal? @this, string format) => ($"{{0:{format}}}").FormatWith(@this);

        /// <summary>
        /// Drops the floating point digits from the end of the money string.
        /// For example for 1500.00 it will yield "£1,500" and for 18.56 it will yield "£18.56".
        /// </summary>
        public static string ToShortMoneyString(this double @this) => "{0:c}".FormatWith(@this).TrimEnd(".00");

        public static string ToShortMoneyString(this double? @this)
        {
            if (@this.HasValue) return @this.ToShortMoneyString();
            else return string.Empty;
        }

        /// <summary>
        /// Drops the floating point digits from the end of the money string.
        /// For example for 1500.00 it will yield "£1,500" and for 18.56 it will yield "£18.56".
        /// </summary>
        public static string ToInformalMoneyString(this double @this)
        {
            var identifiers = new[] { "k", "m", "bn" };

            for (var i = identifiers.Length; i > 0; i--)
            {
                var power = 2 + 3 * (i - 1);

                var figure = Math.Pow(10, power);

                if (@this % figure == 0 && (@this / figure) > 10)
                {
                    @this = @this / figure * 10;

                    if (@this > 1000)
                        return @this.ToShortMoneyString() + identifiers[i - 1];
                    else
                        return @this.ToShortMoneyString()[0].ToString() + @this + identifiers[i - 1];
                }
            }

            return @this.ToShortMoneyString();
        }

        /// <summary>
        /// Converts degree into radians.
        /// </summary>
        public static double ToRadians(this double @this) => Math.PI * @this / HALF_CIRCLE_DEGREES;

        /// <summary>
        /// Return this value as a percentages the of the given total.
        /// </summary>       
        public static double AsPercentageOf(this double @this, double total, bool multiplyBy100 = true, int? roundTo = null)
        {
            var pc = @this / total;

            if (double.IsNaN(pc) || double.IsInfinity(pc)) return 0d;

            if (multiplyBy100) pc *= 100d;

            if (roundTo.HasValue) pc = pc.Round(roundTo.Value);

            return pc;
        }

        /// <summary>
        /// Return this value as a percentages the of the given total.
        /// </summary>
        public static decimal AsPercentageOf(this decimal @this, decimal total, bool multiplyBy100 = true, int? roundTo = null)
        {
            var pc = @this / total;

            if (multiplyBy100) pc *= 100;

            if (roundTo.HasValue)
                pc = Math.Round(pc, roundTo.Value);

            return pc;
        }

        /// <summary>
        /// Rounds up to nearest value.
        /// </summary>
        public static double RoundUpToNearest(this double @this, double roundIntervals)
        {
            var remainder = @this % roundIntervals;
            if (remainder == 0) return @this;

            return @this + (roundIntervals - remainder);
        }

        /// <summary>
        /// Rounds up to nearest value.
        /// </summary>
        public static decimal RoundUpToNearest(this decimal @this, decimal roundIntervals)
        {
            var remainder = @this % roundIntervals;
            if (remainder == 0) return @this;

            return @this + (roundIntervals - remainder);
        }

        /// <summary>
        /// Rounds down to nearest value with the intervals specified.
        /// </summary>
        public static double RoundDownToNearest(this double @this, double roundIntervals) => @this - (@this % roundIntervals);

        /// <summary>
        /// Rounds down to nearest value with the intervals specified.
        /// </summary>
        public static decimal RoundDownToNearest(this decimal @this, decimal roundIntervals) => @this - (@this % roundIntervals);

        /// <summary>
        /// Determines if this double value is almost equal to the specified other value.
        /// This should be used instead of == or != operators due to the nature of double processing in .NET.
        /// </summary>
        /// <param name="tolerance">Specifies the tolerated level of difference.</param>
        public static bool AlmostEquals(this double @this, double otherValue, double tolerance = 0.00001) =>
            Math.Abs(@this - otherValue) <= tolerance;

        /// <summary>
        /// Determines if this float value is almost equal to the specified other value.
        /// This should be used instead of == or != operators due to the nature of float processing in .NET.
        /// </summary>
        /// <param name="tolerance">Specifies the tolerated level of difference.</param>
        public static bool AlmostEquals(this float @this, float otherValue, float tolerance = 0.001f) =>
            Math.Abs(@this - otherValue) <= tolerance;

        public static float LimitMax(this float @this, float maxValue) => @this > maxValue ? maxValue : @this;

        public static float LimitMin(this float @this, float minValue) => @this < minValue ? minValue : @this;

        public static float LimitWithin(this float @this, float minValue, float maxValue) => @this.LimitMin(minValue).LimitMax(maxValue);

        public static double LimitMax(this double @this, double maxValue) => @this > maxValue ? maxValue : @this;

        public static double LimitMin(this double @this, double minValue) => @this < minValue ? minValue : @this;

        public static double LimitWithin(this double @this, double minValue, double maxValue) => @this.LimitMin(minValue).LimitMax(maxValue);

        public static int LimitMax(this int @this, int maxValue) => @this > maxValue ? maxValue : @this;

        public static int LimitMin(this int @this, int minValue) => @this < minValue ? minValue : @this;

        public static int LimitWithin(this int @this, int minValue, int maxValue) => @this.LimitMin(minValue).LimitMax(maxValue);

        public static int CompareTo(this double? @this, double? another)
        {
            if (@this == another) return 0;
            if (another == null) return @this < 0 ? -1 : 1;
            if (@this == null) return another < 0 ? 1 : -1;
            return @this.Value.CompareTo(another.Value);
        }

        public static int CompareTo(this float? @this, float? another)
        {
            if (@this == another) return 0;
            if (another == null) return @this < 0 ? -1 : 1;
            if (@this == null) return another < 0 ? 1 : -1;
            return @this.Value.CompareTo(another.Value);
        }

        public static int CompareTo(this decimal? @this, decimal? another)
        {
            if (@this == another) return 0;
            if (another == null) return @this < 0 ? -1 : 1;
            if (@this == null) return another < 0 ? 1 : -1;
            return @this.Value.CompareTo(another.Value);
        }
    }
}
