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
        public bool GenerateMipmaps { get; set; }
        public TextureConverter.PaletteLengthCopyLocation PaletteLengthCopyLocation { get; set; }

        public TXTRFileTypeSaveConfigToken()
        {
            TextureFormat = TextureFormat.I4;
            TexturePalette = PaletteFormat.IA8;
            GenerateMipmaps = false;
            PaletteLengthCopyLocation = TextureConverter.PaletteLengthCopyLocation.ToWidth;
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
