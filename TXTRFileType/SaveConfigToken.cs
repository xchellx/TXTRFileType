using libWiiSharp.GX;
using PaintDotNet;
using System;

namespace TXTRFileType
{
    [Serializable]
    internal class TXTRFileTypeSaveConfigToken : SaveConfigToken
    {
        public TextureFormat TextureFormat { get; set; }
        public PaletteFormat TexturePalette { get; set; }
        public bool GenerateMipmaps { get; set; }

        public TXTRFileTypeSaveConfigToken()
        {
            TextureFormat = TextureFormat.I4;
            TexturePalette = PaletteFormat.IA8;
            GenerateMipmaps = false;
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
