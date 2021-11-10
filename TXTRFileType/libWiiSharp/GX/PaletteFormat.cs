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

namespace libWiiSharp.GX
{
    public enum PaletteFormat : uint
    {
        /// <summary>
        /// 8-bit greyscale intensity values with an additional 8-bit alpha channel.
        /// </summary>
        [UIUtil.EnumTitleForComboBox("IA8 - 8-Bit Greyscale With Alpha")]
        IA8 = 0x0U,
        /// <summary>
        /// 16-bit colors without alpha.
        /// </summary>
        [UIUtil.EnumTitleForComboBox("RGB565 - 16-Bit Color Without Alpha")]
        RGB565 = 0x1U,
        /// <summary>
        /// 16-bit colors with alpha.
        /// </summary>
        [UIUtil.EnumTitleForComboBox("RGB5A3 - 16-Bit Color With Alpha")]
        RGB5A3 = 0x2U
    }
}
