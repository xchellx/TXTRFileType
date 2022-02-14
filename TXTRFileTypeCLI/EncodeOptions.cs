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

using CommandLine.Text;
using CommandLine;
using System.Collections.Generic;
using System;
using libWiiSharp.Formats;
using libWiiSharp;

namespace TXTRFileTypeCLI
{
    [Verb("encode", HelpText = "Encode an image file to a TXTR file.")]
    internal class EncodeOptions
    {
        [Value(0,
            Required = true,
            HelpText = "The image file to encode.",
            MetaName = nameof(Input))]
        public string Input { get; set; } = string.Empty;

        [Value(1,
            Required = true,
            HelpText = "The output TXTR to encode the image to.",
            MetaName = nameof(Output))]
        public string Output { get; set; } = string.Empty;

        [Option('y', "yes",
            Default = false,
            HelpText = "Do not ask if want to create the folder(s) leading to the output.")]
        public bool DontAsk { get; set; } = false;

        [Option('f', "force",
            Default = false,
            HelpText = "Do not ask if want to overwrite files.")]
        public bool ForceOverwrite { get; set; } = false;

        [Option('t', "texformat",
            Default = TextureFormat.I4,
            HelpText = "The TXTR texture format to encode with.")]
        public TextureFormat TextureFormat { get; set; } = TextureFormat.I4;

        [Option('p', "palformat",
            Default = PaletteFormat.IA8,
            HelpText = "The TXTR palette format to encode with.")]
        public PaletteFormat PaletteFormat { get; set; } = PaletteFormat.IA8;

        [Option('C', "cpypalsize",
            Default = CopyPaletteSize.ToWidth,
            HelpText = "Whether the palette length should be copied to the palette width or palette height.")]
        public CopyPaletteSize CopyPaletteSize { get; set; } = CopyPaletteSize.ToWidth;

        [Option('m', "mipmaps",
            Default = false,
            HelpText = "Generate mipmaps.")]
        public bool GenerateMipmaps { get; set; } = false;

        [Option('W', "wlimit",
            Default = 4,
            HelpText = "The width limit for mipmap generation.")]
        public int MipmapWidthLimit { get; set; } = 4;

        [Option('H', "hlimit",
            Default = 4,
            HelpText = "The height limit for mipmap generation.")]
        public int MipmapHeightLimit { get; set; } = 4;

        [Option('v', "verbose",
            Default = false,
            HelpText = "Output extra verbose information.")]
        public bool Verbose { get; set; } = false;

        [Option('w', "nowarn",
            Default = false,
            HelpText = "Disable warning messages.")]
        public bool NoWarn { get; set; } = false;

        [Option('q', "quiet",
            Default = false,
            HelpText = "Be quiet (do not output any information).")]
        public bool Quiet { get; set; } = false;

        [Option('c', "color",
            Default = false,
            HelpText = "Output to console with colors (for console logging only).")]
        public bool CliColors { get; set; } = false;

        [Usage(ApplicationAlias = "txtrtool")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>() {
                        new Example("Encode test.png into test.TXTR with no mipmap generation and the texture format RGB5A3",
                        new UnParserSettings() { SkipDefault = true },
                        new EncodeOptions {
                            Input = @"test.png",
                            Output = @"test.TXTR",
                            TextureFormat = TextureFormat.RGB5A3
                        }),
                        new Example("Encode test.png into test.TXTR with no mipmap generation, texture format CI8, palette format RGB5A3, and copying the palette length to the palette height",
                        new UnParserSettings() { SkipDefault = true },
                        new EncodeOptions {
                            Input = @"test.png",
                            Output = @"test.TXTR",
                            TextureFormat = TextureFormat.CI8,
                            PaletteFormat = PaletteFormat.RGB5A3,
                            CopyPaletteSize = CopyPaletteSize.ToHeight
                        }),
                        new Example("Encode test.png into test.TXTR with mipmap generation, the texture format CMPR, and a mipmap size limit of 400x400",
                        new UnParserSettings() { SkipDefault = true },
                        new EncodeOptions {
                            Input = @"test.png",
                            Output = @"test.TXTR",
                            TextureFormat = TextureFormat.CMPR,
                            GenerateMipmaps = true,
                            MipmapWidthLimit = 400,
                            MipmapHeightLimit = 400
                        })
                    };
            }
        }

        public override string ToString()
            => $"{nameof(DecodeOptions)}{{{nameof(Input)}={Input},{nameof(Output)}={Output}," +
            $"{nameof(DontAsk)}={DontAsk},{nameof(ForceOverwrite)}={ForceOverwrite}," +
            $"{nameof(TextureFormat)}={TextureFormat},{nameof(PaletteFormat)}={PaletteFormat}," +
            $"{nameof(CopyPaletteSize)}={CopyPaletteSize},{nameof(GenerateMipmaps)}={GenerateMipmaps}," +
            $"{nameof(MipmapWidthLimit)}={MipmapWidthLimit},{nameof(MipmapHeightLimit)}={MipmapHeightLimit}," +
            $"{nameof(Verbose)}={Verbose},{nameof(NoWarn)}={NoWarn},{nameof(Quiet)}={Quiet}," +
            $"{nameof(CliColors)}={CliColors}}}#{GetHashCode()}";

        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(Input);
            hash.Add(Output);
            hash.Add(DontAsk);
            hash.Add(ForceOverwrite);
            hash.Add(TextureFormat);
            hash.Add(PaletteFormat);
            hash.Add(CopyPaletteSize);
            hash.Add(GenerateMipmaps);
            hash.Add(MipmapWidthLimit);
            hash.Add(MipmapHeightLimit);
            hash.Add(Verbose);
            hash.Add(NoWarn);
            hash.Add(Quiet);
            hash.Add(CliColors);
            return hash.ToHashCode();
        }
    }
}
