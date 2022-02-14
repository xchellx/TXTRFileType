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
using BCnEncoder.Decoder;
using BCnEncoder.Encoder;
using BCnEncoder.Shared;

namespace libWiiSharp
{
    internal static partial class TextureConverter
    {
        private static byte[] FromCMPR(byte[] texture, int width, int height)
        {
            // BC1 with 1 bit Alpha
            BcDecoder bc1Decoder = new();
            // struct DXTBlock { uint16_t color1; uint16_t color2; uint8_t lines[4]; }
            byte[] block = new byte[8];
            // Data read position
            long offset = 0L;
            // { r1, g1, b1, a1, ..., r16, g16, b16, a16 }
            ColorRgba32[,] rgba = new ColorRgba32[4, 4];
            uint[] bgra = new uint[width * height];
            for (int ty = 0; ty < height; ty += 8)
            {
                for (int tx = 0; tx < width; tx += 8)
                {
                    for (int by = 0; by < 8; by += 4)
                    {
                        for (int bx = 0; bx < 8; bx += 4)
                        {
                            // Read DXTBlock (sizeof(DXTBlock) == 8)
                            Array.Copy(texture, offset, block, 0, 8);
                            offset += 8L;
                            // Fix DXTBlock endianness
                            S3TC1ReverseBlock(ref block);
                            // BC1 with 1 bit Alpha
                            bc1Decoder.DecodeBlock(block, CompressionFormat.Bc1WithAlpha, rgba);
                            // Write decompressed 16 bit block to pixels
                            for (int py = 0; py < 4; py++)
                            {
                                for (int px = 0; px < 4; px++)
                                {
                                    // Discard exceeding pixels caused by extra GX blocks
                                    if ((ty + by + py) < height && (tx + bx + px) < width)
                                    {
                                        // Pack color to correct position
                                        bgra[(ty + by + py) * width + (tx + bx + px)] = (uint)(
                                            (rgba[py, px].b << 0) | (rgba[py, px].g << 8) | (rgba[py, px].r << 16) | (rgba[py, px].a << 24)
                                        );
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // Packed uints to unpacked bytes
            return Shared.UIntArrayToByteArray(bgra);
        }

        private static byte[] ToCMPR(Image<Bgra32> img, TextureFormat textureFormat)
        {
            byte[] texture = new byte[GetTextureSize(textureFormat, img.Width, img.Height)];
            // BC1 with 1 bit Alpha
            BcEncoder bc1Encoder = new(CompressionFormat.Bc1WithAlpha);
            // struct DXTBlock { uint16_t color1; uint16_t color2; uint8_t lines[4]; }
            byte[] block = new byte[8];
            // Data write position
            long offset = 0L;
            // { r1, g1, b1, a1, ..., r16, g16, b16, a16 }
            ColorRgba32[] rgba = new ColorRgba32[16];
            // Image to packed uint
            uint[] bgra = ImageToRgba(img);
            for (int ty = 0; ty < img.Height; ty += 8)
            {
                for (int tx = 0; tx < img.Width; tx += 8)
                {
                    for (int by = 0; by < 8; by += 4)
                    {
                        for (int bx = 0; bx < 8; bx += 4)
                        {
                            // Read pixels as decompressed 16 bit blocks
                            for (int py = 0; py < 4; py++)
                            {
                                for (int px = 0; px < 4; px++)
                                {
                                    int pi = (py * 4) + px;
                                    // Discard exceeding pixels caused by extra GX blocks
                                    if ((ty + by + py) < img.Height && (tx + bx + px) < img.Width)
                                    {
                                        // Unpack color to correct position
                                        rgba[pi].r = (byte)(bgra[(ty + by + py) * img.Width + (tx + bx + px)] >> 16);
                                        rgba[pi].g = (byte)(bgra[(ty + by + py) * img.Width + (tx + bx + px)] >> 8);
                                        rgba[pi].b = (byte)(bgra[(ty + by + py) * img.Width + (tx + bx + px)] >> 0);
                                        rgba[pi].a = (byte)(bgra[(ty + by + py) * img.Width + (tx + bx + px)] >> 24);
                                    }
                                    else
                                    {
                                        // Add dummy color for extra GX blocks
                                        rgba[pi].r = 0;
                                        rgba[pi].g = 0;
                                        rgba[pi].b = 0;
                                        rgba[pi].a = 0;
                                    }
                                }
                            }
                            // BC1 with 1 bit Alpha
                            Array.Copy(bc1Encoder.EncodeBlock(rgba), 0, block, 0, 8);
                            // Fix DXTBlock endianness
                            S3TC1ReverseBlock(ref block);
                            // Write compressed 8 bit block to pixels
                            Array.Copy(block, 0, texture, offset, 8);
                            offset += 8L;
                        }
                    }
                }
            }
            return texture;
        }
    }
}
