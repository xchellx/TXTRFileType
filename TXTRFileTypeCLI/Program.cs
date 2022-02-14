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

using CommandLine;
using libWiiSharp.Extensions;
using libWiiSharp.Formats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TXTRFileTypeCLI.Logging;
using TXTRFileTypeCLI.Util;
using TXTRFileTypeLib;

namespace TXTRFileTypeCLI
{
    public class Program
    {
        internal static readonly ConsoleLogger logger = new(nameof(Program), LoggerSeverity.INFO,
            ConsoleLoggerFlags.CONSOLE | ConsoleLoggerFlags.DEBUG);

        public static int Main(string[] args)
            => new Parser(config => {
                config.HelpWriter = Console.Error;
                config.MaximumDisplayWidth = Console.BufferWidth;
                config.ParsingCulture = CultureInfo.CurrentCulture;
            })
            .ParseArguments<DecodeOptions, EncodeOptions>(args)
            .MapResult<DecodeOptions, EncodeOptions, int>(OnDecode, OnEncode, OnError);

        private static int OnDecode(DecodeOptions options)
        {
            if (options.NoWarn)
                logger.Severity = LoggerSeverity.ERROR;
            else if (options.Verbose)
                logger.Severity = LoggerSeverity.VERB;
            if (options.Quiet)
                logger.Flags |= ConsoleLoggerFlags.QUIET;
            if (options.CliColors)
                logger.Flags |= ConsoleLoggerFlags.COLORS;

            logger.VerbLine("options = {0}", options);

            if (IOUtil.FileExists(options.Input, out string fileExistsFailReason))
            {
                Image<Bgra32>[] images;
                try
                {
                    logger.Info("Decoding TXTR \"{0}\"", options.Input);
                    logger.Flags |= ConsoleLoggerFlags.NOTAG;
                    images = TXTRFileTypeLibAPI.Read(options.Input, options.DecodeMipmaps, ProgressMessage);
                    logger.InfoLine();
                    logger.Flags &= ~ConsoleLoggerFlags.NOTAG;
                }
                catch (Exception e)
                {
                    if ((logger.Flags & ConsoleLoggerFlags.NOTAG) != ConsoleLoggerFlags.NOTAG)
                        logger.Flags |= ConsoleLoggerFlags.NOTAG;
                    logger.InfoLine();
                    logger.Flags &= ~ConsoleLoggerFlags.NOTAG;
                    logger.VerbLine("Read TXTR - Failed");
                    logger.ErrorLine("Failed to read TXTR.{0}{1}", Environment.NewLine, e);
                    return HRESULT.E_FAIL;
                }

                if (images.Length > 0)
                {
                    if (!IOUtil.DirectoryExists(options.Output, out _))
                    {
                        if (AskYesNo($"The directory \"{options.Output}\" does not exist. Do you want to create it? [y/n]", options.DontAsk))
                            Directory.CreateDirectory(options.Output);

                        if (!IOUtil.DirectoryExists(options.Output, out string directoryExistsFailReason))
                        {
                            logger.VerbLine("Output Directory Exists - Failed");
                            logger.ErrorLine(directoryExistsFailReason);
                            return HRESULT.E_INVALIDARG;
                        }
                    }

                    string fileName = $"{options.OutputPrefix}{Path.GetFileNameWithoutExtension(new FileInfo(options.Input).Name)}{options.OutputSuffix}";
                    logger.VerbLine("fileName = {0}", fileName);
                    bool dontWrite = false;
                    for (int i = 0; i < images.Length; i++)
                    {
                        using (images[i])
                        {
                            string filePath = Path.Combine(options.Output, $"{fileName}{i + 1}.png");
                            logger.VerbLine("filePath = {0}", filePath);
                            if (IOUtil.FileExists(filePath, out _))
                            {
                                if (!AskYesNo($"The file \"{filePath}\" already exists, do you want to overwrite it? [y/n]", options.ForceOverwrite))
                                {
                                    logger.WarnLine("The file \"{0}\" already exists, nothing will be written.", filePath);
                                    dontWrite = true;
                                }
                            }

                            if (!dontWrite)
                            {
                                logger.VerbLine("images[i] = {0}", images[i]);
                                logger.InfoLine("Saving mipmap {0} to \"{1}\"", i + 1, filePath);
                                images[i].SaveAsPng(filePath, new PngEncoder()
                                {
                                    BitDepth = PngBitDepth.Bit16,
                                    ChunkFilter = PngChunkFilter.None,
                                    ColorType = PngColorType.RgbWithAlpha,
                                    CompressionLevel = PngCompressionLevel.NoCompression,
                                    FilterMethod = PngFilterMethod.None,
                                    Gamma = 0.45f, // https://en.wikipedia.org/wiki/Gamma_correction
                                    IgnoreMetadata = false,
                                    InterlaceMethod = PngInterlaceMode.None,
                                    Quantizer = new WuQuantizer(new QuantizerOptions()
                                    {
                                        Dither = null,
                                        DitherScale = 0,
                                        MaxColors = 256
                                    }),
                                    TransparentColorMode = PngTransparentColorMode.Preserve
                                });
                            }
                            else
                                dontWrite = false;
                        }
                    }

                    logger.InfoLine("Decode finished");
                    return HRESULT.S_OK;
                }
                else
                {
                    logger.VerbLine("Images Loaded - Failed");
                    logger.ErrorLine("No images were loaded");
                    return HRESULT.E_FAIL;
                }
            }
            else
            {
                logger.VerbLine("Input File Exists - Failed");
                logger.ErrorLine(fileExistsFailReason);
                return HRESULT.E_INVALIDARG;
            }
        }

