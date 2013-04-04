namespace Steema.TeeChart.Export
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;

    public class PDFPage
    {
        private MemoryStream canvasstream;
        private int catalogNumber;
        private int height;
        private ArrayList m_offsets = new ArrayList();
        private PDFData m_pdfdata;
        private Stream m_stream;
        private string m_string = "";
        private int objectCount;
        private int parentNumber;
        private int resourceNumber;
        private int width;
        private long xrefPos;

        public PDFPage(Stream s, int Width, int Height)
        {
            this.m_stream = s;
            this.width = Width;
            this.height = Height;
            this.m_offsets.Clear();
            if (this.canvasstream == null)
            {
                this.canvasstream = new MemoryStream();
            }
            if (this.m_pdfdata == null)
            {
                this.m_pdfdata = new PDFData(this.canvasstream);
            }
        }

        private void AddToOffsets(long offset)
        {
            this.m_offsets.Add(this.FormatLongToString(offset, 10));
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
            this.objectCount = 0;
            s.Position = 0L;
            this.PageHeader(s);
            this.PageInfo(s);
            this.objectCount++;
            this.AddToOffsets(s.Length);
            this.AddToStream(s, this.objectCount.ToString() + " 0 obj");
            this.m_pdfdata.Stream.Position = 0L;
            this.AddToStream(s, "<< /Length " + this.m_pdfdata.Stream.Length.ToString() + " >>");
            this.AddToStream(s, "stream");
            this.CopyStream(this.m_pdfdata.Stream, s);
            this.AddToStream(s, "endstream");
            this.AddToStream(s, "endobj");
            this.PageFonts(s);
            this.PageImages(s);
            this.PageResources(s);
            this.Pages(s);
            this.Page(s);
            this.PageCatalog(s);
            this.PageXRef(s);
            this.AddToStream(s, "%%EOF");
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

        private void Page(Stream s)
        {
            this.objectCount++;
            this.AddToOffsets(s.Length);
            this.AddToStream(s, this.objectCount.ToString() + " 0 obj");
            this.AddToStream(s, "<< /Type /Page\n /Parent " + this.parentNumber.ToString() + " 0 R");
            this.AddToStream(s, "/MediaBox [ 0 0 " + this.width.ToString() + " " + this.height.ToString() + " ]");
            this.AddToStream(s, "/Contents 2 0 R");
            this.AddToStream(s, "/Resources " + this.resourceNumber.ToString() + " 0 R");
            this.AddToStream(s, ">>");
            this.AddToStream(s, "endobj");
        }

        private void PageCatalog(Stream s)
        {
            this.objectCount++;
            this.catalogNumber = this.objectCount;
            this.AddToOffsets(s.Length);
            this.AddToStream(s, this.objectCount.ToString() + " 0 obj");
            this.AddToStream(s, "<< /Type /Catalog");
            this.AddToStream(s, "/Pages " + this.parentNumber.ToString() + " 0 R");
            this.AddToStream(s, ">>");
            this.AddToStream(s, "endobj");
        }

        private void PageFonts(Stream s)
        {
            foreach (PDFData.PDFFont font in this.m_pdfdata.Fonts)
            {
                this.objectCount++;
                this.AddToOffsets(s.Length);
                font.ObjectNumber = this.objectCount;
                this.AddToStream(s, font.ObjectNumber.ToString() + " 0 obj");
                this.AddToStream(s, "<< /Type /Font");
                this.AddToStream(s, "/Subtype /TrueType");
                this.AddToStream(s, "/BaseFont /" + font.GraphicsFontName);
                this.AddToStream(s, "/Name /" + font.DictFontName());
                this.AddToStream(s, "/Encoding /WinAnsiEncoding");
                this.AddToStream(s, "/FirstChar 0");
                this.AddToStream(s, "/LastChar 255");
                this.AddToStream(s, string.Concat(new object[] { "/FontBBox [", font.FontBBox.Left, " ", font.FontBBox.Bottom, " ", font.FontBBox.Right, " ", font.FontBBox.Top, "]" }));
                this.AddToStream(s, ">>");
                this.AddToStream(s, "endobj");
            }
        }

        private void PageHeader(Stream s)
        {
            this.AddToStream(s, "%PDF-1.4");
        }

        private void PageImages(Stream s)
        {
            GraphicsImages images = this.m_pdfdata.Images;
            for (int i = 0; i < images.Count; i++)
            {
                this.objectCount++;
                this.AddToOffsets(s.Length);
                images[i].ObjectNumber = this.objectCount;
                this.AddToStream(s, images[i].ObjectNumber.ToString() + " 0 obj");
                this.AddToStream(s, "<< /Type /XObject");
                this.AddToStream(s, "/Subtype /Image");
                this.AddToStream(s, "/Name /" + images[i].DictImageName());
                this.AddToStream(s, "/Length " + images[i].ImageStream.Length.ToString());
                this.AddToStream(s, "/Width " + images[i].ImageWidth.ToString());
                this.AddToStream(s, "/Height " + images[i].ImageHeight.ToString());
                this.AddToStream(s, "/ColorSpace /DeviceRGB");
                this.AddToStream(s, "/BitsPerComponent 8");
                this.AddToStream(s, "/Filter [/DCTDecode]");
                this.AddToStream(s, ">>");
                this.AddToStream(s, "stream");
                images[i].ImageStream.Position = 0L;
                this.CopyStream(images[i].ImageStream, s);
                this.AddToStream(s, "\n endstream");
                this.AddToStream(s, "endobj");
            }
        }

        private void PageInfo(Stream s)
        {
            this.objectCount++;
            this.AddToOffsets(s.Length);
            this.AddToStream(s, this.objectCount.ToString() + " 0 obj");
            this.m_string = "<<\n/Creator (TeeChart)\n/Producer (TeeChart)\n";
            this.m_string = this.m_string + "/CreationDate (D:" + DateTime.Now.ToString("yyyyMMddHHmmss") + ")\n";
            this.m_string = this.m_string + "/ModDate ()\n/Keywords ()\n/Title (TChart Export)\n>>";
            this.AddToStream(s, this.m_string);
            this.AddToStream(s, "endobj");
        }

        private void PageResources(Stream s)
        {
            this.objectCount++;
            this.resourceNumber = this.objectCount;
            this.AddToOffsets(s.Length);
            this.AddToStream(s, this.objectCount.ToString() + " 0 obj");
            this.AddToStream(s, "<< /ProcSet [/PDF /Text /ImageC]");
            this.AddToStream(s, "/Font << ");
            PDFData.PDFFonts fonts = this.pdfData.Fonts;
            for (int i = 0; i < fonts.Count; i++)
            {
                this.AddToStream(s, "/" + fonts[i].DictFontName() + " " + fonts[i].ObjectNumber.ToString() + " 0 R");
            }
            this.AddToStream(s, ">>");
            this.AddToStream(s, "/XObject << ");
            GraphicsImages images = this.pdfData.Images;
            for (int j = 0; j < images.Count; j++)
            {
                this.AddToStream(s, "/" + images[j].DictImageName() + " " + images[j].ObjectNumber.ToString() + " 0 R");
            }
            this.AddToStream(s, ">>");
            this.AddToStream(s, ">>");
            this.AddToStream(s, "endobj");
        }

        private void Pages(Stream s)
        {
            this.objectCount++;
            this.AddToOffsets(s.Length);
            this.parentNumber = this.objectCount;
            int num = this.parentNumber + 1;
            this.AddToStream(s, this.objectCount.ToString() + " 0 obj");
            this.m_string = "<< /Type /Pages\n/Kids [" + num.ToString() + " 0 R]\n";
            this.m_string = this.m_string + "/Count 1\n>>";
            this.AddToStream(s, this.m_string);
            this.AddToStream(s, "endobj");
        }

        private void PageTrailer(Stream s)
        {
            this.AddToStream(s, "trailer");
            this.AddToStream(s, "<< /Size " + this.objectCount.ToString());
            this.AddToStream(s, "/Root " + this.catalogNumber.ToString() + " 0 R");
            this.AddToStream(s, "/Info 1 0 R");
            this.AddToStream(s, ">>");
            this.AddToStream(s, "startxref");
            this.AddToStream(s, this.xrefPos.ToString());
        }

        private void PageXRef(Stream s)
        {
            this.objectCount++;
            this.xrefPos = s.Length;
            this.AddToStream(s, "xref");
            this.AddToStream(s, "0 " + this.objectCount.ToString());
            this.AddToStream(s, "0000000000 65535 f");
            for (int i = 0; i < this.m_offsets.Count; i++)
            {
                this.AddToStream(s, this.m_offsets[i].ToString() + " " + this.FormatLongToString(0L, 5) + " n");
            }
            this.PageTrailer(s);
        }

        public PDFData pdfData
        {
            get
            {
                return this.m_pdfdata;
            }
        }
    }
}

