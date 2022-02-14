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

// The DESIGNER macro is used so the Designer can work. For some reason,
// Microsoft made it so the Designer does not work with abstract types.
// This awful hack is the only way around this: using a concrete type
// without implementation details to prevent complex extra logic for
// this hack. The other ways to get abstract types to work with Designer
// (usually solutions from 2008 and earlier) do not work anymore, so this
// will have to do.

using libWiiSharp;
using libWiiSharp.Formats;
using PaintDotNet;
using System;
using System.Drawing;
using System.Windows.Forms;
using TXTRFileType.Extensions;
using TXTRFileType.Models;

namespace TXTRFileType
{
    //== START DESIGNER STUFF ==//
    public partial class TXTRSaveConfigWidget
#if DESIGNER
        : SaveConfigWidget
#else
        : SaveConfigWidget<TXTRFileType, TXTRSaveConfigToken>
#endif
    {
        private GroupBox formatGroupBox;
        private Label textureFormatLabel;
        private ComboBox textureFormatComboBox;
        private Label paletteFormatLabel;
        private ComboBox paletteFormatComboBox;
        private GroupBox mipmapsGroupBox;
        private CheckBox generateMipmapsCheckBox;
        private Label mipmapWidthLimitLabel;
        private NumericUpDown mipmapWidthLimitNumericUpDown;
        private Label mipmapHeightLimitLabel;
        private NumericUpDown mipmapHeightLimitNumericUpDown;
        private GroupBox miscellaneousGroupBox;
        private Label copyPaletteSizeLabel;
        private ComboBox copyPaletteSizeComboBox;

        private void InitializeComponent()
        {
            this.textureFormatLabel = new System.Windows.Forms.Label();
            this.textureFormatComboBox = new System.Windows.Forms.ComboBox();
            this.paletteFormatLabel = new System.Windows.Forms.Label();
            this.paletteFormatComboBox = new System.Windows.Forms.ComboBox();
            this.copyPaletteSizeLabel = new System.Windows.Forms.Label();
            this.copyPaletteSizeComboBox = new System.Windows.Forms.ComboBox();
            this.generateMipmapsCheckBox = new System.Windows.Forms.CheckBox();
            this.mipmapWidthLimitLabel = new System.Windows.Forms.Label();
            this.mipmapWidthLimitNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.mipmapsGroupBox = new System.Windows.Forms.GroupBox();
            this.miscellaneousGroupBox = new System.Windows.Forms.GroupBox();
            this.formatGroupBox = new System.Windows.Forms.GroupBox();
            this.mipmapHeightLimitNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.mipmapHeightLimitLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mipmapWidthLimitNumericUpDown)).BeginInit();
            this.mipmapsGroupBox.SuspendLayout();
            this.miscellaneousGroupBox.SuspendLayout();
            this.formatGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mipmapHeightLimitNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // textureFormatLabel
            // 
            this.textureFormatLabel.AutoSize = true;
            this.textureFormatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textureFormatLabel.Location = new System.Drawing.Point(8, 28);
            this.textureFormatLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.textureFormatLabel.Name = "textureFormatLabel";
            this.textureFormatLabel.Size = new System.Drawing.Size(113, 18);
            this.textureFormatLabel.TabIndex = 0;
            this.textureFormatLabel.Text = "Texture Format:";
            // 
            // textureFormatComboBox
            // 
            this.textureFormatComboBox.BackColor = System.Drawing.SystemColors.Control;
            this.textureFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.textureFormatComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.textureFormatComboBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textureFormatComboBox.FormattingEnabled = true;
            this.textureFormatComboBox.Location = new System.Drawing.Point(8, 54);
            this.textureFormatComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.textureFormatComboBox.MinimumSize = new System.Drawing.Size(184, 0);
            this.textureFormatComboBox.Name = "textureFormatComboBox";
            this.textureFormatComboBox.Size = new System.Drawing.Size(184, 28);
            this.textureFormatComboBox.TabIndex = 1;
            // 
            // paletteFormatLabel
            // 
            this.paletteFormatLabel.AutoSize = true;
            this.paletteFormatLabel.Enabled = false;
            this.paletteFormatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.paletteFormatLabel.Location = new System.Drawing.Point(8, 98);
            this.paletteFormatLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.paletteFormatLabel.Name = "paletteFormatLabel";
            this.paletteFormatLabel.Size = new System.Drawing.Size(109, 18);
            this.paletteFormatLabel.TabIndex = 0;
            this.paletteFormatLabel.Text = "Palette Format:";
            // 
            // paletteFormatComboBox
            // 
            this.paletteFormatComboBox.BackColor = System.Drawing.SystemColors.Control;
            this.paletteFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paletteFormatComboBox.Enabled = false;
            this.paletteFormatComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.paletteFormatComboBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.paletteFormatComboBox.FormattingEnabled = true;
            this.paletteFormatComboBox.Location = new System.Drawing.Point(8, 124);
            this.paletteFormatComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.paletteFormatComboBox.MinimumSize = new System.Drawing.Size(184, 0);
            this.paletteFormatComboBox.Name = "paletteFormatComboBox";
            this.paletteFormatComboBox.Size = new System.Drawing.Size(184, 28);
            this.paletteFormatComboBox.TabIndex = 2;
            // 
            // copyPaletteSizeLabel
            // 
            this.copyPaletteSizeLabel.AutoSize = true;
            this.copyPaletteSizeLabel.Enabled = false;
            this.copyPaletteSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.copyPaletteSizeLabel.Location = new System.Drawing.Point(8, 28);
            this.copyPaletteSizeLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.copyPaletteSizeLabel.Name = "copyPaletteSizeLabel";
            this.copyPaletteSizeLabel.Size = new System.Drawing.Size(129, 18);
            this.copyPaletteSizeLabel.TabIndex = 0;
            this.copyPaletteSizeLabel.Text = "Copy Palette Size:";
            // 
            // copyPaletteSizeComboBox
            // 
            this.copyPaletteSizeComboBox.BackColor = System.Drawing.SystemColors.Control;
            this.copyPaletteSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.copyPaletteSizeComboBox.Enabled = false;
            this.copyPaletteSizeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.copyPaletteSizeComboBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.copyPaletteSizeComboBox.FormattingEnabled = true;
            this.copyPaletteSizeComboBox.Location = new System.Drawing.Point(8, 54);
            this.copyPaletteSizeComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.copyPaletteSizeComboBox.MinimumSize = new System.Drawing.Size(184, 0);
            this.copyPaletteSizeComboBox.Name = "copyPaletteSizeComboBox";
            this.copyPaletteSizeComboBox.Size = new System.Drawing.Size(184, 28);
            this.copyPaletteSizeComboBox.TabIndex = 6;
            // 
            // generateMipmapsCheckBox
            // 
            this.generateMipmapsCheckBox.AutoSize = true;
            this.generateMipmapsCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.generateMipmapsCheckBox.Location = new System.Drawing.Point(8, 28);
            this.generateMipmapsCheckBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.generateMipmapsCheckBox.Name = "generateMipmapsCheckBox";
            this.generateMipmapsCheckBox.Size = new System.Drawing.Size(156, 22);
            this.generateMipmapsCheckBox.TabIndex = 3;
            this.generateMipmapsCheckBox.Text = "Generate Mipmaps";
            this.generateMipmapsCheckBox.UseVisualStyleBackColor = true;
            // 
            // mipmapWidthLimitLabel
            // 
            this.mipmapWidthLimitLabel.AutoSize = true;
            this.mipmapWidthLimitLabel.Enabled = false;
            this.mipmapWidthLimitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.mipmapWidthLimitLabel.Location = new System.Drawing.Point(8, 58);
            this.mipmapWidthLimitLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.mipmapWidthLimitLabel.Name = "mipmapWidthLimitLabel";
            this.mipmapWidthLimitLabel.Size = new System.Drawing.Size(142, 18);
            this.mipmapWidthLimitLabel.TabIndex = 0;
            this.mipmapWidthLimitLabel.Text = "Mipmap Width Limit:";
            // 
            // mipmapWidthLimitNumericUpDown
            // 
            this.mipmapWidthLimitNumericUpDown.Enabled = false;
            this.mipmapWidthLimitNumericUpDown.Location = new System.Drawing.Point(8, 84);
            this.mipmapWidthLimitNumericUpDown.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.mipmapWidthLimitNumericUpDown.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.mipmapWidthLimitNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mipmapWidthLimitNumericUpDown.MinimumSize = new System.Drawing.Size(184, 0);
            this.mipmapWidthLimitNumericUpDown.Name = "mipmapWidthLimitNumericUpDown";
            this.mipmapWidthLimitNumericUpDown.Size = new System.Drawing.Size(184, 27);
            this.mipmapWidthLimitNumericUpDown.TabIndex = 4;
            this.mipmapWidthLimitNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // mipmapsGroupBox
            // 
            this.mipmapsGroupBox.Controls.Add(this.mipmapHeightLimitNumericUpDown);
            this.mipmapsGroupBox.Controls.Add(this.mipmapHeightLimitLabel);
            this.mipmapsGroupBox.Controls.Add(this.generateMipmapsCheckBox);
            this.mipmapsGroupBox.Controls.Add(this.mipmapWidthLimitNumericUpDown);
            this.mipmapsGroupBox.Controls.Add(this.mipmapWidthLimitLabel);
            this.mipmapsGroupBox.Location = new System.Drawing.Point(0, 179);
            this.mipmapsGroupBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.mipmapsGroupBox.MinimumSize = new System.Drawing.Size(200, 0);
            this.mipmapsGroupBox.Name = "mipmapsGroupBox";
            this.mipmapsGroupBox.Padding = new System.Windows.Forms.Padding(8);
            this.mipmapsGroupBox.Size = new System.Drawing.Size(200, 191);
            this.mipmapsGroupBox.TabIndex = 0;
            this.mipmapsGroupBox.TabStop = false;
            this.mipmapsGroupBox.Text = "Mipmaps";
            // 
            // miscellaneousGroupBox
            // 
            this.miscellaneousGroupBox.Controls.Add(this.copyPaletteSizeComboBox);
            this.miscellaneousGroupBox.Controls.Add(this.copyPaletteSizeLabel);
            this.miscellaneousGroupBox.Location = new System.Drawing.Point(0, 386);
            this.miscellaneousGroupBox.Margin = new System.Windows.Forms.Padding(0);
            this.miscellaneousGroupBox.MinimumSize = new System.Drawing.Size(200, 0);
            this.miscellaneousGroupBox.Name = "miscellaneousGroupBox";
            this.miscellaneousGroupBox.Padding = new System.Windows.Forms.Padding(8);
            this.miscellaneousGroupBox.Size = new System.Drawing.Size(200, 93);
            this.miscellaneousGroupBox.TabIndex = 0;
            this.miscellaneousGroupBox.TabStop = false;
            this.miscellaneousGroupBox.Text = "Miscellaneous";
            // 
            // formatGroupBox
            // 
            this.formatGroupBox.Controls.Add(this.textureFormatLabel);
            this.formatGroupBox.Controls.Add(this.textureFormatComboBox);
            this.formatGroupBox.Controls.Add(this.paletteFormatLabel);
            this.formatGroupBox.Controls.Add(this.paletteFormatComboBox);
            this.formatGroupBox.Location = new System.Drawing.Point(0, 0);
            this.formatGroupBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.formatGroupBox.MinimumSize = new System.Drawing.Size(200, 0);
            this.formatGroupBox.Name = "formatGroupBox";
            this.formatGroupBox.Padding = new System.Windows.Forms.Padding(8);
            this.formatGroupBox.Size = new System.Drawing.Size(200, 163);
            this.formatGroupBox.TabIndex = 0;
            this.formatGroupBox.TabStop = false;
            this.formatGroupBox.Text = "Format";
            // 
            // mipmapHeightLimitNumericUpDown
            // 
            this.mipmapHeightLimitNumericUpDown.Enabled = false;
            this.mipmapHeightLimitNumericUpDown.Location = new System.Drawing.Point(8, 153);
            this.mipmapHeightLimitNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.mipmapHeightLimitNumericUpDown.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.mipmapHeightLimitNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mipmapHeightLimitNumericUpDown.MinimumSize = new System.Drawing.Size(184, 0);
            this.mipmapHeightLimitNumericUpDown.Name = "mipmapHeightLimitNumericUpDown";
            this.mipmapHeightLimitNumericUpDown.Size = new System.Drawing.Size(184, 27);
            this.mipmapHeightLimitNumericUpDown.TabIndex = 5;
            this.mipmapHeightLimitNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // mipmapHeightLimitLabel
            // 
            this.mipmapHeightLimitLabel.AutoSize = true;
            this.mipmapHeightLimitLabel.Enabled = false;
            this.mipmapHeightLimitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.mipmapHeightLimitLabel.Location = new System.Drawing.Point(8, 127);
            this.mipmapHeightLimitLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.mipmapHeightLimitLabel.Name = "mipmapHeightLimitLabel";
            this.mipmapHeightLimitLabel.Size = new System.Drawing.Size(146, 18);
            this.mipmapHeightLimitLabel.TabIndex = 4;
            this.mipmapHeightLimitLabel.Text = "Mipmap Height Limit:";
            // 
            // TXTRSaveConfigWidget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.miscellaneousGroupBox);
            this.Controls.Add(this.mipmapsGroupBox);
            this.Controls.Add(this.formatGroupBox);
            this.MinimumSize = new System.Drawing.Size(200, 0);
            this.Name = "TXTRSaveConfigWidget";
            this.Size = new System.Drawing.Size(200, 479);
            ((System.ComponentModel.ISupportInitialize)(this.mipmapWidthLimitNumericUpDown)).EndInit();
            this.mipmapsGroupBox.ResumeLayout(false);
            this.mipmapsGroupBox.PerformLayout();
            this.miscellaneousGroupBox.ResumeLayout(false);
            this.miscellaneousGroupBox.PerformLayout();
            this.formatGroupBox.ResumeLayout(false);
            this.formatGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mipmapHeightLimitNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        //== END DESIGNER STUFF ==//
        //== START PAINT.NET STUFF ==//

