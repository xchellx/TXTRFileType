using System;
using System.Reflection;

namespace TXTRFileType.Util
{
    /// <summary>
    /// Assembly utility class
    /// </summary>
    public class EnumUtil
    {
        /// <summary>
        /// Check if an enum is actually defined and named
        /// </summary>
        /// <param name="e">The enum to check</param>
        /// <returns><see langword="true"/> if the enum <paramref name="e"/> is defined and named</returns>
        public static bool IsFlagDefined(Enum e)
        {
            return !decimal.TryParse(e.ToString(), out _);
        }

        /// <summary>Convenience fields</summary>
        public static class StaticMembers
        {
        }
    }
}
