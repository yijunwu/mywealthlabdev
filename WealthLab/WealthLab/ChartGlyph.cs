namespace WealthLab
{
    using System;
    using System.Drawing;

    public class ChartGlyph
    {
        private bool aboveBar;
        private Color fontColor;
        private Image image;
        private int x;
        private int y;
        private int bar;
        private WealthLab.Position position;
        private string string_0;

        internal ChartGlyph(Image image_1, string string_1, Color color_1, int int_3, bool bool_1)
        {
            this.image = image_1;
            this.string_0 = string_1;
            this.fontColor = color_1;
            this.bar = int_3;
            this.aboveBar = bool_1;
        }

        public bool AboveBar
        {
            get
            {
                return this.aboveBar;
            }
            internal set
            {
                this.aboveBar = value;
            }
        }

        public int Bar
        {
            get
            {
                return this.bar;
            }
        }

        public Color FontColor
        {
            get
            {
                return this.fontColor;
            }
        }

        public Image Glyph
        {
            get
            {
                return this.image;
            }
        }

        public int Height
        {
            get
            {
                return this.image.Height;
            }
        }

        public bool IsTrade
        {
            get
            {
                return (this.position != null);
            }
        }

        public WealthLab.Position Position
        {
            get
            {
                return this.position;
            }
            internal set
            {
                this.position = value;
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
                return this.image.Width;
            }
        }

        public int X
        {
            get
            {
                return this.x;
            }
            internal set
            {
                this.x = value;
            }
        }

        public int Y
        {
            get
            {
                return this.y;
            }
            internal set
            {
                this.y = value;
            }
        }
    }
}

