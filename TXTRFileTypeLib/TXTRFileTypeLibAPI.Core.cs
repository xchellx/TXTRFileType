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

using libWiiSharp.Formats;
using libWiiSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.IO;
using TXTRFileTypeLib.Util;
using SixLabors.ImageSharp.Processing;
using TXTRFileTypeLib.IO;
using System.Text;
using libWiiSharp.Extensions;

namespace TXTRFileTypeLib
{
    public static partial class TXTRFileTypeLibAPI
    {
        private static Image<Bgra32>[] ReadCore(Stream input,
            bool readMipmaps,
            bool keepStreamOpen,
            UpdateProgressDelegate? progressCallback)
        {
            void UpdateProgress(double progress, double max) => progressCallback?.Invoke(progress, max);
            double maxProgress = 0,
                currentProgress = 0;

            using (EndianBinaryReader inputReader = new(input, false, Encoding.ASCII, keepStreamOpen))
            {
                TextureFormat textureFormat = (TextureFormat)inputReader.ReadUInt32();
                if (!textureFormat.IsDefined())
                    throw new InvalidDataException(
                        $"Texture format '{textureFormat}' (0x{textureFormat.AsUInt32():X8}) is not supported");
                bool isIndexed = (textureFormat == TextureFormat.CI4 || textureFormat == TextureFormat.CI8
                    || textureFormat == TextureFormat.CI14X2);

                ushort textureWidth = inputReader.ReadUInt16();
                if (textureWidth < 1)
                    throw new InvalidDataException($"Texture width must be greater than 0: {textureWidth}");

                ushort textureHeight = inputReader.ReadUInt16();
                if (textureHeight < 1)
                    throw new InvalidDataException($"Texture height must be greater than 0: {textureHeight}");

                uint mipmapCount = inputReader.ReadUInt32();
                if (mipmapCount < 1)
                    throw new InvalidDataException($"Mipmap count must be greater than 0: {mipmapCount}");
                else if (mipmapCount > 1 && isIndexed)
                    throw new InvalidDataException(
                        $"Mipmap count must not be greater than 1 on indexed formats: {mipmapCount}");
                if (!readMipmaps)
                    mipmapCount = 1;
                maxProgress = mipmapCount;

                var mipmaps = new Image<Bgra32>[mipmapCount];
                try
                {
                    if (isIndexed)
                    {
                        PaletteFormat paletteFormat = (PaletteFormat)inputReader.ReadUInt32();
                        if (!paletteFormat.IsDefined())
                            throw new InvalidDataException(
                                $"Palette format '{paletteFormat}' (0x{paletteFormat.AsUInt32():X8}) is not supported");

                        ushort paletteWidth = inputReader.ReadUInt16();
                        if (paletteWidth < 1)
                            throw new InvalidDataException($"Palette width must be greater than 0: {paletteWidth}");

                        ushort paletteHeight = inputReader.ReadUInt16();
                        if (paletteHeight < 1)
                            throw new InvalidDataException($"Palette height must be greater than 0: {paletteHeight}");

                        int paletteSize = TextureConverter.GetPaletteSize(paletteFormat, paletteWidth, paletteHeight);
                        int maxCI4PaletteSize = TextureConverter.GetPaletteSize(paletteFormat, 16, 1);
                        int maxCI8PaletteSize = TextureConverter.GetPaletteSize(paletteFormat, 256, 1);
                        int maxCI14X2PaletteSize = TextureConverter.GetPaletteSize(paletteFormat, 16384, 1);
                        // Palette size is actually determined by the palette width and height.
                        // The assumption of CI14X2 = 16384, CI8 = 256, and CI4 = 16 is that of the maximum size. However, the
                        // actual size can be smaller than the maximum size depending on how much of the colors were converted.
                        if (paletteSize < 1)
                            throw new InvalidDataException("Palette data is empty");
                        else if (paletteSize > maxCI4PaletteSize && textureFormat == TextureFormat.CI4)
                            throw new InvalidDataException(
                                $"Palette size exceeds maximum palette size: {paletteSize} > {maxCI4PaletteSize}");
                        else if (paletteSize > maxCI8PaletteSize && textureFormat == TextureFormat.CI8)
                            throw new InvalidDataException(
                                $"Palette size exceeds maximum palette size: {paletteSize} > {maxCI8PaletteSize}");
                        else if (paletteSize > maxCI14X2PaletteSize && textureFormat == TextureFormat.CI14X2)
                            throw new InvalidDataException(
                                $"Palette size exceeds maximum palette size: {paletteSize} > {maxCI14X2PaletteSize}");
                        byte[] paletteData = inputReader.ReadBytes(paletteSize);

                        int mipmapSize = TextureConverter.GetTextureSize(textureFormat, textureWidth, textureHeight);
                        if (mipmapSize <= 0)
                            throw new InvalidDataException($"Mipmap 1 data is empty");

                        mipmaps[0] = TextureConverter.DecodeIndexedTexture(inputReader.ReadBytes(mipmapSize),
                            paletteData, textureWidth, textureHeight, textureFormat, paletteFormat);
                        // Do not flip indexed formats, the texture converter does the flipping for us
                        UpdateProgress(++currentProgress, maxProgress);
                    }
                    else
                    {
                        ushort mipmapWidth = textureWidth,
                            mipmapHeight = textureHeight;

                        for (int mipmapLevel = 0; mipmapLevel < mipmapCount; mipmapLevel++)
                        {
                            int mipmapSize = TextureConverter.GetTextureSize(textureFormat, mipmapWidth, mipmapHeight);
                            if (mipmapSize <= 0)
                                throw new InvalidDataException($"Mipmap {mipmapLevel + 1} data is empty");

                            mipmaps[mipmapLevel] = TextureConverter.DecodeTexture(inputReader.ReadBytes(mipmapSize),
                                mipmapWidth, mipmapHeight, textureFormat);
                            mipmaps[mipmapLevel].Mutate(ctx => ctx.Flip(FlipMode.Vertical));
                            UpdateProgress(++currentProgress, maxProgress);

                            mipmapWidth = (ushort)(mipmapWidth / 2);
                            if (mipmapWidth < 1)
                                throw new InvalidDataException($"Mipmap {mipmapLevel + 1} width must be greater than 0: {mipmapWidth}");

                            mipmapHeight = (ushort)Math.Max(mipmapHeight / 2, 0);
                            if (mipmapHeight < 1)
                                throw new InvalidDataException($"Mipmap {mipmapLevel + 1} height must be greater than 0: {mipmapHeight}");
                        }
                    }

                    return mipmaps;
                }
                catch
                {
                    foreach (Image<Bgra32> mipmap in mipmaps)
                        mipmap?.Dispose();

                    throw;
                }
            }
        }

