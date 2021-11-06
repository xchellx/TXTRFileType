using PaintDotNet;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TXTRFileType
{
    //// START DESIGNER STUFF ////
    internal partial class TXTRFileTypeSaveConfigWidget : SaveConfigWidget
    {
        private Label label1;
        private ComboBox comboBox1;
        private Label label2;
        private ComboBox comboBox2;

        //// END DESIGNER STUFF ////
        //// START PAINT.NET STUFF //
        private Label TextureFormatLabel;
        private ComboBox TextureFormatCombo;
        private Label TexturePaletteLabel;
        private ComboBox TexturePaletteCombo;

        public TXTRFileTypeSaveConfigWidget()
        {
            TextureFormatLabel = new Label
            {
                AutoSize = true,
                Location = new Point(0, 5),
                Size = new Size(93, 15),
                Margin = new Padding(0, 0, 0, 8),
                TabIndex = 0,
                Name = "TextureFormatLabel",
                Text = "Texture Format:"
            };
            TextureFormatLabel.Font = new Font(TextureFormatLabel.Font.FontFamily, 6.8F);
            Controls.Add(TextureFormatLabel);

            TextureFormatCombo = new ComboBox
            {
                AutoSize = true,
                FormattingEnabled = true,
                Location = new Point(97, 0),
                Size = new Size(131, 24),
                Margin = new Padding(4, 0, 0, 8),
                TabIndex = 1,
                Name = "TextureFormatCombo",
                DataSource = Enum.GetValues(typeof(GX.TextureFormat))
            };
            TextureFormatCombo.SelectionChangeCommitted += tokenChanged;
            Controls.Add(TextureFormatCombo);

            TexturePaletteLabel = new Label
            {
                AutoSize = true,
                Location = new Point(0, 37),
                Size = new Size(90, 15),
                Margin = new Padding(0, 0, 0, 8),
                TabIndex = 2,
                Name = "TexturePaletteLabel",
                Text = "Palette Format:"
            };
            TexturePaletteLabel.Font = new Font(TextureFormatLabel.Font.FontFamily, 6.8F);
            Controls.Add(TexturePaletteLabel);

            TexturePaletteCombo = new ComboBox
            {
                AutoSize = true,
                FormattingEnabled = true,
                Location = new Point(94, 32),
                Size = new Size(134, 24),
                Margin = new Padding(4, 0, 0, 8),
                TabIndex = 3,
                Name = "TexturePaletteCombo",
                Enabled = false,
                DataSource = Enum.GetValues(typeof(GX.PaletteFormat))
            };
            TexturePaletteCombo.SelectionChangeCommitted += tokenChanged;
            Controls.Add(TexturePaletteCombo);
        }

        protected override void InitTokenFromWidget()
        {
            TXTRFileTypeSaveConfigToken token = (TXTRFileTypeSaveConfigToken)Token;
            token.TextureFormat = (GX.TextureFormat)TextureFormatCombo.SelectedItem;
            token.TexturePalette = (GX.PaletteFormat)TexturePaletteCombo.SelectedItem;
        }

        protected override void InitWidgetFromToken(SaveConfigToken sourceToken)
        {
            TXTRFileTypeSaveConfigToken token = (TXTRFileTypeSaveConfigToken)sourceToken;
            TextureFormatCombo.SelectedItem = token.TextureFormat;
            TexturePaletteCombo.SelectedItem = token.TexturePalette;
        }

        private void tokenChanged(object sender, EventArgs e)
        {
            CheckControls();
            UpdateToken();
        }

        private void CheckControls()
        {
            if (TextureFormatCombo.SelectedIndex < 0) TextureFormatCombo.SelectedIndex = 0;
            if (TexturePaletteCombo.SelectedIndex < 0) TexturePaletteCombo.SelectedIndex = 0;
            switch (TextureFormatCombo.SelectedItem)
            {
                case GX.TextureFormat.CI4:
                case GX.TextureFormat.CI8:
                case GX.TextureFormat.CI14X2:
                    TexturePaletteCombo.Enabled = true;
                    break;
                default:
                    TexturePaletteCombo.Enabled = false;
                    break;
            }
        }

        //// END PAINT.NET STUFF //
        //// START DESIGNER STUFF ////
        /// <summary>
        /// This is required for designer preview so the UI can easily be templated for actual code setup elsewhere.
        /// As a result, all of this code is temporary preview template code. It is only used for reference.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Palette Format:";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(94, 32);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(4, 0, 0, 8);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(134, 24);
            this.comboBox2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Texture Format:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(97, 0);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 0, 0, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(131, 24);
            this.comboBox1.TabIndex = 0;
            // 
            // TXTRFileTypeSaveConfigWidget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.comboBox2);
            this.Name = "TXTRFileTypeSaveConfigWidget";
            this.Size = new System.Drawing.Size(228, 228);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        //// END DESIGNER STUFF ////
    }
}
