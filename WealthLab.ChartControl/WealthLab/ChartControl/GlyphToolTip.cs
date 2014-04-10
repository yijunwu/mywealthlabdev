namespace WealthLab.ChartControl
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxItem(false)]
    public class GlyphToolTip : UserControl
    {
        private IContainer components;
        private int barNum = -1;
        private string rolloverText;

        public GlyphToolTip()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GlyphToolTip_MouseMove(object sender, MouseEventArgs e)
        {
            base.Visible = false;
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.BackColor = SystemColors.Info;
            base.BorderStyle = BorderStyle.FixedSingle;
            this.ForeColor = SystemColors.InfoText;
            base.Name = "GlyphToolTip";
            base.Padding = new Padding(0, 0, 0, 3);
            base.Size = new Size(0, 3);
            base.MouseMove += new MouseEventHandler(this.GlyphToolTip_MouseMove);
            base.ResumeLayout(false);
        }

        public bool RepositionRequired(ChartGlyph glyph)
        {
            if (glyph.Bar == this.barNum)
            {
                return (glyph.RolloverText != this.rolloverText);
            }
            return true;
        }

        public void Reset()
        {
            this.barNum = -1;
        }

        public ChartGlyph Glyph
        {
            set
            {
                this.barNum = value.Bar;
                this.rolloverText = value.RolloverText;
                base.Controls.Clear();
                string[] strArray = value.RolloverText.Split(new char[] { '\n' });
                int num = 0;
                foreach (string str in strArray)
                {
                    if (str != "")
                    {
                        Label label = new Label {
                            ForeColor = value.FontColor,
                            AutoSize = true,
                            Text = str,
                            Location = new Point(4, (num * 0x11) + 4)
                        };
                        label.MouseMove += new MouseEventHandler(this.GlyphToolTip_MouseMove);
                        num++;
                        base.Controls.Add(label);
                    }
                }
            }
        }
    }
}