        private bool isDisposed;

#if DESIGNER
#pragma warning disable CS8618 // Field initialization is taken care of in InitializeComponent
        public TXTRSaveConfigWidget()
        {
            isDisposed = false;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
        }

        protected override void InitTokenFromWidget()
        {
        }

        protected override void InitWidgetFromToken(SaveConfigToken sourceToken)
        {
        }
#pragma warning restore CS8618 // Field initialization is taken care of in InitializeComponent
#else
#pragma warning disable CS8618 // Field initialization is taken care of in InitializeComponent
        public TXTRSaveConfigWidget(FileType? fileType) : base(fileType)
        {
            isDisposed = false;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
        }
#pragma warning restore CS8618 // Field initialization is taken care of in InitializeComponent

        protected override TXTRSaveConfigToken CreateTokenFromWidget()
        {
            TXTRSaveConfigToken defaultToken = TXTRSaveConfigToken.GetDefault();
            return new TXTRSaveConfigToken()
            {
                TextureFormat = (textureFormatComboBox?.SelectedItem as ComboBoxEnum<TextureFormat>)?.Value ?? defaultToken.TextureFormat,
                PaletteFormat = (paletteFormatComboBox?.SelectedItem as ComboBoxEnum<PaletteFormat>)?.Value ?? defaultToken.PaletteFormat,
                CopyPaletteSize = (copyPaletteSizeComboBox?.SelectedItem as ComboBoxEnum<CopyPaletteSize>)?.Value ?? defaultToken.CopyPaletteSize,
                GenerateMipmaps = generateMipmapsCheckBox?.Checked ?? defaultToken.GenerateMipmaps,
                MipmapWidthLimit = mipmapWidthLimitNumericUpDown != null
                    ? (int)Math.Clamp(mipmapWidthLimitNumericUpDown.Value, int.MinValue, int.MaxValue)
                    : defaultToken.MipmapWidthLimit,
                MipmapHeightLimit = mipmapHeightLimitNumericUpDown != null
                    ? (int)Math.Clamp(mipmapHeightLimitNumericUpDown.Value, int.MinValue, int.MaxValue)
                    : defaultToken.MipmapHeightLimit
            };
        }

