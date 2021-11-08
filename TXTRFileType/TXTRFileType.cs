using ManagedSquish;
using PaintDotNet;
using System;
using System.Diagnostics;
using System.IO;
using TXTRFileType.Extensions;
using TXTRFileType.GX;
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
            // TXTR is in big endian and ASCII
            using (var br = new PrimeBinaryReader(fs, true))
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

                int paletteSize = 0;
                ColorBgra[] paletteData = Array.Empty<ColorBgra>();
                if (textureFormat == TextureFormat.CI4 || textureFormat == TextureFormat.CI8 || textureFormat == TextureFormat.CI14X2)
                {
                    PaletteFormat paletteFormat = (PaletteFormat)br.ReadUInt32();
                    Debug.WriteLine("{0} = {1}", nameof(paletteFormat), paletteFormat);
                    ushort paletteWidth = br.ReadUInt16();
                    Debug.WriteLine("{0} = {1}", nameof(paletteWidth), paletteWidth);
                    ushort paletteHeight = br.ReadUInt16();
                    Debug.WriteLine("{0} = {1}", nameof(paletteHeight), paletteHeight);

                    // TODO: Which is correct?
                    //paletteSize = textureFormat == TextureFormat.CI14X2 ? 16384 : textureFormat == TextureFormat.CI8 ? 256 : 16;
                    paletteSize = paletteWidth * paletteHeight;
                    paletteData = new ColorBgra[paletteSize];
                    switch (paletteFormat)
                    {
                        case PaletteFormat.IA8:
                            for (int e = 0; e < paletteSize; ++e)
                            {
                                byte intensity = br.ReadByte();
                                byte alpha = br.ReadByte();
                                paletteData[e] = new ColorBgra
                                {
                                    R = intensity,
                                    G = intensity,
                                    B = intensity,
                                    A = alpha
                                };
                            }
                            break;
                        case PaletteFormat.RGB565:
                            for (int e = 0; e < paletteSize; ++e)
                            {
                                ushort texel = br.ReadUInt16();
                                paletteData[e] = new ColorBgra
                                {
                                    R = Texture.Convert5To8((byte)(texel >> 11 & 0x1F)),
                                    G = Texture.Convert6To8((byte)(texel >> 5 & 0x3F)),
                                    B = Texture.Convert5To8((byte)(texel & 0x1F)),
                                    A = 0xFF
                                };
                            }
                            break;
                        case PaletteFormat.RGB5A3:
                            for (int e = 0; e < paletteSize; ++e)
                            {
                                ushort texel = br.ReadUInt16();
                                if ((texel & 0x8000) != 0)
                                {
                                    paletteData[e] = new ColorBgra
                                    {
                                        R = Texture.Convert5To8((byte)(texel >> 10 & 0x1F)),
                                        G = Texture.Convert5To8((byte)(texel >> 5 & 0x1F)),
                                        B = Texture.Convert5To8((byte)(texel & 0x1F)),
                                        A = 0xFF
                                    };
                                }
                                else
                                {
                                    paletteData[e] = new ColorBgra
                                    {
                                        R = Texture.Convert4To8((byte)(texel >> 8 & 0xF)),
                                        G = Texture.Convert4To8((byte)(texel >> 4 & 0xF)),
                                        B = Texture.Convert4To8((byte)(texel & 0xF)),
                                        A = Texture.Convert3To8((byte)(texel >> 12 & 0x7))
                                    };
                                }
                            }
                            break;
                        default:
                            throw new NotSupportedException($"Palette format '{paletteFormat}' ({paletteFormat:X8}) is not supported");
                    }
                }
                if (paletteSize != 0 && paletteData.Length == 0)
                    throw new InvalidOperationException("Palette data is empty");

                ushort mipWidth = textureWidth,
                    mipHeight = textureHeight;
                for (int mipLevel = 0; mipLevel < mipCount; mipLevel++)
                {
                    Debug.WriteLine("Decoding mipmap {0}: width = {1}, height = {2}", mipLevel + 1, mipWidth, mipHeight);
                    if (mipWidth < 2 || mipHeight < 2)
                        throw new InvalidOperationException($"Mip {mipLevel + 1}: Width or Height less than 4. Mipmap count may be invalid.");
                    // Would be mipWidth and mipHeight but Paint.NET doesn't allow layers smaller than the image size.
                    BitmapLayer layer = PDNUtil.CreateLayer(textureWidth, textureHeight, $"Mipmap {mipLevel + 1}");
                    (ushort blockWidth, ushort blockHeight) = Texture.GetBlockDimensions(textureFormat);
                    (ushort blockWidthSize, ushort blockHeightSize) = Texture.GetBlockSize(mipWidth, mipHeight, blockWidth, blockHeight);

                    switch (textureFormat)
                    {
                        case TextureFormat.I4:
                            for (int blockY = 0; blockY < blockHeightSize; ++blockY)
                            {
                                int baseY = blockY * blockHeight;
                                for (int blockX = 0; blockX < blockWidthSize; ++blockX)
                                {
                                    int baseX = blockX * blockWidth;
                                    for (int y = 0; y < 8; ++y)
                                    {
                                        byte[] src = br.ReadBytes(4);
                                        for (int x = 0; x < 8; ++x)
                                        {
                                            // Only write valid pixels (extraneous pixels from GX blocks ignored)
                                            if (baseX + x <= mipWidth - 1 && baseY + y <= mipHeight - 1)
                                            {
                                                PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true,
                                                    Texture.Convert4To8((byte)(src[x / 2] >> ((x & 1) != 0 ? 0 : 4) & 0xf)),
                                                    0,
                                                    0,
                                                    255,
                                                    PDNUtil.ColorOptions.R_TO_G | PDNUtil.ColorOptions.R_TO_B);
                                                /*ColorBgra pixel = layer.Surface[baseX + x, baseY + y];
                                                pixel.R = Texture.Convert4To8((byte)(src[x / 2] >> ((x & 1) != 0 ? 0 : 4) & 0xf));
                                                pixel.G = pixel.R;
                                                pixel.B = pixel.R;
                                                pixel.A = 0xFF;
                                                layer.Surface[baseX + x, baseY + y] = pixel;*/
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case TextureFormat.I8:
                            for (int blockY = 0; blockY < blockHeightSize; ++blockY)
                            {
                                int baseY = blockY * blockHeight;
                                for (int blockX = 0; blockX < blockWidthSize; ++blockX)
                                {
                                    int baseX = blockX * blockWidth;
                                    for (int y = 0; y < 4; ++y)
                                    {
                                        byte[] src = br.ReadBytes(8);
                                        for (int x = 0; x < 8; ++x)
                                        {
                                            // Only write valid pixels (extraneous pixels from GX blocks ignored)
                                            if (baseX + x <= mipWidth - 1 && baseY + y <= mipHeight - 1)
                                            {
                                                PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true, src[x], src[x], src[x], 255);
                                                /*ColorBgra pixel = layer.Surface[baseX + x, baseY + y];
                                                pixel.R = src[x];
                                                pixel.G = src[x];
                                                pixel.B = src[x];
                                                pixel.A = 0xFF;
                                                layer.Surface[baseX + x, baseY + y] = pixel;*/
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case TextureFormat.IA4:
                            for (int blockY = 0; blockY < blockHeightSize; ++blockY)
                            {
                                int baseY = blockY * blockHeight;
                                for (int blockX = 0; blockX < blockWidthSize; ++blockX)
                                {
                                    int baseX = blockX * blockWidth;
                                    for (int y = 0; y < 4; ++y)
                                    {
                                        byte[] src = br.ReadBytes(8);
                                        for (int x = 0; x < src.Length; ++x)
                                        {
                                            // Only write valid pixels (extraneous pixels from GX blocks ignored)
                                            if (baseX + x <= mipWidth - 1 && baseY + y <= mipHeight - 1)
                                            {
                                                PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true,
                                                    Texture.Convert4To8((byte)(src[x] >> 4 & 0xF)),
                                                    0,
                                                    0,
                                                    Texture.Convert4To8((byte)(src[x] & 0xF)),
                                                    PDNUtil.ColorOptions.R_TO_G | PDNUtil.ColorOptions.R_TO_B);
                                                /*ColorBgra pixel = layer.Surface[baseX + x, baseY + y];
                                                byte intensity = Texture.Convert4To8((byte)(src[x] >> 4 & 0xF));
                                                pixel.R = intensity;
                                                pixel.G = intensity;
                                                pixel.B = intensity;
                                                pixel.A = Texture.Convert4To8((byte)(src[x] & 0xF));
                                                layer.Surface[baseX + x, baseY + y] = pixel;*/
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case TextureFormat.IA8:
                            for (int blockY = 0; blockY < blockHeightSize; ++blockY)
                            {
                                int baseY = blockY * blockHeight;
                                for (int blockX = 0; blockX < blockWidthSize; ++blockX)
                                {
                                    int baseX = blockX * blockWidth;
                                    for (int y = 0; y < 4; ++y)
                                    {
                                        ushort[] src = br.ReadUInt16(4);
                                        for (int x = 0; x < src.Length; ++x)
                                        {
                                            // Only write valid pixels (extraneous pixels from GX blocks ignored)
                                            if (baseX + x <= mipWidth - 1 && baseY + y <= mipHeight - 1)
                                            {
                                                PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true,
                                                    (byte)(src[x] >> 8),
                                                    0,
                                                    0,
                                                    (byte)(src[x] & 0xFF),
                                                    PDNUtil.ColorOptions.R_TO_G | PDNUtil.ColorOptions.R_TO_B);
                                                /*ColorBgra pixel = layer.Surface[baseX + x, baseY + y];
                                                byte intensity = (byte)(src[x] >> 8);
                                                pixel.R = intensity;
                                                pixel.G = intensity;
                                                pixel.B = intensity;
                                                pixel.A = (byte)(src[x] & 0xFF);
                                                layer.Surface[baseX + x, baseY + y] = pixel;*/
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case TextureFormat.CI4:
                            for (int blockY = 0; blockY < blockHeightSize; ++blockY)
                            {
                                int baseY = blockY * blockHeight;
                                for (int blockX = 0; blockX < blockWidthSize; ++blockX)
                                {
                                    int baseX = blockX * blockWidth;
                                    for (int y = 0; y < 8; ++y)
                                    {
                                        byte[] src = br.ReadBytes(4);
                                        for (int x = 0; x < 8; ++x)
                                        {
                                            // Only write valid pixels (extraneous pixels from GX blocks ignored)
                                            if (baseX + x <= mipWidth - 1 && baseY + y <= mipHeight - 1)
                                                PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true,
                                                    paletteData[src[x / 2] >> ((x & 1) != 0 ? 0 : 4) & 0xF]);
                                                //layer.Surface[baseX + x, baseY + y] = paletteData[src[x / 2] >> ((x & 1) != 0 ? 0 : 4) & 0xF];
                                        }
                                    }
                                }
                            }
                            break;
                        case TextureFormat.CI8:
                            for (int blockY = 0; blockY < blockHeightSize; ++blockY)
                            {
                                int baseY = blockY * blockHeight;
                                for (int blockX = 0; blockX < blockWidthSize; ++blockX)
                                {
                                    int baseX = blockX * blockWidth;
                                    for (int y = 0; y < 4; ++y)
                                    {
                                        byte[] src = br.ReadBytes(8);
                                        for (int x = 0; x < src.Length; ++x)
                                        {
                                            // Only write valid pixels (extraneous pixels from GX blocks ignored)
                                            if (baseX + x <= mipWidth - 1 && baseY + y <= mipHeight - 1)
                                                PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true, paletteData[src[x]]);
                                                //layer.Surface[baseX + x, baseY + y] = paletteData[src[x]];
                                        }
                                    }
                                }
                            }
                            break;
                        case TextureFormat.CI14X2:
                            // TODO: This may be incorrect!
                            for (int blockY = 0; blockY < blockHeightSize; ++blockY)
                            {
                                int baseY = blockY * blockHeight;
                                for (int blockX = 0; blockX < blockWidthSize; ++blockX)
                                {
                                    int baseX = blockX * blockWidth;
                                    for (int y = 0; y < 4; ++y)
                                    {
                                        byte[] src = br.ReadBytes(14);
                                        for (int x = 0; x < src.Length; ++x)
                                        {
                                            // Only write valid pixels (extraneous pixels from GX blocks ignored)
                                            if (baseX + x <= mipWidth - 1 && baseY + y <= mipHeight - 1)
                                                PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true, paletteData[src[x] << 2]);
                                                //layer.Surface[baseX + x, baseY + y] = paletteData[src[x] << 2];
                                        }
                                    }
                                }
                            }
                            break;
                        case TextureFormat.RGB565:
                            for (int blockY = 0; blockY < blockHeightSize; ++blockY)
                            {
                                int baseY = blockY * blockHeight;
                                for (int blockX = 0; blockX < blockWidthSize; ++blockX)
                                {
                                    int baseX = blockX * blockWidth;
                                    for (int y = 0; y < 4; ++y)
                                    {
                                        for (int x = 0; x < 4; ++x)
                                        {
                                            ushort texel = br.ReadUInt16();
                                            // Only write valid pixels (extraneous pixels from GX blocks ignored)
                                            if (baseX + x <= mipWidth - 1 && baseY + y <= mipHeight - 1)
                                            {
                                                PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true,
                                                    Texture.Convert5To8((byte)(texel >> 11 & 0x1F)),
                                                    Texture.Convert6To8((byte)(texel >> 5 & 0x3F)),
                                                    Texture.Convert5To8((byte)(texel & 0x1F)),
                                                    0xFF);
                                                /*ColorBgra pixel = layer.Surface[baseX + x, baseY + y];
                                                pixel.R = Texture.Convert5To8((byte)(texel >> 11 & 0x1F));
                                                pixel.G = Texture.Convert6To8((byte)(texel >> 5 & 0x3F));
                                                pixel.B = Texture.Convert5To8((byte)(texel & 0x1F));
                                                pixel.A = 0xFF;
                                                layer.Surface[baseX + x, baseY + y] = pixel;*/
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case TextureFormat.RGB5A3:
                            for (int blockY = 0; blockY < blockHeightSize; ++blockY)
                            {
                                int baseY = blockY * blockHeight;
                                for (int blockX = 0; blockX < blockWidthSize; ++blockX)
                                {
                                    int baseX = blockX * blockWidth;
                                    for (int y = 0; y < 4; ++y)
                                    {
                                        for (int x = 0; x < 4; ++x)
                                        {
                                            ushort texel = br.ReadUInt16();
                                            // Only write valid pixels (extraneous pixels from GX blocks ignored)
                                            if (baseX + x <= mipWidth - 1 && baseY + y <= mipHeight - 1)
                                            {
                                                //ColorBgra pixel = layer.Surface[baseX + x, baseY + y];
                                                if ((texel & 0x8000) != 0)
                                                {
                                                    // 15 bit color, no alpha
                                                    /*pixel.R = Texture.Convert5To8((byte)(texel >> 10 & 0x1F));
                                                    pixel.G = Texture.Convert5To8((byte)(texel >> 5 & 0x1F));
                                                    pixel.B = Texture.Convert5To8((byte)(texel & 0x1F));
                                                    pixel.A = 0xFF;*/
                                                    PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true,
                                                        Texture.Convert5To8((byte)(texel >> 10 & 0x1F)),
                                                        Texture.Convert5To8((byte)(texel >> 5 & 0x1F)),
                                                        Texture.Convert5To8((byte)(texel & 0x1F)),
                                                        0xFF);
                                                }
                                                else
                                                {
                                                    // 12 bit color, 3 bit alpha
                                                    /*pixel.R = Texture.Convert4To8((byte)(texel >> 8 & 0xF));
                                                    pixel.G = Texture.Convert4To8((byte)(texel >> 4 & 0xF));
                                                    pixel.B = Texture.Convert4To8((byte)(texel & 0xF));
                                                    pixel.A = Texture.Convert3To8((byte)(texel >> 12 & 0x7));*/
                                                    PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true,
                                                        Texture.Convert4To8((byte)(texel >> 8 & 0xF)),
                                                        Texture.Convert4To8((byte)(texel >> 4 & 0xF)),
                                                        Texture.Convert4To8((byte)(texel & 0xF)),
                                                        Texture.Convert3To8((byte)(texel >> 12 & 0x7)));
                                                }
                                                //layer.Surface[baseX + x, baseY + y] = pixel;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case TextureFormat.RGBA8:
                            for (int blockY = 0; blockY < blockHeightSize; ++blockY)
                            {
                                int baseY = blockY * blockHeight;
                                for (int blockX = 0; blockX < blockWidthSize; ++blockX)
                                {
                                    int baseX = blockX * blockWidth;
                                    for (int c = 0; c < 2; ++c)
                                    {
                                        for (int y = 0; y < 4; ++y)
                                        {
                                            byte[] src = br.ReadBytes(8);
                                            for (int x = 0; x < 4; ++x)
                                            {
                                                // Only write valid pixels (extraneous pixels from GX blocks ignored)
                                                if (baseX + x <= mipWidth - 1 && baseY + y <= mipHeight - 1)
                                                {
                                                    if (c != 0)
                                                    {
                                                        PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true,
                                                            0,
                                                            src[x * 2],
                                                            src[x * 2 + 1],
                                                            0,
                                                            PDNUtil.ColorOptions.KEEP_R | PDNUtil.ColorOptions.KEEP_A);
                                                        /*ColorBgra pixel = layer.Surface[baseX + x, baseY + y];
                                                        pixel.G = src[x * 2];
                                                        pixel.B = src[x * 2 + 1];
                                                        layer.Surface[baseX + x, baseY + y] = pixel;*/
                                                    }
                                                    else
                                                    {
                                                        PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true,
                                                            src[x * 2 + 1],
                                                            0,
                                                            0,
                                                            src[x * 2],
                                                            PDNUtil.ColorOptions.KEEP_G | PDNUtil.ColorOptions.KEEP_B);
                                                        /*ColorBgra pixel = layer.Surface[baseX + x, baseY + y];
                                                        pixel.A = src[x * 2];
                                                        pixel.R = src[x * 2 + 1];
                                                        layer.Surface[baseX + x, baseY + y] = pixel;*/
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case TextureFormat.CMPR:
                            /*byte[] src2 = Squish.DecompressImage(br.ReadBytes(mipWidth * mipHeight), mipWidth, mipHeight, SquishFlags.Dxt1GCN);
                            int stride = 4 * (mipWidth * 4 + 31) / 32;
                            int curRowOffs = 0;
                            for (int y = 0; y < mipHeight; y++)
                            {
                                // Set offset to start of current row
                                int curOffs = curRowOffs;
                                for (int x = 0; x < mipWidth; x++)
                                {
                                    PDNUtil.SetPixel(ref layer, x, y, false, true, new ColorBgra()
                                    {
                                        R = src2[curOffs],
                                        G = src2[curOffs + 1],
                                        B = src2[curOffs + 2],
                                        A = src2[curOffs + 3]
                                    });

                                    // Increase offset to next colour
                                    curOffs += 4;
                                }
                                // Increase row offset
                                curRowOffs += stride;
                            }*/
                            for (int blockY = 0; blockY < blockHeightSize; ++blockY)
                            {
                                int baseY = blockY * blockHeight;
                                for (int blockX = 0; blockX < blockWidthSize; ++blockX)
                                {
                                    int baseX = blockX * blockWidth;
                                    // 4x4 pixels * 4 * 1byte channels = 64byte
                                    byte[] src = Squish.Decompress(br.ReadBytes(8), SquishFlags.Dxt1GCN);
                                    for (int y = 0; y < blockHeight; ++y)
                                    {
                                        for (int x = 0; x < blockWidth; ++x)
                                        {
                                            // Only write valid pixels (extraneous pixels from GX blocks ignored)
                                            if (baseX + x <= mipWidth - 1 && baseY + y <= mipHeight - 1)
                                            {
                                                for (int sx = 0; x < 4; sx++)
                                                {
                                                    for (int sy = 0; y < 4; sy++)
                                                    {
                                                        PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true, new ColorBgra()
                                                        {
                                                            R = src[sy],
                                                            G = src[sy + 1],
                                                            B = src[sy + 2],
                                                            A = src[sy + 3]
                                                        });
                                                    }
                                                }
                                                /*PDNUtil.SetPixel(ref layer, baseX + x, baseY + y, false, true, new ColorBgra()
                                                {
                                                    R = (byte)(((src[(y * 4 + x)] >> 24) & 0xFF) / 255),
                                                    G = (byte)(((src[(y * 4 + x)] >> 16) & 0xFF) / 255),
                                                    B = (byte)(((src[(y * 4 + x)] >> 8) & 0xFF) / 255),
                                                    A = (byte)((src[(y * 4 + x)] & 0xFF) / 255)
                                                });*/
                                                /*ColorBgra pixel = layer.Surface[baseX + x, baseY + y];
                                                pixel.R = dst[x];
                                                pixel.G = dst[x + 1];
                                                pixel.B = dst[x + 2];
                                                pixel.A = dst[x + 3];
                                                layer.Surface[baseX + x, baseY + y] = pixel;*/
                                            }
                                        }
                                    }
                                }
                            }
                            /*for (int blockY = 0; blockY < blockHeightSize; ++blockY)
                            {
                                int baseY = blockY * blockHeight;
                                for (int blockX = 0; blockX < blockWidthSize; ++blockX)
                                {
                                    int baseX = blockX * blockWidth;
                                    byte[] src = br.ReadBytes(8);
                                    Squish.RGBA8[] dstDec = Squish.DecompressColourGCN(src);
                                    *//*Debug.WriteLine("dstDec");
                                    for (int grade = 0; grade < dstDec.GetLength(1); grade++)
                                    {
                                        for (int name = 0; name < dstDec.GetLength(0); name++)
                                        {
                                            Debug.WriteLine("{0, -15}", dstDec[name, grade]);
                                        }
                                    }*//*
                                    for (int x = 0; x < 4; x++)
                                    {
                                        for (int y = 0; y < 4; y++)
                                        {
                                            ColorBgra pixel = layer.Surface[baseX + x, baseY + y];
                                            pixel.R = dstDec[y].r;
                                            pixel.G = dstDec[y].g;
                                            pixel.B = dstDec[y].b;
                                            pixel.A = dstDec[y].a;
                                            layer.Surface[baseX + x, baseY + y] = pixel;
                                        }
                                    }
                                    *//*//Squish.DXT1Block src = br.ReadDXT1Block();
                                    byte[][] src = new byte[4][] { br.ReadBytes(8), br.ReadBytes(8), br.ReadBytes(8), br.ReadBytes(8) };
                                    Squish.RGBA8[][] dst = new Squish.RGBA8[4][] {
                                        Squish.DecompressColourGCN(src[0]), Squish.DecompressColourGCN(src[1]),
                                        Squish.DecompressColourGCN(src[2]), Squish.DecompressColourGCN(src[3])
                                    };
                                    //byte[] src = br.ReadBytes(8);
                                    //Squish.RGBA8 dst = Squish.DecompressColourGCN(src);
                                    for (int y = 0; y < 4; y++)
                                    {
                                        for (int x = 0; x < 4; x++)
                                        {
                                            ColorBgra pixel = layer.Surface[baseX + x, baseY + y];
                                            pixel.R = dst[y][x].r;
                                            pixel.G = dst[y][x].g;
                                            pixel.B = dst[y][x].b;
                                            pixel.A = dst[y][x].a;
                                            layer.Surface[baseX + x, baseY + y] = pixel;
                                        }
                                    }*//*
                                }
                            }*/
                            break;
                        default:
                            throw new NotSupportedException($"Texture format '{textureFormat}' ({textureFormat:X8}) is not supported");
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
