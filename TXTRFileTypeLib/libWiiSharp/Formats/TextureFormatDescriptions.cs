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
    internal static class TextureFormatDescriptions
    {
        public const string I4     = nameof(TextureFormat.I4) + " - 4-Bit Greyscale Without Alpha";
        public const string I8     = nameof(TextureFormat.I8) + " - 8-Bit Greyscale Without Alpha";
        public const string IA4    = nameof(TextureFormat.IA4) + " - 4-Bit Greyscale With Alpha";
        public const string IA8    = nameof(TextureFormat.IA8) + " - 8-Bit Greyscale With Alpha";
        public const string CI4    = nameof(TextureFormat.CI4) + " - 4-Bit Palette Indices";
        public const string CI8    = nameof(TextureFormat.CI8) + " - 8-Bit Palette Indices";
        public const string CI14X2 = nameof(TextureFormat.CI14X2) + " - 14-Bit Palette Indices";
        public const string RGB565 = nameof(TextureFormat.RGB565) + " - 16-Bit Color Without Alpha";
        public const string RGB5A3 = nameof(TextureFormat.RGB5A3) + " - 16-Bit Color With Alpha";
        public const string RGBA32 = nameof(TextureFormat.RGBA32) + " - 32-Bit Color With Alpha";
        public const string CMPR   = nameof(TextureFormat.CMPR) + " - DXT1 Compression";
    }
}
