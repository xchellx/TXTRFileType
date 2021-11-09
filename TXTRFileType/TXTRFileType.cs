using libWiiSharp;
using libWiiSharp.GX;
using PaintDotNet;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using TXTRFileType.Extensions;
using TXTRFileType.IO;
using TXTRFileType.Util;

namespace TXTRFileType
{
    public sealed class TXTRFileTypeFactory : IFileTypeFactory
    {
        public FileType[] GetFileTypeInstances()
        {
            return new FileType[] { new TXTRFileTypePlugin() };
        }
    }

    [PluginSupportInfo(typeof(PluginSupportInfo))]
    internal class TXTRFileTypePlugin : FileType
    {
        /// <summary>
        /// Constructs a ExamplePropertyBasedFileType instance
        /// </summary>
        internal TXTRFileTypePlugin()
            : base(
                "Metroid Prime Texture",
                new FileTypeOptions
                {
                    LoadExtensions = new string[] { ".TXTR" },
                    SaveExtensions = new string[] { ".TXTR" },
                    SupportsCancellation = true,
                    SupportsLayers = true
                })
        {
        }

        protected override SaveConfigToken OnCreateDefaultSaveConfigToken()
        {
            return new TXTRFileTypeSaveConfigToken();
        }

        public override SaveConfigWidget CreateSaveConfigWidget()
        {
            return new TXTRFileTypeSaveConfigWidget();
        }

       /* /// <summary>
        /// Determines if the document was saved without altering the pixel values.
        ///
        /// Any settings that change the pixel values should return 'false'.
        ///
        /// Because Paint.NET prompts the user to flatten the image, flattening should not be
        /// considered.
        /// For example, a 32-bit PNG will return 'true' even if the document has multiple layers.
        /// </summary>
        public override bool IsReflexive(SaveConfigToken token)
        {
            TXTRFileTypeSaveConfigToken configToken = (TXTRFileTypeSaveConfigToken)token;
            return false;
            //return configToken.Invert == false;
        }*/

        /// <summary>
        /// Saves a document to a stream respecting the properties
        /// </summary>
        protected override void OnSave(Document input, Stream output, SaveConfigToken token, Surface scratchSurface, ProgressEventHandler progressCallback)
        {
            Debug.WriteLine("OnSave called!");
            TXTRFileTypeSaveConfigToken configToken = (TXTRFileTypeSaveConfigToken)token;
            /*GX.TextureFormat texFormat = configToken.TextureFormat;
            GX.PaletteFormat palFormat = configToken.TexturePalette;

            using (RenderArgs args = new RenderArgs(scratchSurface))
            {
                // Render a flattened view of the Document to the scratch surface.
                input.Render(args, true);
            }
            
            using (EndianBinaryWriter writer = new EndianBinaryWriter(output, isLittleEndian: false, Encoding.ASCII, leaveOpen: true))
            using (MemoryStream ms = new MemoryStream(scratchSurface.Scan0.ToByteArray()))
            using (Bitmap image = (Bitmap)Image.FromStream(ms))
            {
                uint texFormatTmp = (uint)texFormat;
                if (texFormatTmp == 10)
                {
                    writer.Write(0xA);
                }
                else
                {
                    writer.Write(texFormatTmp);
                }
                writer.Write((uint)palFormat);
                writer.Write(mipCount);
                writer.Write(scratchSurface.Width);
                writer.Write(scratchSurface.Height);

                byte[] imageData = null;
                switch (texFormat)
                {
                    case GX.TextureFormat.I4:
                        GX.To.Convert(image, scratchSurface, GX.To.Texture.I4);
                        break;*/
                    /*case GX.TextureFormat.I8:
                        imageData = GX.To.Convert(image, GX.To.Texture.I8);
                        break;
                    case GX.TextureFormat.IA4:
                        imageData = GX.To.Convert(image, GX.To.Texture.IA4);
                        break;
                    case GX.TextureFormat.IA8:
                        imageData = GX.To.Convert(image, GX.To.Texture.IA8);
                        break;
                    case GX.TextureFormat.C4:
                        imageData = GX.To.ConvertWithPalette(texData, palData, (int)width, (int)height, GX.To.Palette.C4);
                        break;
                    case GX.TextureFormat.C8:
                        imageData = GX.To.ConvertWithPalette(texData, palData, (int)width, (int)height, GX.To.Palette.C8);
                        break;
                    case GX.TextureFormat.C14X2:
                        imageData = GX.To.ConvertWithPalette(texData, palData, (int)width, (int)height, GX.To.Palette.C14X2);
                        break;
                    case GX.TextureFormat.RGB565:
                        imageData = GX.To.Convert(image, GX.To.Texture.RGB565);
                        break;
                    case GX.TextureFormat.RGB5A3:
                        imageData = GX.To.Convert(image, GX.To.Texture.RGB5A3);
                        break;
                    case GX.TextureFormat.RGBA32:
                        imageData = GX.To.Convert(image, GX.To.Texture.RGBA32);
                        break;
                    case GX.TextureFormat.CMPR:
                        imageData = GX.To.Convert(image, GX.To.Texture.CMPR);
                        break;*/
                    /*default:
                        ExceptionUtil.ThrowNotSupportedException($"TXTR format '{texFormat:X}' is not supported for saving");
                        break;
                }
            }*/

            //if (invert)
            //{
            //    new UnaryPixelOps.Invert().Apply(scratchSurface, scratchSurface.Bounds);
            //}

            // The stream paint.net hands us must not be closed.
            /*using (EndianBinaryWriter writer = new EndianBinaryWriter(output, isLittleEndian: false, Encoding.ASCII, leaveOpen: true))
            {
                // Write the file header.
                //writer.Write(Encoding.ASCII.GetBytes(HeaderSignature));
                writer.Write(scratchSurface.Width);
                writer.Write(scratchSurface.Height);

                for (int y = 0; y < scratchSurface.Height; y++)
                {
                    // Report progress if the callback is not null.
                    if (progressCallback != null)
                    {
                        double percent = (double)y / scratchSurface.Height;

                        progressCallback(null, new ProgressEventArgs(percent));
                    }

                    for (int x = 0; x < scratchSurface.Width; x++)
                    {
                        // Write the pixel values.
                        ColorBgra color = scratchSurface[x, y];

                        writer.Write(color.Bgra);
                    }
                }
            }*/
        }

