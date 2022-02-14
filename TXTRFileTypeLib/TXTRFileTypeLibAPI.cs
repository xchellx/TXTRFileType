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

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.IO;
using libWiiSharp.Formats;
using libWiiSharp;
using System.ComponentModel;
using libWiiSharp.Extensions;

namespace TXTRFileTypeLib
{
    /// <summary>
    /// Public API surface for TXTRFileTypeLib.
    /// </summary>
    [Description("Public API surface for TXTRFileTypeLib.")]
    public static partial class TXTRFileTypeLibAPI
    {
        /// <summary>
        /// Progress method contract for this API
        /// </summary>
        /// <param name="progress">The current progress</param>
        /// <param name="max">The maximum progress</param>
        [Description("Progress method contract for this API")]
        public delegate void UpdateProgressDelegate(double progress, double max);

        /// <summary>
        /// Read a TXTR from a byte array.<br/>
        /// The returned image array will always be of a count of 1 unless <paramref name="readMipmaps"/> is <see langword="true"/>.<br/>
        /// Strange image sizes and high mipmap counts might create invalid textures and/or throw exceptions.
        /// </summary>
        /// <param name="inputData">The input byte array</param>
        /// <param name="readMipmaps">Read all mipmaps from the TXTR (else only read the first)</param>
        /// <param name="progressCallback">Callback where mipmap read progress will be reported to</param>
        /// <returns>A <see cref="Image{Bgra32}"/> array of all mipmaps read</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="inputData"/> is <see langword="null"/></exception>
        [Description("Read a TXTR from a byte array")]
        public static Image<Bgra32>[] Read(byte[] inputData,
            bool readMipmaps = false,
            UpdateProgressDelegate? progressCallback = null)
        {
            if (inputData != null)
            {
                using (var inputStream = new MemoryStream())
                {
                    inputStream.Write(inputData);
                    return ReadCore(inputStream, readMipmaps, false, progressCallback);
                }
            }
            else
                throw new ArgumentNullException(nameof(inputData));
        }

        /// <summary>
        /// Read a TXTR from a file.<br/>
        /// The returned image array will always be of a count of 1 unless <paramref name="readMipmaps"/> is <see langword="true"/>.<br/>
        /// Strange image sizes and high mipmap counts might create invalid textures and/or throw exceptions.
        /// </summary>
        /// <param name="inputFilePath">The input file path</param>
        /// <param name="readMipmaps">Read all mipmaps from the TXTR (else only read the first)</param>
        /// <param name="progressCallback">Callback where mipmap read progress will be reported to</param>
        /// <returns>A <see cref="Image{Bgra32}"/> array of all mipmaps read</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="inputFilePath"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">If <paramref name="inputFilePath"/> is empty</exception>
        [Description("Read a TXTR from a file")]
        public static Image<Bgra32>[] Read(string inputFilePath,
            bool readMipmaps = false,
            UpdateProgressDelegate? progressCallback = null)
        {
            if (!string.IsNullOrWhiteSpace(inputFilePath))
            {
                using (FileStream inputStream = File.OpenRead(inputFilePath))
                {
                    return ReadCore(inputStream, readMipmaps, false, progressCallback);
                }
            }
            else
                throw inputFilePath == null
                    ? new ArgumentNullException(nameof(inputFilePath))
                    : new ArgumentException("Path is empty", nameof(inputFilePath));
        }

