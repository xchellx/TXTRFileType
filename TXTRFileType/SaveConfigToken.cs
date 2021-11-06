using PaintDotNet;
using System;

namespace TXTRFileType
{
    [Serializable]
    internal class TXTRFileTypeSaveConfigToken : SaveConfigToken
    {
        public GX.TextureFormat TextureFormat { get; set; }
        public GX.PaletteFormat TexturePalette { get; set; }

        public TXTRFileTypeSaveConfigToken()
        {
            TextureFormat = GX.TextureFormat.I4;
            TexturePalette = GX.PaletteFormat.IA8;
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
