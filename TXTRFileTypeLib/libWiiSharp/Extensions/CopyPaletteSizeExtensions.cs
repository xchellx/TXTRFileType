/* This file is part of libWiiSharp
 * Copyright (C) 2009 Leathl
 * 
 * libWiiSharp is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * libWiiSharp is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.ComponentModel;

namespace libWiiSharp.Extensions
{
    /// <summary>
    /// Extension class that holds extensions for the <see cref="CopyPaletteSize"/> enum
    /// </summary>
    [Description("Extension class that holds extensions for the CopyPaletteSize enum")]
    public static class CopyPaletteSizeExtensions
    {
        /// <summary>
        /// Check whether a <see cref="CopyPaletteSize"/> enum is valid
        /// </summary>
        /// <param name="this">The <see cref="CopyPaletteSize"/> enum</param>
        /// <returns><see langword="true"/> if the <see cref="CopyPaletteSize"/> enum is valid else <see langword="false"/></returns>
        [Description("Check whether a CopyPaletteSize enum is valid")]
        public static bool IsDefined(this CopyPaletteSize @this)
            => @this switch
            {
                CopyPaletteSize.ToWidth  => true,
                CopyPaletteSize.ToHeight => true,
                _                        => false
            };

        /// <summary>
        /// Gets the constant known description of a <see cref="CopyPaletteSize"/> enum if provided
        /// </summary>
        /// <param name="this">The <see cref="CopyPaletteSize"/> enum</param>
        /// <returns>If provided, the description of the <see cref="CopyPaletteSize"/> enum else <see langword="null"/></returns>
        [Description("Gets the constant known description of a CopyPaletteSize enum if provided")]
        public static string? GetDescription(this CopyPaletteSize @this)
            => @this switch
            {
                CopyPaletteSize.ToWidth  => CopyPaletteSizeDescriptions.ToWidth,
                CopyPaletteSize.ToHeight => CopyPaletteSizeDescriptions.ToHeight,
                _                        => null
            };
    }
}