        private static int OnEncode(EncodeOptions options)
        {
            if (options.NoWarn)
                logger.Severity = LoggerSeverity.ERROR;
            else if (options.Verbose)
                logger.Severity = LoggerSeverity.VERB;
            if (options.Quiet)
                logger.Flags |= ConsoleLoggerFlags.QUIET;
            if (options.CliColors)
                logger.Flags |= ConsoleLoggerFlags.COLORS;

            logger.VerbLine("options = {0}", options);

            if (IOUtil.FileExists(options.Input, out string fileExistsFailReason))
            {
                bool isIndex = options.TextureFormat == TextureFormat.CI4
                    && options.TextureFormat == TextureFormat.CI8
                    && options.TextureFormat == TextureFormat.CI14X2;
                if (!options.TextureFormat.IsDefined())
                {
                    logger.VerbLine("Valid TextureFormat - Failed");
                    logger.Error("Invalid texture format specified");
                    return HRESULT.E_INVALIDARG;
                }
                else if (!options.PaletteFormat.IsDefined())
                {
                    logger.VerbLine("Valid PaletteFormat - Failed");
                    logger.Error("Invalid palette format specified");
                    return HRESULT.E_INVALIDARG;
                }
                else if (!options.CopyPaletteSize.IsDefined())
                {
                    logger.VerbLine("Valid CopyPaletteSize - Failed");
                    logger.Error("Invalid copy palette size specified");
                    return HRESULT.E_INVALIDARG;
                }
                else if (isIndex && options.GenerateMipmaps)
                {
                    logger.VerbLine("Indexed Without Mipmaps - Failed");
                    logger.Error("Indexed formats cannot have mipmaps");
                    return HRESULT.E_INVALIDARG;
                }
                else if (!isIndex && (options.MipmapWidthLimit < 1 || options.MipmapHeightLimit < 1))
                {
                    logger.VerbLine("Mipmap Width/Height Limit Minimum - Failed");
                    logger.Error($"Mipmap {(options.MipmapWidthLimit < 1 ? "width" : "height")} limit subceeds minimum value 0");
                    return HRESULT.E_INVALIDARG;
                }

                string outputDirectory = Path.GetDirectoryName(options.Output) ?? string.Empty;
                if (outputDirectory.Trim() == string.Empty)
                    outputDirectory = Directory.GetCurrentDirectory();
                logger.VerbLine("outputDirectory = {0}", outputDirectory);
                if (!IOUtil.DirectoryExists(outputDirectory, out _))
                {
                    if (AskYesNo($"The directory \"{outputDirectory}\" does not exist. Do you want to create it? [y/n]", options.DontAsk))
                        Directory.CreateDirectory(outputDirectory);

                    if (!IOUtil.DirectoryExists(outputDirectory, out string directoryExistsFailReason))
                    {
                        logger.VerbLine("Input Directory Exists - Failed");
                        logger.ErrorLine(directoryExistsFailReason);
                        return HRESULT.E_INVALIDARG;
                    }
                }
                if (IOUtil.FileExists(options.Output, out _))
                {
                    if (!AskYesNo($"The file \"{options.Output}\" already exists, do you want to overwrite it? [y/n]", options.ForceOverwrite))
                    {
                        logger.ErrorLine("The file \"{0}\" already exists, nothing will be written.", options.Output);
                        return HRESULT.E_ABORT;
                    }
                }

                Image<Bgra32>? image = null;
                try
                {
                    logger.InfoLine("Loading image \"{0}\"", options.Input);
                    image = Image.Load<Bgra32>(options.Input, new PngDecoder() { IgnoreMetadata = false });
                    logger.VerbLine("image = {0}", image);

                    logger.Info("Encoding image \"{0}\" to \"{1}\"", options.Input, options.Output);
                    logger.Flags |= ConsoleLoggerFlags.NOTAG;
                    TXTRFileTypeLibAPI.Write(image, options.Output, options.TextureFormat,
                        options.PaletteFormat, options.CopyPaletteSize, options.GenerateMipmaps,
                        options.MipmapWidthLimit, options.MipmapHeightLimit, ProgressMessage);
                    logger.InfoLine();
                    logger.Flags &= ~ConsoleLoggerFlags.NOTAG;
                }
                catch (Exception e)
                {
                    if ((logger.Flags & ConsoleLoggerFlags.NOTAG) != ConsoleLoggerFlags.NOTAG)
                        logger.Flags |= ConsoleLoggerFlags.NOTAG;
                    logger.InfoLine();
                    logger.Flags &= ~ConsoleLoggerFlags.NOTAG;
                    logger.VerbLine("Write TXTR - Failed");
                    logger.ErrorLine("Failed to read TXTR.{0}{1}", Environment.NewLine, e);
                    return HRESULT.E_FAIL;
                }
                finally
                {
                    image?.Dispose();
                }

                logger.InfoLine("Encode finished");
                return HRESULT.S_OK;
            }
            else
            {
                logger.VerbLine("Input Exists - Failed");
                logger.ErrorLine(fileExistsFailReason);
                return HRESULT.E_INVALIDARG;
            }
        }

