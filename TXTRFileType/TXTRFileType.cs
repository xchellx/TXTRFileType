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

using libWiiSharp.Formats;
using PaintDotNet;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TXTRFileType.Util;
using TXTRFileTypeLib;

namespace TXTRFileType
{
    [PluginSupportInfo(typeof(TXTRPluginSupportInfo))]
    public class TXTRFileType : FileType<TXTRSaveConfigToken, TXTRSaveConfigWidget>
    {
        public TXTRFileType()
            : base(
                "Metroid Prime Texture",
                new FileTypeOptions
                {
                    LoadExtensions = new string[] { ".TXTR" },
                    SaveExtensions = new string[] { ".TXTR" },
                    SupportsCancellation = true,
                    // This MUST be false despite the bug of layer flattening not respecting actual saved
                    // layered mipmap data.
                    // A reload of the file will fix this issue.
                    SupportsLayers = false
                })
        {
        }

        protected override TXTRSaveConfigToken OnCreateDefaultSaveConfigTokenT()
            => TXTRSaveConfigToken.GetDefault();

        protected override TXTRSaveConfigWidget OnCreateSaveConfigWidgetT()
#if DESIGNER
            => new();
#else
            => new(this);
#endif

        protected override void OnSaveT(Document input, Stream output, TXTRSaveConfigToken token,
            Surface scratchSurface, ProgressEventHandler progressCallback)
        {
            void UpdateProgress(double progress, double max)
                => progressCallback?.Invoke(null, new ProgressEventArgs(Math.Floor((progress / max) * 100.0d), false));
            Surface inputImage = ((BitmapLayer)input.Layers[0]).Surface;

            using (Image<Bgra32> outputImage = new(inputImage.Width, inputImage.Height))
            {
                for (int y = 0; y < outputImage.Height; y++)
                {
                    Span<Bgra32> outputImageRow = outputImage.GetPixelRowSpan(y);
                    for (int x = 0; x < outputImageRow.Length; x++)
                    {
                        ref Bgra32 outputImagePixel = ref outputImageRow[x];
                        outputImagePixel.R = inputImage[x, y].R;
                        outputImagePixel.G = inputImage[x, y].G;
                        outputImagePixel.B = inputImage[x, y].B;
                        outputImagePixel.A = inputImage[x, y].A;
                    }
                }

                TXTRFileTypeLibAPI.Write(outputImage,
                    output,
                    token.TextureFormat,
                    token.PaletteFormat,
                    token.CopyPaletteSize,
                    token.GenerateMipmaps
                    && token.TextureFormat != TextureFormat.CI4
                    && token.TextureFormat != TextureFormat.CI8
                    && token.TextureFormat != TextureFormat.CI14X2,
                    token.MipmapWidthLimit,
                    token.MipmapHeightLimit,
                    true,
                    UpdateProgress);
            }
        }

        protected override Document OnLoad(Stream input)
        {
            bool readMipmaps = false;
            // Checking the current StackTrace frame(s) is the only way to check if
            // a load is a preview or an actual load. Limit the check to 4 frames up.
            for (int i = 0; i < 4; i++)
            {
                string callingClassName = new StackTrace().GetFrame(i + 1)?.GetMethod()?.ReflectedType
                    ?.FullName?.ToLower() ?? string.Empty;

                if (callingClassName.Contains("paintdotnet")
                    && (callingClassName.Contains("save") || callingClassName.Contains("preview")))
                {
                    readMipmaps = true;
                    break;
                }
            }
            if (!readMipmaps)
            {
                readMipmaps = MessageBox.Show(new Win32Window(Process.GetCurrentProcess()),
                    "Read all mipmaps from the TXTR?", "TXTR Read Question", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
            }
            Image<Bgra32>[] inputImages = TXTRFileTypeLibAPI.Read(input, readMipmaps, true);

            if (inputImages.Length > 0)
            {
                Document output = new(inputImages[0].Width, inputImages[0].Height);

                int i = 0;
                foreach (Image<Bgra32> inputImage in inputImages)
                {
                    // Would be mipmap.Width and mipmap.Height but Paint.NET doesn't allow layers
                    // smaller than the image size. However, this doesn't really matter in the end.
                    BitmapLayer outputImage = new(output.Width, output.Height, ColorBgra.Transparent)
                    {
                        Name = $"Mipmap {++i}"
                    };

                    using (inputImage)
                    {
                        for (int y = 0; y < inputImage.Height; y++)
                        {
                            Span<Bgra32> inputImageRow = inputImage.GetPixelRowSpan(y);
                            for (int x = 0; x < inputImageRow.Length; x++)
                            {
                                ref Bgra32 inputImagePixel = ref inputImageRow[x];

                                ColorBgra outputImagePixel = outputImage.Surface[x, y];
                                outputImagePixel.R = inputImagePixel.R;
                                outputImagePixel.G = inputImagePixel.G;
                                outputImagePixel.B = inputImagePixel.B;
                                outputImagePixel.A = inputImagePixel.A;
                                outputImage.Surface[x, y] = outputImagePixel;
                            }
                        }
                    }

                    output.Layers.Add(outputImage);
                }

                return output;
            }
            else
                throw new InvalidOperationException("No image data was loaded");
        }
    }
}
