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
    /// Extension class that holds extensions for the <see cref="TextureFormat"/> enum
    /// </summary>
    [Description("Extension class that holds extensions for the TextureFormat enum")]
    public static class TextureFormatExtensions
    {
        /// <summary>
        /// Convert the <see cref="TextureFormat"/> enum to an <see cref="uint"/>
        /// </summary>
        /// <param name="this">The <see cref="TextureFormat"/> enum</param>
        /// <returns>The <see cref="TextureFormat"/> enum as an <see cref="uint"/></returns>
        [Description("Convert the TextureFormat enum to an uint")]
        public static uint AsUInt32(this TextureFormat @this)
            => (uint)@this;

        /// <summary>
        /// Check whether a <see cref="TextureFormat"/> enum is valid
        /// </summary>
        /// <param name="this">The <see cref="TextureFormat"/> enum</param>
        /// <returns><see langword="true"/> if the <see cref="TextureFormat"/> enum is valid else <see langword="false"/></returns>
        [Description("Check whether a TextureFormat enum is valid")]
        public static bool IsDefined(this TextureFormat @this)
            => @this switch
            {
                TextureFormat.I4     => true,
                TextureFormat.I8     => true,
                TextureFormat.IA4    => true,
                TextureFormat.IA8    => true,
                TextureFormat.CI4    => true,
                TextureFormat.CI8    => true,
                TextureFormat.CI14X2 => true,
                TextureFormat.RGB565 => true,
                TextureFormat.RGB5A3 => true,
                TextureFormat.RGBA32 => true,
                TextureFormat.CMPR   => true,
                _                    => false
            };

        /// <summary>
        /// Gets the constant known description of a <see cref="TextureFormat"/> enum if provided
        /// </summary>
        /// <param name="this">The <see cref="TextureFormat"/> enum</param>
        /// <returns>If provided, the description of the <see cref="TextureFormat"/> enum else <see langword="null"/></returns>
        [Description("Gets the constant known description of a TextureFormat enum if provided")]
        public static string? GetDescription(this TextureFormat @this)
            => @this switch
            {
                TextureFormat.I4     => TextureFormatDescriptions.I4,
                TextureFormat.I8     => TextureFormatDescriptions.I8,
                TextureFormat.IA4    => TextureFormatDescriptions.IA4,
                TextureFormat.IA8    => TextureFormatDescriptions.IA8,
                TextureFormat.CI4    => TextureFormatDescriptions.CI4,
                TextureFormat.CI8    => TextureFormatDescriptions.CI8,
                TextureFormat.CI14X2 => TextureFormatDescriptions.CI14X2,
                TextureFormat.RGB565 => TextureFormatDescriptions.RGB565,
                TextureFormat.RGB5A3 => TextureFormatDescriptions.RGB5A3,
                TextureFormat.RGBA32 => TextureFormatDescriptions.RGBA32,
                TextureFormat.CMPR   => TextureFormatDescriptions.CMPR,
                _                    => null
            };
    }
}
