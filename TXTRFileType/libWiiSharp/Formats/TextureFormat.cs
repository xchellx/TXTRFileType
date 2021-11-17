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

using TXTRFileType.Util;

namespace libWiiSharp.Formats
{
    public enum TextureFormat : uint
    {
        /// <summary>
        /// 4-bit greyscale intensity values. Two pixels per byte.
        /// </summary>
        [UIUtil.EnumTitleForComboBox(nameof(I4) + " - 4-Bit Greyscale Without Alpha")]
        I4     = 0x0U,
        /// <summary>
        /// 8-bit greyscale intensity values.
        /// </summary>
        [UIUtil.EnumTitleForComboBox(nameof(I8) + " - 8-Bit Greyscale Without Alpha")]
        I8     = 0x1U,
        /// <summary>
        /// 4-bit greyscale intensity values with an additional 4-bit alpha channel.
        /// </summary>
        [UIUtil.EnumTitleForComboBox(nameof(IA4) + " - 4-Bit Greyscale With Alpha")]
        IA4    = 0x2U,
        /// <summary>
        /// 8-bit greyscale intensity values with an additional 8-bit alpha channel.
        /// </summary>
        [UIUtil.EnumTitleForComboBox(nameof(IA8) + " - 8-Bit Greyscale With Alpha")]
        IA8    = 0x3U,
        /// <summary>
        /// 4-bit palette indices.
        /// </summary>
        [UIUtil.EnumTitleForComboBox(nameof(CI4) + " - 4-Bit Palette Indices")]
        CI4    = 0x4U,
        /// <summary>
        /// 8-bit palette indices.
        /// </summary>
        [UIUtil.EnumTitleForComboBox(nameof(CI8) + " - 8-Bit Palette Indices")]
        CI8    = 0x5U,
        /// <summary>
        /// 14-bit palette indices (14b index).
        /// </summary>
        [UIUtil.EnumTitleForComboBox(nameof(CI14X2) + " - 14-Bit Palette Indices")]
        CI14X2 = 0x6U,
        /// <summary>
        /// 16-bit colors without alpha.
        /// </summary>
        [UIUtil.EnumTitleForComboBox(nameof(RGB565) + " - 16-Bit Color Without Alpha")]
        RGB565 = 0x7U,
        /// <summary>
        /// 16-bit colors with alpha.
        /// </summary>
        [UIUtil.EnumTitleForComboBox(nameof(RGB5A3) + " - 16-Bit Color With Alpha")]
        RGB5A3 = 0x8U,
        /// <summary>
        /// Uncompressed 32-bit colors with alpha.
        /// </summary>
        [UIUtil.EnumTitleForComboBox(nameof(RGBA32) + " - 32-Bit Color With Alpha")]
        RGBA32 = 0x9U,
        /// <summary>
        /// Compressed textures (almost the same as DXT1, but with a couple small differences)
        /// </summary>
        [UIUtil.EnumTitleForComboBox(nameof(CMPR) + " - DXT1 Compression")]
        CMPR   = 0xAU
    }
}
