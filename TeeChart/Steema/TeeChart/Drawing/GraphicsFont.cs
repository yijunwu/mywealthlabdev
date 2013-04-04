namespace Steema.TeeChart.Drawing
{
    using System;

    public abstract class GraphicsFont
    {
        private GraphicsFonts fonts;
        private TTFontData m_fontdata;
        protected string m_fontname;
        private int m_index;
        private int m_objectnumber;

        public GraphicsFont(GraphicsFonts fonts, ChartFont f)
        {
            this.fonts = fonts;
            this.m_fontname = this.GetFontName(f);
            this.m_index = fonts.Count;
            this.m_fontdata = new TTFontData(f);
        }

        public virtual string DictFontName()
        {
            return "";
        }

        public bool Equals(ChartFont font)
        {
            return this.GetFontName(font).Equals(this.m_fontname);
        }

        protected abstract string GetFontName(ChartFont f);

        public TTFontData FontData
        {
            get
            {
                return this.m_fontdata;
            }
        }

        public string GraphicsFontName
        {
            get
            {
                return this.m_fontname;
            }
            set
            {
                this.m_fontname = value;
            }
        }

        public int Index
        {
            get
            {
                return this.m_index;
            }
        }

        public int ObjectNumber
        {
            get
            {
                return this.m_objectnumber;
            }
            set
            {
                this.m_objectnumber = value;
            }
        }
    }
}

