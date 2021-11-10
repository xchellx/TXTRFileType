using PaintDotNet;
using System;

namespace TXTRFileType.Util
{
    /// <summary>
    /// Utility class of helpful methods for Paint.NET
    /// </summary>
    public static class PDNUtil
    {
        #region ColorOptions

        /// <summary>
        /// Options that affect how <see cref="SetPixel"/> and <see cref="SetColor"/> set the values of a color
        /// </summary>
        [Flags]
        public enum ColorOptions : ushort
        {
            /// <summary>Modify values as is.</summary>
            NONE   = 0b0000_0000_0000_0000,
            /// <summary>Copy R to G.<br/>G = R</summary>
            R_TO_G = 0b0000_0000_0000_0001,
            /// <summary>Copy R to B.<br/>B = R</summary>
            R_TO_B = 0b0000_0000_0000_0010,
            /// <summary>Copy R to A.<br/>A = R</summary>
            R_TO_A = 0b0000_0000_0000_0100,
            /// <summary>Copy G to R.<br/>R = G</summary>
            G_TO_R = 0b0000_0000_0000_1000,
            /// <summary>Copy G to B.<br/>B = G</summary>
            G_TO_B = 0b0000_0000_0001_0000,
            /// <summary>Copy G to A.<br/>A = G</summary>
            G_TO_A = 0b0000_0000_0010_0000,
            /// <summary>Copy B to R.<br/>R = B</summary>
            B_TO_R = 0b0000_0000_0100_0000,
            /// <summary>Copy B to G.<br/>G = B</summary>
            B_TO_G = 0b0000_0000_1000_0000,
            /// <summary>Copy B to A.<br/>A = B</summary>
            B_TO_A = 0b0000_0001_0000_0000,
            /// <summary>Copy A to R.<br/>R = A</summary>
            A_TO_R = 0b0000_0010_0000_0000,
            /// <summary>Copy A to G.<br/>G = A</summary>
            A_TO_G = 0b0000_0100_0000_0000,
            /// <summary>Copy A to B.<br/>B = A</summary>
            A_TO_B = 0b0000_1000_0000_0000,
            /// <summary>Do not modify R.</summary>
            KEEP_R = 0b0001_0000_0000_0000,
            /// <summary>Do not modify G.</summary>
            KEEP_G = 0b0010_0000_0000_0000,
            /// <summary>Do not modify B.</summary>
            KEEP_B = 0b0100_0000_0000_0000,
            /// <summary>Do not modify A.</summary>
            KEEP_A = 0b1000_0000_0000_0000
        }

        #endregion

        #region CreateLayer

        /// <summary>
        /// Create a <see cref="BitmapLayer"/> with the specified width, height, and name
        /// </summary>
        /// <param name="width">The width of the layer</param>
        /// <param name="height">The height of the layer</param>
        /// <param name="name">The name of the layer</param>
        /// <returns></returns>
        public static BitmapLayer CreateLayer(int width, int height, string name)
            => CreateLayer(width, height, name, ColorBgra.Transparent);

        /// <summary>
        /// Create a <see cref="BitmapLayer"/> with the specified width, height, name, and background color
        /// </summary>
        /// <param name="width">The width of the layer</param>
        /// <param name="height">The height of the layer</param>
        /// <param name="name">The name of the layer</param>
        /// <param name="backgroundColor">The background color of the layer</param>
        /// <returns></returns>
        public static BitmapLayer CreateLayer(int width, int height, string name, ColorBgra backgroundColor)
            => new BitmapLayer(width, height, backgroundColor) { Name = name };

        #endregion

        #region SetPixel