        /// <summary>
        /// Read a TXTR from a stream.<br/>
        /// The returned image array will always be of a count of 1 unless <paramref name="readMipmaps"/> is <see langword="true"/>.<br/>
        /// Strange image sizes and high mipmap counts might create invalid textures and/or throw exceptions.<br/>
        /// <paramref name="inputSream"/> will be automatically closed unless <paramref name="keepStreamOpen"/> is <see langword="true"/>.
        /// </summary>
        /// <param name="inputSream">The input stream</param>
        /// <param name="readMipmaps">Read all mipmaps from the TXTR (else only read the first)</param>
        /// <param name="keepStreamOpen">Keep the stream open (do not auto close stream)</param>
        /// <param name="progressCallback">Callback where mipmap read progress will be reported to</param>
        /// <returns>A <see cref="Image{Bgra32}"/> array of all mipmaps read</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="inputSream"/> is <see langword="null"/></exception>
        [Description("Read a TXTR from a stream")]
        public static Image<Bgra32>[] Read(Stream inputSream,
            bool readMipmaps = false,
            bool keepStreamOpen = false,
            UpdateProgressDelegate? progressCallback = null)
        {
            if (inputSream != null)
            {
                return ReadCore(inputSream, readMipmaps, keepStreamOpen, progressCallback);
            }
            else
                throw new ArgumentNullException(nameof(inputSream));
        }

        /// <summary>
        /// Write a TXTR to a byte array.<br/>
        /// Indexed formats (<see cref="TextureFormat.CI4"/>, <see cref="TextureFormat.CI8"/>, and <see cref="TextureFormat.CI14X2"/>)
        /// cannot have mipmaps.<br/>
        /// Strange image sizes and high mipmap counts might create invalid textures and/or throw exceptions.
        /// </summary>
        /// <param name="input">The input image</param>
        /// <param name="textureFormat">The texture format to write the TXTR in</param>
        /// <param name="paletteFormat">The palette format to write the TXTR in if it's written in an indexed texture format</param>
        /// <param name="copyPaletteSize">The location to write the palette length to in the TXTR if it's written in an indexed texture format</param>
        /// <param name="generateMipmaps">Whether mipmaps should be generated</param>
        /// <param name="mipmapWidthLimit">The mipmap width limit for mipmap generation</param>
        /// <param name="mipmapHeightLimit">The mipmap height limit for mipmap generation</param>
        /// <param name="progressCallback">Callback where mipmap write progress will be reported to</param>
        /// <returns>A byte array filled with the TXTR data</returns>
        /// <exception cref="InvalidOperationException">
        /// If <paramref name="textureFormat"/> is <see cref="TextureFormat.CI4"/>, <see cref="TextureFormat.CI8"/>, or <see cref="TextureFormat.CI14X2"/>
        /// and <paramref name="generateMipmaps"/> is <see langword="true"/>
        /// </exception>
        /// <exception cref="ArgumentNullException">If <paramref name="input"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="textureFormat"/>, <paramref name="paletteFormat"/>, or <paramref name="copyPaletteSize"/> is an invalid enum
        /// </exception>
        [Description("Write a TXTR to a byte array")]
        public static byte[] Write(Image<Bgra32> input,
            TextureFormat textureFormat,
            PaletteFormat paletteFormat,
            CopyPaletteSize copyPaletteSize = CopyPaletteSize.ToWidth,
            bool generateMipmaps = false,
            int mipmapWidthLimit = 4,
            int mipmapHeightLimit = 4,
            UpdateProgressDelegate? progressCallback = null)
        {
            if (input != null
                && textureFormat.IsDefined()
                && paletteFormat.IsDefined()
                && copyPaletteSize.IsDefined())
            {
                if (!(generateMipmaps &&
                    (textureFormat == TextureFormat.CI4
                    || textureFormat == TextureFormat.CI8
                    || textureFormat == TextureFormat.CI14X2)))
                {
                    using (var outputStream = new MemoryStream())
                    {
                        WriteCore(input, outputStream, textureFormat,
                            paletteFormat, copyPaletteSize, generateMipmaps,
                            mipmapWidthLimit, mipmapHeightLimit, false,
                            progressCallback);
                        return outputStream.ToArray();
                    }
                }
                else
                    throw new InvalidOperationException("Indexed formats should not have mipmaps");
            }
            else
            {
                if (input == null)
                    throw new ArgumentNullException(nameof(input));
                else if (!textureFormat.IsDefined())
                    throw new ArgumentException(
                        $"Invalid texture format: 0x{textureFormat.AsUInt32():X8}",
                        nameof(textureFormat));
                else if (!paletteFormat.IsDefined())
                    throw new ArgumentException(
                        $"Invalid texture format: 0x{paletteFormat.AsUInt32():X8}",
                        nameof(paletteFormat));
                else
                    throw new ArgumentException("Invalid location for copying the palette size",
                        nameof(copyPaletteSize));
            }
        }

