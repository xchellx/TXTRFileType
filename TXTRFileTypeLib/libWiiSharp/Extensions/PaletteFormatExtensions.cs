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

using libWiiSharp.Formats;
using System.ComponentModel;

namespace libWiiSharp.Extensions
{
    /// <summary>
    /// Extension class that holds extensions for the <see cref="PaletteFormat"/> enum
    /// </summary>
    [Description("Extension class that holds extensions for the PaletteFormat enum")]
    public static class PaletteFormatExtensions
    {
        /// <summary>
        /// Convert the <see cref="PaletteFormat"/> enum to an <see cref="uint"/>
        /// </summary>
        /// <param name="this">The <see cref="PaletteFormat"/> enum</param>
        /// <returns>The <see cref="PaletteFormat"/> enum as an <see cref="uint"/></returns>
        [Description("Convert the PaletteFormat enum to an uint")]
        public static uint AsUInt32(this PaletteFormat @this)
            => (uint)@this;

        /// <summary>
        /// Check whether a <see cref="PaletteFormat"/> enum is valid
        /// </summary>
        /// <param name="this">The <see cref="PaletteFormat"/> enum</param>
        /// <returns><see langword="true"/> if the <see cref="PaletteFormat"/> enum is valid else <see langword="false"/></returns>
        [Description("Check whether a PaletteFormat enum is valid")]
        public static bool IsDefined(this PaletteFormat @this)
            => @this switch
            {
                PaletteFormat.IA8    => true,
                PaletteFormat.RGB565 => true,
                PaletteFormat.RGB5A3 => true,
                _                    => false,
            };

        /// <summary>
        /// Gets the constant known description of a <see cref="PaletteFormat"/> enum if provided
        /// </summary>
        /// <param name="this">The <see cref="PaletteFormat"/> enum</param>
        /// <returns>If provided, the description of the <see cref="PaletteFormat"/> enum else <see langword="null"/></returns>
        [Description("Gets the constant known description of a PaletteFormat enum if provided")]
        public static string? GetDescription(this PaletteFormat @this)
            => @this switch
            {
                PaletteFormat.IA8    => PaletteFormatDescriptions.IA8,
                PaletteFormat.RGB565 => PaletteFormatDescriptions.RGB565,
                PaletteFormat.RGB5A3 => PaletteFormatDescriptions.RGB5A3,
                _                    => null,
            };
    }
}
