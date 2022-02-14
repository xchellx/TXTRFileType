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

using libWiiSharp.Formats;
using System;
using System.Collections.Generic;

namespace libWiiSharp
{
    internal class ColorIndexConverter
    {
        private uint[] rgbaPalette;
        private byte[] texturePalette;
        private byte[] textureData;
        private readonly uint[] rgbaData;
        private readonly TextureFormat textureFormat;
        private readonly PaletteFormat paletteFormat;
        private readonly int width;
        private readonly int height;

        public byte[] Palette { get => texturePalette; }
        public byte[] Data { get => textureData; }

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
            this.texturePalette = Array.Empty<byte>();
            this.rgbaPalette = Array.Empty<uint>();
            this.textureData = Array.Empty<byte>();

            BuildPalette();
        }

        public static byte[] FromCI4(byte[] texture, uint[] paletteData, int width, int height)
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
                            if (y1 >= height || x1 >= width)
                                continue;

                            byte pixel = texture[i++];

                            output[y1 * width + x1] = paletteData[pixel >> 4]; ;
                            if (y1 * width + x1 + 1 < output.Length) output[y1 * width + x1 + 1] = paletteData[pixel & 0x0F];
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        public void ToCI4()
        {
            byte[] indexData = new byte[TextureConverter.GetTextureSize(textureFormat, width, height)];
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

                            uint index1 = GetColorIndex(pixel);

                            if (y1 >= height || x1 >= width)
                                pixel = 0;
                            else if (y1 * width + x1 + 1 >= rgbaData.Length)
                                pixel = 0;
                            else
                                pixel = rgbaData[y1 * width + x1 + 1];

                            uint index2 = GetColorIndex(pixel);

                            indexData[i++] = (byte)(((byte)index1 << 4) | (byte)index2);
                        }
                    }
                }
            }

            this.textureData = indexData;
        }

        public static byte[] FromCI8(byte[] texture, uint[] paletteData, int width, int height)
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
                            if (y1 >= height || x1 >= width)
                                continue;

                            ushort pixel = texture[i++];

                            output[y1 * width + x1] = paletteData[pixel];
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        public void ToCI8()
        {
            byte[] indexData = new byte[TextureConverter.GetTextureSize(textureFormat, width, height)];
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

                            indexData[i++] = (byte)GetColorIndex(pixel);
                        }
                    }
                }
            }

            this.textureData = indexData;
        }

        public static byte[] FromCI14X2(byte[] texture, uint[] paletteData, int width, int height)
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
                            if (y1 >= height || x1 >= width)
                                continue;

                            ushort pixel = Shared.Swap(BitConverter.ToUInt16(texture, i++ * 2));

                            output[y1 * width + x1] = paletteData[pixel & 0x3FFF];
                        }
                    }
                }
            }

            return Shared.UIntArrayToByteArray(output);
        }

        public void ToCI14X2()
        {
            byte[] indexData = new byte[TextureConverter.GetTextureSize(textureFormat, width, height)];
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

                            byte[] temp = BitConverter.GetBytes((ushort)GetColorIndex(pixel));
                            indexData[i++] = temp[1];
                            indexData[i++] = temp[0];
                        }
                    }
                }
            }

            this.textureData = indexData;
        }

        public void BuildPalette()
        {
            int palLength = 256;
            if (textureFormat == TextureFormat.CI4) palLength = 16;
            else if (textureFormat == TextureFormat.CI14X2) palLength = 16384;

            List<uint> palette = new();
            List<ushort> tPalette = new();

            palette.Add(0);
            tPalette.Add(0);

            for (int i = 1; i < rgbaData.Length; i++)
            {
                if (palette.Count == palLength) break;
                if (((rgbaData[i] >> 24) & 0xff) < ((textureFormat == TextureFormat.CI14X2) ? 1 : 25)) continue;

                ushort textureValue = Shared.Swap(ConvertToPaletteValue((int)rgbaData[i]));

                if (!palette.Contains(rgbaData[i]) && !tPalette.Contains(textureValue))
                {
                    palette.Add(rgbaData[i]);
                    tPalette.Add(textureValue);
                }
            }

            while (palette.Count % 16 != 0)
            { palette.Add(0xffffffff); tPalette.Add(0xffff); }

            texturePalette = Shared.UShortArrayToByteArray(tPalette.ToArray());
            rgbaPalette = palette.ToArray();
        }

        private ushort ConvertToPaletteValue(int rgba)
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

        private uint GetColorIndex(uint value)
        {
            uint minDistance = 0x7FFFFFFF;
            uint colorIndex = 0;

            if (((value >> 24) & 0xFF) < ((textureFormat == TextureFormat.CI14X2) ? 1 : 25)) return 0;
            ushort color = ConvertToPaletteValue((int)value);

            for (int i = 0; i < rgbaPalette.Length; i++)
            {
                ushort curPal = ConvertToPaletteValue((int)rgbaPalette[i]);

                if (color == curPal) return (uint)i;
                uint curDistance = GetDistance(color, curPal); //(uint)Math.Abs(Math.Abs(color) - Math.Abs(curVal));

                if (curDistance < minDistance)
                {
                    minDistance = curDistance;
                    colorIndex = (uint)i;
                }
            }

            return colorIndex;
        }

        private uint GetDistance(ushort color, ushort paletteColor)
        {
            uint curCol = ConvertToRgbaValue(color);
            uint palCol = ConvertToRgbaValue(paletteColor);

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

        private uint ConvertToRgbaValue(ushort pixel)
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
    }
}
