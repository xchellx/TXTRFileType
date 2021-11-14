﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Runtime.InteropServices;

namespace TXTRFileType.Util
{
    /// <summary>
    /// Utility class with useful methods for image manipulation and such
    /// </summary>
    public static class ImageUtil
    {
        #region FlipCoordinate

        /// <summary>
        /// Flip a coordinate to its opposite along its own axis
        /// </summary>
        /// <param name="widthOrHeight">Pass image width if flipping x coordinate. Otherwise, pass image height.</param>
        /// <param name="xOrY">Pass x coordinate if using image width. Otherwise, pass y coordinate.</param>
        /// <returns>The flipped coordinate</returns>
        public static int FlipCoordinate(int widthOrHeight, int xOrY)
            => (widthOrHeight - 1) - xOrY;

        /// <summary>
        /// Flip a coordinate to its opposite along its own axis
        /// </summary>
        /// <param name="width">The image width</param>
        /// <param name="height">The image height</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <returns>A tuple of the flipped x and y coordinate</returns>
        public static (int x, int y) FlipCoordinate(int width, int height, int x, int y)
            => (x: FlipCoordinate(width, x), y: FlipCoordinate(height, y));

        #endregion

        #region CountMips

        /// <summary>
        /// Count the amount of mipmaps that can exist within an image's size
        /// </summary>
        /// <param name="width">The image width</param>
        /// <param name="height">The image height</param>
        /// <param name="sizeLimit">The limit to stop counting mipmaps at. Defaults to 4.</param>
        /// <returns>The mipmap count of the mipmaps that can exist within an image's size</returns>
        public static int CountMips(int width, int height, int sizeLimit = 4)
        {
            int widthLevels = (int)Math.Max(Math.Ceiling(Math.Log2(width)) - 1, 1);
            int heightLevels = (int)Math.Max(Math.Ceiling(Math.Log2(height)) - 1, 1);
            return (int)(heightLevels < widthLevels ? heightLevels : widthLevels);
        }

        #endregion

        #region ToRGBA/ToBGRA

        /// <summary>
        /// Convert a byte array of RGBA colors in the format [r, g, b, a, ...] into
        /// BGRA colors in the format [b, g, r, a, ...]
        /// </summary>
        /// <param name="bgra">A byte array of RGBA colors in the format [r, g, b, a, ...]</param>
        public static void ToRGBA(ref byte[] bgra) => SwapChannels(ref bgra, false);

        /// <summary>
        /// Convert a byte array of BGRA colors in the format [b, g, r, a, ...] into
        /// RGBA colors in the format [r, g, b, a, ...]
        /// </summary>
        /// <param name="rgba">A byte array of BGRA colors in the format [b, g, r, a, ...]</param>
        public static void ToBGRA(ref byte[] rgba) => SwapChannels(ref rgba, true);

        private static void SwapChannels(ref byte[] colors, bool toBgra)
        {
            byte[] tmp = new byte[4];
            for (int i = 0; i < colors.Length; i += 4)
            {
                Array.Copy(colors, i, tmp, 0, tmp.Length);
                if (toBgra)
                {
                    // RGBA => BGRA
                    //  0, 1, 2, 3      2, 1, 0, 3      0      2
                    // [r, g, b, a] => [b, g, r, a]    (r <==> b)
                    colors[i + 0] = tmp[2]; // R; R => B
                    colors[i + 1] = tmp[1]; // G
                    colors[i + 2] = tmp[0]; // B; B => R
                    colors[i + 3] = tmp[3]; // A
                }
                else
                {
                    // BGRA => RGBA
                    //  2, 1, 0, 3      0, 1, 2, 3      2      0
                    // [b, g, r, a] => [r, g, b, a]    (b <==> r)
                    colors[i + 2] = tmp[0]; // B; B => R
                    colors[i + 1] = tmp[1]; // G
                    colors[i + 0] = tmp[2]; // R; R => B
                    colors[i + 3] = tmp[3]; // A
                }
            }
        }

        #endregion

        #region ToImage/FromImage

        public static Image<TPixel> ToImage<TPixel>(byte[] data, int width, int height)
            where TPixel : unmanaged, IPixel<TPixel>, IPixel, IEquatable<TPixel>
        {
            if (width == 0) width = 1;
            if (height == 0) height = 1;
            return Image.LoadPixelData<TPixel>(data, width, height);
        }

        public static byte[] FromImage<TPixel>(Image<TPixel> img)
            where TPixel : unmanaged, IPixel<TPixel>, IPixel, IEquatable<TPixel>
        {
            if (img.TryGetSinglePixelSpan(out var pixelSpan))
                return MemoryMarshal.AsBytes(pixelSpan).ToArray();
            else
                throw new Exception("Failed to retrieve raw pixel data from image");
        }

        #endregion

        /// <summary>Convenience fields</summary>
        public static class StaticMembers
        {
        }
    }
}