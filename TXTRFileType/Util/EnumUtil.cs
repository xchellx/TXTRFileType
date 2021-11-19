/*
TXTRFileType
Copyright (C) 2021 xchellx

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Reflection;

namespace TXTRFileType.Util
{
    /// <summary>
    /// Enum utility class
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