        private static void WriteCore(Image<Bgra32> input,
            Stream output,
            TextureFormat textureFormat,
            PaletteFormat paletteFormat,
            CopyPaletteSize copyPaletteSize,
            bool generateMipmaps,
            int mipmapWidthLimit,
            int mipmapHeightLimit,
            bool keepStreamOpen,
            UpdateProgressDelegate? progressCallback)
        {
            void UpdateProgress(double progress, double max) => progressCallback?.Invoke(progress, max);
            double maxProgress = 0,
                currentProgress = 0;

            using (EndianBinaryWriter outputWriter = new(output, false, Encoding.ASCII, keepStreamOpen))
            {
                if (!textureFormat.IsDefined())
                    throw new InvalidDataException(
                        $"Texture format '{textureFormat}' (0x{textureFormat.AsUInt32():X8}) is not supported");
                outputWriter.Write(textureFormat.AsUInt32());
                bool isIndexed = (textureFormat == TextureFormat.CI4 || textureFormat == TextureFormat.CI8
                    || textureFormat == TextureFormat.CI14X2);

                if (input.Width < 1)
                    throw new InvalidDataException($"Texture width must be greater than 0: {input.Width}");
                else if (input.Width > ushort.MaxValue)
                    throw new InvalidDataException(
                        $"Texture width exceeds the max size of an unsigned short: {input.Width} > {ushort.MaxValue}");
                outputWriter.Write((ushort)input.Width);

                if (input.Height < 1)
                    throw new InvalidDataException($"Texture height must be greater than 0: {input.Height}");
                else if (input.Height > ushort.MaxValue)
                    throw new InvalidDataException(
                        $"Texture height exceeds the max size of an unsigned short: {input.Width} > {ushort.MaxValue}");
                outputWriter.Write((ushort)input.Height);

                // Mipmaps make no sense with indexed formats, especially with how TXTR is structured. Plus, there is not
                // a single example of a TXTR with an indexed format that includes mipmaps.
                uint mipmapCount = 1;
                if (generateMipmaps && !isIndexed)
                {
                    // Mipmap count is of the base texture + all the mipmaps
                    mipmapCount = (uint)Math.Clamp(ImageUtil.CountMipmaps(input.Width, input.Height,
                        mipmapWidthLimit, mipmapHeightLimit, true), uint.MinValue, uint.MaxValue);
                    // Make sure mipcount does not exceed minsize
                    uint maxMipCount = (uint)Math.Clamp(ImageUtil.CountMipmaps(input.Width, input.Height,
                        1, 1, true), uint.MinValue, uint.MaxValue);
                    if (mipmapCount > maxMipCount)
                        mipmapCount = maxMipCount;
                }
                if (mipmapCount < 1)
                    throw new InvalidDataException($"Mipmap count must be greater than 0: {mipmapCount}");
                else if (mipmapCount > 1 && isIndexed)
                    throw new InvalidDataException(
                        $"Mipmap count must not be greater than 1 on indexed formats: {mipmapCount}");
                outputWriter.Write((uint)mipmapCount);
                maxProgress = mipmapCount;

                if (isIndexed)
                {
                    if (!paletteFormat.IsDefined())
                        throw new InvalidDataException(
                            $"Palette format '{paletteFormat}' (0x{paletteFormat.AsUInt32():X8}) is not supported");
                    outputWriter.Write(paletteFormat.AsUInt32());

                    // Do not flip indexed formats, the texture converter does the flipping for us
                    (ushort paletteWidth, ushort paletteHeight) = TextureConverter.EncodeIndexedTexture(input,
                        out byte[] mipmapData, out byte[] paletteData, textureFormat, paletteFormat, copyPaletteSize);

                    if (paletteWidth < 1)
                        throw new InvalidDataException($"Palette width must be greater than 0: {paletteWidth}");
                    outputWriter.Write((ushort)paletteWidth);

                    if (paletteHeight < 1)
                        throw new InvalidDataException($"Palette height must be greater than 0: {paletteHeight}");
                    outputWriter.Write((ushort)paletteHeight);

                    int paletteSize = TextureConverter.GetPaletteSize(paletteFormat, paletteWidth, paletteHeight);
                    int maxCI4PaletteSize = TextureConverter.GetPaletteSize(paletteFormat, 16, 1);
                    int maxCI8PaletteSize = TextureConverter.GetPaletteSize(paletteFormat, 256, 1);
                    int maxCI14X2PaletteSize = TextureConverter.GetPaletteSize(paletteFormat, 16384, 1);
                    // Palette size is actually determined by the palette width and height.
                    // The assumption of CI14X2 = 16384, CI8 = 256, and CI4 = 16 is that of the maximum size. However, the
                    // actual size can be smaller than the maximum size depending on how much of the colors were converted.
                    if (paletteSize < 1)
                        throw new InvalidDataException("Palette data is empty");
                    else if (paletteSize > maxCI4PaletteSize && textureFormat == TextureFormat.CI4)
                        throw new InvalidDataException(
                            $"Palette size exceeds maximum palette size: {paletteSize} > {maxCI4PaletteSize}");
                    else if (paletteSize > maxCI8PaletteSize && textureFormat == TextureFormat.CI8)
                        throw new InvalidDataException(
                            $"Palette size exceeds maximum palette size: {paletteSize} > {maxCI8PaletteSize}");
                    else if (paletteSize > maxCI14X2PaletteSize && textureFormat == TextureFormat.CI14X2)
                        throw new InvalidDataException(
                            $"Palette size exceeds maximum palette size: {paletteSize} > {maxCI14X2PaletteSize}");
                    outputWriter.Write(paletteData);

                    outputWriter.Write(mipmapData);
                    UpdateProgress(++currentProgress, maxProgress);
                }
                else
                {
                    input.Mutate(ctx => ctx.Flip(FlipMode.Vertical));

                    ushort mipmapWidth = (ushort)input.Width,
                        mipmapHeight = (ushort)input.Height;
                    for (int mipmapLevel = 0; mipmapLevel < mipmapCount; mipmapLevel++)
                    {
                        if (mipmapLevel > 0)
                        {
                            mipmapWidth = (ushort)(mipmapWidth / 2);
                            mipmapHeight = (ushort)(mipmapHeight / 2);
                            input.Mutate(x => x.Resize(new ResizeOptions()
                            {
                                Mode = ResizeMode.Min,
                                Size = new Size(mipmapWidth, mipmapHeight),
                                Sampler = KnownResamplers.Box,
                                Compand = true,
                                PremultiplyAlpha = false // This is handled by the texture converter
                            }));
                        }

                        TextureConverter.EncodeTexture(input, out byte[] mipmapData, textureFormat);
                        outputWriter.Write(mipmapData);
                        UpdateProgress(++currentProgress, maxProgress);
                    }
                }
            }
        }
    }
}
