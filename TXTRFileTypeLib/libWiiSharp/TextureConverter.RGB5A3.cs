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
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;

namespace libWiiSharp
{
    internal static partial class TextureConverter
    {
        private static byte[] FromRGB5A3(byte[] texture, int width, int height)
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
                            if (y1 >= height || x1 >= width)
                                continue;

                            ushort pixel = Shared.Swap(BitConverter.ToUInt16(texture, inp++ * 2));

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

        private static byte[] ToRGB5A3(Image<Bgra32> img, TextureFormat textureFormat)
        {
            uint[] pixeldata = ImageToRgba(img);
            int w = img.Width;
            int h = img.Height;
            int z = -1;
            byte[] output = new byte[GetTextureSize(textureFormat, img.Width, img.Height)];

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
    }
}
