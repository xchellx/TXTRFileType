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
