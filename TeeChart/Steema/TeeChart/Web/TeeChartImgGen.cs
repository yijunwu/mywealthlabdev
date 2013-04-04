namespace Steema.TeeChart.Web
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Web;

    [ToolboxItem(false)]
    public class TeeChartImgGen : WebChart, IHttpHandler
    {
        protected internal string getMimeType(string ext)
        {
            switch (ext)
            {
                case "png":
                    return "png";

                case "jpg":
                    return "jpeg";

                case "gif":
                    return "gif";

                case "tif":
                    return "tiff";

                case "bmp":
                    return "bmp";

                case "emf":
                    return "emf";
            }
            return ext;
        }

        public void ProcessRequest(HttpContext context)
        {
            string str = context.Request["ChartID"];
            if (context.Cache[str] != null)
            {
                MemoryStream stream = new MemoryStream();
                stream = (MemoryStream) context.Cache[str];
                context.Response.ContentType = "image/" + this.getMimeType(str.Substring(str.Length - 3));
                context.Response.OutputStream.Write(stream.ToArray(), 0, (int) stream.Length);
                stream.Close();
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}

