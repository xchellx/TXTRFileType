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
    public class TXTRSaveConfigToken : SaveConfigToken, ICloneable
    {
        public TextureFormat TextureFormat { get; set; }
        public PaletteFormat PaletteFormat { get; set; }
        public CopyPaletteSize CopyPaletteSize { get; set; }
        public bool GenerateMipmaps { get; set; }
        public int MipmapWidthLimit { get; set; }
        public int MipmapHeightLimit { get; set; }

        public TXTRSaveConfigToken()
        {
            TextureFormat = TextureFormat.I4;
            PaletteFormat = PaletteFormat.IA8;
            CopyPaletteSize = CopyPaletteSize.ToWidth;
            GenerateMipmaps = false;
            MipmapWidthLimit = 1;
            MipmapHeightLimit = 1;
        }

        // Why does Paint.NET call Clone when it wants the default token? Who knows!
        // Regardless, this hack is ugly and I wish there was a better way, such as
        // an overridable GetDefault method in SaveConfigToken.
        // 
        // If you change the save config, be sure to keep in mind this method as it
        // can break things if you don't do so. Sorry!
        public override object Clone()
            => MipmapWidthLimit == default ? GetDefault() : MemberwiseClone();

        public static TXTRSaveConfigToken GetDefault()
            => new()
            {
                TextureFormat = TextureFormat.I4,
                PaletteFormat = PaletteFormat.IA8,
                CopyPaletteSize = CopyPaletteSize.ToWidth,
                GenerateMipmaps = false,
                MipmapWidthLimit = 1,
                MipmapHeightLimit = 1
            };
    }
}
