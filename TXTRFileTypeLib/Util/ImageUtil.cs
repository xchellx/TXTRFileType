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

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Runtime.InteropServices;

namespace TXTRFileTypeLib.Util
{
    /// <summary>
    /// Utility class with useful methods for image manipulation and such
    /// </summary>
    public static class ImageUtil
    {
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
            => (FlipCoordinate(width, x), FlipCoordinate(height, y));

        /// <summary>
        /// Count the amount of mipmaps that can exist within an image's size
        /// </summary>
        /// <param name="width">The image width</param>
        /// <param name="height">The image height</param>
        /// <param name="countBase">Whether to include the base image in the mipmap count</param>
        /// <param name="widthLimit"></param>
        /// <param name="heightLimit"></param>
        /// <returns></returns>
        public static int CountMipmaps(int width, int height, int widthLimit,
            int heightLimit, bool countBase = false)
        {
            int wi = countBase ? 1 : 0,
                hi = countBase ? 1 : 0;
            while (width > widthLimit || height > heightLimit)
            {
                if (width > widthLimit)
                {
                    width /= 2;
                    ++wi;
                }
                if (height > heightLimit)
                {
                    height /= 2;
                    ++hi;
                }
            }
            return Math.Max(wi < hi ? wi : hi, 0);
        }

        /// <summary>
        /// Get a <see cref="Image{TPixel}"/> from the bytes of an image
        /// </summary>
        /// <typeparam name="TPixel">The type of pixel data in <paramref name="data"/></typeparam>
        /// <param name="data">The bytes of the image</param>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        /// <returns>A <see cref="Image{TPixel}"/> instance constructed from <paramref name="data"/></returns>
        public static Image<TPixel> ToImage<TPixel>(byte[] data, int width, int height)
            where TPixel : unmanaged, IPixel<TPixel>, IPixel, IEquatable<TPixel>
            => Image.LoadPixelData<TPixel>(data, width > 0 ? width : 1, height > 0 ? height : 1);

        /// <summary>
        /// Get the bytes of a <see cref="Image{TPixel}"/>
        /// </summary>
        /// <typeparam name="TPixel">The type of pixel data in <paramref name="img"/></typeparam>
        /// <param name="img">A <see cref="Image{TPixel}"/></param>
        /// <returns>The bytes of <paramref name="img"/></returns>
        public static byte[] FromImage<TPixel>(Image<TPixel> img)
            where TPixel : unmanaged, IPixel<TPixel>, IPixel, IEquatable<TPixel>
            => (img.TryGetSinglePixelSpan(out Span<TPixel> pixelSpan))
            ? MemoryMarshal.AsBytes(pixelSpan).ToArray()
            : throw new Exception("Failed to retrieve raw pixel data from image");
    }
}
