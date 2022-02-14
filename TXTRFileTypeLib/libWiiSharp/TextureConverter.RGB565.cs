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
        private static byte[] FromRGB565(byte[] texture, int width, int height)
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
                            if (y1 >= height || x1 >= width)
                                continue;

                            ushort pixel = Shared.Swap(BitConverter.ToUInt16(texture, inp++ * 2));

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

        private static byte[] ToRGB565(Image<Bgra32> img, TextureFormat textureFormat)
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
    }
}
