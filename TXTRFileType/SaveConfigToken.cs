/*
TXTRFileType
Copyright (C) 2021 xchellx

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using libWiiSharp;
using libWiiSharp.Formats;
using PaintDotNet;
using System;

namespace TXTRFileType
{
    [Serializable]
    internal class TXTRFileTypeSaveConfigToken : SaveConfigToken, ICloneable
    {
        public TextureFormat TextureFormat { get; set; } = TextureFormat.I4;
        public PaletteFormat TexturePalette { get; set; } = PaletteFormat.IA8;
        public TextureConverter.PaletteLengthCopyLocation PaletteLengthCopyLocation { get; set; } = TextureConverter.PaletteLengthCopyLocation.ToWidth;
        public bool GenerateMipmaps { get; set; } = false;
        public int MipSizeLimit { get; set; } = 1;

        public TXTRFileTypeSaveConfigToken()
        {
            TextureFormat = TextureFormat.I4;
            TexturePalette = PaletteFormat.IA8;
            PaletteLengthCopyLocation = TextureConverter.PaletteLengthCopyLocation.ToWidth;
            GenerateMipmaps = false;
            MipSizeLimit = 1;
        }

        public override object Clone()
        {
            return MipSizeLimit == default(int) ? GetDefault() : MemberwiseClone();
        }

        public static TXTRFileTypeSaveConfigToken GetDefault()
        {
            return new TXTRFileTypeSaveConfigToken()
            {
                TextureFormat = TextureFormat.I4,
                TexturePalette = PaletteFormat.IA8,
                PaletteLengthCopyLocation = TextureConverter.PaletteLengthCopyLocation.ToWidth,
                GenerateMipmaps = false,
                MipSizeLimit = 1
            };
        }
    }
}
