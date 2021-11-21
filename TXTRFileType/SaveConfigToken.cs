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
    internal class TXTRFileTypeSaveConfigToken : SaveConfigToken
    {
        public TextureFormat TextureFormat { get; set; }
        public PaletteFormat TexturePalette { get; set; }
        public TextureConverter.PaletteLengthCopyLocation PaletteLengthCopyLocation { get; set; }
        public bool GenerateMipmaps { get; set; }

        public TXTRFileTypeSaveConfigToken()
        {
            TextureFormat = TextureFormat.I4;
            TexturePalette = PaletteFormat.IA8;
            PaletteLengthCopyLocation = TextureConverter.PaletteLengthCopyLocation.ToWidth;
            GenerateMipmaps = false;
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
