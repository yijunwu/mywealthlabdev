namespace Steema.TeeChart.Export
{
    using System;

    public class ImageFormats
    {
        public const int Count = 12;
        internal static string[] FormatDescriptions = new string[] { Texts.AsBMP, Texts.AsEMF, Texts.AsJPEG, Texts.AsPNG, Texts.AsGIF, Texts.AsTIFF, Texts.AsVML, Texts.AsSVG, Texts.AsPDF, Texts.AsPS, Texts.AsXAML, Texts.AsFlex };
        internal static Type[] Formats = new Type[] { typeof(BitmapFormat), typeof(MetafileFormat), typeof(JPEGFormat), typeof(PNGFormat), typeof(GIFFormat), typeof(TIFFFormat), typeof(VMLFormat), typeof(SVGFormat), typeof(PDFFormat), typeof(EPSFormat), typeof(XAMLFormat), typeof(FlexFormat) };
    }
}

