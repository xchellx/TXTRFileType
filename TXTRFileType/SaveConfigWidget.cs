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
using libWiiSharp.Formats;
using PaintDotNet;
using System;
using System.Windows.Forms;
using TXTRFileType.Util;

namespace TXTRFileType
{
    //// START DESIGNER STUFF ////
    internal partial class TXTRFileTypeSaveConfigWidget : SaveConfigWidget
    {
        private Label textureFormatLabel;
        private ComboBox textureFormatComboBox;
        private Label paletteFormatLabel;
        private ComboBox paletteFormatComboBox;
        private Label paletteLengthCopyLocationLabel;
        private ComboBox paletteLengthCopyLocationComboBox;
        private CheckBox generateMipmapsCheckBox;

        //// END DESIGNER STUFF ////
        //// START PAINT.NET STUFF ////

        public TXTRFileTypeSaveConfigWidget() {
            InitializeComponent();
            // Bind combobox items to enum values (with titles)
            UIUtil.BindEnumToCombobox<TextureFormat>(textureFormatComboBox, TextureFormat.I4);
            UIUtil.BindEnumToCombobox<PaletteFormat>(paletteFormatComboBox, PaletteFormat.IA8);
            UIUtil.BindEnumToCombobox<TextureConverter.PaletteLengthCopyLocation>(paletteLengthCopyLocationComboBox,
                TextureConverter.PaletteLengthCopyLocation.ToWidth);
        }

        protected override void InitTokenFromWidget()
        {
            TXTRFileTypeSaveConfigToken token = (TXTRFileTypeSaveConfigToken)Token;
            token.TextureFormat = (TextureFormat)((UIUtil.ComboBoxEnumItem<TextureFormat>)textureFormatComboBox.SelectedItem).Value;
            token.TexturePalette = (PaletteFormat)((UIUtil.ComboBoxEnumItem<PaletteFormat>)paletteFormatComboBox.SelectedItem).Value;
            token.PaletteLengthCopyLocation = (TextureConverter.PaletteLengthCopyLocation)
                ((UIUtil.ComboBoxEnumItem<TextureConverter.PaletteLengthCopyLocation>)
                paletteLengthCopyLocationComboBox.SelectedItem).Value;
            token.GenerateMipmaps = generateMipmapsCheckBox.Checked;
        }

        protected override void InitWidgetFromToken(SaveConfigToken sourceToken)
        {
            TXTRFileTypeSaveConfigToken token = (TXTRFileTypeSaveConfigToken)sourceToken;
            textureFormatComboBox.SelectedItem = UIUtil.ComboBoxEnumItem<TextureFormat>.CreateComboBoxEnumItem(token.TextureFormat);
            paletteFormatComboBox.SelectedItem = UIUtil.ComboBoxEnumItem<PaletteFormat>.CreateComboBoxEnumItem(token.TexturePalette);
            paletteLengthCopyLocationComboBox.SelectedItem = UIUtil.ComboBoxEnumItem<TextureConverter.PaletteLengthCopyLocation>
                .CreateComboBoxEnumItem(token.PaletteLengthCopyLocation);
            generateMipmapsCheckBox.Checked = token.GenerateMipmaps;
        }

        private void tokenChanged(object sender, EventArgs e)
        {
            CheckControls();
            UpdateToken();
        }

        private void CheckControls()
        {
            if (textureFormatComboBox.SelectedIndex < 0)
                textureFormatComboBox.SelectedIndex = 0;
            if (paletteFormatComboBox.SelectedIndex < 0)
                paletteFormatComboBox.SelectedIndex = 0;
            if (paletteLengthCopyLocationComboBox.SelectedIndex < 0)
                paletteLengthCopyLocationComboBox.SelectedIndex = 0;
            switch ((TextureFormat)((UIUtil.ComboBoxEnumItem<TextureFormat>)textureFormatComboBox.SelectedItem).Value)
            {
                case TextureFormat.CI4:
                case TextureFormat.CI8:
                case TextureFormat.CI14X2:
                    paletteFormatComboBox.Enabled = true;
                    generateMipmapsCheckBox.Enabled = false;
                    paletteLengthCopyLocationComboBox.Enabled = true;
                    break;
                default:
                    paletteFormatComboBox.Enabled = false;
                    generateMipmapsCheckBox.Enabled = true;
                    paletteLengthCopyLocationComboBox.Enabled = false;
                    break;
            }
        }

        //// END PAINT.NET STUFF ////
        //// START DESIGNER STUFF ////
        