        /// <summary>
        /// Write a TXTR to a file.<br/>
        /// Indexed formats (<see cref="TextureFormat.CI4"/>, <see cref="TextureFormat.CI8"/>, and <see cref="TextureFormat.CI14X2"/>)
        /// cannot have mipmaps.<br/>
        /// Strange image sizes and high mipmap counts might create invalid textures and/or throw exceptions.
        /// </summary>
        /// <param name="input">The input image</param>
        /// <param name="outputFilePath">The output file path</param>
        /// <param name="textureFormat">The texture format to write the TXTR in</param>
        /// <param name="paletteFormat">The palette format to write the TXTR in if it's written in an indexed texture format</param>
        /// <param name="copyPaletteSize">The location to write the palette length to in the TXTR if it's written in an indexed texture format</param>
        /// <param name="generateMipmaps">Whether mipmaps should be generated</param>
        /// <param name="mipmapWidthLimit">The mipmap width limit for mipmap generation</param>
        /// <param name="mipmapHeightLimit">The mipmap height limit for mipmap generation</param>
        /// <param name="progressCallback">Callback where mipmap write progress will be reported to</param>
        /// <exception cref="InvalidOperationException">
        /// If <paramref name="textureFormat"/> is <see cref="TextureFormat.CI4"/>, <see cref="TextureFormat.CI8"/>, or <see cref="TextureFormat.CI14X2"/>
        /// and <paramref name="generateMipmaps"/> is <see langword="true"/>
        /// </exception>
        /// <exception cref="ArgumentNullException">If <paramref name="input"/> or <paramref name="outputFilePath"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="textureFormat"/>, <paramref name="paletteFormat"/>, or <paramref name="copyPaletteSize"/> is an invalid enum
        /// </exception>
        [Description("Write a TXTR to a file")]
        public static void Write(Image<Bgra32> input,
            string outputFilePath,
            TextureFormat textureFormat,
            PaletteFormat paletteFormat,
            CopyPaletteSize copyPaletteSize = CopyPaletteSize.ToWidth,
            bool generateMipmaps = false,
            int mipmapWidthLimit = 4,
            int mipmapHeightLimit = 4,
            UpdateProgressDelegate? progressCallback = null)
        {
            if (input != null
                && !string.IsNullOrWhiteSpace(outputFilePath)
                && textureFormat.IsDefined()
                && paletteFormat.IsDefined()
                && copyPaletteSize.IsDefined())
            {
                if (!(generateMipmaps &&
                    (textureFormat == TextureFormat.CI4
                    || textureFormat == TextureFormat.CI8
                    || textureFormat == TextureFormat.CI14X2)))
                {
                    using (FileStream outputStream = File.OpenWrite(outputFilePath))
                    {
                        WriteCore(input, outputStream, textureFormat,
                            paletteFormat, copyPaletteSize, generateMipmaps,
                            mipmapWidthLimit, mipmapHeightLimit, false,
                            progressCallback);
                    }
                }
                else
                    throw new InvalidOperationException("Indexed formats should not have mipmaps");
            }
            else
            {
                if (input == null)
                    throw new ArgumentNullException(nameof(input));
                else if (string.IsNullOrEmpty(outputFilePath))
                    throw outputFilePath == null
                        ? new ArgumentNullException(nameof(outputFilePath))
                        : new ArgumentException("Path is empty", nameof(outputFilePath));
                else if (!textureFormat.IsDefined())
                    throw new ArgumentException(
                        $"Invalid texture format: 0x{textureFormat.AsUInt32():X8}",
                        nameof(textureFormat));
                else if (!paletteFormat.IsDefined())
                    throw new ArgumentException(
                        $"Invalid texture format: 0x{paletteFormat.AsUInt32():X8}",
                        nameof(paletteFormat));
                else
                    throw new ArgumentException("Invalid location for copying the palette size",
                        nameof(copyPaletteSize));
            }
        }

