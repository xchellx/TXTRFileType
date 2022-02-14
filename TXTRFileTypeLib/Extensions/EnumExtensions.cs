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
using System.ComponentModel;
using System.Reflection;

namespace TXTRFileTypeLib.Extensions
{
    /// <summary>
    /// Extension class that holds extensions for the <see cref="Enum"/> class (and derivatives)
    /// </summary>
    [Description("Extension class that holds extensions for the Enum class (and derivatives)")]
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the string from the <see cref="DescriptionAttribute"/> of the enum field
        /// </summary>
        /// <param name="this">The <see cref="Enum"/> instance</param>
        /// <returns>The description string from the enum's <see cref="DescriptionAttribute"/> else <see langword="null"/></returns>
        [Description("Get the string from the DescriptionAttribute of the enum field")]
        public static string? GetDescription(this Enum? @this)
        {
            FieldInfo? enumField = @this?.GetType()?.GetField(@this?.ToString() ?? string.Empty);
            if (enumField != null)
                return (Attribute.GetCustomAttribute(enumField, typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description;
            else
                return null;
        }
    }
}
