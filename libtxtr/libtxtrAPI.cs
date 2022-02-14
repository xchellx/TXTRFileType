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

using libWiiSharp;
using libWiiSharp.Extensions;
using libWiiSharp.Formats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using TXTRFileTypeLib;

namespace libtxtr
{
    /// <summary>
    /// Public API surface for libtxtr.
    /// </summary>
    [Description("Public API surface for libtxtr.")]
#pragma warning disable IDE1006
    public static class libtxtrAPI
#pragma warning restore IDE1006
    {
        static libtxtrAPI()
        {
            images = new List<Image<Bgra32>>();
            saveImage = null;
            lastErrorMsg = new StringBuilder(4096, 4096);
            lastErrorMsg.Append("No error");
        }

        private static readonly List<Image<Bgra32>> images;

        private static Image<Bgra32>? saveImage;

        private static readonly StringBuilder lastErrorMsg;

        /// <summary>
        /// Status code that indicates success.
        /// </summary>
        public const int STATUS_SUCCESS = 0;

        /// <summary>
        /// Status code that indicates an undetermined failure.
        /// </summary>
        public const int STATUS_FAILED = 1;

        /// <summary>
        /// Status code that indicates an index out of range error.
        /// </summary>
        public const int STATUS_IDXOUTOFRANGE = 2;

        /// <summary>
        /// Status code that indicates no images are loaded.
        /// </summary>
        public const int STATUS_NOLOADEDIMGS = 3;

        /// <summary>
        /// Status code that indicates marshaling a memory pointer failed.
        /// </summary>
        public const int STATUS_MARSHALFAIL = 4;

        /// <summary>
        /// Status code that indicates a memory pointer is null.
        /// </summary>
        public const int STATUS_NULLPTR = 5;

        /// <summary>
        /// Status code that indicates an argument is invalid.
        /// </summary>
        public const int STATUS_INVALIDARG = 6;

        /// <summary>
        /// Status code that indicates there are images already loaded and/or the save image is already initialized.
        /// </summary>
        public const int STATUS_IMGALRINIT = 7;

        /// <summary>
        /// Status code that indicates the save image is not initialized.
        /// </summary>
        public const int STATUS_IMGNOTINIT = 8;

        /// <summary>
        /// Enum type code for <see cref="TextureFormat"/>.<br/>
        /// See: <see cref="GetEnumDescription(int, uint, IntPtr, bool)"/>
        /// </summary>
        public const int ENUMTYPE_TEXTUREFORMAT = 1;

        /// <summary>
        /// Enum type code for <see cref="PaletteFormat"/>.<br/>
        /// See: <see cref="GetEnumDescription(int, uint, IntPtr, bool)"/>
        /// </summary>
        public const int ENUMTYPE_PALETTEFORMAT = 2;

        /// <summary>
        /// Enum type code for <see cref="CopyPaletteSize"/>.<br/>
        /// See: <see cref="GetEnumDescription(int, uint, IntPtr, bool)"/>
        /// </summary>
        public const int ENUMTYPE_COPYPALETTESIZE = 3;

        // For speed: all if branches are flattened as much as possible

        /// <summary>
        /// Read an TXTR file to an image.<br/>
        /// <strong>Note:</strong><br/>
        /// <paramref name="filePathPtr"/> must be passed null terminated ASCII bytes.<br/>
        /// <paramref name="progressCallbackPtr"/> can be <see cref="IntPtr.Zero"/> (null pointer is valid; parameter is optional).<br/>
        /// <strong>Possible Status Codes:</strong><br/>
        /// <see cref="STATUS_SUCCESS"/> - No error.<br/>
        /// <see cref="STATUS_NULLPTR"/> - <paramref name="filePathPtr"/> is <see cref="IntPtr.Zero"/> (null pointer).<br/>
        /// <see cref="STATUS_IMGALRINIT"/> - The are already loaded images. See: <see cref="DisposeLoadedImages"/> and <see cref="IsImagesLoaded"/><br/>
        /// <see cref="STATUS_MARSHALFAIL"/> - Failed to marshal <paramref name="filePathPtr"/> to a string.<br/>
        /// <see cref="STATUS_FAILED"/> - Failed to read the TXTR. See: <see cref="GetLastInteropError(IntPtr, bool)"/>
        /// </summary>
        /// <param name="filePathPtr">A pointer to a char buffer that contains the string of the file path to read TXTR the from</param>
        /// <param name="readMipmaps">Whether all mipmaps should be read or just the first mipmap</param>
        /// <param name="progressCallbackPtr">A function pointer to the progress callback, can be <see cref="IntPtr.Zero"/> (null pointer is valid; parameter is optional)</param>
        /// <returns>The status code of whether the method succeeded or not. See: <see cref="GetLastInteropError(IntPtr, bool)"/></returns>
        [Description("Read an TXTR file to an image.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(Open), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int Open(IntPtr filePathPtr, bool readMipmaps, IntPtr progressCallbackPtr)
        {
            if (images.Count == 0 && filePathPtr != IntPtr.Zero)
            {
                string? filePath = Marshal.PtrToStringAnsi(filePathPtr);
                if (filePath != null)
                {
                    Image<Bgra32>[] newImages;
                    try
                    {
                        newImages = TXTRFileTypeLibAPI.Read(filePath,
                            readMipmaps,
                            progressCallbackPtr != IntPtr.Zero
                            ? Marshal.GetDelegateForFunctionPointer<TXTRFileTypeLibAPI.UpdateProgressDelegate>(progressCallbackPtr)
                            : null);
                    }
                    catch (Exception e)
                    {
                        lastErrorMsg.Clear();
                        lastErrorMsg.AppendLine("Exception occurred while reading the image. Stacktrace:");
                        lastErrorMsg.Append(e);
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.AppendLine("==== DEBUG INFO ====");
                        lastErrorMsg.Append("Method name = ");
                        lastErrorMsg.Append(nameof(Open));
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.Append("File path pointer = ");
                        lastErrorMsg.Append(filePathPtr);
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.Append("File path = ");
                        lastErrorMsg.Append(filePath);
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.Append("Image count = ");
                        lastErrorMsg.Append(images.Count);
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.Append("Progress callback pointer = ");
                        lastErrorMsg.Append(progressCallbackPtr);
                        return STATUS_FAILED;
                    }
                    images.Clear();
                    images.AddRange(newImages);

                    lastErrorMsg.Clear();
                    lastErrorMsg.Append("No error");
                    return STATUS_SUCCESS;
                }
                else
                {
                    lastErrorMsg.Clear();
                    lastErrorMsg.Append("Failed to marshal input file path");
                    return STATUS_MARSHALFAIL;
                }
            }
            else
            {
                lastErrorMsg.Clear();
                if (images.Count != 0)
                {
                    lastErrorMsg.Append("There are already image(s) loaded. Please use the \"");
                    lastErrorMsg.Append(nameof(DisposeLoadedImages));
                    lastErrorMsg.Append("\" method to dispose all loaded image(s).");
                    return STATUS_IMGALRINIT;
                }
                else
                {
                    lastErrorMsg.Append("The pointer to the file path is null");
                    return STATUS_NULLPTR;
                }
            }
        }

        /// <summary>
        /// Write an image to a TXTR file.<br/>
        /// <strong>Note:</strong><br/>
        /// <paramref name="filePathPtr"/> must be passed null terminated ASCII bytes.<br/>
        /// <paramref name="progressCallbackPtr"/> can be <see cref="IntPtr.Zero"/> (null pointer is valid; parameter is optional).<br/>
        /// <strong>Possible Status Codes:</strong><br/>
        /// <see cref="STATUS_SUCCESS"/> - No error.<br/>
        /// <see cref="STATUS_NULLPTR"/> - <paramref name="filePathPtr"/> is <see cref="IntPtr.Zero"/> (null pointer).<br/>
        /// <see cref="STATUS_IMGNOTINIT"/> - The save image is not initialized. See: <see cref="InitializeSaveImage(int, int)"/> and <see cref="IsSaveImageInitialized"/><br/>
        /// <see cref="STATUS_MARSHALFAIL"/> - Failed to marshal <paramref name="filePathPtr"/> to a string.<br/>
        /// <see cref="STATUS_INVALIDARG"/> - <paramref name="textureFormat"/>, <paramref name="paletteFormat"/>, and/or <paramref name="copyPaletteSize"/> is not a valid enumeration.<br/>
        /// <see cref="STATUS_FAILED"/> - Failed to write the TXTR. See: <see cref="GetLastInteropError(IntPtr, bool)"/><br/>
        /// </summary>
        /// <param name="filePathPtr">A pointer to a char buffer that contains the string of the file path to write the TXTR to</param>
        /// <param name="textureFormat">The texture format to save the TXTR in. See: <see cref="TextureFormat"/></param>
        /// <param name="paletteFormat">The palette format to save the TXTR in. See: <see cref="PaletteFormat"/></param>
        /// <param name="copyPaletteSize">The location to write the palette length to in the TXTR. See: <see cref="CopyPaletteSize"/></param>
        /// <param name="generateMipmaps">Whether mipmaps should be generated</param>
        /// <param name="mipmapWidthLimit">Width limit for mipmap generation</param>
        /// <param name="mipmapHeightLimit">Height limit for mipmap generation</param>
        /// <param name="progressCallbackPtr">A function pointer to the progress callback, can be <see cref="IntPtr.Zero"/> (null pointer is valid; parameter is optional)</param>
        /// <returns>The status code of whether the method succeeded or not. See: <see cref="GetLastInteropError(IntPtr, bool)"/></returns>
        [Description("Write an image to a TXTR file.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(Save), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int Save(IntPtr filePathPtr, uint textureFormat, uint paletteFormat, uint copyPaletteSize,
            bool generateMipmaps, int mipmapWidthLimit, int mipmapHeightLimit, IntPtr progressCallbackPtr)
        {
            if (saveImage != null && filePathPtr != IntPtr.Zero)
            {
                string? filePath = Marshal.PtrToStringAnsi(filePathPtr);
                if (filePath != null && ((TextureFormat)textureFormat).IsDefined()
                    && ((PaletteFormat)paletteFormat).IsDefined()
                    && ((CopyPaletteSize)copyPaletteSize).IsDefined())
                {
                    try
                    {
                        TXTRFileTypeLibAPI.Write(saveImage, filePath, (TextureFormat)textureFormat, (PaletteFormat)paletteFormat,
                            (CopyPaletteSize)copyPaletteSize, generateMipmaps, mipmapWidthLimit, mipmapHeightLimit,
                            progressCallbackPtr != IntPtr.Zero
                            ? Marshal.GetDelegateForFunctionPointer<TXTRFileTypeLibAPI.UpdateProgressDelegate>(progressCallbackPtr)
                            : null);
                    }
                    catch (Exception e)
                    {
                        lastErrorMsg.Clear();
                        lastErrorMsg.AppendLine("Exception occurred while writing the image. Stacktrace:");
                        lastErrorMsg.Append(e);
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.AppendLine("==== DEBUG INFO ====");
                        lastErrorMsg.Append("Method name = ");
                        lastErrorMsg.Append(nameof(Save));
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.Append("File path pointer = ");
                        lastErrorMsg.Append(filePathPtr);
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.Append("File path = ");
                        lastErrorMsg.Append(filePath);
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.Append("Image count = ");
                        lastErrorMsg.Append(images.Count);
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.Append("Progress callback pointer = ");
                        lastErrorMsg.Append(progressCallbackPtr);
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.Append("Save image = ");
                        lastErrorMsg.Append(saveImage.ToString());
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.Append("Texture format = ");
                        if (((TextureFormat)textureFormat).IsDefined())
                            lastErrorMsg.Append((TextureFormat)textureFormat);
                        else
                        {
                            lastErrorMsg.Append("Unknown (");
                            lastErrorMsg.AppendFormat("0x{0:X8}", textureFormat);
                            lastErrorMsg.Append(')');
                        }
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.Append("Palette format = ");
                        if (((PaletteFormat)paletteFormat).IsDefined())
                            lastErrorMsg.Append((PaletteFormat)paletteFormat);
                        else
                        {
                            lastErrorMsg.Append("Unknown (");
                            lastErrorMsg.AppendFormat("0x{0:X8}", paletteFormat);
                            lastErrorMsg.Append(')');
                        }
                        lastErrorMsg.AppendLine();
                        lastErrorMsg.Append("Copy palette size = ");
                        if (((CopyPaletteSize)copyPaletteSize).IsDefined())
                            lastErrorMsg.Append((CopyPaletteSize)copyPaletteSize);
                        else
                        {
                            lastErrorMsg.Append("Unknown (");
                            lastErrorMsg.Append(copyPaletteSize);
                            lastErrorMsg.Append(')');
                        }
                        return STATUS_FAILED;
                    }

                    lastErrorMsg.Clear();
                    lastErrorMsg.Append("No error");
                    return STATUS_SUCCESS;
                }
                else
                {
                    lastErrorMsg.Clear();
                    if (filePath == null)
                    {
                        lastErrorMsg.Append("Failed to marshal input file path");
                        return STATUS_MARSHALFAIL;
                    }
                    else
                    {
                        lastErrorMsg.Append("The input ");
                        if (!((TextureFormat)textureFormat).IsDefined())
                            lastErrorMsg.Append("texture format");
                        else if (!((PaletteFormat)paletteFormat).IsDefined())
                            lastErrorMsg.Append("palette format");
                        else
                            lastErrorMsg.Append("copy palette size");
                        lastErrorMsg.Append(" does not fit any of the values in the ");
                        if (!((TextureFormat)textureFormat).IsDefined())
                            lastErrorMsg.Append(nameof(TextureFormat));
                        else if (!((PaletteFormat)paletteFormat).IsDefined())
                            lastErrorMsg.Append(nameof(PaletteFormat));
                        else
                            lastErrorMsg.Append(nameof(CopyPaletteSize));
                        lastErrorMsg.Append(" enumeration");
                        return STATUS_INVALIDARG;
                    }
                }
            }
            else
            {
                lastErrorMsg.Clear();
                if (saveImage == null)
                {
                    lastErrorMsg.Append("The save image is not initialized. Please use the \"");
                    lastErrorMsg.Append(nameof(InitializeSaveImage));
                    lastErrorMsg.Append("\" method to initialize the save image.");
                    return STATUS_IMGNOTINIT;
                }
                else
                {
                    lastErrorMsg.Append("The pointer to the file path is null");
                    return STATUS_NULLPTR;
                }
            }
        }

        /// <summary>
        /// Gets a pixel in a loaded image at the specified index.<br/>
        /// <strong>Note:</strong><br/>
        /// r, g, b, and a are written to the pointers as a byte.<br/>
        /// <strong>Possible Status Codes:</strong><br/>
        /// <see cref="STATUS_SUCCESS"/> - No error.<br/>
        /// <see cref="STATUS_INVALIDARG"/> - <paramref name="x"/> and/or <paramref name="y"/> is less than 0 and/or exceeds the loaded image width and/or height<br/>
        /// <see cref="STATUS_IDXOUTOFRANGE"/> - <paramref name="index"/> is out of range of the loaded images.<br/>
        /// <see cref="STATUS_NOLOADEDIMGS"/> - There are no images loaded. See: <see cref="Open(IntPtr, bool, IntPtr)"/> and <see cref="IsImagesLoaded"/><br/>
        /// <see cref="STATUS_NULLPTR"/> - <paramref name="rPtr"/>, <paramref name="gPtr"/>, <paramref name="bPtr"/>, and/or <paramref name="aPtr"/> is <see cref="IntPtr.Zero"/> (null pointer).
        /// </summary>
        /// <param name="index">The loaded image index</param>
        /// <param name="x">The x coordinate of the target pixel</param>
        /// <param name="y">The y coordinate of the target pixel</param>
        /// <param name="rPtr">A pointer to a byte for which the red component of the color can be written to</param>
        /// <param name="gPtr">A pointer to a byte for which the green component of the color can be written to</param>
        /// <param name="bPtr">A pointer to a byte for which the blue component of the color can be written to</param>
        /// <param name="aPtr">A pointer to a byte for which the alpha component of the color can be written to</param>
        /// <returns>The status code of whether the method succeeded or not. See: <see cref="GetLastInteropError(IntPtr, bool)"/></returns>
        [Description("Gets a pixel in a loaded image at the specified index.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(GetPixel), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int GetPixel(int index, int x, int y, IntPtr rPtr, IntPtr gPtr, IntPtr bPtr, IntPtr aPtr)
        {
            if (rPtr != IntPtr.Zero && gPtr != IntPtr.Zero && bPtr != IntPtr.Zero && aPtr != IntPtr.Zero
                && images.Count != 0 && index >= 0 && index < images.Count && x >= 0
                && x < images[index].Width && y >= 0 && y < images[index].Height)
            {
                Marshal.WriteByte(rPtr, images[index][x, y].R);
                Marshal.WriteByte(gPtr, images[index][x, y].G);
                Marshal.WriteByte(bPtr, images[index][x, y].B);
                Marshal.WriteByte(aPtr, images[index][x, y].A);

                lastErrorMsg.Clear();
                lastErrorMsg.Append("No error");
                return STATUS_SUCCESS;
            }
            else
            {
                lastErrorMsg.Clear();
                if (rPtr == IntPtr.Zero || gPtr == IntPtr.Zero || bPtr == IntPtr.Zero || aPtr == IntPtr.Zero)
                {
                    lastErrorMsg.Append("The input pointer to the output ");
                    if (rPtr == IntPtr.Zero)
                        lastErrorMsg.Append('r');
                    else if (gPtr == IntPtr.Zero)
                        lastErrorMsg.Append('g');
                    else if (bPtr == IntPtr.Zero)
                        lastErrorMsg.Append('b');
                    else
                        lastErrorMsg.Append('a');
                    lastErrorMsg.Append(" is null");
                    return STATUS_NULLPTR;
                }
                else if (images.Count == 0)
                {
                    lastErrorMsg.Append("There are no images loaded");
                    return STATUS_NOLOADEDIMGS;
                }
                else if (index < 0 || index >= images.Count)
                {
                    lastErrorMsg.Append("The specified index ");
                    lastErrorMsg.Append(index);
                    lastErrorMsg.Append(" is out of range of the loaded images");
                    return STATUS_IDXOUTOFRANGE;
                }
                else
                {
                    lastErrorMsg.Append("The input ");
                    if (x <= 0 || x >= images[index].Width)
                    {
                        lastErrorMsg.Append("x (");
                        lastErrorMsg.Append(x);
                        lastErrorMsg.Append(')');
                    }
                    else
                    {
                        lastErrorMsg.Append("y (");
                        lastErrorMsg.Append(y);
                        lastErrorMsg.Append(')');
                    }
                    lastErrorMsg.Append(" coordinate exceeds the image ");
                    if (x <= 0 || x >= images[index].Width)
                    {
                        lastErrorMsg.Append("width (");
                        lastErrorMsg.Append(images[index].Width);
                        lastErrorMsg.Append(')');
                    }
                    else
                    {
                        lastErrorMsg.Append("height (");
                        lastErrorMsg.Append(images[index].Height);
                        lastErrorMsg.Append(')');
                    }
                    return STATUS_INVALIDARG;
                }
            }
        }

        /// <summary>
        /// Sets a pixel to the passed RGBA color values on the save image.<br/>
        /// See: <see cref="InitializeSaveImage(int, int)"/>, <see cref="IsSaveImageInitialized"/>, and <see cref="DisposeSaveImage"/><br/>
        /// <strong>Possible Status Codes:</strong><br/>
        /// <see cref="STATUS_SUCCESS"/> - No error.<br/>
        /// <see cref="STATUS_INVALIDARG"/> - <paramref name="x"/> and/or <paramref name="y"/> is less than 0 and/or exceeds the save image width and/or height<br/>
        /// <see cref="STATUS_IMGNOTINIT"/> - The save image is not initialized. See: <see cref="InitializeSaveImage(int, int)"/> and <see cref="IsSaveImageInitialized"/>
        /// </summary>
        /// <param name="x">The x coordinate of the target pixel</param>
        /// <param name="y">The y coordinate of the target pixel</param>
        /// <param name="r">The red component of the color</param>
        /// <param name="g">The green component of the color</param>
        /// <param name="b">The blue component of the color</param>
        /// <param name="a">The alpha component of the color</param>
        /// <returns>The status code of whether the method succeeded or not. See: <see cref="GetLastInteropError(IntPtr, bool)"/></returns>
        [Description("Sets a pixel to the passed RGBA color values on the save image.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(SetPixel), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int SetPixel(int x, int y, byte r, byte g, byte b, byte a)
        {
            if (saveImage != null && x >= 0 && x < saveImage.Width && y >= 0 && y < saveImage.Height)
            {
                Bgra32 pixel = saveImage[x, y];
                pixel.R = r;
                pixel.G = g;
                pixel.B = b;
                pixel.A = a;
                saveImage[x, y] = pixel;

                lastErrorMsg.Clear();
                lastErrorMsg.Append("No error");
                return STATUS_SUCCESS;
            }
            else
            {
                lastErrorMsg.Clear();
                if (saveImage == null)
                {
                    lastErrorMsg.Append("The save image is not initialized. Please use the \"");
                    lastErrorMsg.Append(nameof(InitializeSaveImage));
                    lastErrorMsg.Append("\" method to initialize the save image.");
                    return STATUS_IMGNOTINIT;
                }
                else
                {
                    lastErrorMsg.Append("The input ");
                    if (x <= 0 || x >= saveImage.Width)
                    {
                        lastErrorMsg.Append("x (");
                        lastErrorMsg.Append(x);
                        lastErrorMsg.Append(')');
                    }
                    else
                    {
                        lastErrorMsg.Append("y (");
                        lastErrorMsg.Append(y);
                        lastErrorMsg.Append(')');
                    }
                    lastErrorMsg.Append(" coordinate ");
                    if (x <= 0 || x >= saveImage.Width)
                    {
                        if (x <= 0)
                            lastErrorMsg.Append("is less than 0");
                        else
                        {
                            lastErrorMsg.Append("exceeds the image width(");
                            lastErrorMsg.Append(saveImage.Width);
                            lastErrorMsg.Append(')');
                        }
                    }
                    else
                    {
                        if (y <= 0)
                            lastErrorMsg.Append("is less than 0");
                        else
                        {
                            lastErrorMsg.Append("exceeds the image height (");
                            lastErrorMsg.Append(saveImage.Height);
                            lastErrorMsg.Append(')');
                        }
                    }
                    return STATUS_INVALIDARG;
                }
            }
        }

        /// <summary>
        /// Gets whether there are any loaded images.<br/>
        /// See: <see cref="Open(IntPtr, bool, IntPtr)"/>
        /// </summary>
        /// <returns><see langword="true"/> if there are any loaded images else <see langword="false"/></returns>
        [Description("Gets whether there are any loaded images.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(IsImagesLoaded), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static bool IsImagesLoaded() => images.Count > 0;

        /// <summary>
        /// Intitializes the save image with the specified width and height.<br/>
        /// <strong>Possible Status Codes:</strong><br/>
        /// <see cref="STATUS_SUCCESS"/> - No error.<br/>
        /// <see cref="STATUS_INVALIDARG"/> - <paramref name="width"/> and/or <paramref name="height"/> is less than or equal to 0.<br/>
        /// <see cref="STATUS_IMGALRINIT"/> - The save image is already initialized. See: <see cref="DisposeSaveImage"/> and <see cref="IsSaveImageInitialized"/><br/>
        /// <see cref="STATUS_FAILED"/> - Failed initializing the image. See: <see cref="GetLastInteropError(IntPtr, bool)"/>
        /// </summary>
        /// <param name="width">The width of the save image</param>
        /// <param name="height">The height of the save image</param>
        /// <returns>The status code of whether the method succeeded or not. See: <see cref="GetLastInteropError(IntPtr, bool)"/></returns>
        [Description("Intitializes the save image with the specified width and height.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(InitializeSaveImage), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int InitializeSaveImage(int width, int height)
        {
            if (saveImage == null && width > 0 && height > 0)
            {
                try
                {
                    saveImage = new Image<Bgra32>(width, height);
                }
                catch (Exception e)
                {
                    lastErrorMsg.Clear();
                    lastErrorMsg.AppendLine("Exception occurred while initializing the save image. Stacktrace:");
                    lastErrorMsg.Append(e);
                    lastErrorMsg.AppendLine();
                    lastErrorMsg.AppendLine("==== DEBUG INFO ====");
                    lastErrorMsg.Append("Method name = ");
                    lastErrorMsg.Append(nameof(InitializeSaveImage));
                    lastErrorMsg.AppendLine();
                    lastErrorMsg.Append("Width = ");
                    lastErrorMsg.Append(width);
                    lastErrorMsg.AppendLine();
                    lastErrorMsg.Append("Height = ");
                    lastErrorMsg.Append(height);
                    lastErrorMsg.AppendLine();
                    lastErrorMsg.Append("Image count = ");
                    lastErrorMsg.Append(images.Count);
                    lastErrorMsg.AppendLine();
                    lastErrorMsg.Append("Save image = ");
                    lastErrorMsg.Append(saveImage?.ToString() ?? "null");
                    return STATUS_FAILED;
                }

                lastErrorMsg.Clear();
                lastErrorMsg.Append("No error");
                return STATUS_SUCCESS;
            }
            else
            {
                lastErrorMsg.Clear();
                if (saveImage != null)
                {
                    lastErrorMsg.Append("The save image is already initialized. Please use the \"");
                    lastErrorMsg.Append(nameof(DisposeSaveImage));
                    lastErrorMsg.Append("\" method to dispose the save image.");
                    return STATUS_IMGALRINIT;
                }
                else
                {
                    lastErrorMsg.Append("The input ");
                    if (width <= 0)
                        lastErrorMsg.Append("width");
                    else
                        lastErrorMsg.Append("height");
                    lastErrorMsg.Append(" must be greater than 0");
                    return STATUS_INVALIDARG;
                }
            }
        }

        /// <summary>
        /// Gets whether the save image was initialized.<br/>
        /// See: <see cref="InitializeSaveImage(int, int)"/>
        /// </summary>
        /// <returns><see langword="true"/> if the save image is initialized else <see langword="false"/></returns>
        [Description("ets whether the save image was initialized.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(IsSaveImageInitialized), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static bool IsSaveImageInitialized() => saveImage != null;

        /// <summary>
        /// Gets the count of loaded images.<br/>
        /// <strong>Note:</strong><br/>
        /// The loaded image count is written to the pointer as a signed 32bit integer.<br/>
        /// <strong>Possible Status Codes:</strong><br/>
        /// <see cref="STATUS_SUCCESS"/> - No error.<br/>
        /// <see cref="STATUS_NULLPTR"/> - <paramref name="imageCountPtr"/> is <see cref="IntPtr.Zero"/> (null pointer).<br/>
        /// <see cref="STATUS_NOLOADEDIMGS"/> - There are no images loaded. See: <see cref="Open(IntPtr, bool, IntPtr)"/> and <see cref="IsImagesLoaded"/>
        /// </summary>
        /// <param name="imageCountPtr">A pointer to an int for which the loaded image count can be written to</param>
        /// <returns>The status code of whether the method succeeded or not. See: <see cref="GetLastInteropError(IntPtr, bool)"/></returns>
        [Description("Gets the count of loaded images.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(GetImageCount), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int GetImageCount(IntPtr imageCountPtr)
        {
            if (imageCountPtr != IntPtr.Zero && images.Count != 0)
            {
                Marshal.WriteInt32(imageCountPtr, images.Count);

                lastErrorMsg.Clear();
                lastErrorMsg.Append("No error");
                return STATUS_SUCCESS;
            }
            else
            {
                lastErrorMsg.Clear();
                if (imageCountPtr == IntPtr.Zero)
                {
                    lastErrorMsg.Append("The input pointer to the output image count is null");
                    return STATUS_NULLPTR;
                }
                else
                {
                    lastErrorMsg.Append("There are no images loaded");
                    return STATUS_NOLOADEDIMGS;
                }
            }
        }

        /// <summary>
        /// Get the dimensions (width and height) of a loaded image.<br/>
        /// <strong>Note:</strong><br/>
        /// The width and height are written to the pointers as signed 32bit integers.<br/>
        /// <strong>Possible Status Codes:</strong><br/>
        /// <see cref="STATUS_SUCCESS"/> - No error.<br/>
        /// <see cref="STATUS_NULLPTR"/> - <paramref name="widthPtr"/> and/or <paramref name="heightPtr"/> is <see cref="IntPtr.Zero"/> (null pointer).<br/>
        /// <see cref="STATUS_NOLOADEDIMGS"/> - There are no images loaded. See: <see cref="Open(IntPtr, bool, IntPtr)"/> and <see cref="IsImagesLoaded"/><br/>
        /// <see cref="STATUS_IDXOUTOFRANGE"/> - <paramref name="index"/> is out of range of the loaded images.
        /// </summary>
        /// <param name="index">The loaded image index</param>
        /// <param name="widthPtr">A pointer to an int for which the width can be written to</param>
        /// <param name="heightPtr">A pointer to an int for which the height can be written to</param>
        /// <returns>The status code of whether the method succeeded or not. See: <see cref="GetLastInteropError(IntPtr, bool)"/></returns>
        [Description("Get the dimensions (width and height) of a loaded image.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(GetImageDimensions), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int GetImageDimensions(int index, IntPtr widthPtr, IntPtr heightPtr)
        {
            if (widthPtr != IntPtr.Zero && heightPtr != IntPtr.Zero && images.Count != 0
                && index >= 0 && index < images.Count)
            {
                Marshal.WriteInt32(widthPtr, images[index].Width);
                Marshal.WriteInt32(heightPtr, images[index].Height);

                lastErrorMsg.Clear();
                lastErrorMsg.Append("No error");
                return STATUS_SUCCESS;
            }
            else
            {
                lastErrorMsg.Clear();
                if (widthPtr == IntPtr.Zero || heightPtr == IntPtr.Zero)
                {
                    lastErrorMsg.Append("The input pointer to the output ");
                    if (widthPtr == IntPtr.Zero)
                        lastErrorMsg.Append("width");
                    else
                        lastErrorMsg.Append("height");
                    lastErrorMsg.Append(" is null");
                    return STATUS_NULLPTR;
                }
                else if (images.Count == 0)
                {
                    lastErrorMsg.Append("There are no images loaded");
                    return STATUS_NOLOADEDIMGS;
                }
                else
                {
                    lastErrorMsg.Append("The specified index ");
                    lastErrorMsg.Append(index);
                    lastErrorMsg.Append(" is out of range of the loaded images");
                    return STATUS_IDXOUTOFRANGE;
                }
            }
        }

        /// <summary>
        /// Get the last error message.<br/>
        /// <strong>Note:</strong><br/>
        /// The last error message is not modified by this method.<br/>
        /// The last error message is written to the pointer as ASCII bytes.<br/>
        /// The last error message will not be null terminated unless <paramref name="nullTerminated"/>
        /// is <see langword="true"/>.<br/>
        /// <strong>Possible Status Codes:</strong><br/>
        /// <see cref="STATUS_SUCCESS"/> - No error.<br/>
        /// <see cref="STATUS_NULLPTR"/> - <paramref name="errorMsgPtr"/> is <see cref="IntPtr.Zero"/> (null pointer).
        /// </summary>
        /// <param name="errorMsgPtr">A pointer to a char buffer for which the last error message can be written to</param>
        /// <param name="nullTerminated">Whether the last error message will be null terminated (for compatibility with python ctypes)</param>
        /// <returns>The status code of whether the method succeeded or not</returns>
        [Description("Get the last error message.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(GetLastInteropError), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int GetLastInteropError(IntPtr errorMsgPtr, bool nullTerminated)
        {
            // This must NOT modify the last error message!
            if (errorMsgPtr != IntPtr.Zero)
            {
                string msg = lastErrorMsg.ToString();
                if (nullTerminated)
                    msg += '\0';
                Marshal.Copy(Encoding.ASCII.GetBytes(msg), 0, errorMsgPtr, lastErrorMsg.Length);
                return STATUS_SUCCESS;
            }
            else
                return STATUS_NULLPTR;
        }

        /// <summary>
        /// Dispose all loaded images if there are any loaded.<br/>
        /// <strong>Note:</strong><br/>
        /// It is strongly advised you call this at all possible endpoints in the control flow of your code or else memory leaks will ensue.<br/>
        /// <strong>Possible Status Codes:</strong><br/>
        /// <see cref="STATUS_SUCCESS"/> - No error.<br/>
        /// <see cref="STATUS_NOLOADEDIMGS"/> - There are no images loaded. See: <see cref="Open(IntPtr, bool, IntPtr)"/> and <see cref="IsImagesLoaded"/>
        /// </summary>
        /// <returns>The status code of whether the method succeeded or not. See: <see cref="GetLastInteropError(IntPtr, bool)"/></returns>
        [Description(" Dispose all loaded images if there are any loaded.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(DisposeLoadedImages), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int DisposeLoadedImages()
        {
            if (images.Count != 0)
            {
                for (int i = 0; i < images.Count; i++)
                    images[i].Dispose();
                images.Clear();

                lastErrorMsg.Clear();
                lastErrorMsg.Append("No error");
                return STATUS_SUCCESS;
            }
            else
            {
                lastErrorMsg.Clear();
                lastErrorMsg.Append("There are no images loaded");
                return STATUS_NOLOADEDIMGS;
            }
        }

        /// <summary>
        /// Dispose the save image if it is initialized.<br/>
        /// <strong>Note:</strong><br/>
        /// It is strongly advised you call this at all possible endpoints in the control flow of your code or else memory leaks will ensue.<br/>
        /// <strong>Possible Status Codes:</strong><br/>
        /// <see cref="STATUS_SUCCESS"/> - No error.<br/>
        /// <see cref="STATUS_IMGNOTINIT"/> - The save image is already disposed. See: <see cref="InitializeSaveImage(int, int)"/> and <see cref="IsSaveImageInitialized"/>
        /// </summary>
        /// <returns>The status code of whether the method succeeded or not. See: <see cref="GetLastInteropError(IntPtr, bool)"/></returns>
        [Description("Dispose the save image if it is initialized.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(DisposeSaveImage), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int DisposeSaveImage()
        {
            if (saveImage != null)
            {
                saveImage.Dispose();
                saveImage = null;

                lastErrorMsg.Clear();
                lastErrorMsg.Append("No error");
                return STATUS_SUCCESS;
            }
            else
            {
                lastErrorMsg.Clear();
                lastErrorMsg.Append("The save image is already disposed.");
                return STATUS_IMGNOTINIT;
            }
        }

        /// <summary>
        /// Get a description from an enum that's supported by this API.<br/>
        /// <strong>Note:</strong><br/>
        /// The description is written to the pointer as ASCII bytes.<br/>
        /// The description will not be null terminated unless <paramref name="nullTerminated"/>
        /// is <see langword="true"/>.<br/>
        /// <strong>Possible Status Codes:</strong><br/>
        /// <see cref="STATUS_SUCCESS"/> - No error.<br/>
        /// <see cref="STATUS_INVALIDARG"/> - <paramref name="enumType"/> is invalid.<br/>
        /// <see cref="STATUS_FAILED"/> - Failed to get enum description. <paramref name="enumValue"/> might not fit the enum fields.<br/>
        /// <see cref="STATUS_NULLPTR"/> - <paramref name="descriptionPtr"/> is <see cref="IntPtr.Zero"/> (null pointer).
        /// </summary>
        /// <param name="enumType">The enum type code</param>
        /// <param name="enumValue">The enum value</param>
        /// <param name="descriptionPtr">A pointer to a char buffer for which the description can be copied to</param>
        /// <param name="nullTerminated">Whether the description will be null terminated (for compatibility with python ctypes)</param>
        /// <returns>The status code of whether the method succeeded or not. See: <see cref="GetLastInteropError(IntPtr, bool)"/></returns>
        [Description("Get a description from an enum that's supported by this API.")]
        [UnmanagedCallersOnly(EntryPoint = nameof(GetEnumDescription), CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int GetEnumDescription(int enumType, uint enumValue, IntPtr descriptionPtr, bool nullTerminated)
        {
            if (descriptionPtr != IntPtr.Zero)
            {
                string? enumDescription;
                switch (enumType)
                {
                    case ENUMTYPE_TEXTUREFORMAT:
                        enumDescription = ((TextureFormat)enumValue).GetDescription();
                        break;
                    case ENUMTYPE_PALETTEFORMAT:
                        enumDescription = ((PaletteFormat)enumValue).GetDescription();
                        break;
                    case ENUMTYPE_COPYPALETTESIZE:
                        enumDescription = ((CopyPaletteSize)enumValue).GetDescription();
                        break;
                    default:
                        lastErrorMsg.Clear();
                        lastErrorMsg.Append("Invalid enum type value: ");
                        lastErrorMsg.Append(enumType);
                        return STATUS_INVALIDARG;
                }

                if (enumDescription != null)
                {
                    if (nullTerminated)
                        enumDescription += '\0';
                    Marshal.Copy(Encoding.ASCII.GetBytes(enumDescription), 0, descriptionPtr, enumDescription.Length);

                    lastErrorMsg.Clear();
                    lastErrorMsg.Append("No error");
                    return STATUS_SUCCESS;
                }
                else
                {
                    lastErrorMsg.Clear();
                    lastErrorMsg.Append("Failed to get enum description. Enum value might not fit the enum fields.");
                    return STATUS_FAILED;
                }
            }
            else
            {
                lastErrorMsg.Clear();
                lastErrorMsg.Append("The input pointer to the output enum description is null");
                return STATUS_NULLPTR;
            }
        }
    }
}
