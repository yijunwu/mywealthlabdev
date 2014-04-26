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
        private string rolloverText;

        ///WYJ fix, original signature: internal ChartGlyph(Image image_1, string string_1, Color color_1, int int_3, bool bool_1)
        internal ChartGlyph(Image image, string rolloverText, Color fontColor, int bar, bool aboveBar)
        {
            this.image = image;
            this.rolloverText = rolloverText;
            this.fontColor = fontColor;
            this.bar = bar;
            this.aboveBar = aboveBar;
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
                return this.rolloverText;
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

