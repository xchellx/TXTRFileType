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

namespace libWiiSharp
{
    /// <summary>
    /// This enum specifies whether the palette length should be copies to the
    /// palette width or palette height upon texture conversion
    /// </summary>
    [Description("This enum specifies whether the palette length should be copies to the palette width or palette height upon texture conversion")]
    public enum CopyPaletteSize
    {
        /// <summary>
        /// Copy palette length to width.
        /// </summary>
        [Description(CopyPaletteSizeDescriptions.ToWidth)]
        ToWidth,
        /// <summary>
        /// Copy palette length to height.
        /// </summary>
        [Description(CopyPaletteSizeDescriptions.ToHeight)]
        ToHeight
    }
}
