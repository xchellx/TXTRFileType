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

namespace libWiiSharp.Formats
{
    /// <summary>
    /// Enum that specifies what format a GX texture is (how its pixel data is stored)
    /// </summary>
    [Description("Enum that specifies what format a GX texture is (how its texture pixel data is stored)")]
    public enum TextureFormat : uint
    {
        /// <summary>
        /// 4-bit greyscale intensity values. Two pixels per byte.
        /// </summary>
        [Description(TextureFormatDescriptions.I4)]
        I4     = 0x0U,
        /// <summary>
        /// 8-bit greyscale intensity values.
        /// </summary>
        [Description(TextureFormatDescriptions.I8)]
        I8     = 0x1U,
        /// <summary>
        /// 4-bit greyscale intensity values with an additional 4-bit alpha channel.
        /// </summary>
        [Description(TextureFormatDescriptions.IA4)]
        IA4    = 0x2U,
        /// <summary>
        /// 8-bit greyscale intensity values with an additional 8-bit alpha channel.
        /// </summary>
        [Description(TextureFormatDescriptions.IA8)]
        IA8    = 0x3U,
        /// <summary>
        /// 4-bit palette indices.
        /// </summary>
        [Description(TextureFormatDescriptions.CI4)]
        CI4    = 0x4U,
        /// <summary>
        /// 8-bit palette indices.
        /// </summary>
        [Description(TextureFormatDescriptions.CI8)]
        CI8    = 0x5U,
        /// <summary>
        /// 14-bit palette indices (14b index).
        /// </summary>
        [Description(TextureFormatDescriptions.CI14X2)]
        CI14X2 = 0x6U,
        /// <summary>
        /// 16-bit colors without alpha.
        /// </summary>
        [Description(TextureFormatDescriptions.RGB565)]
        RGB565 = 0x7U,
        /// <summary>
        /// 16-bit colors with alpha.
        /// </summary>
        [Description(TextureFormatDescriptions.RGB5A3)]
        RGB5A3 = 0x8U,
        /// <summary>
        /// Uncompressed 32-bit colors with alpha.
        /// </summary>
        [Description(TextureFormatDescriptions.RGBA32)]
        RGBA32 = 0x9U,
        /// <summary>
        /// DXT1 Compression
        /// </summary>
        [Description(TextureFormatDescriptions.CMPR)]
        CMPR   = 0xAU
    }
}