        /// <summary>
        /// Write a TXTR to a stream.<br/>
        /// Indexed formats (<see cref="TextureFormat.CI4"/>, <see cref="TextureFormat.CI8"/>, and <see cref="TextureFormat.CI14X2"/>)
        /// cannot have mipmaps.<br/>
        /// Strange image sizes and high mipmap counts might create invalid textures and/or throw exceptions.<br/>
        /// <paramref name="outputStream"/> will be automatically closed unless <paramref name="keepStreamOpen"/> is <see langword="true"/>.
        /// </summary>
        /// <param name="input">The input image</param>
        /// <param name="outputStream">The output stream</param>
        /// <param name="textureFormat">The texture format to write the TXTR in</param>
        /// <param name="paletteFormat">The palette format to write the TXTR in if it's written in an indexed texture format</param>
        /// <param name="copyPaletteSize">The location to write the palette length to in the TXTR if it's written in an indexed texture format</param>
        /// <param name="generateMipmaps">Whether mipmaps should be generated</param>
        /// <param name="mipmapWidthLimit">The mipmap width limit for mipmap generation</param>
        /// <param name="mipmapHeightLimit">The mipmap height limit for mipmap generation</param>
        /// <param name="keepStreamOpen">Keep the stream open (do not auto close stream)</param>
        /// <param name="progressCallback">Callback where mipmap write progress will be reported to</param>
        /// <exception cref="InvalidOperationException">
        /// If <paramref name="textureFormat"/> is <see cref="TextureFormat.CI4"/>, <see cref="TextureFormat.CI8"/>, or <see cref="TextureFormat.CI14X2"/>
        /// and <paramref name="generateMipmaps"/> is <see langword="true"/>
        /// </exception>
        /// <exception cref="ArgumentNullException">If <paramref name="input"/> or <paramref name="outputStream"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="textureFormat"/>, <paramref name="paletteFormat"/>, or <paramref name="copyPaletteSize"/> is an invalid enum
        /// </exception>
        [Description("Write a TXTR to a stream")]
        public static void Write(Image<Bgra32> input,
            Stream outputStream,
            TextureFormat textureFormat,
            PaletteFormat paletteFormat,
            CopyPaletteSize copyPaletteSize = CopyPaletteSize.ToWidth,
            bool generateMipmaps = false,
            int mipmapWidthLimit = 4,
            int mipmapHeightLimit = 4,
            bool keepStreamOpen = false,
            UpdateProgressDelegate? progressCallback = null)
        {
            if (input != null
                && outputStream != null
                && textureFormat.IsDefined()
                && paletteFormat.IsDefined()
                && copyPaletteSize.IsDefined())
            {
                if (!(generateMipmaps &&
                    (textureFormat == TextureFormat.CI4
                    || textureFormat == TextureFormat.CI8
                    || textureFormat == TextureFormat.CI14X2)))
                {
                    WriteCore(input, outputStream, textureFormat,
                        paletteFormat, copyPaletteSize, generateMipmaps,
                        mipmapWidthLimit, mipmapHeightLimit, keepStreamOpen,
                        progressCallback);
                }
                else
                    throw new InvalidOperationException("Indexed formats should not have mipmaps");
            }
            else
            {
                if (input == null)
                    throw new ArgumentNullException(nameof(input));
                else if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                else if (!textureFormat.IsDefined())
                    throw new ArgumentException(
                        $"Invalid texture format: 0x{textureFormat.AsUInt32():X8}",
                        nameof(textureFormat));
                else if (!paletteFormat.IsDefined())
                    throw new ArgumentException(
                        $"Invalid texture format: 0x{paletteFormat.AsUInt32():X8}",
                        nameof(paletteFormat));
                else
                    throw new ArgumentException("Invalid location for copying the palette size",
                        nameof(copyPaletteSize));
            }
        }
    }
}