        protected override void InitWidgetFromToken(TXTRSaveConfigToken sourceToken)
        {
            TXTRSaveConfigToken token = sourceToken ?? TXTRSaveConfigToken.GetDefault();
            textureFormatComboBox.SelectedItem = new ComboBoxEnum<TextureFormat>(token.TextureFormat);
            paletteFormatComboBox.SelectedItem = new ComboBoxEnum<PaletteFormat>(token.PaletteFormat);
            copyPaletteSizeComboBox.SelectedItem = new ComboBoxEnum<CopyPaletteSize>(token.CopyPaletteSize);
            generateMipmapsCheckBox.Checked = token.GenerateMipmaps;
            mipmapWidthLimitNumericUpDown.Value = token.MipmapWidthLimit;
            mipmapHeightLimitNumericUpDown.Value = token.MipmapHeightLimit;
        }
#endif

        public void NotifyTokenChanged(object? sender, EventArgs e) => UpdateToken();

        public void OnTokenChanged(object? sender, EventArgs e)
        {
            if (textureFormatComboBox.SelectedIndex < 0)
                textureFormatComboBox.SelectedIndex = 0;
            if (paletteFormatComboBox.SelectedIndex < 0)
                paletteFormatComboBox.SelectedIndex = 0;
            if (copyPaletteSizeComboBox.SelectedIndex < 0)
                copyPaletteSizeComboBox.SelectedIndex = 0;

            switch (((ComboBoxEnum<TextureFormat>)textureFormatComboBox.SelectedItem).Value)
            {
                case TextureFormat.CI4:
                case TextureFormat.CI8:
                case TextureFormat.CI14X2:
                    paletteFormatLabel.Enabled = true;
                    paletteFormatComboBox.Enabled = true;
                    generateMipmapsCheckBox.Enabled = false;
                    copyPaletteSizeLabel.Enabled = true;
                    copyPaletteSizeComboBox.Enabled = true;
                    break;
                default:
                    paletteFormatLabel.Enabled = false;
                    paletteFormatComboBox.Enabled = false;
                    generateMipmapsCheckBox.Enabled = true;
                    copyPaletteSizeLabel.Enabled = false;
                    copyPaletteSizeComboBox.Enabled = false;
                    break;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Bind combobox items to enum values (with titles)
            textureFormatComboBox.BindEnumToCombobox(TextureFormat.I4);
            paletteFormatComboBox.BindEnumToCombobox(PaletteFormat.IA8);
            copyPaletteSizeComboBox.BindEnumToCombobox(CopyPaletteSize.ToWidth);

            // Ensure comboboxs have their dropdown width wrap to the max content width
            textureFormatComboBox.DropDownWidth = textureFormatComboBox.GetMaxDropDownWidth();
            paletteFormatComboBox.DropDownWidth = textureFormatComboBox.GetMaxDropDownWidth();
            copyPaletteSizeComboBox.DropDownWidth = textureFormatComboBox.GetMaxDropDownWidth();

            // Cap max mipmap size limit
            mipmapWidthLimitNumericUpDown.Maximum = int.MaxValue;
            mipmapHeightLimitNumericUpDown.Maximum = int.MaxValue;

            // Disable TabStop on specific controls
            formatGroupBox.TabStop = false;
            textureFormatLabel.TabStop = false;
            paletteFormatLabel.TabStop = false;
            mipmapsGroupBox.TabStop = false;
            mipmapWidthLimitLabel.TabStop = false;
            mipmapHeightLimitLabel.TabStop = false;
            miscellaneousGroupBox.TabStop = false;
            copyPaletteSizeLabel.TabStop = false;

            // Attach events
            Parent.SizeChanged += SyncWidth;

            TokenChanged += OnTokenChanged;
            textureFormatComboBox.SelectionChangeCommitted += NotifyTokenChanged;
            paletteFormatComboBox.SelectionChangeCommitted += NotifyTokenChanged;
            generateMipmapsCheckBox.CheckedChanged += NotifyTokenChanged;
            mipmapWidthLimitNumericUpDown.ValueChanged += NotifyTokenChanged;
            mipmapHeightLimitNumericUpDown.ValueChanged += NotifyTokenChanged;
            copyPaletteSizeComboBox.SelectionChangeCommitted += NotifyTokenChanged;

            ForeColorChanged += SyncColors;
            BackColorChanged += SyncColors;
            formatGroupBox.EnabledChanged += SyncColors;
            textureFormatLabel.EnabledChanged += SyncColors;
            textureFormatComboBox.EnabledChanged += SyncColors;
            paletteFormatLabel.EnabledChanged += SyncColors;
            paletteFormatComboBox.EnabledChanged += SyncColors;
            mipmapsGroupBox.EnabledChanged += SyncColors;
            generateMipmapsCheckBox.EnabledChanged += SyncColors;
            mipmapWidthLimitLabel.EnabledChanged += SyncColors;
            mipmapWidthLimitNumericUpDown.EnabledChanged += SyncColors;
            mipmapHeightLimitLabel.EnabledChanged += SyncColors;
            mipmapHeightLimitNumericUpDown.EnabledChanged += SyncColors;
            miscellaneousGroupBox.EnabledChanged += SyncColors;
            copyPaletteSizeLabel.EnabledChanged += SyncColors;
            copyPaletteSizeComboBox.EnabledChanged += SyncColors;

            generateMipmapsCheckBox.EnabledChanged += Mipmaps_SyncEnabled;
            generateMipmapsCheckBox.CheckedChanged += Mipmaps_SyncEnabled;

            // Fire some events initially
            SyncWidth(this, new EventArgs());
            SyncColors(this, new EventArgs());
            Mipmaps_SyncEnabled(this, new EventArgs());
        }

        private void SyncColors(object? sender, EventArgs e)
        {
            // Controls follow ForeColor and BackColor for painting background and text. However, WinForms SystemColors
            // does not change with the Dark Mode setting in Windows 10. Thus, the ForeColor and BackColor must be set
            // to the ForeColor and BackColor of the equivalent Paint.NET custom WinForms control that has its colors
            // synced with Dark Mode. Luckily, we wont have to hack away at Paint.NET as SaveConfigWidget is a custom
            // control synced with Dark Mode, so we just have to align the colors from this control itself.
            // 
            // ForeColor
            formatGroupBox.ForeColor = ForeColor;
            textureFormatLabel.ForeColor = ForeColor;
            textureFormatComboBox.ForeColor = ForeColor;
            paletteFormatLabel.ForeColor = ForeColor;
            paletteFormatComboBox.ForeColor = ForeColor;
            mipmapsGroupBox.ForeColor = ForeColor;
            generateMipmapsCheckBox.ForeColor = ForeColor;
            mipmapWidthLimitLabel.ForeColor = ForeColor;
            mipmapWidthLimitNumericUpDown.ForeColor = ForeColor;
            mipmapHeightLimitLabel.ForeColor = ForeColor;
            mipmapHeightLimitNumericUpDown.ForeColor = ForeColor;
            miscellaneousGroupBox.ForeColor = ForeColor;
            copyPaletteSizeLabel.ForeColor = ForeColor;
            copyPaletteSizeComboBox.ForeColor = ForeColor;

            // Label, CheckBox, and Button use Control.DisabledColor for it's text when its disabled instead of
            // ForeColor. Control.DisabledColor is calculated by getting the inverted difference from the BackColor
            // and it's alpha value. Since there is no way to set Control.DisabledColor (it has a getter only), we
            // instead trick it by enforcing that a transparent background is supported, then setting the BackColor
            // to the opposite ForeColor with it's alpha set to 1 where it is fully transparent but just enough to
            // make it take into account the BackColor to get the correct Control.DisabledColor.
            // See:
            // https://referencesource.microsoft.com/#system.windows.forms/winforms/Managed/System/WinForms/Label.cs,1527
            // https://referencesource.microsoft.com/#system.windows.forms/winforms/Managed/System/WinForms/Control.cs,2251
            // 
            // BackColor
            Color disabledColorHack = Color.FromArgb(1, ForeColor.R, ForeColor.G, ForeColor.B);
            formatGroupBox.BackColor = disabledColorHack;
            textureFormatLabel.BackColor = disabledColorHack;
            textureFormatComboBox.BackColor = BackColor;
            paletteFormatLabel.BackColor = disabledColorHack;
            paletteFormatComboBox.BackColor = BackColor;
            mipmapsGroupBox.BackColor = disabledColorHack;
            generateMipmapsCheckBox.BackColor = disabledColorHack;
            mipmapWidthLimitLabel.BackColor = disabledColorHack;
            mipmapWidthLimitNumericUpDown.BackColor = BackColor;
            mipmapHeightLimitLabel.BackColor = disabledColorHack;
            mipmapHeightLimitNumericUpDown.BackColor = BackColor;
            miscellaneousGroupBox.BackColor = disabledColorHack;
            copyPaletteSizeLabel.BackColor = disabledColorHack;
            copyPaletteSizeComboBox.BackColor = BackColor;

            // Force the controls to update their visual appearance
            formatGroupBox.Update();
            textureFormatLabel.Update();
            textureFormatComboBox.Update();
            paletteFormatLabel.Update();
            paletteFormatComboBox.Update();
            mipmapsGroupBox.Update();
            generateMipmapsCheckBox.Update();
            mipmapWidthLimitLabel.Update();
            mipmapWidthLimitNumericUpDown.Update();
            mipmapHeightLimitLabel.Update();
            mipmapHeightLimitNumericUpDown.Update();
            miscellaneousGroupBox.Update();
            copyPaletteSizeLabel.Update();
            copyPaletteSizeComboBox.Update();
        }

        private void SyncWidth(object? sender, EventArgs e)
        {
            // Paint.NET only gives us a region of size 200x200 which then has its width scaled by the DPI X scaling factor
            // and height auto-sized. Therefore, the available width might not always be 200. Instead, listen for the parent
            // SaveConfigPanel size to change and adapt to that new DPI-scaled size. This doesn't matter in the grand scheme
            // of things as Paint.NET's SaveConfigPanel has auto-scrollbars enabled so even if content goes OOB, it'll still
            // be visible but it looks better visually when done without width scrollbars.
            Width = Parent.Size.Width - SystemInformation.VerticalScrollBarWidth;

            // Sync widths and stretch controls
            formatGroupBox.Width = Width - formatGroupBox.Location.X;
            textureFormatComboBox.Width = formatGroupBox.Width - (textureFormatComboBox.Location.X
                - formatGroupBox.Location.X + formatGroupBox.Padding.Left);
            paletteFormatComboBox.Width = formatGroupBox.Width - (paletteFormatComboBox.Location.X
                - formatGroupBox.Location.X + formatGroupBox.Padding.Left);
            mipmapsGroupBox.Width = Width - mipmapsGroupBox.Location.X;
            mipmapWidthLimitNumericUpDown.Width = mipmapsGroupBox.Width - (mipmapWidthLimitNumericUpDown.Location.X
                - mipmapsGroupBox.Location.X + mipmapsGroupBox.Padding.Left);
            mipmapHeightLimitNumericUpDown.Width = mipmapsGroupBox.Width - (mipmapHeightLimitNumericUpDown.Location.X
                - mipmapsGroupBox.Location.X + mipmapsGroupBox.Padding.Left);
            miscellaneousGroupBox.Width = Width - miscellaneousGroupBox.Location.X;
            copyPaletteSizeComboBox.Width = miscellaneousGroupBox.Width - (copyPaletteSizeComboBox.Location.X
                - miscellaneousGroupBox.Location.X + miscellaneousGroupBox.Padding.Left);
        }

        private void Mipmaps_SyncEnabled(object? sender, EventArgs e)
        {
            // Sync CheckBox related controls enabled state to its own enabled state
            // Although a GroupBox could do this, it would disable the checkbox aswell,
            // making it impossible to check the CheckBox to enable things again.
            bool enable = generateMipmapsCheckBox.Enabled && generateMipmapsCheckBox.Checked;
            mipmapWidthLimitLabel.Enabled = enable;
            mipmapWidthLimitNumericUpDown.Enabled = enable;
            mipmapHeightLimitLabel.Enabled = enable;
            mipmapHeightLimitNumericUpDown.Enabled = enable;
        }

        private void DisposeEvents()
        {
            // Dipose events as events can be considered unmanaged with how they leak memory
            Parent.SizeChanged -= SyncWidth;

            TokenChanged -= OnTokenChanged;
            textureFormatComboBox.SelectionChangeCommitted -= NotifyTokenChanged;
            paletteFormatComboBox.SelectionChangeCommitted -= NotifyTokenChanged;
            generateMipmapsCheckBox.CheckedChanged -= NotifyTokenChanged;
            mipmapWidthLimitNumericUpDown.ValueChanged -= NotifyTokenChanged;
            mipmapHeightLimitNumericUpDown.ValueChanged -= NotifyTokenChanged;
            copyPaletteSizeComboBox.SelectionChangeCommitted -= NotifyTokenChanged;

            ForeColorChanged -= SyncColors;
            BackColorChanged -= SyncColors;
            formatGroupBox.EnabledChanged -= SyncColors;
            textureFormatLabel.EnabledChanged -= SyncColors;
            textureFormatComboBox.EnabledChanged -= SyncColors;
            paletteFormatLabel.EnabledChanged -= SyncColors;
            paletteFormatComboBox.EnabledChanged -= SyncColors;
            mipmapsGroupBox.EnabledChanged -= SyncColors;
            generateMipmapsCheckBox.EnabledChanged -= SyncColors;
            mipmapWidthLimitLabel.EnabledChanged -= SyncColors;
            mipmapWidthLimitNumericUpDown.EnabledChanged -= SyncColors;
            mipmapHeightLimitLabel.EnabledChanged -= SyncColors;
            mipmapHeightLimitNumericUpDown.EnabledChanged -= SyncColors;
            miscellaneousGroupBox.EnabledChanged -= SyncColors;
            copyPaletteSizeLabel.EnabledChanged -= SyncColors;
            copyPaletteSizeComboBox.EnabledChanged -= SyncColors;

            generateMipmapsCheckBox.EnabledChanged -= Mipmaps_SyncEnabled;
            generateMipmapsCheckBox.CheckedChanged -= Mipmaps_SyncEnabled;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                isDisposed = true;
                DisposeEvents();
            }
            base.Dispose(disposing);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                DisposeEvents();
            }
            base.OnHandleDestroyed(e);
        }

        //== END PAINT.NET STUFF ==//
    }
}
