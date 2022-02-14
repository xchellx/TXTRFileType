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
        private static byte[] FromRGBA32(byte[] texture, int width, int height)
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
                                if ((x1 >= width) || (y1 >= height))
                                    continue;

                                ushort pixel = Shared.Swap(BitConverter.ToUInt16(texture, inp++ * 2));

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

        private static byte[] ToRGBA32(Image<Bgra32> img, TextureFormat textureFormat)
        {
            uint[] pixeldata = ImageToRgba(img);
            int w = img.Width;
            int h = img.Height;
            int z = 0, iv = 0;
            byte[] output = new byte[GetTextureSize(textureFormat, img.Width, img.Height)];
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
    }
}