        private static int OnError(IEnumerable<Error> errors)
        {
            bool errorsExist = false;
            foreach (Error error in errors)
            {
                errorsExist = true;
                if (!error.GetType().ToString().ToLower().Contains("help"))
                {
                    Console.WriteLine(error);
                    Console.WriteLine();
                }
            }

            if (errorsExist)
            {
                string n = Environment.NewLine;
                Console.WriteLine(
                    "Notes:"
                    + $"{n}    - Odd texture sizes might cause weird results and/or errors."

                    + $"{n}{n}Texture Formats:"
                    + $"{n}    - I4       = 4-bit greyscale intensity values. Two pixels per byte."
                    + $"{n}    - I8       = 8-bit greyscale intensity values."
                    + $"{n}    - IA4      = 4-bit greyscale intensity values with an additional 4-bit alpha channel."
                    + $"{n}    - IA8      = 8-bit greyscale intensity values with an additional 8-bit alpha channel."
                    + $"{n}    - CI4      = 4-bit palette indices."
                    + $"{n}    - CI8      = 8-bit palette indices."
                    + $"{n}    - CI14X2   = 14-bit palette indices (14b index)."
                    + $"{n}    - RGB565   = 16-bit colors without alpha."
                    + $"{n}    - RGB5A3   = 16-bit colors with alpha."
                    + $"{n}    - RGBA32   = Uncompressed 32-bit colors with alpha."
                    + $"{n}    - CMPR     = DXT1 Compression"

                    + $"{n}{n}Palette Formats:"
                    + $"{n}    - IA8      = 8-bit greyscale intensity values with an additional 8-bit alpha channel."
                    + $"{n}    - RGB565   = 16-bit colors without alpha."
                    + $"{n}    - RGB5A3   = 16-bit colors with alpha."

                    + $"{n}{n}Copy Palette Size Locations:"
                    + $"{n}    - ToWidth  = Copy palette length to width."
                    + $"{n}    - ToHeight = Copy palette length to height."
                );
            }

            return HRESULT.S_FALSE;
        }

        private static bool AskYesNo(string message, bool overrideYN = false)
        {
            if (!overrideYN)
            {
                Console.Error.WriteLine(message);
                return Console.ReadKey(true).Key != ConsoleKey.N;
            }
            else
                return true;
        }

        private static void ProgressMessage(double progress, double max)
        {
            logger.Info(" ");
            logger.Info(Math.Floor((progress / max) * 100.0d));
        }
    }
}