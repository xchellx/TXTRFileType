using libWiiSharp;
using libWiiSharp.Formats;
using PaintDotNet;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
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
        /// Constructs a TXTRFileTypePlugin instance
        /// </summary>
        internal TXTRFileTypePlugin()
            : base(
                "Metroid Prime Texture",
                new FileTypeOptions
                {
                    LoadExtensions = new string[] { ".TXTR" },
                    SaveExtensions = new string[] { ".TXTR" },
                    SupportsCancellation = true,
                    SupportsLayers = false
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

        /// <summary>
        /// The minimum size a mipmap can reach by width and height
        /// </summary>
        private const int sizeLimit = 4;

        /// <summary>
        /// Saves a document to a stream respecting the properties
        /// </summary>
        protected override void OnSave(Document input, Stream output, SaveConfigToken token, Surface scratchSurface, ProgressEventHandler progressCallback)
        {
            // Publish progress function convenience function
            Action<double> progressCallbackFunc = (p) => { if (progressCallback != null) progressCallback(null, new ProgressEventArgs(p, true)); };
            
            // Get save configuration values from token
            TXTRFileTypeSaveConfigToken configToken = (TXTRFileTypeSaveConfigToken)token;
            TextureFormat textureFormat = configToken.TextureFormat;
            PaletteFormat paletteFormat = configToken.TexturePalette;
            bool generateMipmaps = configToken.GenerateMipmaps;
            TextureConverter.PaletteLengthCopyLocation paletteLengthCopyLocation = configToken.PaletteLengthCopyLocation;

            bool isIndexed = (textureFormat == TextureFormat.CI4 || textureFormat == TextureFormat.CI8 || textureFormat == TextureFormat.CI14X2);

            // Count mipamps
            int mipCount = 1;
            // Mipmaps make no sense with indexed formats, especially with how TXTR is structured. Plus, there is not
            // a single example of a TXTR with an indexed format that includes mipmaps.
            if (generateMipmaps && !isIndexed)
                mipCount = ImageUtil.CountMips(input.Width, input.Height, sizeLimit);

            int maxProgress = mipCount;
            int curProgress = 0;

            // The stream paint.net hands us must not be closed.
            // TXTR is in big endian
            using (EndianBinaryWriter bw = new EndianBinaryWriter(output, false, Encoding.Unicode, true))
            {
                ushort mipWidth = (ushort)input.Width;
                ushort mipHeight = (ushort)input.Height;

                // Write header
                bw.Write((uint)textureFormat);
                bw.Write((ushort)input.Width);
                bw.Write((ushort)input.Height);
                bw.Write((uint)mipCount);

                // Load pixels to image
                // Only use the first layer, as mipmaps have to generate from the base (largest) layer
                using (Image<Bgra32> imgPdn = ImageUtil.ToImage<Bgra32>(((BitmapLayer)input.Layers[0]).Surface.Scan0.ToByteArray(),
                    mipWidth, mipHeight))
                {
                    // Flip indexed formats on save only (texture converter did the flipping for us at load)
                    if (isIndexed)
                        imgPdn.Mutate(x => x.Flip(FlipMode.Vertical));
                    (byte[] textureData, byte[] paletteData, ushort paletteWidth, ushort paletteHeight)
                        = TextureConverter.CreateTexture(textureFormat, paletteFormat, paletteLengthCopyLocation, imgPdn);

                    // Write palette
                    if (isIndexed)
                    {
                        bw.Write((uint)paletteFormat);
                        bw.Write((ushort)paletteWidth);
                        bw.Write((ushort)paletteHeight);
                        bw.Write(paletteData);
                    }

                    // Write main image
                    bw.Write(textureData);
                    ++curProgress;
                    progressCallbackFunc((curProgress / maxProgress) * 100);

                    // Write mipmaps
                    for (int mipLevel = 0; mipLevel < mipCount - 1; mipLevel++)
                    {
                        mipWidth /= 2;
                        mipHeight /= 2;

                        // Resize image with box filter 
                        imgPdn.Mutate(x => x.Resize(new ResizeOptions()
                        {
                            Mode = ResizeMode.Min,
                            Size = new Size(mipWidth, mipHeight),
                            Sampler = KnownResamplers.Box,
                            Compand = true,
                            PremultiplyAlpha = false // This is handled by the texture converter
                        }));

                        // Write mip image
                        (byte[] mipData, _, _, _) = TextureConverter.CreateTexture(textureFormat, paletteFormat,
                            paletteLengthCopyLocation, imgPdn);
                        bw.Write(mipData);
                        ++curProgress;
                        progressCallbackFunc((curProgress / maxProgress) * 100);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a document from a stream
        /// </summary>
        protected override Document OnLoad(Stream input)
        {
            // The stream paint.net hands us must not be closed.
            // TXTR is in big endian
            using (EndianBinaryReader br = new EndianBinaryReader(input, false, Encoding.Unicode, true))
            {
                // Read header
                TextureFormat textureFormat = (TextureFormat)br.ReadUInt32();
                Debug.WriteLine("{0} = {1}", nameof(textureFormat), textureFormat);
                ushort textureWidth = br.ReadUInt16();
                Debug.WriteLine("{0} = {1}", nameof(textureWidth), textureWidth);
                ushort textureHeight = br.ReadUInt16();
                Debug.WriteLine("{0} = {1}", nameof(textureHeight), textureHeight);
                uint mipCount = br.ReadUInt32();
                if (mipCount < 1)
                    throw new InvalidDataException($"Invalid mipmap count: '{mipCount}'");
                Debug.WriteLine("{0} = {1}", nameof(mipCount), mipCount);
                if (textureWidth < 1 || textureHeight < 1)
                    throw new InvalidDataException($"Invalid dimensions: width='{textureWidth}', height={textureHeight}");

                Document document = new Document(textureWidth, textureHeight);
                bool isIndexed = (textureFormat == TextureFormat.CI4 || textureFormat == TextureFormat.CI8 || textureFormat == TextureFormat.CI14X2);

                // Read palette
                PaletteFormat paletteFormat = PaletteFormat.IA8;
                ushort paletteWidth = 0;
                ushort paletteHeight = 0;
                int paletteSize = 0;
                byte[] paletteData = Array.Empty<byte>();
                if (isIndexed)
                {
                    paletteFormat = (PaletteFormat)br.ReadUInt32();
                    Debug.WriteLine("{0} = {1}", nameof(paletteFormat), paletteFormat);
                    paletteWidth = br.ReadUInt16();
                    Debug.WriteLine("{0} = {1}", nameof(paletteWidth), paletteWidth);
                    paletteHeight = br.ReadUInt16();
                    Debug.WriteLine("{0} = {1}", nameof(paletteHeight), paletteHeight);

                    // Palette size is actually determined by the palette width and height.
                    // The assumption of CI14X2 = 16384, CI8 = 256, and CI4 = 16 is that of the maximum size. However, the
                    // actual size can be smaller than the maximum size depending on how much of the colors where converted.
                    paletteSize = (paletteWidth * paletteHeight) * 2;
                    if ((paletteSize > (16 * 2) && textureFormat == TextureFormat.CI4)
                        || (paletteSize > (256 * 2) && textureFormat == TextureFormat.CI8)
                        || (paletteSize > (16384 * 2) && textureFormat == TextureFormat.CI14X2))
                        throw new InvalidDataException("Palette data exceeds maximum palette size)");
                    paletteData = br.ReadBytes(paletteSize);
                }
                if (paletteSize != 0 && paletteData.Length == 0)
                    throw new InvalidDataException("Palette data is empty");

                // Read main image and mipmaps
                ushort mipWidth = textureWidth;
                ushort mipHeight = textureHeight;
                for (int mipLevel = 0; mipLevel < mipCount; mipLevel++)
                {
                    Debug.WriteLine("Decoding mipmap {0}: width = {1}, height = {2}", mipLevel + 1, mipWidth, mipHeight);
                    if (mipWidth < sizeLimit || mipHeight < sizeLimit)
                        throw new InvalidDataException($"Mip {mipLevel + 1}: Width or Height less than {sizeLimit}. Mipmap count may be invalid.");
                    // Would be mipWidth and mipHeight but Paint.NET doesn't allow layers smaller than the image size.
                    // However, this doesn't really matter in the end.
                    BitmapLayer layer = PDNUtil.CreateLayer(textureWidth, textureHeight, $"Mipmap {mipLevel + 1}");

                    // Convert image data and load it
                    using (Image<Bgra32> image = TextureConverter.ExtractTexture(textureFormat, paletteFormat,
                        br.ReadBytes(TextureConverter.GetTextureSize(textureFormat, mipWidth, mipHeight)), paletteData, mipWidth, mipHeight))
                    {
                        // Write converted pixel data to Paint.NET layer
                        for (int y = 0; y < image.Height; y++)
                        {
                            Span<Bgra32> row = image.GetPixelRowSpan(y);
                            for (int x = 0; x < row.Length; x++)
                            {
                                ref Bgra32 pixel = ref row[x];
                                // Flip indexed formats on save only (texture converter did the flipping for us at load)
                                PDNUtil.SetPixel(ref layer, x, y, false, !isIndexed, pixel.R, pixel.G, pixel.B, pixel.A);
                            }
                        }
                    }

                    document.Layers.Add(layer);
                    mipWidth /= 2;
                    mipHeight /= 2;
                }

                return document;
            }
        }
    }
}
