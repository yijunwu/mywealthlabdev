namespace WealthLab
{
    using System;
    using System.Drawing;

    public class ChartGlyph
    {
        private bool bool_0;
        private Color color_0;
        private Image image_0;
        private int int_0;
        private int int_1;
        private int int_2;
        private WealthLab.Position position_0;
        private string string_0;

        internal ChartGlyph(Image image_1, string string_1, Color color_1, int int_3, bool bool_1)
        {
            this.image_0 = image_1;
            this.string_0 = string_1;
            this.color_0 = color_1;
            this.int_2 = int_3;
            this.bool_0 = bool_1;
        }

        public bool AboveBar
        {
            get
            {
                return this.bool_0;
            }
            internal set
            {
                this.bool_0 = value;
            }
        }

        public int Bar
        {
            get
            {
                return this.int_2;
            }
        }

        public Color FontColor
        {
            get
            {
                return this.color_0;
            }
        }

        public Image Glyph
        {
            get
            {
                return this.image_0;
            }
        }

        public int Height
        {
            get
            {
                return this.image_0.Height;
            }
        }

        public bool IsTrade
        {
            get
            {
                return (this.position_0 != null);
            }
        }

        public WealthLab.Position Position
        {
            get
            {
                return this.position_0;
            }
            internal set
            {
                this.position_0 = value;
            }
        }

        public string RolloverText
        {
            get
            {
                return this.string_0;
            }
        }

        public int Width
        {
            get
            {
                return this.image_0.Width;
            }
        }

        public int X
        {
            get
            {
                return this.int_0;
            }
            internal set
            {
                this.int_0 = value;
            }
        }

        public int Y
        {
            get
            {
                return this.int_1;
            }
            internal set
            {
                this.int_1 = value;
            }
        }
    }
}

