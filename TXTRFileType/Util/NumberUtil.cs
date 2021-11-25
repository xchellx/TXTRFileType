using System;

namespace TXTRFileType.Util
{
    /// <summary>
    /// Utility class with useful methods for number related stuff such as casting and maths
    /// </summary>
    public static class NumberUtil
    {
        /// <summary>
        /// Safely cast a <see cref="decimal"/> to an <see cref="int"/>
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The <see cref="decimal"/> value safely casted to <see cref="int"/></returns>
        public static int ToInt(decimal value)
            => (int)Math.Max(int.MinValue, Math.Min(int.MaxValue, value));

        /// <summary>Convenience fields</summary>
        public static class StaticMembers
        {
        }
    }
}