        /// <summary>
        /// Creates a document from a stream
        /// </summary>
        protected override Document OnLoad(Stream fs)
        {
            Debug.WriteLine("OnLoad called!");
            var doc = new Document(1, 1);

            // The stream paint.net hands us must not be closed.
            // TXTR is in big endian
            using (var br = new EndianBinaryReader(fs, true, Encoding.Unicode, true))
            {
                TextureFormat textureFormat = (TextureFormat)br.ReadUInt32();
                Debug.WriteLine("{0} = {1}", nameof(textureFormat), textureFormat);
                ushort textureWidth = br.ReadUInt16();
                Debug.WriteLine("{0} = {1}", nameof(textureWidth), textureWidth);
                ushort textureHeight = br.ReadUInt16();
                Debug.WriteLine("{0} = {1}", nameof(textureHeight), textureHeight);
                uint mipCount = br.ReadUInt32();
                Debug.WriteLine("{0} = {1}", nameof(mipCount), mipCount);
                if (textureWidth < 1 || textureHeight < 1)
                    throw new InvalidOperationException($"Invalid dimensions: width='{textureWidth}', height={textureHeight}");
                doc = new Document(textureWidth, textureHeight);

                PaletteFormat paletteFormat = PaletteFormat.IA8;
                ushort paletteWidth = 0;
                ushort paletteHeight = 0;
                int paletteSize = 0;
                byte[] paletteData = Array.Empty<byte>();
                if (textureFormat == TextureFormat.CI4 || textureFormat == TextureFormat.CI8 || textureFormat == TextureFormat.CI14X2)
                {
                    paletteFormat = (PaletteFormat)br.ReadUInt32();
                    Debug.WriteLine("{0} = {1}", nameof(paletteFormat), paletteFormat);
                    paletteWidth = br.ReadUInt16();
                    Debug.WriteLine("{0} = {1}", nameof(paletteWidth), paletteWidth);
                    paletteHeight = br.ReadUInt16();
                    Debug.WriteLine("{0} = {1}", nameof(paletteHeight), paletteHeight);

                    // TODO: Which is correct?
                    //paletteSize = (textureFormat == TextureFormat.CI14X2 ? 16384 : textureFormat == TextureFormat.CI8 ? 256 : 16) * 2;
                    paletteSize = (paletteWidth * paletteHeight) * 2;
                    paletteData = br.ReadBytes(paletteSize);
                }
                if (paletteSize != 0 && paletteData.Length == 0)
                    throw new InvalidOperationException("Palette data is empty");

                ushort mipWidth = textureWidth;
                ushort mipHeight = textureHeight;
                for (int mipLevel = 0; mipLevel < mipCount; mipLevel++)
                {
                    Debug.WriteLine("Decoding mipmap {0}: width = {1}, height = {2}", mipLevel + 1, mipWidth, mipHeight);
                    if (mipWidth < 2 || mipHeight < 2)
                        throw new InvalidOperationException($"Mip {mipLevel + 1}: Width or Height less than 4. Mipmap count may be invalid.");
                    // Would be mipWidth and mipHeight but Paint.NET doesn't allow layers smaller than the image size.
                    BitmapLayer layer = PDNUtil.CreateLayer(textureWidth, textureHeight, $"Mipmap {mipLevel + 1}");

                    using (Image<Bgra32> image = TextureConverter.ExtractTexture(textureFormat, paletteFormat, br.ReadBytes(
                        TextureConverter.GetTextureSize(textureFormat, mipWidth, mipHeight)), paletteData, mipWidth, mipHeight))
                    {
                        for (int y = 0; y < image.Height; y++)
                        {
                            Span<Bgra32> row = image.GetPixelRowSpan(y);
                            for (int x = 0; x < row.Length; x++)
                            {
                                ref Bgra32 pixel = ref row[x];
                                PDNUtil.SetPixel(ref layer, x, y, false, paletteSize == 0, pixel.R, pixel.G, pixel.B, pixel.A);
                            }
                        }
                    }

                    doc.Layers.Add(layer);
                    mipWidth /= 2;
                    mipHeight /= 2;
                }
            }

            return doc;
        }
    }
}
