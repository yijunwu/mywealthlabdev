namespace Steema.TeeChart.Export
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;

    public class PDFData
    {
        private PDFFonts m_fontlist = new PDFFonts();
        private GraphicsImages m_imagelist = new GraphicsImages();
        private int m_intPosition;
        private System.IO.Stream m_objStream;

        public PDFData(System.IO.Stream stream)
        {
            this.m_objStream = stream;
        }

        public PDFFonts Fonts
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

        public class PDFFont : GraphicsFont
        {
            public PDFFont(PDFData.PDFFonts fonts, ChartFont f) : base(fonts, f)
            {
            }

            public override string DictFontName()
            {
                return ("F" + base.Index.ToString());
            }

            protected override string GetFontName(ChartFont f)
            {
                string name = f.Name;
                if (f.Bold && !f.Italic)
                {
                    name = name + ",Bold";
                }
                else if (f.Italic && !f.Bold)
                {
                    name = name + ",Italic";
                }
                else if (f.Bold && f.Italic)
                {
                    name = name + ",BoldItalic";
                }
                return name.Replace(" ", "");
            }

            public Rectangle FontBBox
            {
                get
                {
                    return base.FontData.FontBBox;
                }
            }
        }

        public class PDFFonts : GraphicsFonts
        {
            public override int Add(ChartFont font)
            {
                PDFData.PDFFont font2 = new PDFData.PDFFont(this, font);
                return base.Add(font2);
            }

            public PDFData.PDFFont this[int index]
            {
                get
                {
                    return (PDFData.PDFFont) base[index];
                }
            }
        }
    }
}

