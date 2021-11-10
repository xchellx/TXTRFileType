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

using libWiiSharp.GX;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace libWiiSharp
{
    public static class TextureConverter
    {
        #region Public Functions
        public static Image<Bgra32> ExtractTexture(TextureFormat textureFormat, PaletteFormat paletteFormat, byte[] textureData,
            byte[] paletteData, int textureWidth, int textureHeight)
        {
            byte[] rgbaData;

            switch (textureFormat)
            {
                case TextureFormat.I4:
                    rgbaData = fromI4(textureData, textureWidth, textureHeight);
                    break;
                case TextureFormat.I8:
                    rgbaData = fromI8(textureData, textureWidth, textureHeight);
                    break;
                case TextureFormat.IA4:
                    rgbaData = fromIA4(textureData, textureWidth, textureHeight);
                    break;
                case TextureFormat.IA8:
                    rgbaData = fromIA8(textureData, textureWidth, textureHeight);
                    break;
                case TextureFormat.RGB565:
                    rgbaData = fromRGB565(textureData, textureWidth, textureHeight);
                    break;
                case TextureFormat.RGB5A3:
                    rgbaData = fromRGB5A3(textureData, textureWidth, textureHeight);
                    break;
                case TextureFormat.RGBA32:
                    rgbaData = fromRGBA32(textureData, textureWidth, textureHeight);
                    break;
                case TextureFormat.CI4:
                    rgbaData = fromCI4(textureData, paletteToRgba(paletteFormat, paletteData), textureWidth, textureHeight);
                    break;
                case TextureFormat.CI8:
                    rgbaData = fromCI8(textureData, paletteToRgba(paletteFormat, paletteData), textureWidth, textureHeight);
                    break;
                case TextureFormat.CI14X2:
                    rgbaData = fromCI14X2(textureData, paletteToRgba(paletteFormat, paletteData), textureWidth, textureHeight);
                    break;
                case TextureFormat.CMPR:
                    rgbaData = fromCMPR(textureData, textureWidth, textureHeight);
                    break;
                default:
                    throw new NotSupportedException($"Texture format '{textureFormat}' ({(uint)textureFormat:X8}) is not supported");
            }

            return rgbaToImage(rgbaData, textureWidth, textureHeight);
        }

        public static (byte[] textureData, byte[] paletteData, ushort paletteWidth, ushort paletteHeight) CreateTexture(
            TextureFormat textureFormat, PaletteFormat paletteFormat, Image<Bgra32> img)
        {
            byte[] textureData = Array.Empty<byte>();
            byte[] paletteData = Array.Empty<byte>();
            ushort paletteWidth = 0;
            ushort paletteHeight = 0;

            switch (textureFormat)
            {
                case TextureFormat.I4:
                    textureData = toI4(img);
                    break;
                case TextureFormat.I8:
                    textureData = toI8(img);
                    break;
                case TextureFormat.IA4:
                    textureData = toIA4(img);
                    break;
                case TextureFormat.IA8:
                    textureData = toIA8(img);
                    break;
                case TextureFormat.RGB565:
                    textureData = toRGB565(img);
                    break;
                case TextureFormat.RGB5A3:
                    textureData = toRGB5A3(img);
                    break;
                case TextureFormat.RGBA32:
                    textureData = toRGBA32(img);
                    break;
                case TextureFormat.CI4:
                case TextureFormat.CI8:
                case TextureFormat.CI14X2:
                    // Assigned later on
                    break;
                // TODO: Saving CMPR
                //case TextureFormat.CMPR:
                    //textureData = toCMPR(img);
                    //break;
                default:
                    throw new NotSupportedException($"Texture format '{textureFormat}' ({(uint)textureFormat:X8}) is not supported");
            }

            if (textureFormat == TextureFormat.CI4 || textureFormat == TextureFormat.CI8 || textureFormat == TextureFormat.CI14X2)
            {
                ColorIndexConverter cic = new ColorIndexConverter(imageToRgba(img), img.Width, img.Height, textureFormat, paletteFormat);

                textureData = cic.Data;
                paletteData = cic.Palette;

                paletteWidth = (ushort)(paletteData.Length / 2);
                paletteHeight = 1;
            }

            return (textureData: textureData, paletteData: paletteData, paletteWidth: paletteWidth, paletteHeight: paletteHeight);
        }

        public static int GetTextureSize(TextureFormat textureFormat, int width, int height)
        {
            switch (textureFormat)
            {
                case TextureFormat.I4:
                    return Shared.AddPadding(width, 8) * Shared.AddPadding(height, 8) / 2;
                case TextureFormat.I8:
                case TextureFormat.IA4:
                    return Shared.AddPadding(width, 8) * Shared.AddPadding(height, 4);
                case TextureFormat.IA8:
                case TextureFormat.RGB565:
                case TextureFormat.RGB5A3:
                    return Shared.AddPadding(width, 4) * Shared.AddPadding(height, 4) * 2;
                case TextureFormat.RGBA32:
                    return Shared.AddPadding(width, 4) * Shared.AddPadding(height, 4) * 4;
                case TextureFormat.CI4:
                    return Shared.AddPadding(width, 8) * Shared.AddPadding(height, 8) / 2;
                case TextureFormat.CI8:
                    return Shared.AddPadding(width, 8) * Shared.AddPadding(height, 4);
                case TextureFormat.CI14X2:
                    return Shared.AddPadding(width, 4) * Shared.AddPadding(height, 4) * 2;
                case TextureFormat.CMPR:
                    return width * height;
                default:
                    throw new NotSupportedException($"Texture format '{textureFormat}' ({(uint)textureFormat:X8}) is not supported");
            }
        }
        #endregion

        #region Private Functions
        private static uint[] imageToRgba(Image<Bgra32> img)
        {
            IMemoryGroup<Bgra32> iMem = img.GetPixelMemoryGroup();
            Memory<Bgra32> mem = iMem.ToArray()[0];
            return Shared.ByteArrayToUIntArray(MemoryMarshal.AsBytes(mem.Span).ToArray());
        }

        private static Image<Bgra32> rgbaToImage(byte[] data, int width, int height)
        {
            if (width == 0) width = 1;
            if (height == 0) height = 1;
            return Image.LoadPixelData<Bgra32>(data, width, height);
        }

        private static uint[] paletteToRgba(PaletteFormat paletteFormat, byte[] paletteData)
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
                        throw new NotSupportedException($"Palette format '{paletteFormat}' ({(uint)paletteFormat:X8}) is not supported");
                }

                output[i] = (uint)((r << 0) | (g << 8) | (b << 16) | (a << 24));
            }

            return output;
        }

        private static int avg(int w0, int w1, int c0, int c1)
        {
            int a0 = c0 >> 11;
            int a1 = c1 >> 11;
            int a = (w0 * a0 + w1 * a1) / (w0 + w1);
            int c = (a << 11) & 0xffff;

            a0 = (c0 >> 5) & 63;
            a1 = (c1 >> 5) & 63;
            a = (w0 * a0 + w1 * a1) / (w0 + w1);
            c = c | ((a << 5) & 0xffff);

            a0 = c0 & 31;
            a1 = c1 & 31;
            a = (w0 * a0 + w1 * a1) / (w0 + w1);
            c = c | a;

            return c;
        }
        #endregion

        #region Conversions
        #region RGBA32
        private static byte[] fromRGBA32(byte[] texture, int width, int height)
        {
            uint[] output = new uint[width * height];
            int inp = 0;

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        for (int y1 = y; y1 < y + 4; y1++)
                        {
                            for (int x1 = x; x1 < x + 4; x1++)
                            {
                                ushort pixel = Shared.Swap(BitConverter.ToUInt16(texture, inp++ * 2));

                                if ((x1 >= width) || (y1 >= height))
                                    continue;

                                if (k == 0)
                                {
                                    int a = (pixel >> 8) & 0xff;
                                    int r = (pixel >> 0) & 0xff;
                                    output[x1 + (y1 * width)] |= (uint)((r << 16) | (a << 24));
                                }
                                else
                                {
                                    int g = (pixel >> 8) & 0xff;
                                    int b = (pixel >> 0) & 0xff;
                                    output[x1 + (y1 * width)] |= (uint)((g << 8) | (b << 0));
                                }
                            }
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        private static byte[] toRGBA32(Image<Bgra32> img)
        {
            uint[] pixeldata = imageToRgba(img);
            int w = img.Width;
            int h = img.Height;
            int z = 0, iv = 0;
            byte[] output = new byte[Shared.AddPadding(w, 4) * Shared.AddPadding(h, 4) * 4];
            uint[] lr = new uint[32], lg = new uint[32], lb = new uint[32], la = new uint[32];

            for (int y1 = 0; y1 < h; y1 += 4)
            {
                for (int x1 = 0; x1 < w; x1 += 4)
                {
                    for (int y = y1; y < (y1 + 4); y++)
                    {
                        for (int x = x1; x < (x1 + 4); x++)
                        {
                            uint rgba;

                            if (y >= h || x >= w)
                                rgba = 0;
                            else
                                rgba = pixeldata[x + (y * w)];

                            lr[z] = (uint)(rgba >> 16) & 0xff;
                            lg[z] = (uint)(rgba >> 8) & 0xff;
                            lb[z] = (uint)(rgba >> 0) & 0xff;
                            la[z] = (uint)(rgba >> 24) & 0xff;

                            z++;
                        }
                    }

                    if (z == 16)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            output[iv++] = (byte)(la[i]);
                            output[iv++] = (byte)(lr[i]);
                        }
                        for (int i = 0; i < 16; i++)
                        {
                            output[iv++] = (byte)(lg[i]);
                            output[iv++] = (byte)(lb[i]);
                        }

                        z = 0;
                    }
                }
            }

            return output;
        }
        #endregion

        #region RGB5A3
        private static byte[] fromRGB5A3(byte[] texture, int width, int height)
        {
            uint[] output = new uint[width * height];
            int inp = 0;
            int r, g, b;
            int a = 0;

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    for (int y1 = y; y1 < y + 4; y1++)
                    {
                        for (int x1 = x; x1 < x + 4; x1++)
                        {
                            ushort pixel = Shared.Swap(BitConverter.ToUInt16(texture, inp++ * 2));

                            if (y1 >= height || x1 >= width)
                                continue;

                            if ((pixel & (1 << 15)) != 0)
                            {
                                b = (((pixel >> 10) & 0x1F) * 255) / 31;
                                g = (((pixel >> 5) & 0x1F) * 255) / 31;
                                r = (((pixel >> 0) & 0x1F) * 255) / 31;
                                a = 255;
                            }
                            else
                            {
                                a = (((pixel >> 12) & 0x07) * 255) / 7;
                                b = (((pixel >> 8) & 0x0F) * 255) / 15;
                                g = (((pixel >> 4) & 0x0F) * 255) / 15;
                                r = (((pixel >> 0) & 0x0F) * 255) / 15;
                            }

                            output[(y1 * width) + x1] = (uint)((r << 0) | (g << 8) | (b << 16) | (a << 24));
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        private static byte[] toRGB5A3(Image<Bgra32> img)
        {
            uint[] pixeldata = imageToRgba(img);
            int w = img.Width;
            int h = img.Height;
            int z = -1;
            byte[] output = new byte[Shared.AddPadding(w, 4) * Shared.AddPadding(h, 4) * 2];

            for (int y1 = 0; y1 < h; y1 += 4)
            {
                for (int x1 = 0; x1 < w; x1 += 4)
                {
                    for (int y = y1; y < y1 + 4; y++)
                    {
                        for (int x = x1; x < x1 + 4; x++)
                        {
                            int newpixel;

                            if (y >= h || x >= w)
                                newpixel = 0;
                            else
                            {
                                int rgba = (int)pixeldata[x + (y * w)];
                                newpixel = 0;

                                int r = (rgba >> 16) & 0xff;
                                int g = (rgba >> 8) & 0xff;
                                int b = (rgba >> 0) & 0xff;
                                int a = (rgba >> 24) & 0xff;

                                if (a <= 0xda) //RGB4A3
                                {
                                    newpixel &= ~(1 << 15);

                                    r = ((r * 15) / 255) & 0xf;
                                    g = ((g * 15) / 255) & 0xf;
                                    b = ((b * 15) / 255) & 0xf;
                                    a = ((a * 7) / 255) & 0x7;

                                    newpixel |= (a << 12) | (r << 8) | (g << 4) | b;
                                }
                                else //RGB5
                                {
                                    newpixel |= (1 << 15);

                                    r = ((r * 31) / 255) & 0x1f;
                                    g = ((g * 31) / 255) & 0x1f;
                                    b = ((b * 31) / 255) & 0x1f;

                                    newpixel |= (r << 10) | (g << 5) | b;
                                }
                            }

                            output[++z] = (byte)(newpixel >> 8);
                            output[++z] = (byte)(newpixel & 0xff);
                        }
                    }
                }
            }

            return output;
        }
        #endregion

        #region RGB565
        private static byte[] fromRGB565(byte[] texture, int width, int height)
        {
            uint[] output = new uint[width * height];
            int inp = 0;

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    for (int y1 = y; y1 < y + 4; y1++)
                    {
                        for (int x1 = x; x1 < x + 4; x1++)
                        {
                            ushort pixel = Shared.Swap(BitConverter.ToUInt16(texture, inp++ * 2));

                            if (y1 >= height || x1 >= width)
                                continue;

                            int b = (((pixel >> 11) & 0x1F) << 3) & 0xff;
                            int g = (((pixel >> 5) & 0x3F) << 2) & 0xff;
                            int r = (((pixel >> 0) & 0x1F) << 3) & 0xff;

                            output[y1 * width + x1] = (uint)((r << 0) | (g << 8) | (b << 16) | (255 << 24));
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        private static byte[] toRGB565(Image<Bgra32> img)
        {
            uint[] pixeldata = imageToRgba(img);
            int w = img.Width;
            int h = img.Height;
            int z = -1;
            byte[] output = new byte[Shared.AddPadding(w, 4) * Shared.AddPadding(h, 4) * 2];

            for (int y1 = 0; y1 < h; y1 += 4)
            {
                for (int x1 = 0; x1 < w; x1 += 4)
                {
                    for (int y = y1; y < y1 + 4; y++)
                    {
                        for (int x = x1; x < x1 + 4; x++)
                        {
                            ushort newpixel;

                            if (y >= h || x >= w)
                                newpixel = 0;
                            else
                            {
                                uint rgba = pixeldata[x + (y * w)];

                                uint b = (rgba >> 16) & 0xff;
                                uint g = (rgba >> 8) & 0xff;
                                uint r = (rgba >> 0) & 0xff;

                                newpixel = (ushort)(((b >> 3) << 11) | ((g >> 2) << 5) | ((r >> 3) << 0));
                            }

                            output[++z] = (byte)(newpixel >> 8);
                            output[++z] = (byte)(newpixel & 0xff);
                        }
                    }
                }
            }

            return output;
        }
        #endregion

        #region I4
        private static byte[] fromI4(byte[] texture, int width, int height)
        {
            uint[] output = new uint[width * height];
            int inp = 0;

            for (int y = 0; y < height; y += 8)
            {
                for (int x = 0; x < width; x += 8)
                {
                    for (int y1 = y; y1 < y + 8; y1++)
                    {
                        for (int x1 = x; x1 < x + 8; x1 += 2)
                        {
                            int pixel = texture[inp++];

                            if (y1 >= height || x1 >= width)
                                continue;

                            int i = (pixel >> 4) * 255 / 15;
                            output[y1 * width + x1] = (uint)((i << 0) | (i << 8) | (i << 16) | (255 << 24));

                            i = (pixel & 0x0F) * 255 / 15;
                            if (y1 * width + x1 + 1 < output.Length) output[y1 * width + x1 + 1] = (uint)((i << 0) | (i << 8) | (i << 16) | (255 << 24));
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        private static byte[] toI4(Image<Bgra32> img)
        {
            uint[] pixeldata = imageToRgba(img);
            int w = img.Width;
            int h = img.Height;
            int inp = 0;
            byte[] output = new byte[Shared.AddPadding(w, 8) * Shared.AddPadding(h, 8) / 2];

            for (int y1 = 0; y1 < h; y1 += 8)
            {
                for (int x1 = 0; x1 < w; x1 += 8)
                {
                    for (int y = y1; y < y1 + 8; y++)
                    {
                        for (int x = x1; x < x1 + 8; x += 2)
                        {
                            byte newpixel;

                            if (x >= w || y >= h)
                                newpixel = 0;
                            else
                            {
                                uint rgba = pixeldata[x + (y * w)];

                                uint r = (rgba >> 0) & 0xff;
                                uint g = (rgba >> 8) & 0xff;
                                uint b = (rgba >> 16) & 0xff;

                                uint i1 = ((r + g + b) / 3) & 0xff;

                                if ((x + (y * w) + 1) >= pixeldata.Length) rgba = 0;
                                else rgba = pixeldata[x + (y * w) + 1];

                                r = (rgba >> 0) & 0xff;
                                g = (rgba >> 8) & 0xff;
                                b = (rgba >> 16) & 0xff;

                                uint i2 = ((r + g + b) / 3) & 0xff;

                                newpixel = (byte)((((i1 * 15) / 255) << 4) | (((i2 * 15) / 255) & 0xf));
                            }

                            output[inp++] = newpixel;
                        }
                    }
                }
            }

            return output;
        }
        #endregion

        #region I8
        private static byte[] fromI8(byte[] texture, int width, int height)
        {
            uint[] output = new uint[width * height];
            int inp = 0;

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 8)
                {
                    for (int y1 = y; y1 < y + 4; y1++)
                    {
                        for (int x1 = x; x1 < x + 8; x1++)
                        {
                            int pixel = texture[inp++];

                            if (y1 >= height || x1 >= width)
                                continue;

                            output[y1 * width + x1] = (uint)((pixel << 0) | (pixel << 8) | (pixel << 16) | (255 << 24));
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        private static byte[] toI8(Image<Bgra32> img)
        {
            uint[] pixeldata = imageToRgba(img);
            int w = img.Width;
            int h = img.Height;
            int inp = 0;
            byte[] output = new byte[Shared.AddPadding(w, 8) * Shared.AddPadding(h, 4)];

            for (int y1 = 0; y1 < h; y1 += 4)
            {
                for (int x1 = 0; x1 < w; x1 += 8)
                {
                    for (int y = y1; y < y1 + 4; y++)
                    {
                        for (int x = x1; x < x1 + 8; x++)
                        {
                            byte newpixel;

                            if (x >= w || y >= h)
                                newpixel = 0;
                            else
                            {
                                uint rgba = pixeldata[x + (y * w)];

                                uint r = (rgba >> 0) & 0xff;
                                uint g = (rgba >> 8) & 0xff;
                                uint b = (rgba >> 16) & 0xff;

                                newpixel = (byte)(((r + g + b) / 3) & 0xff);
                            }

                            output[inp++] = newpixel;
                        }
                    }
                }
            }

            return output;
        }
        #endregion

        #region IA4
        private static byte[] fromIA4(byte[] texture, int width, int height)
        {
            uint[] output = new uint[width * height];
            int inp = 0;

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 8)
                {
                    for (int y1 = y; y1 < y + 4; y1++)
                    {
                        for (int x1 = x; x1 < x + 8; x1++)
                        {
                            int pixel = texture[inp++];

                            if (y1 >= height || x1 >= width)
                                continue;

                            int i = ((pixel & 0x0F) * 255 / 15) & 0xff;
                            int a = (((pixel >> 4) * 255) / 15) & 0xff;

                            output[y1 * width + x1] = (uint)((i << 0) | (i << 8) | (i << 16) | (a << 24));
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        private static byte[] toIA4(Image<Bgra32> img)
        {
            uint[] pixeldata = imageToRgba(img);
            int w = img.Width;
            int h = img.Height;
            int inp = 0;
            byte[] output = new byte[Shared.AddPadding(w, 8) * Shared.AddPadding(h, 4)];

            for (int y1 = 0; y1 < h; y1 += 4)
            {
                for (int x1 = 0; x1 < w; x1 += 8)
                {
                    for (int y = y1; y < y1 + 4; y++)
                    {
                        for (int x = x1; x < x1 + 8; x++)
                        {
                            byte newpixel;

                            if (x >= w || y >= h)
                                newpixel = 0;
                            else
                            {
                                uint rgba = pixeldata[x + (y * w)];

                                uint r = (rgba >> 0) & 0xff;
                                uint g = (rgba >> 8) & 0xff;
                                uint b = (rgba >> 16) & 0xff;

                                uint i = ((r + g + b) / 3) & 0xff;
                                uint a = (rgba >> 24) & 0xff;

                                newpixel = (byte)((((i * 15) / 255) & 0xf) | (((a * 15) / 255) << 4));
                            }

                            output[inp++] = newpixel;
                        }
                    }
                }
            }

            return output;
        }
        #endregion

        #region IA8
        private static byte[] fromIA8(byte[] texture, int width, int height)
        {
            uint[] output = new uint[width * height];
            int inp = 0;

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    for (int y1 = y; y1 < y + 4; y1++)
                    {
                        for (int x1 = x; x1 < x + 4; x1++)
                        {
                            int pixel = Shared.Swap(BitConverter.ToUInt16(texture, inp++ * 2));

                            if (y1 >= height || x1 >= width)
                                continue;

                            uint a = (uint)(pixel >> 8);
                            uint i = (uint)(pixel & 0xff);

                            output[y1 * width + x1] = (i << 0) | (i << 8) | (i << 16) | (a << 24);
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        private static byte[] toIA8(Image<Bgra32> img)
        {
            uint[] pixeldata = imageToRgba(img);
            int w = img.Width;
            int h = img.Height;
            int inp = 0;
            byte[] output = new byte[Shared.AddPadding(w, 4) * Shared.AddPadding(h, 4) * 2];

            for (int y1 = 0; y1 < h; y1 += 4)
            {
                for (int x1 = 0; x1 < w; x1 += 4)
                {
                    for (int y = y1; y < y1 + 4; y++)
                    {
                        for (int x = x1; x < x1 + 4; x++)
                        {
                            ushort newpixel;

                            if (x >= w || y >= h)
                                newpixel = 0;
                            else
                            {
                                uint rgba = pixeldata[x + (y * w)];

                                uint r = (rgba >> 0) & 0xff;
                                uint g = (rgba >> 8) & 0xff;
                                uint b = (rgba >> 16) & 0xff;

                                uint i = ((r + g + b) / 3) & 0xff;
                                uint a = (rgba >> 24) & 0xff;

                                newpixel = (ushort)((a << 8) | i);
                            }

                            byte[] temp = BitConverter.GetBytes(newpixel);
                            Array.Reverse(temp);

                            output[inp++] = (byte)(newpixel >> 8);
                            output[inp++] = (byte)(newpixel & 0xff);
                        }
                    }
                }
            }

            return output;
        }
        #endregion

        #region CI4
        private static byte[] fromCI4(byte[] texture, uint[] paletteData, int width, int height)
        {
            uint[] output = new uint[width * height];
            int i = 0;

            for (int y = 0; y < height; y += 8)
            {
                for (int x = 0; x < width; x += 8)
                {
                    for (int y1 = y; y1 < y + 8; y1++)
                    {
                        for (int x1 = x; x1 < x + 8; x1 += 2)
                        {
                            byte pixel = texture[i++];

                            if (y1 >= height || x1 >= width)
                                continue;

                            output[y1 * width + x1] = paletteData[pixel >> 4]; ;
                            if (y1 * width + x1 + 1 < output.Length) output[y1 * width + x1 + 1] = paletteData[pixel & 0x0F];
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        //toCI4 done in class ColorIndexConverter
        #endregion

        #region CI8
        private static byte[] fromCI8(byte[] texture, uint[] paletteData, int width, int height)
        {
            uint[] output = new uint[width * height];
            int i = 0;

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 8)
                {
                    for (int y1 = y; y1 < y + 4; y1++)
                    {
                        for (int x1 = x; x1 < x + 8; x1++)
                        {
                            ushort pixel = texture[i++];

                            if (y1 >= height || x1 >= width)
                                continue;

                            output[y1 * width + x1] = paletteData[pixel];
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        //toCI8 done in class ColorIndexConverter
        #endregion

        #region CI14X2
        private static byte[] fromCI14X2(byte[] texture, uint[] paletteData, int width, int height)
        {
            uint[] output = new uint[width * height];
            int i = 0;

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    for (int y1 = y; y1 < y + 4; y1++)
                    {
                        for (int x1 = x; x1 < x + 4; x1++)
                        {
                            ushort pixel = Shared.Swap(BitConverter.ToUInt16(texture, i++ * 2));

                            if (y1 >= height || x1 >= width)
                                continue;

                            output[y1 * width + x1] = paletteData[pixel & 0x3FFF];
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        //toCI14X2 done in class ColorIndexConverter
        #endregion

        #region CMPR
        private static byte[] fromCMPR(byte[] texture, int width, int height)
        {
            uint[] output = new uint[width * height];
            ushort[] c = new ushort[4];
            int[] pix = new int[4];
            int inp = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int ww = Shared.AddPadding(width, 8);

                    int x0 = x & 0x03;
                    int x1 = (x >> 2) & 0x01;
                    int x2 = x >> 3;

                    int y0 = y & 0x03;
                    int y1 = (y >> 2) & 0x01;
                    int y2 = y >> 3;

                    int off = (8 * x1) + (16 * y1) + (32 * x2) + (4 * ww * y2);

                    c[0] = Shared.Swap(BitConverter.ToUInt16(texture, off));
                    c[1] = Shared.Swap(BitConverter.ToUInt16(texture, off + 2));

                    if (c[0] > c[1])
                    {
                        c[2] = (ushort)avg(2, 1, c[0], c[1]);
                        c[3] = (ushort)avg(1, 2, c[0], c[1]);
                    }
                    else
                    {
                        c[2] = (ushort)avg(1, 1, c[0], c[1]);
                        c[3] = 0;
                    }

                    uint pixel = Shared.Swap(BitConverter.ToUInt32(texture, off + 4));

                    int ix = x0 + (4 * y0);
                    int raw = c[(pixel >> (30 - (2 * ix))) & 0x03];

                    pix[0] = (raw >> 8) & 0xf8;
                    pix[1] = (raw >> 3) & 0xf8;
                    pix[2] = (raw << 3) & 0xf8;
                    pix[3] = 0xff;
                    if (((pixel >> (30 - (2 * ix))) & 0x03) == 3 && c[0] <= c[1]) pix[3] = 0x00;

                    output[inp] = (uint)((pix[0] << 16) | (pix[1] << 8) | (pix[2] << 0) | (pix[3] << 24));
                    inp++;
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        //There's currently no conversion to CMPR
        #endregion
        #endregion

        #region ColorIndexConverter
        private class ColorIndexConverter
        {
            private uint[] rgbaPalette;
            private byte[] texturePalette;
            private uint[] rgbaData;
            private byte[] textureData;
            private TextureFormat textureFormat;
            private PaletteFormat paletteFormat;
            private int width;
            private int height;

            public byte[] Palette { get { return texturePalette; } }
            public byte[] Data { get { return textureData; } }

            public ColorIndexConverter(uint[] rgbaData, int width, int height, TextureFormat textureFormat, PaletteFormat paletteFormat)
            {
                if (textureFormat != TextureFormat.CI4 && textureFormat != TextureFormat.CI8 && textureFormat != TextureFormat.CI14X2)
                    throw new Exception("Texture format must be either CI4 or CI8 or CI14X2!");
                if (paletteFormat != PaletteFormat.IA8 && paletteFormat != PaletteFormat.RGB565 && paletteFormat != PaletteFormat.RGB5A3)
                    throw new Exception("Palette format must be either IA8, RGB565 or RGB5A3!");

                this.rgbaData = rgbaData;
                this.width = width;
                this.height = height;
                this.textureFormat = textureFormat;
                this.paletteFormat = paletteFormat;

                buildPalette();

                if (textureFormat == TextureFormat.CI4) toCI4();
                else if (textureFormat == TextureFormat.CI8) toCI8();
                else toCI14X2();
            }

            #region Private Functions
            private void toCI4()
            {
                byte[] indexData = new byte[libWiiSharp.Shared.AddPadding(width, 8) * libWiiSharp.Shared.AddPadding(height, 8) / 2];
                int i = 0;

                for (int y = 0; y < height; y += 8)
                {
                    for (int x = 0; x < width; x += 8)
                    {
                        for (int y1 = y; y1 < y + 8; y1++)
                        {
                            for (int x1 = x; x1 < x + 8; x1 += 2)
                            {
                                uint pixel;

                                if (y1 >= height || x1 >= width)
                                    pixel = 0;
                                else
                                    pixel = rgbaData[y1 * width + x1];

                                uint index1 = getColorIndex(pixel);

                                if (y1 >= height || x1 >= width)
                                    pixel = 0;
                                else if (y1 * width + x1 + 1 >= rgbaData.Length)
                                    pixel = 0;
                                else
                                    pixel = rgbaData[y1 * width + x1 + 1];

                                uint index2 = getColorIndex(pixel);

                                indexData[i++] = (byte)(((byte)index1 << 4) | (byte)index2);
                            }
                        }
                    }
                }

                this.textureData = indexData;
            }

            private void toCI8()
            {
                byte[] indexData = new byte[libWiiSharp.Shared.AddPadding(width, 8) * libWiiSharp.Shared.AddPadding(height, 4)];
                int i = 0;

                for (int y = 0; y < height; y += 4)
                {
                    for (int x = 0; x < width; x += 8)
                    {
                        for (int y1 = y; y1 < y + 4; y1++)
                        {
                            for (int x1 = x; x1 < x + 8; x1++)
                            {
                                uint pixel;

                                if (y1 >= height || x1 >= width)
                                    pixel = 0;
                                else
                                    pixel = rgbaData[y1 * width + x1];

                                indexData[i++] = (byte)getColorIndex(pixel);
                            }
                        }
                    }
                }

                this.textureData = indexData;
            }

            private void toCI14X2()
            {
                byte[] indexData = new byte[libWiiSharp.Shared.AddPadding(width, 4) * libWiiSharp.Shared.AddPadding(height, 4) * 2];
                int i = 0;

                for (int y = 0; y < height; y += 4)
                {
                    for (int x = 0; x < width; x += 4)
                    {
                        for (int y1 = y; y1 < y + 4; y1++)
                        {
                            for (int x1 = x; x1 < x + 4; x1++)
                            {
                                uint pixel;

                                if (y1 >= height || x1 >= width)
                                    pixel = 0;
                                else
                                    pixel = rgbaData[y1 * width + x1];

                                byte[] temp = BitConverter.GetBytes((ushort)getColorIndex(pixel));
                                indexData[i++] = temp[1];
                                indexData[i++] = temp[0];
                            }
                        }
                    }
                }

                this.textureData = indexData;
            }

            private void buildPalette()
            {
                int palLength = 256;
                if (textureFormat == TextureFormat.CI4) palLength = 16;
                else if (textureFormat == TextureFormat.CI14X2) palLength = 16384;

                List<uint> palette = new List<uint>();
                List<ushort> tPalette = new List<ushort>();

                palette.Add(0);
                tPalette.Add(0);

                for (int i = 1; i < rgbaData.Length; i++)
                {
                    if (palette.Count == palLength) break;
                    if (((rgbaData[i] >> 24) & 0xff) < ((textureFormat == TextureFormat.CI14X2) ? 1 : 25)) continue;

                    ushort textureValue = libWiiSharp.Shared.Swap(convertToPaletteValue((int)rgbaData[i]));

                    if (!palette.Contains(rgbaData[i]) && !tPalette.Contains(textureValue))
                    {
                        palette.Add(rgbaData[i]);
                        tPalette.Add(textureValue);
                    }
                }

                while (palette.Count % 16 != 0)
                { palette.Add(0xffffffff); tPalette.Add(0xffff); }

                texturePalette = libWiiSharp.Shared.UShortArrayToByteArray(tPalette.ToArray());
                rgbaPalette = palette.ToArray();
            }

            private ushort convertToPaletteValue(int rgba)
            {
                int newpixel = 0, r, g, b, a;

                if (paletteFormat == PaletteFormat.IA8)
                {
                    int intensity = ((((rgba >> 0) & 0xff) + ((rgba >> 8) & 0xff) + ((rgba >> 16) & 0xff)) / 3) & 0xff;
                    int alpha = (rgba >> 24) & 0xff;

                    newpixel = (ushort)((alpha << 8) | intensity);
                }
                else if (paletteFormat == PaletteFormat.RGB565)
                {
                    newpixel = (ushort)(((((rgba >> 16) & 0xff) >> 3) << 11) | ((((rgba >> 8) & 0xff) >> 2) << 5) | ((((rgba >> 0) & 0xff) >> 3) << 0));
                }
                else
                {
                    r = (rgba >> 16) & 0xff;
                    g = (rgba >> 8) & 0xff;
                    b = (rgba >> 0) & 0xff;
                    a = (rgba >> 24) & 0xff;

                    if (a <= 0xda) //RGB4A3
                    {
                        newpixel &= ~(1 << 15);

                        r = ((r * 15) / 255) & 0xf;
                        g = ((g * 15) / 255) & 0xf;
                        b = ((b * 15) / 255) & 0xf;
                        a = ((a * 7) / 255) & 0x7;

                        newpixel |= a << 12;
                        newpixel |= b << 0;
                        newpixel |= g << 4;
                        newpixel |= r << 8;
                    }
                    else //RGB5
                    {
                        newpixel |= (1 << 15);

                        r = ((r * 31) / 255) & 0x1f;
                        g = ((g * 31) / 255) & 0x1f;
                        b = ((b * 31) / 255) & 0x1f;

                        newpixel |= b << 0;
                        newpixel |= g << 5;
                        newpixel |= r << 10;
                    }
                }

                return (ushort)newpixel;
            }

            private uint getColorIndex(uint value)
            {
                uint minDistance = 0x7FFFFFFF;
                uint colorIndex = 0;

                if (((value >> 24) & 0xFF) < ((textureFormat == TextureFormat.CI14X2) ? 1 : 25)) return 0;
                ushort color = convertToPaletteValue((int)value);

                for (int i = 0; i < rgbaPalette.Length; i++)
                {
                    ushort curPal = convertToPaletteValue((int)rgbaPalette[i]);

                    if (color == curPal) return (uint)i;
                    uint curDistance = getDistance(color, curPal); //(uint)Math.Abs(Math.Abs(color) - Math.Abs(curVal));

                    if (curDistance < minDistance)
                    {
                        minDistance = curDistance;
                        colorIndex = (uint)i;
                    }
                }

                return colorIndex;
            }

            private uint getDistance(ushort color, ushort paletteColor)
            {
                uint curCol = convertToRgbaValue(color);
                uint palCol = convertToRgbaValue(paletteColor);

                uint curA = (curCol >> 24) & 0xFF;
                uint curR = (curCol >> 16) & 0xFF;
                uint curG = (curCol >> 8) & 0xFF;
                uint curB = (curCol >> 0) & 0xFF;

                uint palA = (palCol >> 24) & 0xFF;
                uint palR = (palCol >> 16) & 0xFF;
                uint palG = (palCol >> 8) & 0xFF;
                uint palB = (palCol >> 0) & 0xFF;

                uint distA = Math.Max(curA, palA) - Math.Min(curA, palA);
                uint distR = Math.Max(curR, palR) - Math.Min(curR, palR);
                uint distG = Math.Max(curG, palG) - Math.Min(curG, palG);
                uint distB = Math.Max(curB, palB) - Math.Min(curB, palB);

                return distA + distR + distG + distB;
            }

            private uint convertToRgbaValue(ushort pixel)
            {
                int rgba = 0, r, g, b, a;

                if (paletteFormat == PaletteFormat.IA8)
                {
                    int i = (pixel >> 8);
                    a = pixel & 0xff;

                    rgba = (i << 0) | (i << 8) | (i << 16) | (a << 24);
                }
                else if (paletteFormat == PaletteFormat.RGB565)
                {
                    b = (((pixel >> 11) & 0x1F) << 3) & 0xff;
                    g = (((pixel >> 5) & 0x3F) << 2) & 0xff;
                    r = (((pixel >> 0) & 0x1F) << 3) & 0xff;
                    a = 255;

                    rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);
                }
                else
                {
                    if ((pixel & (1 << 15)) != 0)
                    {
                        b = (((pixel >> 10) & 0x1F) * 255) / 31;
                        g = (((pixel >> 5) & 0x1F) * 255) / 31;
                        r = (((pixel >> 0) & 0x1F) * 255) / 31;
                        a = 255;
                    }
                    else
                    {
                        a = (((pixel >> 12) & 0x07) * 255) / 7;
                        b = (((pixel >> 8) & 0x0F) * 255) / 15;
                        g = (((pixel >> 4) & 0x0F) * 255) / 15;
                        r = (((pixel >> 0) & 0x0F) * 255) / 15;
                    }

                    rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);
                }

                return (uint)rgba;
            }
            #endregion
        }
        #endregion
    }
}
