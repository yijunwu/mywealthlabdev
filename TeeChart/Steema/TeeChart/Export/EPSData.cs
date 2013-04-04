namespace Steema.TeeChart.Export
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.IO;
    using System.Reflection;

    public class EPSData
    {
        private EPSFonts m_fontlist = new EPSFonts();
        private GraphicsImages m_imagelist = new GraphicsImages();
        private int m_intPosition;
        private System.IO.Stream m_objStream;

        public EPSData(System.IO.Stream stream)
        {
            this.m_objStream = stream;
        }

        public EPSFonts Fonts
        {
            get
            {
                return this.m_fontlist;
            }
            set
            {
                this.m_fontlist = value;
            }
        }

        public GraphicsImages Images
        {
            get
            {
                return this.m_imagelist;
            }
            set
            {
                this.m_imagelist = value;
            }
        }

        public int Position
        {
            get
            {
                return this.m_intPosition;
            }
            set
            {
                this.m_intPosition = value;
            }
        }

        public System.IO.Stream Stream
        {
            get
            {
                return this.m_objStream;
            }
            set
            {
                this.m_objStream = value;
            }
        }

        public class EPSFont : GraphicsFont
        {
            public EPSFont(EPSData.EPSFonts fonts, ChartFont f) : base(fonts, f)
            {
            }

            public override string DictFontName()
            {
                return base.GraphicsFontName;
            }

            protected override string GetFontName(ChartFont f)
            {
                string name = f.Name;
                name = f.DrawingFont.FontFamily.Name;
                if (f.Bold && !f.Italic)
                {
                    name = name + "-Bold";
                }
                else if (f.Italic && !f.Bold)
                {
                    name = name + "-Italic";
                }
                else if (f.Bold && f.Italic)
                {
                    name = name + "-BoldItalic";
                }
                return name.Replace(" ", "-");
            }

            public string EPSFontName
            {
                get
                {
                    return base.GraphicsFontName;
                }
            }
        }

        public class EPSFonts : GraphicsFonts
        {
            public override int Add(ChartFont font)
            {
                EPSData.EPSFont font2 = new EPSData.EPSFont(this, font);
                return base.Add(font2);
            }

            public EPSData.EPSFont this[int index]
            {
                get
                {
                    return (EPSData.EPSFont) base[index];
                }
            }
        }
    }
}

