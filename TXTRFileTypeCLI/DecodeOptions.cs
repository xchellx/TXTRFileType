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

namespace TXTRFileTypeCLI
{
    [Verb("decode", isDefault: true, HelpText = "Decode a TXTR file to a directory.")]
    internal class DecodeOptions
    {
        [Value(0,
            Required = true,
            HelpText = "The TXTR file to decode.",
            MetaName = nameof(Input))]
        public string Input { get; set; } = string.Empty;

        [Option('o', "output",
            Default = "out",
            HelpText = "The output folder where all the decoded mipmaps in the TXTR will be saved to.")]
        public string Output { get; set; } = "out";

        [Option('y', "yes",
            Default = false,
            HelpText = "Do not ask if want to create the output folder.")]
        public bool DontAsk { get; set; } = false;

        [Option('f', "force",
            Default = false,
            HelpText = "Do not ask if want to overwrite files.")]
        public bool ForceOverwrite { get; set; } = false;

        [Option('m', "mipmaps",
            Default = false,
            HelpText = "Decode all mipmaps from the TXTR.")]
        public bool DecodeMipmaps { get; set; } = false;

        [Option('p', "prefix",
            Default = "txtr_",
            HelpText = "The prefix for each output file name.")]
        public string OutputPrefix { get; set; } = "txtr_";

        [Option('s', "suffix",
            Default = "_mipmap",
            HelpText = "The suffix for each output file name.")]
        public string OutputSuffix { get; set; } = "_mipmap";

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
                        new Example("Decode only the first mipmap from 46434ed3.TXTR to the folder 46434ed3-TXTR",
                        new UnParserSettings() { SkipDefault = true },
                        new DecodeOptions {
                            Input = @"46434ed3.TXTR",
                            Output = @"46434ed3-TXTR"
                        }),
                        new Example("Decode all mipmaps from 46434ed3.TXTR to the folder 46434ed3-TXTR",
                        new UnParserSettings() { SkipDefault = true },
                        new DecodeOptions {
                            Input = @"46434ed3.TXTR",
                            Output = @"46434ed3-TXTR",
                            DecodeMipmaps = true
                        }),
                        new Example("Decode all mipmaps from 46434ed3.TXTR to the folder 46434ed3-TXTR, each file having the prefix t_ and the suffix _m",
                        new UnParserSettings() { SkipDefault = true },
                        new DecodeOptions {
                            Input = @"46434ed3.TXTR",
                            Output = @"46434ed3-TXTR",
                            DecodeMipmaps = true,
                            OutputPrefix = "t_",
                            OutputSuffix = "_m"
                        })
                    };
            }
        }

        public override string ToString()
            => $"{nameof(DecodeOptions)}{{{nameof(Input)}={Input},{nameof(Output)}={Output}," +
            $"{nameof(DontAsk)}={DontAsk},{nameof(ForceOverwrite)}={ForceOverwrite}," +
            $"{nameof(DecodeMipmaps)}={DecodeMipmaps},{nameof(OutputPrefix)}={OutputPrefix}," +
            $"{nameof(OutputSuffix)}={OutputSuffix},{nameof(Verbose)}={Verbose}," +
            $"{nameof(NoWarn)}={NoWarn},{nameof(Quiet)}={Quiet}," +
            $"{nameof(CliColors)}={CliColors}}}#{GetHashCode()}";

        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(Input);
            hash.Add(Output);
            hash.Add(DontAsk);
            hash.Add(ForceOverwrite);
            hash.Add(DecodeMipmaps);
            hash.Add(OutputPrefix);
            hash.Add(OutputSuffix);
            hash.Add(Verbose);
            hash.Add(NoWarn);
            hash.Add(Quiet);
            hash.Add(CliColors);
            return hash.ToHashCode();
        }
    }
}