        /// <summary>
        /// Set the color of a pixel in a layer with options
        /// </summary>
        /// <param name="layer">The layer to set the pixel on</param>
        /// <param name="x">The x index of the pixel</param>
        /// <param name="y">The y index of the pixel</param>
        /// <param name="r">The red component to set to the pixel</param>
        /// <param name="g">The green component to set to the pixel</param>
        /// <param name="b">The blue component to set to the pixel</param>
        /// <param name="a">The alpha component to set to the pixel</param>
        /// <param name="colorOptions">The options on how to set the pixel's color</param>
        public static void SetPixel(ref BitmapLayer layer, int x, int y, byte r, byte g, byte b, byte a, ColorOptions colorOptions = ColorOptions.NONE)
            => SetPixel(ref layer, x, y, false, false, r, g, b, a, colorOptions);

        /// <summary>
        /// Set the color of a pixel in a layer with options
        /// </summary>
        /// <param name="layer">The layer to set the pixel on</param>
        /// <param name="x">The x index of the pixel</param>
        /// <param name="y">The y index of the pixel</param>
        /// <param name="dstColor">The color to set the pixel to</param>
        /// <param name="colorOptions">The options on how to set the pixel's color</param>
        public static void SetPixel(ref BitmapLayer layer, int x, int y, ColorBgra dstColor, ColorOptions colorOptions = ColorOptions.NONE)
            => SetPixel(ref layer, x, y, false, false, dstColor, colorOptions);

        /// <summary>
        /// Set the color of a pixel in a layer with options optionally flipping x and/or y indices
        /// </summary>
        /// <param name="layer">The layer to set the pixel on</param>
        /// <param name="x">The x index of the pixel</param>
        /// <param name="y">The y index of the pixel</param>
        /// <param name="flipX">Flip the x index</param>
        /// <param name="flipY">Flip the y index</param>
        /// <param name="r">The red component to set to the pixel</param>
        /// <param name="g">The green component to set to the pixel</param>
        /// <param name="b">The blue component to set to the pixel</param>
        /// <param name="a">The alpha component to set to the pixel</param>
        /// <param name="colorOptions">The options on how to set the pixel's color</param>
        public static void SetPixel(ref BitmapLayer layer, int x, int y, bool flipX, bool flipY, byte r, byte g, byte b, byte a, ColorOptions colorOptions = ColorOptions.NONE)
            => SetPixel(ref layer, x, y, flipX, flipY, new ColorBgra() { R = r, G = g, B = b, A = a }, colorOptions);

        /// <summary>
        /// Set the color of a pixel in a layer with options optionally flipping x and/or y indices
        /// </summary>
        /// <param name="layer">The layer to set the pixel on</param>
        /// <param name="x">The x index of the pixel</param>
        /// <param name="y">The y index of the pixel</param>
        /// <param name="flipX">Flip the x index</param>
        /// <param name="flipY">Flip the y index</param>
        /// <param name="dstColor">The color to set the pixel to</param>
        /// <param name="colorOptions">The options on how to set the pixel's color</param>
        public static void SetPixel(ref BitmapLayer layer, int x, int y, bool flipX, bool flipY, ColorBgra dstColor, ColorOptions colorOptions = ColorOptions.NONE)
        {
            (int newX, int newY) = FlipCoordinate(layer.Width, layer.Height, x, y);
            ColorBgra srcColor = layer.Surface[newX, newY];
            SetColor(ref srcColor, dstColor, colorOptions);
            layer.Surface[newX, newY] = srcColor;
        }

        #endregion

        #region SetColor

        /// <summary>
        /// Set the values of a color with options
        /// </summary>
        /// <param name="srcColor">The original reference color</param>
        /// <param name="r">The red component to set to <paramref name="srcColor"/></param>
        /// <param name="g">The green component to set to <paramref name="srcColor"/></param>
        /// <param name="b">The blue component to set to <paramref name="srcColor"/></param>
        /// <param name="a">The alpha component to set to <paramref name="srcColor"/></param>
        /// <param name="colorOptions">The options on how to set the color</param>
        public static void SetColor(ref ColorBgra srcColor, byte r, byte g, byte b, byte a, ColorOptions colorOptions = ColorOptions.NONE)
            => SetColor(ref srcColor, new ColorBgra() { R = r, G = g, B = b, A = a }, colorOptions);

