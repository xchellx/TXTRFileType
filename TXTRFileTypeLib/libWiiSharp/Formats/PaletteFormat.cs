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
    /// Enum that specifies what format a GX texture's palette is (how its pixel data is stored)
    /// </summary>
    [Description("Enum that specifies what format a GX texture's palette is (how its palette pixel data is stored)")]
    public enum PaletteFormat : uint
    {
        /// <summary>
        /// 8-bit greyscale intensity values with an additional 8-bit alpha channel.
        /// </summary>
        [Description(PaletteFormatDescriptions.IA8)]
        IA8 = 0x0U,
        /// <summary>
        /// 16-bit colors without alpha.
        /// </summary>
        [Description(PaletteFormatDescriptions.RGB565)]
        RGB565 = 0x1U,
        /// <summary>
        /// 16-bit colors with alpha.
        /// </summary>
        [Description(PaletteFormatDescriptions.RGB5A3)]
        RGB5A3 = 0x2U
    }
}
