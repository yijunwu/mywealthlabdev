namespace WealthLab.ChartDrawingObjects
{
    using Fidelity.Components;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.ChartControl;

    public class CDOTextNote : CDOPointBased
    {
        private Color color_0;
        private static DrawingObjectHelper drawingObjectHelper_0 = new TextNoteHelper();
        private Font font_1;
        private RectangleF rectangleF_0;
        private string string_2;
        private TextNoteSettings textNoteSettings_0;

        public CDOTextNote()
        {
        }

        public CDOTextNote(ChartPane pane, DateTime dateTime_0, double value) : base(pane, dateTime_0, value)
        {
        }

        public override void ChangeSettings(UserControl userControl_0)
        {
            TextNoteSettings settings = userControl_0 as TextNoteSettings;
            this.Text = settings.Text;
            this.TextColor = settings.TextColor;
            this.TextFont = settings.TextFont;
        }

        public override UserControl GetSettingsUI()
        {
            if (this.textNoteSettings_0 == null)
            {
                this.textNoteSettings_0 = new TextNoteSettings();
            }
            this.textNoteSettings_0.Text = this.Text;
            this.textNoteSettings_0.TextColor = this.TextColor;
            this.textNoteSettings_0.TextFont = this.TextFont;
            return this.textNoteSettings_0;
        }

        protected override bool IsMouseOver(int int_0, int int_1)
        {
            return this.rectangleF_0.Contains((float) int_0, (float) int_1);
        }

        protected override void OnSelected(int int_0, int int_1)
        {
        }

        public bool PreSetSettings(ISettingsHost host)
        {
            UserControl settingsUI = this.GetSettingsUI();
            DrawingObjectProperties properties = new DrawingObjectProperties {
                Text = drawingObjectHelper_0.FriendlyName + " Properties"
            };
            properties.AddUserControl(settingsUI);
            if (properties.ShowDialog() == DialogResult.OK)
            {
                this.ChangeSettings(settingsUI);
                this.WriteSettings(host);
                return true;
            }
            return false;
        }

        protected override void Read(BinaryReader binaryReader_0)
        {
            base.Read(binaryReader_0);
            this.Text = binaryReader_0.ReadString();
            int argb = binaryReader_0.ReadInt32();
            this.TextColor = Color.FromArgb(argb);
            string familyName = binaryReader_0.ReadString();
            double num2 = binaryReader_0.ReadDouble();
            FontStyle style = (FontStyle) binaryReader_0.ReadInt32();
            this.TextFont = new Font(familyName, (float) num2, style);
            base._mover = base.Handles[0];
        }

        public override void ReadSettings(ISettingsHost host)
        {
            if ((this.Text != null) && !(this.Text == ""))
            {
                string str = "DrawObj." + base.GetType().Name + ".";
                this.Text = host.Get(str + "Text", "");
                this.TextColor = host.Get(str + "TextColor", Color.Black);
                this.TextFont = host.Get(str + "TextFont", new Font("Arial", 10f));
            }
            else
            {
                this.PreSetSettings(host);
                if ((this.Text == "") || (this.Text == null))
                {
                    throw new Exception("CDOTextNote.ReadSettings  No Text Entered.");
                }
            }
        }

        protected override void Render(Graphics graphics_0)
        {
            if (base._mover.Bar != -1)
            {
                this.rectangleF_0.X = base._mover.X;
                this.rectangleF_0.Y = base._mover.Y;
                this.rectangleF_0.Width = 200f;
                this.rectangleF_0.Height = 100f;
                graphics_0.DrawString(this.Text, this.TextFont, new SolidBrush(this.TextColor), this.rectangleF_0);
                this._origin.X = base._mover.X;
                this._origin.Y = base._mover.Y;
            }
        }

        protected override void Write(BinaryWriter binaryWriter_0)
        {
            base.Write(binaryWriter_0);
            binaryWriter_0.Write(this.Text);
            binaryWriter_0.Write(this.TextColor.ToArgb());
            binaryWriter_0.Write(this.TextFont.Name);
            binaryWriter_0.Write((double) this.TextFont.Size);
            binaryWriter_0.Write((int) this.TextFont.Style);
        }

        public override void WriteSettings(ISettingsHost host)
        {
            string str = "DrawObj." + base.GetType().Name + ".";
            host.Set(str + "Text", this.Text);
            host.Set(str + "TextColor", this.TextColor);
            host.Set(str + "TextFont", this.TextFont);
        }

        protected override DrawingObjectHelper Helper
        {
            get
            {
                return drawingObjectHelper_0;
            }
        }

        public string Text
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        public Color TextColor
        {
            get
            {
                return this.color_0;
            }
            set
            {
                this.color_0 = value;
            }
        }

        public Font TextFont
        {
            get
            {
                return this.font_1;
            }
            set
            {
                this.font_1 = value;
            }
        }
    }
}

