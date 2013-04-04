namespace Steema.TeeChart.Export
{
    using System;
    using System.IO;
    using System.Text;

    public class EPSPage
    {
        private MemoryStream canvasstream;
        private int height;
        private EPSData m_epsdata;
        private Stream m_stream;
        private string m_string = "";
        private int width;

        public EPSPage(Stream s, int Width, int Height)
        {
            this.m_stream = s;
            this.width = Width;
            this.height = Height;
            if (this.canvasstream == null)
            {
                this.canvasstream = new MemoryStream();
            }
            if (this.m_epsdata == null)
            {
                this.m_epsdata = new EPSData(this.canvasstream);
            }
        }

        private void AddToStream(Stream s, string text)
        {
            string str = text + "\n";
            byte[] bytes = Encoding.ASCII.GetBytes(str);
            s.Write(bytes, 0, bytes.Length);
        }

        public void ConstructPage()
        {
            if (this.m_stream != null)
            {
                this.ConstructPage(this.m_stream);
            }
        }

        public void ConstructPage(Stream s)
        {
            s.Position = 0L;
            this.PageHeader(s);
            this.PageInfo(s);
            this.PageDict(s);
            this.FontDict(s);
            this.AddToStream(s, "%%EndProlog");
            this.m_epsdata.Stream.Position = 0L;
            this.CopyStream(this.m_epsdata.Stream, s);
        }

        private void CopyStream(Stream src, Stream dst)
        {
            int num;
            byte[] buffer = new byte[0x8000];
            while ((num = src.Read(buffer, 0, 0x8000)) > 0)
            {
                dst.Write(buffer, 0, num);
            }
        }

        private void FontDict(Stream s)
        {
        }

        private string FormatLongToString(long Value, int length)
        {
            string str = "";
            string str2 = "";
            if (Value >= 0L)
            {
                str2 = Value.ToString();
            }
            int num = length - str2.Length;
            for (int i = 0; i < num; i++)
            {
                str = str + "0";
            }
            return (str + str2);
        }

        private void PageDict(Stream s)
        {
            this.m_string = "/bd{bind def} bind def /ld{load def}bd /ed{exch def}bd /xd{cvx def}bd\n";
            this.m_string = this.m_string + "/np/newpath ld /cp/closepath ld /m/moveto ld /l/lineto ld /rm/rmoveto ld/rl/rlineto ld\n";
            this.m_string = this.m_string + "/rot/rotate ld /sc/scale ld /tr/translate ld\n";
            this.m_string = this.m_string + "/cpt/currentpoint ld\n";
            this.m_string = this.m_string + "/sw/setlinewidth ld /sd/setdash ld /rgb/setrgbcolor ld\n";
            this.m_string = this.m_string + "/gs/gsave ld /gr/grestore ld\n";
            this.m_string = this.m_string + "/st/stroke ld /fi/fill ld /s/show ld\n";
            this.m_string = this.m_string + "/ltext{cpt st m s}def \n";
            this.m_string = this.m_string + "/rtext{cpt st m dup stringwidth pop neg 0 rm s} def\n";
            this.m_string = this.m_string + "/ctext{cpt st m dup stringwidth pop -2 div 0 rm s} def";
            this.AddToStream(s, this.m_string);
            this.m_string = "/ellipsedict 8 dict def\n";
            this.m_string = this.m_string + "ellipsedict /mtrx matrix put\n";
            this.m_string = this.m_string + "/ellipse\n";
            this.m_string = this.m_string + "{ ellipsedict begin\n";
            this.m_string = this.m_string + "  np\n";
            this.m_string = this.m_string + "   /endangle exch def\n";
            this.m_string = this.m_string + "   /startangle exch def\n";
            this.m_string = this.m_string + "   /yrad exch def\n";
            this.m_string = this.m_string + "   /xrad exch def\n";
            this.m_string = this.m_string + "   /y exch def\n";
            this.m_string = this.m_string + "   /x exch def\n";
            this.m_string = this.m_string + "  /savematrix mtrx currentmatrix def\n";
            this.m_string = this.m_string + "  x y tr xrad yrad sc";
            this.m_string = this.m_string + "  0 0 1 startangle endangle arc\n";
            this.m_string = this.m_string + "  savematrix setmatrix\n";
            this.m_string = this.m_string + "  end\n";
            this.m_string = this.m_string + "} def";
            this.AddToStream(s, this.m_string);
            this.m_string = "/piedict 8 dict def\n";
            this.m_string = this.m_string + "piedict /mtrx matrix put\n";
            this.m_string = this.m_string + "/pie\n";
            this.m_string = this.m_string + "{ piedict begin\n";
            this.m_string = this.m_string + "  np\n";
            this.m_string = this.m_string + "   /endangle exch def\n";
            this.m_string = this.m_string + "   /startangle exch def\n";
            this.m_string = this.m_string + "   /yrad exch def\n";
            this.m_string = this.m_string + "   /xrad exch def\n";
            this.m_string = this.m_string + "   /y exch def\n";
            this.m_string = this.m_string + "   /x exch def\n";
            this.m_string = this.m_string + "  /savematrix mtrx currentmatrix def\n";
            this.m_string = this.m_string + "  x y tr xrad yrad sc\n";
            this.m_string = this.m_string + "  newpath\n";
            this.m_string = this.m_string + "  0 0 m\n";
            this.m_string = this.m_string + "  0 0 1 startangle endangle arc\n";
            this.m_string = this.m_string + "  closepath\n";
            this.m_string = this.m_string + "  savematrix setmatrix\n";
            this.m_string = this.m_string + "  end\n";
            this.m_string = this.m_string + "} def\n";
            this.m_string = this.m_string + "/FSD {findfont exch scalefont def} bind def\n";
            this.m_string = this.m_string + "/SMS {setfont moveto show} bind def\n";
            this.m_string = this.m_string + "/MS {moveto show} bind def";
            this.AddToStream(s, this.m_string);
        }

        private void PageHeader(Stream s)
        {
            this.AddToStream(s, "%!PS-Adobe-3.0 EPSF-3.0");
        }

        private void PageInfo(Stream s)
        {
            this.m_string = "%%BoundingBox: 0 0 " + this.width.ToString() + " " + this.height.ToString() + "\n";
            this.m_string = this.m_string + "%%Creator: TeeChart\n";
            this.m_string = this.m_string + "%%Title: (tChart Export)\n";
            this.m_string = this.m_string + "%%Pages: 1\n";
            this.m_string = this.m_string + "%%DocumentFonts: ";
            string str = "";
            foreach (EPSData.EPSFont font in this.epsData.Fonts)
            {
                str = str + font.EPSFontName + " ";
            }
            this.m_string = this.m_string + str + "\n";
            this.m_string = this.m_string + "%%DocumentNeededFonts: " + str + "\n";
            this.m_string = this.m_string + "%%LanguageLevel: 2\n";
            this.m_string = this.m_string + "%%EndComments";
            this.AddToStream(s, this.m_string);
        }

        public EPSData epsData
        {
            get
            {
                return this.m_epsdata;
            }
        }
    }
}

