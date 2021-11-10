using libWiiSharp.GX;
using PaintDotNet;
using System;
using System.Drawing;
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
        private CheckBox generateMipmapsCheckBox;

        //// END DESIGNER STUFF ////
        //// START PAINT.NET STUFF ////

        public TXTRFileTypeSaveConfigWidget()
        {
            InitializeComponent();
            UIUtil.BindEnumToCombobox<TextureFormat>(textureFormatComboBox, TextureFormat.I4);
            UIUtil.BindEnumToCombobox<PaletteFormat>(paletteFormatComboBox, PaletteFormat.IA8);
        }

        protected override void InitTokenFromWidget()
        {
            TXTRFileTypeSaveConfigToken token = (TXTRFileTypeSaveConfigToken)Token;
            token.TextureFormat = (TextureFormat)((UIUtil.ComboBoxEnumItem<TextureFormat>)textureFormatComboBox.SelectedItem).Value;
            token.TexturePalette = (PaletteFormat)((UIUtil.ComboBoxEnumItem<PaletteFormat>)paletteFormatComboBox.SelectedItem).Value;
            token.GenerateMipmaps = generateMipmapsCheckBox.Checked;
        }

        protected override void InitWidgetFromToken(SaveConfigToken sourceToken)
        {
            TXTRFileTypeSaveConfigToken token = (TXTRFileTypeSaveConfigToken)sourceToken;
            textureFormatComboBox.SelectedItem = UIUtil.ComboBoxEnumItem<TextureFormat>.CreateComboBoxEnumItem(token.TextureFormat);
            paletteFormatComboBox.SelectedItem = UIUtil.ComboBoxEnumItem<PaletteFormat>.CreateComboBoxEnumItem(token.TexturePalette);
            generateMipmapsCheckBox.Checked = token.GenerateMipmaps;
        }

        private void tokenChanged(object sender, EventArgs e)
        {
            CheckControls();
            UpdateToken();
        }

        private void CheckControls()
        {
            if (textureFormatComboBox.SelectedIndex < 0) textureFormatComboBox.SelectedIndex = 0;
            if (paletteFormatComboBox.SelectedIndex < 0) paletteFormatComboBox.SelectedIndex = 0;
            switch ((TextureFormat)((UIUtil.ComboBoxEnumItem<TextureFormat>)textureFormatComboBox.SelectedItem).Value)
            {
                case TextureFormat.CI4:
                case TextureFormat.CI8:
                case TextureFormat.CI14X2:
                    paletteFormatComboBox.Enabled = true;
                    generateMipmapsCheckBox.Enabled = false;
                    break;
                default:
                    paletteFormatComboBox.Enabled = false;
                    generateMipmapsCheckBox.Enabled = true;
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
            this.textureFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.textureFormatComboBox.FormattingEnabled = true;
            this.textureFormatComboBox.Location = new System.Drawing.Point(0, 26);
            this.textureFormatComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.textureFormatComboBox.MinimumSize = new System.Drawing.Size(103, 0);
            this.textureFormatComboBox.Name = "textureFormatComboBox";
            this.textureFormatComboBox.Size = new System.Drawing.Size(203, 28);
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
            this.paletteFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paletteFormatComboBox.Enabled = false;
            this.paletteFormatComboBox.FormattingEnabled = true;
            this.paletteFormatComboBox.Location = new System.Drawing.Point(0, 96);
            this.paletteFormatComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.paletteFormatComboBox.MinimumSize = new System.Drawing.Size(106, 0);
            this.paletteFormatComboBox.Name = "paletteFormatComboBox";
            this.paletteFormatComboBox.Size = new System.Drawing.Size(203, 28);
            this.paletteFormatComboBox.TabIndex = 2;
            this.paletteFormatComboBox.SelectionChangeCommitted += new System.EventHandler(this.tokenChanged);
            // 
            // generateMipmapsCheckBox
            // 
            this.generateMipmapsCheckBox.AutoSize = true;
            this.generateMipmapsCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.generateMipmapsCheckBox.Location = new System.Drawing.Point(0, 140);
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
            this.Controls.Add(this.generateMipmapsCheckBox);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "TXTRFileTypeSaveConfigWidget";
            this.Size = new System.Drawing.Size(203, 200);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //// END DESIGNER STUFF ////
        //// START PAINT.NET STUFF ////

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Ensure comboboxs have their dropdown width wrap to the max content width
            textureFormatComboBox.DropDownWidth = UIUtil.GetDropDownWidth(textureFormatComboBox);
            paletteFormatComboBox.DropDownWidth = UIUtil.GetDropDownWidth(paletteFormatComboBox);

            // Listen for parent resize
            Parent.SizeChanged += (sender, e) =>
            {
                // Resize things to fit parent
                if (Width != Parent.Size.Width)
                {
                    Width = Parent.Size.Width;
                    textureFormatComboBox.Width = (Parent.Size.Width - textureFormatComboBox.Location.X) - SystemInformation.VerticalScrollBarWidth;
                    paletteFormatComboBox.Width = (Parent.Size.Width - paletteFormatComboBox.Location.X) - SystemInformation.VerticalScrollBarWidth;
                }
            };
        }

        //// END PAINT.NET STUFF ////
    }
}