        /// <summary>
        /// Set the values of a color with options
        /// </summary>
        /// <param name="srcColor">The original reference color</param>
        /// <param name="dstColor">The color to set to <paramref name="srcColor"/></param>
        /// <param name="colorOptions">The options on how to set the color</param>
        public static void SetColor(ref ColorBgra srcColor, ColorBgra dstColor, ColorOptions colorOptions = ColorOptions.NONE)
        {
            // RGBA
            if (!((colorOptions & ColorOptions.KEEP_R) == ColorOptions.KEEP_R))
                srcColor.R = dstColor.R;
            if (!((colorOptions & ColorOptions.KEEP_G) == ColorOptions.KEEP_G))
                srcColor.G = dstColor.G;
            if (!((colorOptions & ColorOptions.KEEP_B) == ColorOptions.KEEP_B))
                srcColor.B = dstColor.B;
            if (!((colorOptions & ColorOptions.KEEP_A) == ColorOptions.KEEP_A))
                srcColor.A = dstColor.A;
            // R
            if (!((colorOptions & ColorOptions.KEEP_R) == ColorOptions.KEEP_R))
            {
                if (((colorOptions & ColorOptions.G_TO_R) == ColorOptions.G_TO_R))
                    srcColor.R = dstColor.G;
                if (((colorOptions & ColorOptions.B_TO_R) == ColorOptions.B_TO_R))
                    srcColor.R = dstColor.B;
                if (((colorOptions & ColorOptions.A_TO_R) == ColorOptions.A_TO_R))
                    srcColor.R = dstColor.A;
            }
            // G
            if (!((colorOptions & ColorOptions.KEEP_G) == ColorOptions.KEEP_G))
            {
                if (((colorOptions & ColorOptions.R_TO_G) == ColorOptions.R_TO_G))
                    srcColor.G = dstColor.R;
                if (((colorOptions & ColorOptions.B_TO_G) == ColorOptions.B_TO_G))
                    srcColor.G = dstColor.B;
                if (((colorOptions & ColorOptions.A_TO_G) == ColorOptions.A_TO_G))
                    srcColor.G = dstColor.A;
            }
            // B
            if (!((colorOptions & ColorOptions.KEEP_B) == ColorOptions.KEEP_B))
            {
                if (((colorOptions & ColorOptions.R_TO_B) == ColorOptions.R_TO_B))
                    srcColor.B = dstColor.R;
                if (((colorOptions & ColorOptions.G_TO_B) == ColorOptions.G_TO_B))
                    srcColor.B = dstColor.G;
                if (((colorOptions & ColorOptions.A_TO_B) == ColorOptions.A_TO_B))
                    srcColor.B = dstColor.A;
            }
            // A
            if (!((colorOptions & ColorOptions.KEEP_A) == ColorOptions.KEEP_A))
            {
                if (((colorOptions & ColorOptions.R_TO_A) == ColorOptions.R_TO_A))
                    srcColor.A = dstColor.R;
                if (((colorOptions & ColorOptions.G_TO_A) == ColorOptions.G_TO_A))
                    srcColor.A = dstColor.G;
                if (((colorOptions & ColorOptions.B_TO_A) == ColorOptions.B_TO_A))
                    srcColor.A = dstColor.B;
            }
        }

        #endregion

        #region FlipPixel

        public static int FlipCoordinate(int widthOrHeight, int xOrY)
            => (widthOrHeight - 1) - xOrY;

        public static (int x, int y) FlipCoordinate(int width, int height, int x, int y)
            => (x: FlipCoordinate(width, x), y: FlipCoordinate(height, y));

        #endregion

        /// <summary>Convenience fields</summary>
        public static class StaticMembers
        {
        }
    }
}
