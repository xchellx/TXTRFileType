/* This file is part of libWiiSharp
 * Copyright (C) 2009 Leathl
 * 
 * libWiiSharp is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * libWiiSharp is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

//TPL conversion based on Wii.py by Xuzz, SquidMan, megazig, Matt_P, Omega and The Lemon Man.
//Zetsubou by SquidMan was also a reference.
//Thanks to the authors!

using libWiiSharp.Extensions;
using libWiiSharp.Formats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using TXTRFileTypeLib.Util;

namespace libWiiSharp
{
    internal static partial class TextureConverter
    {
        public static Image<Bgra32> DecodeTexture(byte[] textureData, int textureWidth, int textureHeight,
            TextureFormat textureFormat)
        {
            if (textureFormat != TextureFormat.CI4 && textureFormat != TextureFormat.CI8 && textureFormat != TextureFormat.CI14X2)
            {
                byte[] rgbaData = textureFormat switch
                {
                    TextureFormat.I4     => FromI4(textureData, textureWidth, textureHeight),
                    TextureFormat.I8     => FromI8(textureData, textureWidth, textureHeight),
                    TextureFormat.IA4    => FromIA4(textureData, textureWidth, textureHeight),
                    TextureFormat.IA8    => FromIA8(textureData, textureWidth, textureHeight),
                    TextureFormat.RGB565 => FromRGB565(textureData, textureWidth, textureHeight),
                    TextureFormat.RGB5A3 => FromRGB5A3(textureData, textureWidth, textureHeight),
                    TextureFormat.RGBA32 => FromRGBA32(textureData, textureWidth, textureHeight),
                    TextureFormat.CMPR   => FromCMPR(textureData, textureWidth, textureHeight),
                    _                    => throw new NotSupportedException($"Texture format '{textureFormat}' (0x{textureFormat.AsUInt32():X8}) is not supported"),
                };

                return RgbaToImage(rgbaData, textureWidth, textureHeight);
            }
            else
                throw new InvalidOperationException($"Use {nameof(DecodeIndexedTexture)} instead for indexed textures");
        }

        public static Image<Bgra32> DecodeIndexedTexture(byte[] textureData, byte[] paletteData, int textureWidth,
            int textureHeight, TextureFormat textureFormat, PaletteFormat paletteFormat)
        {
            if (textureFormat == TextureFormat.CI4 || textureFormat == TextureFormat.CI8 || textureFormat == TextureFormat.CI14X2)
            {
                byte[] rgbaData = null!;

                switch (textureFormat)
                {
                    case TextureFormat.CI4:
                        rgbaData = ColorIndexConverter.FromCI4(textureData, PaletteToRgba(paletteFormat, paletteData), textureWidth, textureHeight);
                        break;
                    case TextureFormat.CI8:
                        rgbaData = ColorIndexConverter.FromCI8(textureData, PaletteToRgba(paletteFormat, paletteData), textureWidth, textureHeight);
                        break;
                    case TextureFormat.CI14X2:
                        rgbaData = ColorIndexConverter.FromCI14X2(textureData, PaletteToRgba(paletteFormat, paletteData), textureWidth, textureHeight);
                        break;
                };

                return RgbaToImage(rgbaData, textureWidth, textureHeight);
            }
            else
                throw new InvalidOperationException($"Use {nameof(DecodeTexture)} instead for normal textures");
        }

        public static void EncodeTexture(Image<Bgra32> image, out byte[] textureData, TextureFormat textureFormat)
        {
            if (textureFormat != TextureFormat.CI4 && textureFormat != TextureFormat.CI8 && textureFormat != TextureFormat.CI14X2)
            {
                textureData = textureFormat switch
                {
                    TextureFormat.I4     => ToI4(image, textureFormat),
                    TextureFormat.I8     => ToI8(image, textureFormat),
                    TextureFormat.IA4    => ToIA4(image, textureFormat),
                    TextureFormat.IA8    => ToIA8(image, textureFormat),
                    TextureFormat.RGB565 => ToRGB565(image, textureFormat),
                    TextureFormat.RGB5A3 => ToRGB5A3(image, textureFormat),
                    TextureFormat.RGBA32 => ToRGBA32(image, textureFormat),
                    TextureFormat.CMPR   => ToCMPR(image, textureFormat),
                    _ => throw new NotSupportedException($"Texture format '{textureFormat}' (0x{textureFormat.AsUInt32():X8}) is not supported"),
                };
            }
            else
                throw new InvalidOperationException($"Use {nameof(EncodeIndexedTexture)} instead for indexed textures");
        }

        public static (ushort paletteWidth, ushort paletteHeight) EncodeIndexedTexture(Image<Bgra32> image,
            out byte[] textureData, out byte[] paletteData, TextureFormat textureFormat, PaletteFormat paletteFormat,
            CopyPaletteSize copyPaletteSize)
        {
            if (textureFormat == TextureFormat.CI4 || textureFormat == TextureFormat.CI8 || textureFormat == TextureFormat.CI14X2)
            {
                ColorIndexConverter cic = new(ImageToRgba(image), image.Width, image.Height, textureFormat, paletteFormat);

                switch (textureFormat)
                {
                    case TextureFormat.CI4:
                        cic.ToCI4();
                        break;
                    case TextureFormat.CI8:
                        cic.ToCI8();
                        break;
                    case TextureFormat.CI14X2:
                        cic.ToCI14X2();
                        break;
                }

                textureData = cic.Data;
                paletteData = cic.Palette;

                ushort paletteLength = (ushort)Math.Clamp(paletteData.Length / 2, ushort.MinValue, ushort.MaxValue);
                return copyPaletteSize switch
                {
                    CopyPaletteSize.ToWidth  => (paletteLength, 1),
                    CopyPaletteSize.ToHeight => (1, paletteLength),
                    _                        => throw new ArgumentException("Invalid copy palette size location", nameof(copyPaletteSize)),
                };
            }
            else
                throw new InvalidOperationException($"Use {nameof(EncodeTexture)} instead for normal textures");
        }

        private static uint[] ImageToRgba(Image<Bgra32> img)
        {
            return Shared.ByteArrayToUIntArray(ImageUtil.FromImage(img));
        }

        private static Image<Bgra32> RgbaToImage(byte[] data, int width, int height)
        {
            return ImageUtil.ToImage<Bgra32>(data, width, height);
        }

        private static uint[] PaletteToRgba(PaletteFormat paletteFormat, byte[] paletteData)
        {
            int itemcount = paletteData.Length / 2;
            int r, g, b, a;

            uint[] output = new uint[itemcount];
            for (int i = 0; i < itemcount; i++)
            {
                if (i >= itemcount) continue;

                ushort pixel = BitConverter.ToUInt16(new byte[] { paletteData[i * 2 + 1], paletteData[i * 2] }, 0);

                switch (paletteFormat)
                {
                    case PaletteFormat.IA8: //IA8
                        r = pixel & 0xff;
                        b = r;
                        g = r;
                        a = pixel >> 8;
                        break;
                    case PaletteFormat.RGB565: //RGB565
                        b = (((pixel >> 11) & 0x1F) << 3) & 0xff;
                        g = (((pixel >> 5) & 0x3F) << 2) & 0xff;
                        r = (((pixel >> 0) & 0x1F) << 3) & 0xff;
                        a = 255;
                        break;
                    case PaletteFormat.RGB5A3: //RGB5A3
                        if ((pixel & (1 << 15)) != 0) //RGB555
                        {
                            a = 255;
                            b = (((pixel >> 10) & 0x1F) * 255) / 31;
                            g = (((pixel >> 5) & 0x1F) * 255) / 31;
                            r = (((pixel >> 0) & 0x1F) * 255) / 31;
                        }
                        else //RGB4A3
                        {
                            a = (((pixel >> 12) & 0x07) * 255) / 7;
                            b = (((pixel >> 8) & 0x0F) * 255) / 15;
                            g = (((pixel >> 4) & 0x0F) * 255) / 15;
                            r = (((pixel >> 0) & 0x0F) * 255) / 15;
                        }
                        break;
                    default:
                        throw new NotSupportedException($"Palette format '{paletteFormat}' (0x{paletteFormat.AsUInt32():X8}) is not supported");
                }

                output[i] = (uint)((r << 0) | (g << 8) | (b << 16) | (a << 24));
            }

            return output;
        }

        private static void S3TC1ReverseBlock(ref byte[] block)
        {
            // Temporarily store endpoint bytes
            byte endpt1b1 = block[0], endpt1b2 = block[1], endpt2b1 = block[2], endpt2b2 = block[3];
            // Reverse bytes of endpoint 1 (ushort); 0xFFXX -> 0xXXFF
            block[0] = endpt1b2;
            block[1] = endpt1b1;
            // Reverse bytes of endpoint 2 (ushort); 0xFFXX -> 0xXXFF
            block[2] = endpt2b2;
            block[3] = endpt2b1;
            // Reverse the bits of the 4 indices (byte[4])
            block[4] = S3TC1ReverseByte(block[4]);
            block[5] = S3TC1ReverseByte(block[5]);
            block[6] = S3TC1ReverseByte(block[6]);
            block[7] = S3TC1ReverseByte(block[7]);
        }

        private static byte S3TC1ReverseByte(byte blockByte)
        {
            // Bits 1 and 2 -> 0bXXXX_XX11
            byte blockBit1 = (byte)(blockByte & 0b0000_0011);
            // Bits 3 and 4 -> 0bXXXX_11XX
            byte blockBit2 = (byte)(blockByte & 0b0000_1100);
            // Bits 5 and 6 -> 0bXX11_XXXX
            byte blockBit3 = (byte)(blockByte & 0b0011_0000);
            // Bits 7 and 8 -> 0b11XX_XXXX
            byte blockBit4 = (byte)(blockByte & 0b1100_0000);
            //   8675 4321      8675 4321 | Method
            // 0bXXXX_XX11 -> 0b11XX_XXXX | Shift left 6 bits
            // 0bXXXX_11XX -> 0bXX11_XXXX | Shift left 2 bits
            // 0bXX11_XXXX -> 0bXXXX_11XX | Shift right 2 bits
            // 0b11XX_XXXX -> 0bXXXX_XX11 | Shift right 6 bits
            return (byte)((blockBit1 << 6) | (blockBit2 << 2) | (blockBit3 >> 2) | (blockBit4 >> 6));
        }

        internal static int GetTextureSize(TextureFormat format, int width, int height)
            => format switch
            {
                TextureFormat.I4 or TextureFormat.CI4 or TextureFormat.CMPR => (width + 7 >> 3) * (height + 7 >> 3) * 32,
                TextureFormat.I8 or TextureFormat.IA4 or TextureFormat.CI8 => (width + 7 >> 3) * (height + 7 >> 2) * 32,
                TextureFormat.IA8 or TextureFormat.RGB565 or TextureFormat.RGB5A3 or TextureFormat.CI14X2 => (width + 3 >> 2) * (height + 3 >> 2) * 32,
                TextureFormat.RGBA32 => (width + 3 >> 2) * (height + 3 >> 2) * 64,
                _ => throw new NotSupportedException($"Texture format 0x{format.AsUInt32():X8} is not supported")
            };

        internal static int GetPaletteSize(PaletteFormat format, int width, int height)
            => format switch
            {
                // width * height [/ n OR * n] = blockSize
                PaletteFormat.IA8 => width * height * 2,
                PaletteFormat.RGB565 => width * height * 2,
                PaletteFormat.RGB5A3 => width * height * 2,
                _ => throw new NotSupportedException($"Palette format 0x{format.AsUInt32():X8} is not supported")
            };
    }
}
