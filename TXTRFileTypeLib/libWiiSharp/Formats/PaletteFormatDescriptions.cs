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

namespace libWiiSharp.Formats
{
    internal static class PaletteFormatDescriptions
    {
        public const string IA8    = nameof(PaletteFormat.IA8) + " - 8-Bit Greyscale With Alpha";
        public const string RGB565 = nameof(PaletteFormat.RGB565) + " - 16-Bit Color Without Alpha";
        public const string RGB5A3 = nameof(PaletteFormat.RGB5A3) + " - 16-Bit Color With Alpha";
    }
}