        private void InitializeComponent()
        {
            this.textureFormatLabel = new System.Windows.Forms.Label();
            this.textureFormatComboBox = new System.Windows.Forms.ComboBox();
            this.paletteFormatLabel = new System.Windows.Forms.Label();
            this.paletteFormatComboBox = new System.Windows.Forms.ComboBox();
            this.paletteLengthCopyLocationLabel = new System.Windows.Forms.Label();
            this.paletteLengthCopyLocationComboBox = new System.Windows.Forms.ComboBox();
            this.generateMipmapsCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textureFormatLabel
            // 
            this.textureFormatLabel.AutoSize = true;
            this.textureFormatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textureFormatLabel.Location = new System.Drawing.Point(0, 0);
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
            this.textureFormatComboBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textureFormatComboBox.FormattingEnabled = true;
            this.textureFormatComboBox.Location = new System.Drawing.Point(0, 26);
            this.textureFormatComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.textureFormatComboBox.MinimumSize = new System.Drawing.Size(205, 0);
            this.textureFormatComboBox.Name = "textureFormatComboBox";
            this.textureFormatComboBox.Size = new System.Drawing.Size(205, 28);
            this.textureFormatComboBox.TabIndex = 1;
            this.textureFormatComboBox.SelectionChangeCommitted += new System.EventHandler(this.tokenChanged);
            // 
            // paletteFormatLabel
            // 
            this.paletteFormatLabel.AutoSize = true;
            this.paletteFormatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.paletteFormatLabel.Location = new System.Drawing.Point(0, 70);
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
            this.paletteFormatComboBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.paletteFormatComboBox.FormattingEnabled = true;
            this.paletteFormatComboBox.Location = new System.Drawing.Point(0, 96);
            this.paletteFormatComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.paletteFormatComboBox.MinimumSize = new System.Drawing.Size(205, 0);
            this.paletteFormatComboBox.Name = "paletteFormatComboBox";
            this.paletteFormatComboBox.Size = new System.Drawing.Size(205, 28);
            this.paletteFormatComboBox.TabIndex = 2;
            this.paletteFormatComboBox.SelectionChangeCommitted += new System.EventHandler(this.tokenChanged);
            // 
            // paletteLengthCopyLocationLabel
            // 
            this.paletteLengthCopyLocationLabel.AutoSize = true;
            this.paletteLengthCopyLocationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.paletteLengthCopyLocationLabel.Location = new System.Drawing.Point(0, 140);
            this.paletteLengthCopyLocationLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.paletteLengthCopyLocationLabel.Name = "paletteLengthCopyLocationLabel";
            this.paletteLengthCopyLocationLabel.Size = new System.Drawing.Size(205, 18);
            this.paletteLengthCopyLocationLabel.TabIndex = 0;
            this.paletteLengthCopyLocationLabel.Text = "Palette Length Copy Location:";
            // 
            // paletteLengthCopyLocationComboBox
            // 
            this.paletteLengthCopyLocationComboBox.BackColor = System.Drawing.SystemColors.Control;
            this.paletteLengthCopyLocationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paletteLengthCopyLocationComboBox.Enabled = false;
            this.paletteLengthCopyLocationComboBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.paletteLengthCopyLocationComboBox.FormattingEnabled = true;
            this.paletteLengthCopyLocationComboBox.Location = new System.Drawing.Point(0, 166);
            this.paletteLengthCopyLocationComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.paletteLengthCopyLocationComboBox.MinimumSize = new System.Drawing.Size(205, 0);
            this.paletteLengthCopyLocationComboBox.Name = "paletteLengthCopyLocationComboBox";
            this.paletteLengthCopyLocationComboBox.Size = new System.Drawing.Size(205, 28);
            this.paletteLengthCopyLocationComboBox.TabIndex = 4;
            this.paletteLengthCopyLocationComboBox.SelectionChangeCommitted += new System.EventHandler(this.tokenChanged);
            // 
            // generateMipmapsCheckBox
            // 
            this.generateMipmapsCheckBox.AutoSize = true;
            this.generateMipmapsCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.generateMipmapsCheckBox.Location = new System.Drawing.Point(0, 210);
            this.generateMipmapsCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.generateMipmapsCheckBox.Name = "generateMipmapsCheckBox";
            this.generateMipmapsCheckBox.Size = new System.Drawing.Size(156, 22);
            this.generateMipmapsCheckBox.TabIndex = 3;
            this.generateMipmapsCheckBox.Text = "Generate Mipmaps";
            this.generateMipmapsCheckBox.UseVisualStyleBackColor = true;
            this.generateMipmapsCheckBox.CheckedChanged += new System.EventHandler(this.tokenChanged);
            // 
            // TXTRFileTypeSaveConfigWidget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.textureFormatLabel);
            this.Controls.Add(this.textureFormatComboBox);
            this.Controls.Add(this.paletteFormatLabel);
            this.Controls.Add(this.paletteFormatComboBox);
            this.Controls.Add(this.paletteLengthCopyLocationLabel);
            this.Controls.Add(this.paletteLengthCopyLocationComboBox);
            this.Controls.Add(this.generateMipmapsCheckBox);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "TXTRFileTypeSaveConfigWidget";
            this.Size = new System.Drawing.Size(205, 232);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //// END DESIGNER STUFF ////
        //// START PAINT.NET STUFF ////

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // There is a bug of WinForms ButtonBaseAdapter (for which BaseButton uses which CheckBox inherits) which causes
            // the incorrect text disabled color (black). This is because the paint color used is [Control]LightLight which
            // WinForms doesn't automatically change for the Dark Mode setting in Windows 10. There is no way to fix this as
            // all things containing possible ways to change it are makred as internal or private.
            // This bug is because Microsoft used ButtonBase for CheckBox which had its painting code defined for use as a
            // button which means that it'll inherit the colors a button would use that fits the style of a typical button
            // when CheckBox is not a typical button. Therefore, the text will become black when disabled for a CheckBox.
            // This isn't pleasant but there is nothing else that can be done to fix it.
            // See:
            // https://referencesource.microsoft.com/#system.windows.forms/winforms/managed/system/winforms/ButtonInternal/ButtonBaseAdapter.cs,751
            // https://referencesource.microsoft.com/#system.windows.forms/winforms/managed/system/winforms/ButtonInternal/ButtonBaseAdapter.cs,623

            // Ensure comboboxs have their dropdown width wrap to the max content width
            textureFormatComboBox.DropDownWidth = UIUtil.GetDropDownWidth(textureFormatComboBox);
            paletteFormatComboBox.DropDownWidth = UIUtil.GetDropDownWidth(paletteFormatComboBox);
            paletteLengthCopyLocationComboBox.DropDownWidth = UIUtil.GetDropDownWidth(paletteLengthCopyLocationComboBox);

            // Thanfully, ComboBox does follow ForeColor and BackColor for painting its background and text. However, WinForms
            // SystemColors does not change with the Dark Mode setting in Windows 10. Thus, the ForeColor and BackColor must be
            // set to equivalent controls that had their colors updated for Dark Mode by WinForms (as WinForms updates these
            // colors for certain controls and not for others).
            EventHandler fixForeColors = (sender, e) => { textureFormatComboBox.ForeColor = paletteFormatComboBox.ForeColor = paletteLengthCopyLocationComboBox.ForeColor = ForeColor; };
            EventHandler fixBackColors = (sender, e) => {  textureFormatComboBox.BackColor = paletteFormatComboBox.BackColor = paletteLengthCopyLocationComboBox.BackColor = BackColor; };
            fixForeColors(null, new EventArgs());
            fixBackColors(null, new EventArgs());
            ForeColorChanged += fixForeColors;
            BackColorChanged += fixBackColors;

            // Paint.NET only gives us a region of size 200x200 (which then has its width scaled by the DPI X scaling factor
            // and height auto-sized). Therefore, the available width might not always be 200. Instead, listen for the parent
            // SaveConfigPanel size to change and adapt to that new DPI-scaled size. This doesn't matter in the grand scheme
            // of things as Paint.NET's SaveConfigPanel has auto-scrollbars enabled so even if content goes OOB, it'll still
            // be visible but it looks better visually when done without width scrollbars.
            // Listen for parent resize
            Parent.SizeChanged += (sender, e) =>
            {
                int availableWidth = Parent.Size.Width - SystemInformation.VerticalScrollBarWidth;
                // Resize things to fit parent
                if (Width != availableWidth)
                {
                    Width = availableWidth;
                    textureFormatComboBox.Width = (availableWidth - textureFormatComboBox.Location.X);
                    paletteFormatComboBox.Width = (availableWidth - paletteFormatComboBox.Location.X);
                    paletteLengthCopyLocationComboBox.Width = (availableWidth - paletteLengthCopyLocationComboBox.Location.X);
                }
            };
        }

        //// END PAINT.NET STUFF ////
    }
}
