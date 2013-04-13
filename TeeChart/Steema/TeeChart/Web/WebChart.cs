namespace Steema.TeeChart.Web
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Editors.Export;
    using Steema.TeeChart.Export;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Themes;
    using Steema.TeeChart.Tools;
    using System;
    using System.Security;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Printing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Text;
    using System.Web;
    using System.Web.Caching;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;
    using System.Xml;

    [Serializable, ToolboxBitmap(typeof(WebChart), "Images.WebChart.bmp"), Designer(typeof(WebChart.Designer)), DefaultProperty("Chart"), LicenseProvider(typeof(FrAccessProvider)), ToolboxData("<{0}:WebChart runat=server></{0}:WebChart>")]
    public class WebChart : WebControl, IComponent, IDisposable, INamingContainer, IPostBackEventHandler, IPostBackDataHandler, IChart, ISerializable
    {
        private bool autoPostback;
        private int clickedX;
        private int clickedY;
        private string config;
        protected internal string designPath;
        protected internal Steema.TeeChart.Chart iChart;
        private string iGetChartFile;
        private bool isObjectevent;
        public static string lastID = "";
        private FrAccess license;
        private PictureFormats pictureFormat;
        private bool refreshing;
        private string shareURL;
        private TempChartStyle tempChart;
        private string tmpFolder;
        public MemoryStream tmpImg;

        public event PaintChartEventHandler AfterDraw;

        public event PaintChartEventHandler BeforeDraw;

        public event PaintChartEventHandler BeforeDrawAxes;

        public event PaintChartEventHandler BeforeDrawSeries;

        public event ClickEventHandler ClickAxis;

        public event ClickEventHandler ClickBackground;

        public event ClickEventHandler ClickLegend;

        public event SeriesEventHandler ClickSeries;

        public event ClickEventHandler ClickTitle;

        public event GetAxesChartRectEventHandler GetAxesChartRect;

        public event GetAxisLabelEventHandler GetAxisLabel;

        public event GetLegendPosEventHandler GetLegendPos;

        public event GetLegendRectEventHandler GetLegendRect;

        public event GetLegendTextEventHandler GetLegendText;

        public event GetNextAxisLabelEventHandler GetNextAxisLabel;

        public WebChart()
        {
            this.tmpFolder = "_chart_temp";
            this.shareURL = "";
            this.designPath = "";
            this.iGetChartFile = "GetChart.aspx";
            this.iChart = new Steema.TeeChart.Chart();
            this.pictureFormat = PictureFormats.PNG;
            ColorPalettes.ApplyPalette(this.Chart, 0);
            if (this.TempChart == TempChartStyle.File)
            {
                this.GetShareFolder();
            }
            ((IChart) this).DoSetControlStyle();
            this.license = (FrAccess) LicenseManager.Validate(typeof(WebChart), this);
        }

        public WebChart(SerializationInfo info, StreamingContext context) : this()
        {
            this.Chart.Import.DeserializeFrom(info, context);
            this.Width = this.Chart.Width;
            this.Height = this.Chart.Height;
        }

        public void Clear()
        {
            this.iChart.Clear(this);
        }

        internal string CreateDesignPictureFile()
        {
            ImageExportFormat format = this.iChart.Export.Image.FromFormat(this.pictureFormat);
            int num = Utils.Round(this.Width.Value);
            int num2 = Utils.Round(this.Height.Value);
            if (num == 0)
            {
                num = 400;
            }
            if (num2 == 0)
            {
                num2 = 300;
            }
            format.Width = num;
            format.Height = num2;
            string str = "";
            if (base.Site == null)
            {
                return "";
            }
            str = this.ClientID + "." + format.FileExtension;
            format.Save(Path.GetTempPath() + @"\" + str);
            if (base.Style["Position"] != null)
            {
                return ("<IMG style=\"position: " + base.Style["Position"] + "; left:" + ((base.Style["Left"] == null) ? "0" : base.Style["Left"]) + "; top: " + ((base.Style["Top"] == null) ? "0" : base.Style["Top"]) + "\" SRC=\"" + Path.GetTempPath() + @"\" + str + "\">");
            }
            return ("<IMG SRC=\"" + Path.GetTempPath() + @"\" + str + "\">");
        }

        public void CreatePictureFile(HtmlTextWriter writer, int flag)
        {
            string str9;
            this.Chart.parent = this;
            ImageExportFormat format = this.iChart.Export.Image.FromFormat(this.pictureFormat);
            format.Width = Utils.Round(this.Width.Value);
            format.Height = Utils.Round(this.Height.Value);
            string str = DateTime.Now.Ticks.ToString();
            if (str == lastID)
            {
                str = str + "1";
            }
            lastID = str;
            string fileRoot = this.ClientID.ToString() + this.Context.Request.UserHostAddress.ToString().Replace(".", "") + str.Replace(",", "").Replace("/", "").Replace(":", "").Replace(" ", "").Replace(".", "") + format.FileExtension;
            if ((this.tempChart == TempChartStyle.Session) && (this.Context.Session == null))
            {
                this.tempChart = TempChartStyle.File;
            }
            MemoryStream stream = new MemoryStream();
            bool flag2 = false;
            ScrollTool scrollTool = null;
            foreach (Steema.TeeChart.Tools.Tool tool2 in this.Chart.Tools)
            {
                if ((tool2 is ScrollTool) && tool2.Active)
                {
                    scrollTool = (ScrollTool) tool2;
                    flag2 = true;
                }
            }
            if (flag2)
            {
                format.Save(stream);
                this.saveFileToImg(this.Context, format, stream, "", fileRoot + "Base", true);
                scrollTool.initScrollVars(this.Chart);
            }
            string fileName = "";
            if (scrollTool != null)
            {
                scrollTool.Attributes.imgList = new Hashtable();
            }
            int i = 0;
            MemoryStream chartImg = new MemoryStream();
            if (flag2)
            {
                while ((scrollTool.Attributes.newWidth - (scrollTool.Attributes.newChartRect.X - scrollTool.Attributes.newLeftWall)) > 0)
                {
                    chartImg = new MemoryStream();
                    fileName = scrollTool.GenerateImageList(ref chartImg, ref i, format, fileRoot);
                    this.saveFileToImg(this.Context, format, chartImg, fileRoot, fileName, true);
                }
            }
            else
            {
                fileName = fileRoot;
                this.saveFileToImg(this.Context, format, chartImg, fileRoot, fileName, false);
            }
            string str4 = "";
            string str5 = "";
            foreach (string str6 in base.Attributes.Keys)
            {
                if (str6.ToUpper().IndexOf("CLICKED") == -1)
                {
                    if (str6.ToLower() == "style")
                    {
                        str5 = str5 + str6 + "=\"" + base.Attributes[str6];
                    }
                    else
                    {
                        str4 = str4 + str6 + "=\"" + base.Attributes[str6] + "\"";
                    }
                }
            }
            ZoomTool tool3 = new ZoomTool();
            bool flag3 = false;
            string customVariables = "";
            bool mapSet = false;
            string str8 = "";
            if (flag != 1)
            {
                str9 = "<MAP name=\"MAP" + this.ClientID.ToString() + "zoom\" ";
            }
            else
            {
                str9 = "<MAP name=\"MAP" + this.ClientID.ToString() + "\" ";
            }
            string mapElements = "";
            int hotspotCanvasIndex = 0;
            bool mapScript = false;
            bool useHelperScript = false;
            string str11 = "";
            string key = "Steema.TeeChart.Web.twebchart.js";
            System.Type type = typeof(ToolResource);
            if (!this.AutoPostback)
            {
                foreach (Steema.TeeChart.Tools.Tool tool4 in this.Chart.Tools)
                {
                    if ((tool4 is IHotspot) && tool4.Active)
                    {
                        if (flag2)
                        {
                            str8 = str8 + this.getScrollingHotspots((IHotspot) tool4, ref useHelperScript, ref mapScript, ref mapElements, ref hotspotCanvasIndex, ref mapSet, scrollTool);
                        }
                        else
                        {
                            str8 = str8 + this.getHotspotMap((IHotspot) tool4, ref useHelperScript, ref mapScript, ref mapElements, ref hotspotCanvasIndex, ref mapSet, this.Chart.ChartRect);
                        }
                    }
                    else if ((tool4 is ScrollTool) && tool4.Active)
                    {
                        scrollTool = (ScrollTool) tool4;
                        bool flag7 = false;
                        if (!this.Page.ClientScript.IsClientScriptIncludeRegistered(type, key))
                        {
                            flag7 = true;
                            this.Page.ClientScript.RegisterClientScriptResource(type, key);
                        }
                        scrollTool.setAttributes(this.ClientID.ToString());
                        customVariables = scrollTool.CustomVariables;
                        if (str11 == "")
                        {
                            str11 = "<script language=\"jscript\">function zoomRect(){} function activateContent(){}</script>" + (flag7 ? ("\n<script src=\"" + this.Page.ClientScript.GetWebResourceUrl(type, key) + "\" type=\"text/javascript\"></script>\n") : "");
                        }
                    }
                    else if ((tool4 is ZoomTool) && tool4.Active)
                    {
                        tool3 = (ZoomTool) tool4;
                        bool flag8 = false;
                        if (!this.Page.ClientScript.IsClientScriptIncludeRegistered(type, key))
                        {
                            flag8 = true;
                            this.Page.ClientScript.RegisterClientScriptResource(type, key);
                        }
                        tool3.setAttributes(this.ClientID.ToString());
                        customVariables = tool3.CustomVariables;
                        if (str11 == "")
                        {
                            str11 = "<script language=\"jscript\">function zoomRect(){} function activateContent(){}</script>" + (flag8 ? ("\n<script src=\"" + this.Page.ClientScript.GetWebResourceUrl(type, key) + "\" type=\"text/javascript\"></script>\n") : "");
                        }
                        flag3 = true;
                    }
                }
                if (mapSet && !flag2)
                {
                    str8 = str9 + mapElements + ">" + str8 + "</MAP>";
                }
            }
            else
            {
                if (!this.Page.ClientScript.IsClientScriptIncludeRegistered(type, key))
                {
                    this.Page.ClientScript.RegisterClientScriptResource(type, key);
                }
                if (str11 == "")
                {
                    str11 = "\n<script src=\"" + this.Page.ClientScript.GetWebResourceUrl(type, key) + "\" type=\"text/javascript\"></script>\n";
                }
            }
            string str13 = "";
            str13 = string.Concat(new object[] { "<div ", str4, " ", (str5.Length > 0) ? (str5 + "; ") : "style=\"", " z-index : 100; height:", format.Height, this.GetUnitType(this.Height), "; width:", format.Width, this.GetUnitType(this.Width), "\" id=", this.ClientID.ToString(), "> ", str11, "<IMG GALLERYIMG=\"no\" " });
            if (flag3 || flag2)
            {
                str13 = str13 + tool3.Attributes.Attribs;
            }
            if (mapSet)
            {
                string str14;
                if (flag != 1)
                {
                    str14 = this.ClientID.ToString() + "zoom";
                }
                else
                {
                    str14 = this.ClientID.ToString();
                }
                str13 = str13 + "USEMAP=\"#MAP" + str14 + "\" ";
                if ((!this.Page.ClientScript.IsStartupScriptRegistered(typeof(string), "Startup") && mapScript) && useHelperScript)
                {
                    this.Page.ClientScript.RegisterStartupScript(typeof(string), "Startup", this.GetMapScriptBlock(hotspotCanvasIndex));
                }
            }
            if (flag3 || flag2)
            {
                if (!this.Page.ClientScript.IsStartupScriptRegistered(typeof(string), "ZoomCode"))
                {
                    this.Page.ClientScript.RegisterStartupScript(typeof(string), "ZoomCode", this.GetZoomScriptBlock(tool3.Attributes.NoClickPostback, tool3.Attributes.DragZoom, tool3.Attributes.CanvasIndex, tool3.Attributes.FillTransp, tool3.Attributes.PenColor, customVariables));
                }
            }
            else if (!this.Page.ClientScript.IsStartupScriptRegistered(typeof(string), "ZoomCode"))
            {
                this.Page.ClientScript.RegisterStartupScript(typeof(string), "ZoomCode", this.GetClickEventScriptBlock());
            }
            string str15 = fileRoot;
            string str16 = str15;
            if (flag2)
            {
                str15 = fileRoot + "Base";
            }
            if (this.tempChart == TempChartStyle.Session)
            {
                str16 = "SRC=\"" + this.iGetChartFile + "?Chart=" + str15 + "\" ";
                writer.Write(str13 + str16);
            }
            else if (this.tempChart == TempChartStyle.Httphandler)
            {
                str16 = "src=\"~/TeeChartImgGen.ashx?Width=" + format.Width.ToString() + "&Height=" + format.Height.ToString() + "&ChartID=" + str15 + "\" ";
                writer.Write(str13 + str16);
            }
            else if (this.tempChart == TempChartStyle.Cache)
            {
                str16 = "SRC=\"" + this.iGetChartFile + "?Chart=" + str15 + "\" ";
                writer.Write(str13 + str16);
            }
            else
            {
                str16 = "SRC=\"" + this.shareURL + "/" + this.tmpFolder + "/" + fileName + "." + format.FileExtension + "\" ";
                writer.Write(str13 + str16);
            }
            writer.Write("id=\"" + this.ClientID.ToString() + "img\" Name=\"" + this.ClientID.ToString() + "img\" ");
            if (flag2)
            {
                str16 = str16.Substring(5, str16.LastIndexOf("Base") - 5);
                writer.Write(string.Concat(new object[] { 
                    " Border=0 onload=\"registerScrollSettings(2,'", this.ClientID.ToString(), "',", scrollTool.Attributes.scrollPages.ToString(), ",", scrollTool.Attributes.newWidth.ToString(), ",", scrollTool.Attributes.startPos.ToString(), ",'", str16, "',", scrollTool.Attributes.sourceRect.Left, ",", scrollTool.Attributes.sourceRect.Top, ",", scrollTool.Attributes.sourceRect.Width, 
                    ",", scrollTool.Attributes.sourceRect.Height, ",", scrollTool.Attributes.bottomAxisPos, ",", scrollTool.Attributes.mouseAction.ToString(), ")\">"
                 }));
            }
            else if (flag3)
            {
                writer.Write(" Border=0 onload=\"registerChart(1,'" + this.ClientID.ToString() + "')\">");
            }
            else if (!this.AutoPostback)
            {
                writer.Write(" Border=0>");
            }
            else
            {
                writer.Write(" Border=0 onload=\"registerChart(1,'" + this.ClientID.ToString() + "')\">");
            }
            if (mapSet && !flag2)
            {
                writer.Write("<BR>" + str8);
            }
            writer.Write("</div>");
            if (flag2)
            {
                writer.Write("\n<br><div onmouseup=\"handleDragScrollStop('" + this.ClientID.ToString() + "', event)\" onmousedown=\"handleDragScrollClick('" + this.ClientID.ToString() + "', event)\" onmousemove=\"handleDragIt('" + this.ClientID.ToString() + "inlay',event)\">" + str8 + "</div>");
            }
        }

        protected internal void CreatePictureStream(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            ImageExportFormat format = this.iChart.Export.Image.FromFormat(this.pictureFormat);
            format.Width = Utils.Round(this.Width.Value);
            format.Height = Utils.Round(this.Height.Value);
            builder.Append("~/TeeChartImgGen.ashx");
            writer.WriteBeginTag("img");
            writer.WriteAttribute("src", builder.ToString() + "?Width=" + format.Width.ToString() + "&Height=" + format.Height.ToString() + "&ChartID=" + this.ClientID.ToString());
            if (this.ClientID != null)
            {
                writer.WriteAttribute("id", this.ClientID);
            }
            writer.Write('>');
        }

        public override void Dispose()
        {
            if (this.license != null)
            {
                this.license.Dispose();
                this.license = null;
            }
            this.iChart.Dispose();
            base.Dispose();
        }

        private string GetActiveContentScriptBlock()
        {
            StringBuilder sB = new StringBuilder();
            this.GetActiveContentScriptBlock(ref sB);
            return sB.ToString();
        }

        private void GetActiveContentScriptBlock(ref StringBuilder sB)
        {
            sB.Append("\n");
            sB.Append("var ActionType = { Zooming:1, Scrolling:2 };\n");
            sB.Append("\n");
            sB.Append("/*charts list*/\n");
            sB.Append("var chartList = \"\";\n");
            sB.Append("\n");
            sB.Append("this.blockEvent = function(evt) {\n");
            sB.Append("  evt = (evt) ? evt : event;\n");
            sB.Append("  evt.cancelBubble = true;\n");
            sB.Append("  draggingEvent(evt);\n");
            sB.Append("  return false;\n");
            sB.Append("}\n");
            sB.Append("\n");
            sB.Append("this.draggingEvent = function(evt)\n");
            sB.Append("{\n");
            sB.Append("  evt = (evt) ? evt : event;\n");
            sB.Append("  \n");
            sB.Append("  if(window.event){\n");
            sB.Append("    evt.cancelBubble = true;\n");
            sB.Append("    evt.returnValue = false;\n");
            sB.Append("  }else{\n");
            sB.Append("     evt = (evt) ? evt : event;\n");
            sB.Append("     evt.stopPropagation();\n");
            sB.Append("     evt.preventDefault();\n");
            sB.Append("  }\n");
            sB.Append("  evt.cancelBubble = true;\n");
            sB.Append("}\n");
            sB.Append("\n");
            sB.Append("\n");
            sB.Append("this.getLocation = function(chObj) {\n");
            sB.Append("\n");
            sB.Append("\t\tvar dynaObj=chObj;\n");
            sB.Append("\t\tvar overallTop=0;\n");
            sB.Append("\t\tvar overallLeft=0;\n");
            sB.Append("\t\tif (is_ie)\n");
            sB.Append("\t\t{\n");
            sB.Append("\t\t  var objb = dynaObj.getBoundingClientRect();\n");
            sB.Append("\t\t  overallLeft += objb.left;\n");
            sB.Append("\t\t  overallTop += objb.top;\n");
            sB.Append("\t\t}\n");
            sB.Append("\t\telse\n");
            sB.Append("\t\t  while (dynaObj.offsetParent){\n");
            sB.Append("\t\t\t  overallTop=overallTop+dynaObj.offsetTop;\n");
            sB.Append("\t\t\t  overallLeft=overallLeft+dynaObj.offsetLeft; \n");
            sB.Append("\t\t    dynaObj=dynaObj.offsetParent;\n");
            sB.Append("\t\t  }\n");
            sB.Append("\t\tif ((chObj.style.position!=\"absolute\") || (chObj.style.posTop==null)\n");
            sB.Append("\t\t\t\t\t\t\t\t\t\t\t\t || (chObj.style.top==\"\")){\n");
            sB.Append("\t\t  chObj.style.posTop=overallTop;\n");
            sB.Append("\t\t}\n");
            sB.Append("\t\tif ((chObj.style.position!=\"absolute\") || (chObj.style.posLeft==null)\n");
            sB.Append("\t\t\t\t\t\t\t\t\t\t\t\t || (chObj.style.left==\"\")){\n");
            sB.Append("\t\t  chObj.style.posLeft=overallLeft;\n");
            sB.Append("\t\t}\n");
            sB.Append("}\n");
            sB.Append("\n");
            sB.Append("this.registerChart = function(toolType,chObjStr) {\n");
            sB.Append("\n");
            sB.Append("  action=toolType;\n");
            sB.Append("  if (action==ActionType.Zooming){\n");
            sB.Append("    if (document.getElementById(chObjStr)!=null){\n");
            sB.Append("      var _chObj = document.getElementById(chObjStr);\n");
            sB.Append("      _chObj.oncontextmenu = blockEvent;\n");
            sB.Append("      _chObj.ondrag = blockEvent;\n");
            sB.Append("      _chObj.ondragstart = blockEvent;\n");
            sB.Append("      _chObj.onmousedown = function (event) { activateContent(chObjStr,true,event); };\n");
            sB.Append("      _chObj.onmouseup = function (event) { activateContent(chObjStr,false,event); };\n");
            sB.Append("    }\n");
            sB.Append("  }  \n");
            sB.Append(" \n");
            sB.Append("  var charts = chartList.split(';');\n");
            sB.Append("  var x;\n");
            sB.Append("  if (charts.length>0){\n");
            sB.Append("\t  if (charts[0].length>0)\n");
            sB.Append("\t  {\n");
            sB.Append("\t    for (var i=0;i<charts.length;i++){\n");
            sB.Append("\t\t    if (charts[i].indexOf(chObjStr)!=-1){\n");
            sB.Append("\t\t      return;\n");
            sB.Append("\t\t    }\n");
            sB.Append("\t    }\n");
            sB.Append("\t  }\n");
            sB.Append("  }\n");
            sB.Append("  chartList = (chartList.length>0) ? chartList+';' + chObjStr : chObjStr;\n");
            sB.Append("}\n");
            sB.Append("\n");
        }

        private string GetClickEventScriptBlock()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script language=\"javascript\">\n\n");
            builder.Append("\n");
            string postBackEventReference = this.Page.ClientScript.GetPostBackEventReference(this, "");
            postBackEventReference = postBackEventReference.Substring(0, postBackEventReference.IndexOf('('));
            builder.Append("\t\tfunction activateContent(chart,empty,evt)\n");
            builder.Append("\t\t{\n");
            builder.Append("\t\t  evt = (evt) ? evt : event;\n");
            builder.Append("\t\t  var oVDiv=document.getElementById(chart);\n");
            builder.Append("\t\t  if ((oVDiv.style.position!=\"absolute\") || (oVDiv.style.posTop==null)\n");
            builder.Append("\t\t                                           || (oVDiv.style.top==\"\")\n");
            builder.Append("\t\t                                           || (oVDiv.style.posLeft==null)\n");
            builder.Append("\t\t                                           || (oVDiv.style.left==\"\"))\n");
            builder.Append("\t\t  {\n");
            builder.Append("\t\t    getLocation(oVDiv);\n");
            builder.Append("\t\t  }\n");
            builder.Append("\n");
            builder.Append("\t\t  var eClientX;\n");
            builder.Append("\t\t  var eClientY;\n");
            builder.Append("\t\t  var xPos;\n");
            builder.Append("\t\t  var yPos;\n");
            builder.Append("\t\t  var scrollOffsetX;\n");
            builder.Append("\t\t  var scrollOffsetY;\n");
            builder.Append("\t\t    eClientX=evt.clientX;\n");
            builder.Append("\t\t    eClientY=evt.clientY;\n");
            builder.Append("\t\t  if (is_ie) {\n");
            builder.Append("\t\t    scrollOffsetX=iebody.scrollLeft;\n");
            builder.Append("\t\t    scrollOffsetY=iebody.scrollTop;\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  else {\n");
            builder.Append("\t\t    scrollOffsetX=window.pageXOffset;\n");
            builder.Append("\t\t    scrollOffsetY=window.pageYOffset;\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  xPos=eClientX+scrollOffsetX;\n");
            builder.Append("\t\t  yPos=eClientY+scrollOffsetY;\n");
            builder.Append("\t\t  " + postBackEventReference + "(chart,'chart$'+chart+';xPos$'\n");
            builder.Append("\t                                         +(xPos-oVDiv.style.posLeft)+';yPos$'\n");
            builder.Append("\t                                         +(yPos-oVDiv.style.posTop));\n");
            builder.Append("\t\t}\n");
            builder.Append("\n");
            builder.Append("\n");
            builder.Append("</script>\n");
            return builder.ToString();
        }

        private string getHotspotMap(IHotspot s, ref bool useHelperScript, ref bool mapScript, ref string mapElements, ref int hotspotCanvasIndex, ref bool mapSet, Rectangle bounds)
        {
            string str = s.GenerateMap(bounds);
            if (str != "")
            {
                mapElements = mapElements + " " + s.MapElements;
                mapSet = true;
                if ((s is SeriesHotspot) && ((s as SeriesHotspot).MapAction == MapAction.Script))
                {
                    hotspotCanvasIndex = (s as SeriesHotspot).HotspotCanvasIndex;
                    mapScript = true;
                    if ((s as SeriesHotspot).HelperScript != HotspotHelperScripts.None)
                    {
                        useHelperScript = true;
                    }
                }
            }
            return str;
        }

        private string GetMapScriptBlock(int hotspotCanvasIndex)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\n<style>.Annotation { border-top: 1 solid buttonhighlight; border-left: 1 solid buttonhighlight; border-right: 1 solid buttonshadow; border-bottom: 1 solid buttonshadow; BACKGROUND-COLOR: FFFFE6;}</style>\n");
            builder.Append("<div class=\"Annotation\" id=\"Annotation\" style=\"Z-INDEX: " + hotspotCanvasIndex.ToString() + "; VISIBILITY: hidden; POSITION: absolute\"></div>\n");
            builder.Append("<script language=\"javascript\">\n");
            builder.Append("var Annotation;\n");
            builder.Append("var AnnotationVisible=false;\n");
            builder.Append("var timer;\n");
            builder.Append("var is_ie = navigator.appName == 'Microsoft Internet Explorer';\n");
            builder.Append("var iebody=(document.compatMode && document.compatMode != \"BackCompat\")? document.documentElement : document.body;\n");
            builder.Append("\n");
            builder.Append("document.onmousemove = MoveAnnotation;\n");
            builder.Append("\n");
            builder.Append("\n");
            builder.Append("function MoveAnnotation(e)\n");
            builder.Append("{\n");
            builder.Append("\tif(AnnotationVisible)\n");
            builder.Append("\t{\n");
            builder.Append("\t\tAnnotation=document.getElementById('Annotation');\n");
            builder.Append("\t\tvar Annotation_width = Annotation.offsetWidth;\n");
            builder.Append("\t\tvar Annotation_height= Annotation.offsetHeight;\n");
            builder.Append("\t\tAnnotation.style.visibility = \"visible\";\n");
            builder.Append("\n");
            builder.Append("\t\t  var eClientX;\n");
            builder.Append("\t\t  var eClientY;\n");
            builder.Append("\t\t  var scrollOffsetX;\n");
            builder.Append("\t\t  var scrollOffsetY;\n");
            builder.Append("\t\t  if (is_ie) {\n");
            builder.Append("\t\t    eClientX=event.clientX;\n");
            builder.Append("\t\t    eClientY=event.clientY;\n");
            builder.Append("\t\t    scrollOffsetX=iebody.scrollLeft;\n");
            builder.Append("\t\t    scrollOffsetY=iebody.scrollTop;\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  else {\n");
            builder.Append("\t\t    eClientX=e.clientX;\n");
            builder.Append("\t\t    eClientY=e.clientY;\n");
            builder.Append("\t\t    scrollOffsetX=window.pageXOffset;\n");
            builder.Append("\t\t    scrollOffsetY=window.pageYOffset;\n");
            builder.Append("\t\t  }\n");
            builder.Append("\n");
            builder.Append("\t\tAnnotation.style.left = eClientX<(document.body.clientWidth-Annotation_width) ? eClientX+10 : document.body.clientWidth-Annotation_width+'px';\n");
            builder.Append("\t\tAnnotation.style.top = eClientY<(document.body.clientHeight-Annotation_height) ? eClientY+10 : eClientY-Annotation_height-5+'px';\n");
            builder.Append("\t}\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("function ShowAnnotation(text)\n");
            builder.Append("{\n");
            builder.Append("\tAnnotation=document.getElementById('Annotation');\n");
            builder.Append("\tAnnotation.innerHTML = text;\n");
            builder.Append("\tAnnotationVisible = (text != \"\")? true : false;\n");
            builder.Append("\tif(text != \"\")\n");
            builder.Append("\t{\n");
            builder.Append("\t\tAnnotation_height=Annotation.offsetHeight;\n");
            builder.Append("\t\tFadeIn('Annotation',0); \n");
            builder.Append("\t} \n");
            builder.Append("\telse \n");
            builder.Append("\t{\n");
            builder.Append("\t\tclearTimeout(timer);\n");
            builder.Append("\t\tAnnotation.style.visibility=\"hidden\";\n");
            builder.Append("\t}\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("function FadeIn(secID,opacity)\n");
            builder.Append("{\n");
            builder.Append("\tobjID = document.getElementById(secID);\n");
            builder.Append("\t\n");
            builder.Append("\tif (opacity <= 100)\n");
            builder.Append("\t{\n");
            builder.Append("\t\tSetOpacity(objID, opacity);\n");
            builder.Append("\t\topacity += 10;\n");
            builder.Append("\t\ttimer=window.setTimeout(\"FadeIn('\"+objID.id+\"',\"+opacity+\")\", 30);\n");
            builder.Append("\t}\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("function SetOpacity(secID, opacity)\n");
            builder.Append("{\n");
            builder.Append("\topacity = (opacity == 100)?99.999:opacity;\n");
            builder.Append("\t\n");
            builder.Append("\tsecID.style.opacity = opacity;\n");
            builder.Append("\tsecID.style.filter = \"alpha(opacity:\"+opacity+\")\";\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("</script>\n");
            return builder.ToString();
        }

        [SecurityPermission(SecurityAction.Demand), SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.Chart.Export.Template.Serialize(info, context);
        }

        private string getScrollingHotspots(IHotspot s, ref bool useHelperScript, ref bool mapScript, ref string mapElements, ref int hotspotCanvasIndex, ref bool mapSet, ScrollTool scrollTool)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < scrollTool.Attributes.frames.Count; i++)
            {
                builder.Append("<MAP name=\"MAP" + this.ClientID.ToString() + i.ToString() + "\">");
                builder.Append(this.getHotspotMap(s, ref useHelperScript, ref mapScript, ref mapElements, ref hotspotCanvasIndex, ref mapSet, (Rectangle) scrollTool.Attributes.frames[i]));
                builder.Append("</MAP>");
            }
            return builder.ToString();
        }

        private void GetShareFolder()
        {
            this.shareURL = Utils.VirtualShare();
            this.designPath = Utils.ShareFolder();
        }

        private string GetUnitType(Unit unit)
        {
            UnitType type = unit.Type;
            if (type != UnitType.Pixel)
            {
                if (type == UnitType.Percentage)
                {
                    return "%";
                }
                return "px";
            }
            return "px";
        }

        private string GetZoomActiveContentScriptBlock()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script language=\"javascript\">\n\n");
            builder.Append("\t\t\t\t \n");
            builder.Append("var ActionType = { Zooming:1, Scrolling:2 };\n");
            builder.Append("\n");
            builder.Append("var mouseDownX;\n");
            builder.Append("var mouseDownY;\n");
            builder.Append("var mouseUpX;\n");
            builder.Append("var mouseUpY;\n");
            builder.Append("var mouseDownSet=false;\n");
            builder.Append("var zooming=true;\n");
            builder.Append("var selectedChart;\n");
            builder.Append("var callingChart;\n");
            builder.Append("var is_ie = navigator.appName == 'Microsoft Internet Explorer';\n");
            builder.Append("var iebody=(document.compatMode && document.compatMode != \"BackCompat\")? document.documentElement : document.body;\n");
            builder.Append("\n");
            builder.Append("/*charts list*/\n");
            builder.Append("var chartList = \"\";\n");
            builder.Append("/*Chart array*/\n");
            builder.Append("var pageCharts = new Array();\n");
            builder.Append("var scrollImages;\n");
            builder.Append("var images = new Array();\n");
            builder.Append("var scrollRect; /*scroll region rect*/\n");
            builder.Append("var bAxisPos;\n");
            builder.Append("var scrollBars=new Array();\n");
            builder.Append("var action=ActionType.Zooming;\n");
            builder.Append("\n");
            builder.Append("var zoomZone;\n");
            builder.Append("\n");
            builder.Append("/*****************\n");
            builder.Append(" INIT\n");
            builder.Append("*****************/\n");
            builder.Append("\n");
            builder.Append("this.getLocation = function(chObj) {\n");
            builder.Append("\n");
            builder.Append("\t\tvar dynaObj=chObj;\n");
            builder.Append("\t\tvar overallTop=dynaObj.offsetTop;\n");
            builder.Append("\t\tvar overallLeft=dynaObj.offsetLeft;\n");
            builder.Append("\t\twhile (dynaObj.offsetParent!=null){\n");
            builder.Append("\t\t  dynaObj=dynaObj.offsetParent;\n");
            builder.Append("\t\t\toverallTop=overallTop+dynaObj.offsetTop;\n");
            builder.Append("\t\t\toverallLeft=overallLeft+dynaObj.offsetLeft; \n");
            builder.Append("\t\t}\n");
            builder.Append("\t\tif ((chObj.style.position!=\"absolute\") || (chObj.style.posTop==null)\n");
            builder.Append("\t\t\t\t\t\t\t\t\t\t\t\t || (chObj.style.top==\"\")){\n");
            builder.Append("\t\t  chObj.style.posTop=overallTop;\n");
            builder.Append("\t\t}\n");
            builder.Append("\t\tif ((chObj.style.position!=\"absolute\") || (chObj.style.posLeft==null)\n");
            builder.Append("\t\t\t\t\t\t\t\t\t\t\t\t || (chObj.style.left==\"\")){\n");
            builder.Append("\t\t  chObj.style.posLeft=overallLeft;\n");
            builder.Append("\t\t}\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.registerScrollSettings = function(toolType,chObjStr,imgCount,\n");
            builder.Append("                                       chartWidth,startPos,fileRoot,\n");
            builder.Append("                                       rLeft,rTop,rWidth,rHeight,bAxPos,mAction){                                      \n");
            builder.Append("  registerChart(toolType,chObjStr);\n");
            builder.Append("  \n");
            builder.Append("  if (document.getElementById(chObjStr)!=null){\n");
            builder.Append("    var _chObj = document.getElementById(chObjStr);\n");
            builder.Append("    _chObj.innerWidth=chartWidth;\n");
            builder.Append("    _chObj.startPos = startPos;\n");
            builder.Append("    _chObj.mAction = mAction;\n");
            builder.Append("  }\n");
            builder.Append("  \n");
            builder.Append("  bAxisPos=bAxPos;\n");
            builder.Append("  \n");
            builder.Append("  if (toolType==2){\n");
            builder.Append("    action=ActionType.Scrolling;\n");
            builder.Append("    scrollRect=new Array(rLeft,rTop,rWidth,rHeight);\n");
            builder.Append("    preload(imgCount,fileRoot,scrollRect);\n");
            builder.Append("  }\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.registerChart = function(toolType,chObjStr) {\n");
            builder.Append("\n");
            builder.Append("  action=toolType;\n");
            builder.Append("  if (action==ActionType.Zooming){\n");
            builder.Append("    if (document.getElementById(chObjStr)!=null){\n");
            builder.Append("      var _chObj = document.getElementById(chObjStr);\n");
            builder.Append("      _chObj.oncontextmenu = blockEvent;\n");
            builder.Append("      _chObj.ondrag = blockEvent;\n");
            builder.Append("      _chObj.ondragstart = blockEvent;\n");
            builder.Append("      _chObj.onmousedown = function (event) { activateContent(chObjStr,true,event); };\n");
            builder.Append("      _chObj.onmouseup = function (event) { activateContent(chObjStr,false,event); };\n");
            builder.Append("    }\n");
            builder.Append("  }  \n");
            builder.Append(" \n");
            builder.Append("  var charts = chartList.split(';');\n");
            builder.Append("  var x;\n");
            builder.Append("  if (charts.length>0){\n");
            builder.Append("\t  if (charts[0].length>0)\n");
            builder.Append("\t  {\n");
            builder.Append("\t    for (var i=0;i<charts.length;i++){\n");
            builder.Append("\t\t    if (charts[i].indexOf(chObjStr)!=-1){\n");
            builder.Append("\t\t      return;\n");
            builder.Append("\t\t    }\n");
            builder.Append("\t    }\n");
            builder.Append("\t  }\n");
            builder.Append("  }\n");
            builder.Append("  chartList = (chartList.length>0) ? chartList+';' + chObjStr : chObjStr;\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.connectZoomLayer = function(chObjStr){\n");
            builder.Append("\n");
            builder.Append("  if ((document.getElementById(chObjStr)!=null) \n");
            builder.Append("      && (document.getElementById(chObjStr+'inlay')==null)){\n");
            builder.Append("    \n");
            builder.Append("\t  var _chObj=document.getElementById(chObjStr);\n");
            builder.Append("\t  \n");
            builder.Append("\t  getLocation(_chObj);\n");
            builder.Append("\n");
            builder.Append("\t  var _layerElement = document.createElement('DIV');\n");
            builder.Append("\t  var style = _layerElement.style; \n");
            builder.Append("  \t\n");
            builder.Append("\t  style.visibility = \"visible\"; \n");
            builder.Append("  \t\n");
            builder.Append("\t  if (action == ActionType.Zooming){\n");
            builder.Append("      style.height = '0px';\n");
            builder.Append("    }\n");
            builder.Append("    else if (action == ActionType.Scrolling){\n");
            builder.Append("      style.width = (scrollRect[2]-1)+'px';\n");
            builder.Append("      style.height = scrollRect[3]+'px';\n");
            builder.Append("      style.left = _chObj.style.posLeft + scrollRect[0] +\"px\"; \n");
            builder.Append("      style.top = _chObj.style.posTop + scrollRect[1] + \"px\";\n");
            builder.Append("    \t\n");
            builder.Append("      _layerElement.innerHTML = fillScrollArea(chObjStr);\n");
            builder.Append("\t  }\n");
            builder.Append("\t  \n");
            builder.Append("\t  _layerElement.id=chObjStr+'inlay';\n");
            builder.Append("\n");
            builder.Append("\t  style.overflow = 'hidden';\n");
            builder.Append("\t  style.zIndex = _chObj.style.zIndex + 10;      \n");
            builder.Append("\t  style.backgroundColor = 'gray'; /*background colour required*/\n");
            builder.Append("\t  style.visibility='visible';   \n");
            builder.Append("\t  style.filter='alpha(opacity=100)'; /*IE*/\n");
            builder.Append("\t  style.opacity=1.0;  /*Mozilla*/\n");
            builder.Append("\t  style.position = 'absolute';\n");
            builder.Append("\n");
            builder.Append("    if (action == ActionType.Scrolling){\n");
            builder.Append("\t    var scroller=makeScrollBar(_layerElement);\n");
            builder.Append("\t    scrollBars[scrollBars.length]=scroller;\n");
            builder.Append("      document.body.appendChild(scroller);\n");
            builder.Append("      \n");
            builder.Append("      _chObj.oncontextmenu = blockEvent;\n");
            builder.Append("      _chObj.ondrag = blockEvent;\n");
            builder.Append("      _chObj.onmousemove = function (event) { handleDragScrollStop(chObjStr,event); };\n");
            builder.Append("      _chObj.onmouseup = function (event) { handleDragScrollStop(chObjStr,event); };\n");
            builder.Append("    }\n");
            builder.Append("    return _layerElement;\n");
            builder.Append("  }\n");
            builder.Append("  else\n");
            builder.Append("  {\n");
            builder.Append("\t  return _layerElement;  \n");
            builder.Append("  }\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.getMap = function(id,i)\n");
            builder.Append("{\n");
            builder.Append("  return \"#MAP\"+id+\"\"+i;\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.fillScrollArea = function(id)\n");
            builder.Append("{\n");
            builder.Append("  var _chObj=document.getElementById(id);\n");
            builder.Append("  var imgStr = \"<div id='\"+id+\"inlayzone' onmouseup=\\\"handleDragScrollStop('\"\n");
            builder.Append("                          +_chObj.id+\"', event)\\\" onmousedown=\\\"handleDragScrollClick('\"\n");
            builder.Append("                          +_chObj.id+\"', event)\\\" onmousemove=\\\"handleDragIt('\"\n");
            builder.Append("                          +id+\"inlay',event)\\\"><table cellpadding=0 cellspacing=0><tr>\";\n");
            builder.Append("  \n");
            builder.Append("  for(i=0; i<images.length; i++)\n");
            builder.Append("    imgStr = imgStr+\"<td><IMG GALLERYIMG='no' USEMAP='\"\n");
            builder.Append("                          +getMap(id,i)+\"' parent='\"+id+\"inlayzone' class='inlayimg' id='\"\n");
            builder.Append("                          +id+\"img\"+i+\"' valign=top; Border=0></td>\";\n");
            builder.Append("\n");
            builder.Append("  imgStr = imgStr+\"</tr></table>\";\n");
            builder.Append("  imgStr = imgStr+\"</div>\";\n");
            builder.Append("  \n");
            builder.Append("  return imgStr;\n");
            builder.Append("}\n");
            builder.Append("\t\t\n");
            builder.Append("/*****************\n");
            builder.Append(" SCROLLBAR\n");
            builder.Append("*****************/\n");
            builder.Append("\n");
            builder.Append("var minEnd=0;\n");
            builder.Append("var maxEnd=1;\n");
            builder.Append("var vertScroll=0;\n");
            builder.Append("var horizScroll=1;\n");
            builder.Append("var scrollHeight=16;\n");
            builder.Append("var btnWidth=16;\n");
            builder.Append("var sliderWidth=32;\n");
            builder.Append("\n");
            builder.Append("this.makeScrollBar = function(parent)\n");
            builder.Append("{\n");
            builder.Append("  var scrollBack = document.createElement('DIV');\n");
            builder.Append("  var style=scrollBack.style;\n");
            builder.Append(" \n");
            builder.Append("  scrollBack.ownerContentID = parent.id;\n");
            builder.Append("  scrollBack.ownerWidth = parent.style.width;\n");
            builder.Append("  scrollBack.contentElem = parent;\n");
            builder.Append("  scrollBack.index=scrollBars.length;\n");
            builder.Append("  scrollBack.contentScrollWidth = scrollBack.contentElem.scrollWidth;\n");
            builder.Append("  \n");
            builder.Append("  scrollBack.className = 'runner';\n");
            builder.Append("  \n");
            builder.Append("  scrollBack.scrollWrapper = null;\n");
            builder.Append("  scrollBack.upButton = null;\n");
            builder.Append("  scrollBack.dnButton = null;\n");
            builder.Append("  scrollBack.slider = null;\n");
            builder.Append("  scrollBack.buttonLength = 0;\n");
            builder.Append("  scrollBack.sliderLength = 0;\n");
            builder.Append("  scrollBack.scrollWrapperLength = 0\n");
            builder.Append("  scrollBack.dragZone = {left:0, top:0, right:0, bottom:0};\n");
            builder.Append("  \n");
            builder.Append("  var top=(parseInt(parent.style.top)*1)+bAxisPos-scrollHeight+3;\n");
            builder.Append("  style.top=top+'px';\n");
            builder.Append("  style.left=parent.style.left;\n");
            builder.Append("  style.height=scrollHeight+'px';\n");
            builder.Append("  style.width=parent.style.width;\n");
            builder.Append("  \n");
            builder.Append("  scrollBack.innerHTML = '<div></div>';\n");
            builder.Append("\n");
            builder.Append("  style.zIndex = parent.style.zIndex + 10;      \n");
            builder.Append("  style.backgroundColor = 'gray';\n");
            builder.Append("  style.backgroundImage=\"url('\"+scrollImages[2]+\"')\";\n");
            builder.Append("  style.filter='alpha(opacity=100)'; /*IE*/\n");
            builder.Append("  style.opacity=1;  /*Mozilla*/\n");
            builder.Append("  style.position = 'absolute';\n");
            builder.Append("  \n");
            builder.Append("  scrollBack.id=parent.id+'scroller';\n");
            builder.Append("  \n");
            builder.Append("  scrollBack.appendChild(addScrollEnd(scrollBack,minEnd,horizScroll));\n");
            builder.Append("  scrollBack.appendChild(addScrollEnd(scrollBack,maxEnd,horizScroll));  \n");
            builder.Append("  scrollBack.appendChild(addSlider(scrollBack,horizScroll));\n");
            builder.Append("     \n");
            builder.Append("  scrollBack.onmousedown = handleScrollClick;\n");
            builder.Append("  scrollBack.onmouseup = handleScrollStop;\n");
            builder.Append("  scrollBack.onmousemove = dragIt;\n");
            builder.Append("  scrollBack.oncontextmenu = blockEvent;\n");
            builder.Append("  scrollBack.ondrag = blockEvent;\n");
            builder.Append("\n");
            builder.Append("  return scrollBack;\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.blockEvent = function(evt) {\n");
            builder.Append("  evt = (evt) ? evt : event;\n");
            builder.Append("  evt.cancelBubble = true;\n");
            builder.Append("  draggingEvent(evt);\n");
            builder.Append("  return false;\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.cancelEvent =function()\n");
            builder.Append("{\n");
            builder.Append("  window.event.returnValue = false;\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.draggingEvent = function(evt)\n");
            builder.Append("{\n");
            builder.Append("  evt = (evt) ? evt : event;\n");
            builder.Append("  \n");
            builder.Append("  if(window.event){\n");
            builder.Append("    evt.cancelBubble = true;\n");
            builder.Append("    evt.returnValue = false;\n");
            builder.Append("  }else{\n");
            builder.Append("     evt = (evt) ? evt : event;\n");
            builder.Append("     evt.stopPropagation();\n");
            builder.Append("     evt.preventDefault();\n");
            builder.Append("  }\n");
            builder.Append("  evt.cancelBubble = true;\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.addScrollEnd = function(parent,end,orientation)\n");
            builder.Append("{\n");
            builder.Append("  var endObj=document.createElement('DIV');\n");
            builder.Append("  var style=endObj.style;\n");
            builder.Append("  \n");
            builder.Append("  if (orientation==1) /*at the moment only horizontal supported*/\n");
            builder.Append("  {\n");
            builder.Append("\tswitch (end)\n");
            builder.Append("\t{\n");
            builder.Append("\t  case 0: \n");
            builder.Append("\t  {\n");
            builder.Append("\t\tendObj.id=parent.id+'minEnd';\n");
            builder.Append("\t\tendObj.className = 'decreaseVal';\n");
            builder.Append("\t\tstyle.backgroundImage=\"url('\"+scrollImages[0]+\"')\";\n");
            builder.Append("\t\tstyle.left=0+'px';\n");
            builder.Append("\t\tbreak;\n");
            builder.Append("\t  }\n");
            builder.Append("\t  case 1: \n");
            builder.Append("\t  {\n");
            builder.Append("\t\tendObj.id=parent.id+'maxEnd';\n");
            builder.Append("\t\tendObj.className = 'increaseVal';\n");
            builder.Append("\t\tstyle.backgroundImage=\"url('\"+scrollImages[1]+\"')\";\n");
            builder.Append("\t\tstyle.left=(parseInt(parent.style.width)-btnWidth)+'px';\n");
            builder.Append("\t\tbreak;\n");
            builder.Append("\t  }\n");
            builder.Append("\t}\n");
            builder.Append("    endObj.index=parent.index;\n");
            builder.Append("\tstyle.top=0+'px';\n");
            builder.Append("\tstyle.height=parent.style.height;\n");
            builder.Append("\tstyle.backgroundColor = 'gray';\n");
            builder.Append("\tstyle.opacity=1;\n");
            builder.Append("\tstyle.filter='alpha(opacity=100)'; /*IE*/\n");
            builder.Append("\tstyle.width=btnWidth+'px';\n");
            builder.Append("\tstyle.position = 'absolute';\n");
            builder.Append("  }\n");
            builder.Append("  \n");
            builder.Append("  return endObj;\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.addSlider = function(parent,orientation)\n");
            builder.Append("{\n");
            builder.Append("  var slider=document.createElement('DIV');\n");
            builder.Append("  if (orientation==1) /*at the moment only horizontal supported*/\n");
            builder.Append("  {\n");
            builder.Append("    slider.id=parent.id+'Slider';\n");
            builder.Append("    slider.className = 'sliderRegion';\n");
            builder.Append("    slider.index=parent.index;\n");
            builder.Append("    parent.slider=slider;\n");
            builder.Append("    slider.style.zIndex = parent.style.zIndex + 10;\n");
            builder.Append("    slider.style.backgroundImage=\"url('\"+scrollImages[3]+\"')\";\n");
            builder.Append("    var str=scrollImages[3]\n");
            builder.Append("    slider.style.left=btnWidth+'px';\n");
            builder.Append("    slider.style.top=0+'px';\n");
            builder.Append("    slider.style.height=parent.style.height;\n");
            builder.Append("    slider.style.backgroundColor = 'gray';\n");
            builder.Append("    slider.style.opacity=1;\n");
            builder.Append("    slider.style.filter='alpha(opacity=100)'; /*IE*/\n");
            builder.Append("    slider.style.width=sliderWidth+'px';\n");
            builder.Append("    slider.style.position = 'absolute';\n");
            builder.Append("    slider.style.cursor='pointer';\n");
            builder.Append("  }\n");
            builder.Append("  \n");
            builder.Append("  return slider;\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.preload = function (numImgs,imageName,scrollRect,chObjStr) \n");
            builder.Append("{\n");
            builder.Append("\t var i = 0;\n");
            builder.Append("\t images = new Array();\n");
            builder.Append("\t /*load name list*/\n");
            builder.Append("\t for(i=0; i<numImgs; i++) \n");
            builder.Append("\t\tloadChartImage(i,imageName);\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.loadChartImage = function (index, imageName)\n");
            builder.Append("{  \n");
            builder.Append("   images[index]=imageName+index;\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.loadScrollImages = function (scrollURLs)\n");
            builder.Append("{\n");
            builder.Append("  scrollImages = scrollURLs;\n");
            builder.Append("  /* start preloading*/\n");
            builder.Append("  for(i=0; i<scrollImages.length; i++) \n");
            builder.Append("\tloadImage(i,scrollImages[i]);\n");
            builder.Append("}\n");
            builder.Append("\t\n");
            builder.Append("this.loadImage = function (index, imageName)\n");
            builder.Append("{  \n");
            builder.Append("   /*create object*/\n");
            builder.Append("   imageObj = new Image();\n");
            builder.Append("   imageObj.src=imageName;\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.setScrollOffset = function (index,id)\n");
            builder.Append("{\n");
            builder.Append("  var _chObj=document.getElementById(id);\n");
            builder.Append("  var pos=1;\n");
            builder.Append("  if (_chObj.startPos>0)\n");
            builder.Append("    pos=(_chObj.startPos*1.0/100.0)*(_chObj.innerWidth*1.0);\n");
            builder.Append("  scrollEngaged = true;\n");
            builder.Append("  setTimeout(\"scrollBy(\" + index + \", \" + Math.round(pos) + \")\", 10);\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.initLayers = function ()\n");
            builder.Append("{\n");
            builder.Append("  if (action == ActionType.Scrolling)\n");
            builder.Append("  {\n");
            builder.Append("      loadScrollImages(scrollStrings);\n");
            builder.Append("      var x;\n");
            builder.Append("      if (chartList.indexOf(';')!=-1)\n");
            builder.Append("      { \n");
            builder.Append("\t    pageCharts = chartList.split(';');\n");
            builder.Append("\t    for (x in pageCharts)\n");
            builder.Append("\t    { \n");
            builder.Append("\t      if (pageCharts[x].length>0) \n");
            builder.Append("\t      {\n");
            builder.Append("\t\t    document.body.appendChild(connectZoomLayer(pageCharts[x]));\n");
            builder.Append("\t\t    setImages(pageCharts[x]);\n");
            builder.Append("\t\t    setScrollOffset(x,pageCharts[x]);\n");
            builder.Append("\t      }\n");
            builder.Append("\t    } \n");
            builder.Append("      } \n");
            builder.Append("      else\n");
            builder.Append("      { \n");
            builder.Append("\t    document.body.appendChild(connectZoomLayer(chartList));\n");
            builder.Append("\t    setImages(chartList);\n");
            builder.Append("\t    setScrollOffset(0,chartList);\n");
            builder.Append("      }\n");
            builder.Append("  }\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.setImages = function(id)\n");
            builder.Append("{\n");
            builder.Append("  if (document.images)\n");
            builder.Append("    {\n");
            builder.Append("      for(i=0; i<images.length; i++)\n");
            builder.Append("      {\n");
            builder.Append("        var img=document.getElementById(id+\"img\"+i);\n");
            builder.Append("        img.src= images[i];\n");
            builder.Append("      }\n");
            builder.Append("    }\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("/***************************\n");
            builder.Append("    EVENT HANDLING\n");
            builder.Append("****************************/\n");
            builder.Append("/* Global variables */\n");
            builder.Append("var scrollEngaged = false;\n");
            builder.Append("var scrollInterval;\n");
            builder.Append("var scrollBars = new Array();\n");
            builder.Append("\n");
            builder.Append("var diff;\n");
            builder.Append("var offsetX;\n");
            builder.Append("var offsetY;\n");
            builder.Append("var engaged=false;\n");
            builder.Append("var dragEngaged=false;\n");
            builder.Append("\n");
            builder.Append("this.dragIt = function(evt) {\n");
            builder.Append("  evt = (evt) ? evt : event;\n");
            builder.Append("  var target = (evt.target) ? evt.target : evt.srcElement;\n");
            builder.Append("  var x, y, width, height;\n");
            builder.Append("  if ((target.className==\"sliderRegion\") && (engaged==true)){\n");
            builder.Append("  \n");
            builder.Append("    if (evt.pageX){\n");
            builder.Append("      offsetX = evt.pageX;\n");
            builder.Append("      offsetY = evt.pageY;\n");
            builder.Append("    }else{\n");
            builder.Append("      offsetX = evt.clientX;\n");
            builder.Append("      offsetY = evt.clientY;\n");
            builder.Append("    }\n");
            builder.Append("    \n");
            builder.Append("    var sl=document.getElementById(target.id);\n");
            builder.Append("    getLocation(sl);\n");
            builder.Append("    barLeft=parseInt(sl.offsetParent.style.left)\n");
            builder.Append("    var pos=offsetX-barLeft-diff;\n");
            builder.Append("    var scroller = scrollBars[sl.index];\n");
            builder.Append("    var barLength = parseInt(scroller.style.width) - (btnWidth*2);\n");
            builder.Append("    if ((pos>btnWidth-1) && (pos<(barLength+1+btnWidth-sliderWidth)))\n");
            builder.Append("    {\n");
            builder.Append("      sl.style.left=pos+'px';\n");
            builder.Append("      var percentPos=(pos-btnWidth)/(barLength-sliderWidth-1);\n");
            builder.Append("      scroller.contentElem.scrollLeft=(scroller.contentElem.scrollWidth-(parseInt(scroller.style.width)))*percentPos;\n");
            builder.Append("    }\n");
            builder.Append("  }\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.getParentInlay=function(target){\n");
            builder.Append("  \n");
            builder.Append("  var node=target;\n");
            builder.Append("  while ((node.offsetParent!=null)){\n");
            builder.Append("    \n");
            builder.Append("    node=node.offsetParent;\n");
            builder.Append("    if (node.className=='inlayzone') return node;\n");
            builder.Append("  }\n");
            builder.Append("  return node;\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("var lastDragX=null;\n");
            builder.Append("var resetCount=0;\n");
            builder.Append("var debugMouseDown=false;\n");
            builder.Append("var debugNextMouseMove=false;\n");
            builder.Append("\n");
            builder.Append("this.handleDragIt = function(inlay,evt) {\n");
            builder.Append("\n");
            builder.Append("  evt = (evt) ? evt : event;\n");
            builder.Append("  var inlayElement=document.getElementById(inlay);\n");
            builder.Append("  var x, y, width, height;\n");
            builder.Append("  if (dragEngaged==true){\n");
            builder.Append("    \n");
            builder.Append("    if (evt.pageX){\n");
            builder.Append("      offsetX = evt.pageX;\n");
            builder.Append("      offsetY = evt.pageY;\n");
            builder.Append("    }else{\n");
            builder.Append("      offsetX = evt.clientX;\n");
            builder.Append("      offsetY = evt.clientY;\n");
            builder.Append("    }\n");
            builder.Append("    \n");
            builder.Append("    if (lastDragX!=null){\n");
            builder.Append("      inlayElement.scrollLeft=inlayElement.scrollWidth\n");
            builder.Append("                              -((inlayElement.scrollWidth-inlayElement.scrollLeft)+(offsetX-lastDragX));\n");
            builder.Append("    }  \n");
            builder.Append("    else\n");
            builder.Append("    {\n");
            builder.Append("      inlayElement.scrollLeft=inlayElement.scrollWidth-offsetX;\n");
            builder.Append("      resetCount++;\n");
            builder.Append("    }\n");
            builder.Append("    \n");
            builder.Append("    draggingEvent(evt);\n");
            builder.Append("  }\n");
            builder.Append("      \n");
            builder.Append("  lastDragX=offsetX;\n");
            builder.Append("  updateSlider(0);\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("/* onmouse up handler */\n");
            builder.Append("this.handleDragScrollStop = function(chObjStr,evt) {\n");
            builder.Append("    var inlayzone=document.getElementById(chObjStr+'inlayzone');\n");
            builder.Append("    inlayzone.style.cursor='default';\n");
            builder.Append("    scrollEngaged = false;\n");
            builder.Append("    dragEngaged = false;\n");
            builder.Append("    draggingEvent(evt);\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.handleDragScrollClick = function(chObjStr,evt) {\n");
            builder.Append("\n");
            builder.Append("    evt = (evt) ? evt : event;\n");
            builder.Append("    if (evt.pageX){\n");
            builder.Append("      offsetX = evt.pageX;\n");
            builder.Append("    }else{\n");
            builder.Append("      offsetX = evt.clientX;\n");
            builder.Append("    }\n");
            builder.Append("    var inlayzone=document.getElementById(chObjStr+'inlayzone');\n");
            builder.Append("    if (document.getElementById(chObjStr).mAction==1){\n");
            builder.Append("      inlayzone.style.cursor='pointer';\n");
            builder.Append("      dragEngaged=true;\n");
            builder.Append("    }\n");
            builder.Append("    \n");
            builder.Append("    debugMouseDown=true;\n");
            builder.Append("    lastDragX=offsetX;\n");
            builder.Append("    draggingEvent(evt);\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.engage = function(evt) {\n");
            builder.Append("  evt = (evt) ? evt : event;\n");
            builder.Append("  var target = (evt.target) ? evt.target : evt.srcElement;\n");
            builder.Append("  if (target.className==\"sliderRegion\") \n");
            builder.Append("  {\n");
            builder.Append("    engaged=true;\n");
            builder.Append("    if (evt.pageX) \n");
            builder.Append("    {\n");
            builder.Append("      offsetX = evt.pageX;\n");
            builder.Append("      offsetY = evt.pageY;\n");
            builder.Append("    } \n");
            builder.Append("    else \n");
            builder.Append("    {\n");
            builder.Append("      offsetX = evt.clientX;\n");
            builder.Append("      offsetY = evt.clientY;\n");
            builder.Append("    }\n");
            builder.Append("    var sl=document.getElementById(target.id);\n");
            builder.Append("    getLocation(sl);\n");
            builder.Append("    \n");
            builder.Append("    var sliderLeft=(parseInt(sl.offsetParent.style.left)*1.0)+(parseInt(sl.style.left)*1.0);\n");
            builder.Append("    diff=offsetX-sliderLeft;\n");
            builder.Append("   }\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("this.handleScrollStop = function() {\n");
            builder.Append("    scrollEngaged = false;\n");
            builder.Append("    engaged = false;\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("var ix=0;\n");
            builder.Append("\n");
            builder.Append("this.handleScrollClick = function(evt) {\n");
            builder.Append("    engage(evt);\n");
            builder.Append("    ix++;\n");
            builder.Append("    var fontSize, contentHeight;\n");
            builder.Append("    evt = (evt) ? evt : event;\n");
            builder.Append("    var target = (evt.target) ? evt.target : evt.srcElement;\n");
            builder.Append("    target = (target.nodeType == 3) ? target.parentNode : target;\n");
            builder.Append("    var index = target.index;\n");
            builder.Append("    fontSize=0;\n");
            builder.Append("    switch (target.className) {\n");
            builder.Append("        case \"increaseVal\" :\n");
            builder.Append("            scrollEngaged = true;\n");
            builder.Append("            scrollInterval=setInterval(\"scrollBy(\" + index + \", 1)\", 5);\n");
            builder.Append("            return false;\n");
            builder.Append("            break;\n");
            builder.Append("        case \"decreaseVal\" :\n");
            builder.Append("            scrollEngaged = true;\n");
            builder.Append("            scrollInterval=setInterval(\"scrollBy(\" + index + \", -1)\", 5);\n");
            builder.Append("            return false;\n");
            builder.Append("            break;\n");
            builder.Append("        case \"sliderRegion\" :\n");
            builder.Append("            break;\n");
            builder.Append("        case \"runner\" :\n");
            builder.Append("            scrollEngaged = true;\n");
            builder.Append("            var evtX = (evt.offsetX) ? evt.offsetX : ((evt.layerX) ? evt.layerX : -1);\n");
            builder.Append("            if (evtX >= 0) {\n");
            builder.Append("                var pageSize = parseInt(scrollBars[index].ownerWidth)*-1;\n");
            builder.Append("                var sliderElemStyle = scrollBars[index].slider.style;\n");
            builder.Append("                if (evtX > (parseInt(sliderElemStyle.left) + \n");
            builder.Append("                    scrollBars[index].sliderLength)) {\n");
            builder.Append("                    pageSize = -pageSize;\n");
            builder.Append("                }\n");
            builder.Append("                scrollInterval = setInterval(\"scrollBy(\" + index + \", \" + pageSize + \")\", 100);\n");
            builder.Append("                evt.cancelBubble = true;\n");
            builder.Append("                return false;\n");
            builder.Append("            }\n");
            builder.Append("    }\n");
            builder.Append("    return false;\n");
            builder.Append("}\n");
            builder.Append("var ctr=0;\n");
            builder.Append("this.scrollBy = function(index, val) {\n");
            builder.Append("    var scroller = scrollBars[index];\n");
            builder.Append("    var barLength = parseInt(scroller.style.width) - (btnWidth*2);\n");
            builder.Append("    \n");
            builder.Append("    inlayWidth=parseInt(scroller.style.width);\n");
            builder.Append("    \n");
            builder.Append("    if (scrollEngaged)\n");
            builder.Append("    {\n");
            builder.Append("      if (val>0)\n");
            builder.Append("      {\n");
            builder.Append("        if ((scroller.contentElem.scrollWidth-val)>(scroller.contentElem.scrollLeft+inlayWidth))\n");
            builder.Append("        {\n");
            builder.Append("          scroller.contentElem.scrollLeft=scroller.contentElem.scrollLeft+val;\n");
            builder.Append("          ctr++;\n");
            builder.Append("        }\n");
            builder.Append("        else  \n");
            builder.Append("        {\n");
            builder.Append("          scroller.contentElem.scrollLeft=scroller.contentElem.scrollWidth-inlayWidth;\n");
            builder.Append("          clearInterval(scrollInterval);\n");
            builder.Append("        }\n");
            builder.Append("      }\n");
            builder.Append("      else\n");
            builder.Append("        scroller.contentElem.scrollLeft=scroller.contentElem.scrollLeft+val;\n");
            builder.Append("      \n");
            builder.Append("      updateSlider(index);\n");
            builder.Append("    }\n");
            builder.Append("    else\n");
            builder.Append("      clearInterval(scrollInterval);\n");
            builder.Append("}\n");
            builder.Append("\n");
            builder.Append("\n");
            builder.Append("/* Position slider after scrolling by arrow/page region */\n");
            builder.Append("function updateSlider(index) {\n");
            builder.Append("    var scroller = scrollBars[index];\n");
            builder.Append("    var barLength = parseInt(scroller.style.width) - (btnWidth*2);\n");
            builder.Append("    var inlayZoneWidth = scroller.contentElem.scrollWidth;\n");
            builder.Append("    var inlayZoneDisp = scroller.contentElem.scrollLeft+(parseInt(scroller.style.width)*1)-(btnWidth*2);\n");
            builder.Append("    var percentDisp = scroller.contentElem.scrollLeft/(scroller.contentElem.scrollWidth-(parseInt(scroller.style.width)));\n");
            builder.Append("    if (percentDisp>1) percentDisp=1;\n");
            builder.Append("    \n");
            builder.Append("    if (scroller.contentElem.scrollLeft==0)\n");
            builder.Append("      scroller.slider.style.left=btnWidth+'px';\n");
            builder.Append("    else if (scroller.contentElem.scrollLeft>=(scroller.contentElem.scrollWidth-parseInt(scroller.style.width)))\n");
            builder.Append("    {\n");
            builder.Append("      scroller.slider.style.left  = parseInt(scroller.style.width)-(btnWidth+sliderWidth) + 'px';\n");
            builder.Append("    }\n");
            builder.Append("    else\n");
            builder.Append("    {\n");
            builder.Append("      scroller.slider.style.left  = (btnWidth+Math.round(percentDisp*(barLength-sliderWidth))) + 'px';\n");
            builder.Append("    }  \n");
            builder.Append("}\n");
            builder.Append("/*********************/\n");
            builder.Append("\n");
            builder.Append("\t\t</script>\n");
            return builder.ToString();
        }

        private string GetZoomScriptBlock(bool noClickPostback, bool zoomDragZoom, int zoomCanvasIndex, int zoomFillTransp, string zoomPenColor, string customVariables)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script language=\"javascript\">\n\n");
            builder.Append("\t\t\t\t \n");
            if (zoomDragZoom)
            {
                builder.Append("\t\tvar lastChart;\n");
                builder.Append("\t\tfunction activateContent(chart,mSet, e)\n");
                builder.Append("\t\t{\n");
                builder.Append("\t\t\te = (e) ? e : event;\n");
                builder.Append("\t\t  (chart=='') ? (chart=lastChart) : (lastChart=chart);\n");
            }
            else
            {
                builder.Append("\t\tfunction activateContent(chart,e)\n");
                builder.Append("\t\t{\n");
            }
            builder.Append("\t\t  var oVDiv=document.getElementById(chart);\n");
            builder.Append("\t\t  if ((oVDiv.style.position!=\"absolute\") || (oVDiv.style.posTop==null)\n");
            builder.Append("\t\t                                           || (oVDiv.style.top==\"\")\n");
            builder.Append("\t\t                                           || (oVDiv.style.posLeft==null)\n");
            builder.Append("\t\t                                           || (oVDiv.style.left==\"\"))\n");
            builder.Append("\t\t  {\n");
            builder.Append("\t\t    getLocation(oVDiv);\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  \n");
            builder.Append("\t\t  var eClientX;\n");
            builder.Append("\t\t  var eClientY;\n");
            builder.Append("\t\t  var scrollOffsetX;\n");
            builder.Append("\t\t  var scrollOffsetY;\n");
            builder.Append("\t\t  if (is_ie) {\n");
            builder.Append("\t\t    event.cancelBubble = true;\n");
            builder.Append("\t\t    eClientX=event.clientX;\n");
            builder.Append("\t\t    eClientY=event.clientY;\n");
            builder.Append("\t\t    scrollOffsetX=iebody.scrollLeft;\n");
            builder.Append("\t\t    scrollOffsetY=iebody.scrollTop;\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  else {\n");
            builder.Append("\t\t    e.cancelBubble = true;\n");
            builder.Append("\t\t    eClientX=e.clientX;\n");
            builder.Append("\t\t    eClientY=e.clientY;\n");
            builder.Append("\t\t    scrollOffsetX=window.pageXOffset;\n");
            builder.Append("\t\t    scrollOffsetY=window.pageYOffset;\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  \n");
            if (zoomDragZoom)
            {
                builder.Append("\t\t  if (mSet==true)\n");
            }
            else
            {
                builder.Append("\t\t  if (mouseDownSet==false)\n");
            }
            builder.Append("\t\t  {\n");
            builder.Append("\t\t    selectedChart=oVDiv;\n");
            builder.Append("\t\t    callingChart=chart;\n");
            builder.Append("\t\t    mouseDownX=eClientX+scrollOffsetX;\n");
            builder.Append("\t\t    mouseDownY=eClientY+scrollOffsetY;\n");
            builder.Append("\t\t    mouseDownSet=true;\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  else\n");
            builder.Append("\t\t  {\n");
            builder.Append("\t\t    if ((selectedChart==oVDiv) && (mouseDownSet==true))\n");
            builder.Append("\t\t    {\n");
            if (zoomDragZoom)
            {
                builder.Append("\t\t            mouseUpX=eClientX+scrollOffsetX;\n");
                builder.Append("\t\t            mouseUpY=eClientY+scrollOffsetY;\n");
                builder.Append("\t\t\t\t\tif ((mouseUpX<mouseDownX) || (mouseUpY<mouseDownY)) {\n");
                builder.Append("\t\t\t\t\t  zooming=false;}\n");
                builder.Append("\t\t\t\t\t  else {\n");
                builder.Append("\t\t\t\t\t  zooming=true;}\n");
            }
            builder.Append("\t\t\t\t\tmouseDownSet=false;\n");
            builder.Append("\t\t\t\t\t\n");
            string postBackEventReference = this.Page.ClientScript.GetPostBackEventReference(this, "");
            postBackEventReference = postBackEventReference.Substring(0, postBackEventReference.IndexOf('('));
            builder.Append("\t\t\t\t\tif (zooming==false)\n");
            builder.Append("\t\t\t\t\t{\n");
            builder.Append("\t\t  \t\t  " + postBackEventReference + "(chart,'chart$'+chart+';zoom$0');\n");
            builder.Append("\t\t\t\t\t}\n");
            builder.Append("\t\t\t\t\telse\n");
            builder.Append("\t\t\t\t\t{\n");
            builder.Append("\t\t\t\t\t  if (((mouseUpY-mouseDownY)<2) || ((mouseUpX-mouseDownX)<2))\n");
            builder.Append("\t\t\t\t\t  {\n");
            if (noClickPostback)
            {
                builder.Append("\t\t          //no postback of clicks set. \n");
            }
            else
            {
                builder.Append("\t\t          " + postBackEventReference + "(chart,'chart$'+chart+';xPos$'\n");
                builder.Append("\t                                         +(mouseUpX-oVDiv.style.posLeft)+';yPos$'\n");
                builder.Append("\t                                         +(mouseUpY-oVDiv.style.posTop));\n");
            }
            builder.Append("\t\t\t\t\t  }\n");
            builder.Append("\t\t\t\t\t  else\n");
            builder.Append("\t\t\t\t\t  {\n");
            builder.Append("\t\t\t\t\t  var target = (e.target) ? e.target : e.srcElement;\n");
            builder.Append("\t\t\t\t\t\t  " + postBackEventReference + "(chart,'chart$'+chart+';zoom$1;x0$'\n");
            builder.Append("\t                                               +(mouseDownX-oVDiv.style.posLeft)+';y0$'\n");
            builder.Append("\t                                               +(mouseDownY-oVDiv.style.posTop)+';x1$'\n");
            builder.Append("\t                                               +(mouseUpX-oVDiv.style.posLeft)+';y1$'\n");
            builder.Append("\t                                               +(mouseUpY-oVDiv.style.posTop));\n");
            builder.Append("\t\t\t\t\t  }\n");
            builder.Append("\t\t\t\t\t}\n");
            builder.Append("\t\t      document.getElementById('zoomdiv').style.visibility='hidden';\n");
            builder.Append("\t\t    }\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  draggingEvent(e);\n");
            builder.Append("\t\t}\n");
            builder.Append("\t\t\n");
            builder.Append("\t\tfunction zoomRect(chart,e)\n");
            builder.Append("\t\t{\n");
            builder.Append("\t\t  var oVDiv=document.getElementById(chart);\n");
            builder.Append("\t\t  \n");
            builder.Append("\n");
            builder.Append("\t\t  var eClientX;\n");
            builder.Append("\t\t  var eClientY;\n");
            builder.Append("\t\t  var scrollOffsetX;\n");
            builder.Append("\t\t  var scrollOffsetY;\n");
            builder.Append("\t\t  if (is_ie) {\n");
            builder.Append("\t\t    event.cancelBubble = true;\n");
            builder.Append("\t\t    eClientX=event.clientX;\n");
            builder.Append("\t\t    eClientY=event.clientY; event.cancelBubble = true;\n");
            builder.Append("\t\t    scrollOffsetX=iebody.scrollLeft;\n");
            builder.Append("\t\t    scrollOffsetY=iebody.scrollTop;\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  else {\n");
            builder.Append("\t\t    eClientX=e.clientX;\n");
            builder.Append("\t\t    eClientY=e.clientY;\n");
            builder.Append("\t\t    scrollOffsetX=window.pageXOffset;\n");
            builder.Append("\t\t    scrollOffsetY=window.pageYOffset;\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  \n");
            builder.Append("\t\t  if (mouseDownSet==true)\n");
            builder.Append("\t\t  {\n");
            builder.Append("\t\t    mouseUpX=eClientX+scrollOffsetX;\n");
            builder.Append("\t\t    mouseUpY=eClientY+scrollOffsetY;\n");
            builder.Append("\t\t  \n");
            builder.Append("\t\t\t\tif (((mouseUpX!=mouseDownX) || (mouseUpY!=mouseDownY)) && selectedChart==oVDiv)\n");
            builder.Append("\t\t\t\t{\n");
            builder.Append("\t\t\t\t\tif (mouseDownSet==true)\n");
            builder.Append("\t\t\t\t\t{\n");
            builder.Append("\t\t\t\t\t\tvar x;\n");
            builder.Append("\t\t\t\t\t\tvar y;\n");
            builder.Append("\t\t\t\t\t\tvar zObj=document.getElementById(\"zoomdiv\");\n");
            builder.Append("\t\t\t\t\t\tif (zObj!=null)\n");
            builder.Append("\t\t\t\t\t\t\t\t\tzObj.style.visibility=\"hidden\"; \n");
            builder.Append("\t\t\t\t\t\t\n");
            builder.Append("\t\t\t\t\t\tzooming=true;\n");
            builder.Append("\t\t\t\t\t\t\n");
            builder.Append("\t\t\t\t\t\tif (mouseUpX<mouseDownX)\n");
            builder.Append("\t\t\t\t\t\t{\n");
            builder.Append("\t\t\t\t\t\t  zooming=false;\n");
            builder.Append("\t\t\t\t\t\t  x=mouseUpX;\n");
            builder.Append("\t\t\t\t\t\t  width=mouseDownX-x;\n");
            builder.Append("\t\t\t\t\t\t}\n");
            builder.Append("\t\t\t\t\t\telse\n");
            builder.Append("\t\t\t\t\t\t{\n");
            builder.Append("\t\t\t\t\t\t  x=mouseDownX;\n");
            builder.Append("\t\t\t\t\t\t  width=mouseUpX-x;\n");
            builder.Append("\t\t\t\t\t\t}\n");
            builder.Append("\t\t\t\t\t\tif (mouseUpY<mouseDownY)\n");
            builder.Append("\t\t\t\t\t\t{\n");
            builder.Append("\t\t\t\t\t\t  zooming=false;\n");
            builder.Append("\t\t\t\t\t\t  y=mouseUpY;\n");
            builder.Append("\t\t\t\t\t\t  height=mouseDownY-y;\n");
            builder.Append("\t\t\t\t\t\t}\n");
            builder.Append("\t\t\t\t\t\telse\n");
            builder.Append("\t\t\t\t\t\t{\n");
            builder.Append("\t\t\t\t\t\t  y=mouseDownY;\n");
            builder.Append("\t\t\t\t\t\t  height=mouseUpY-y;\n");
            builder.Append("\t\t\t\t\t\t}\n");
            builder.Append("\t\t\t\t\t\tmoveObj(width,height,x-1,y-1,'zoomdiv'); \n");
            builder.Append("\t\t\t\t\t}\n");
            builder.Append("\t\t\t\t}\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  draggingEvent(e);\n");
            builder.Append("\t\t}\n");
            builder.Append("\t\t\n");
            builder.Append("\t\tfunction resizeObj(object,e) \n");
            builder.Append("\t\t{ \n");
            builder.Append("\t\t  object.style.posLeft=parseInt(object.style.left);\n");
            builder.Append("\t\t  object.style.posTop=parseInt(object.style.top);\n");
            builder.Append("\t\t  object.style.posWidth=parseInt(object.style.width);\n");
            builder.Append("\t\t  object.style.posHeight=parseInt(object.style.height);\n");
            builder.Append("\t\t  var eClientX;\n");
            builder.Append("\t\t  var eClientY;\n");
            builder.Append("\t\t  var scrollOffsetX;\n");
            builder.Append("\t\t  var scrollOffsetY;\n");
            builder.Append("\t\t  if (is_ie) {\n");
            builder.Append("\t\t    event.cancelBubble = true;\n");
            builder.Append("\t\t    eClientX=event.clientX;\n");
            builder.Append("\t\t    eClientY=event.clientY;\n");
            builder.Append("\t\t    scrollOffsetX=iebody.scrollLeft;\n");
            builder.Append("\t\t    scrollOffsetY=iebody.scrollTop;\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  else {\n");
            builder.Append("\t\t    e.cancelBubble = true;\n");
            builder.Append("\t\t    eClientX=e.clientX;\n");
            builder.Append("\t\t    eClientY=e.clientY;\n");
            builder.Append("\t\t    scrollOffsetX=window.pageXOffset;\n");
            builder.Append("\t\t    scrollOffsetY=window.pageYOffset;\n");
            builder.Append("\t\t  }\n");
            builder.Append("\t\t  var style = object.style;\n");
            builder.Append("\t\t  if (((eClientX>style.posLeft) && (eClientY>style.posTop)) && \n");
            builder.Append("\t\t     (((eClientX-style.posLeft)<style.posWidth) || ((eClientY-style.posTop)<style.posHeight))) { \n");
            builder.Append("\t\t    style.width=(eClientX-(style.posLeft-scrollOffsetX))+'px';\n");
            builder.Append("\t\t    style.height=(eClientY-(style.posTop-scrollOffsetY))+'px';\n");
            builder.Append("\t\t  } \n");
            builder.Append("\t\t} \n");
            builder.Append("\t\tfunction moveObj(w,h,x,y,object) \n");
            builder.Append("\t\t{ \n");
            builder.Append("\t\t  var obj = document.getElementById(object); \n");
            builder.Append("\t\t  var style = obj.style; \n");
            builder.Append("\t\t  style.visibility=\"visible\"; \n");
            builder.Append("\t\t  style.width = w+\"px\"; \n");
            builder.Append("\t\t  style.height = h+\"px\"; \n");
            builder.Append("\t\t  style.left = x+\"px\"; \n");
            builder.Append("\t\t  style.top = y+\"px\"; \n");
            builder.Append("\t\t} \n");
            builder.Append("\t\t\n");
            builder.Append("\t\t\n");
            builder.Append("\t\tvar scrollStrings=new Array('" + this.Page.ClientScript.GetWebResourceUrl(typeof(ToolResource), "Steema.TeeChart.Web.Images.scrollHzLt.gif") + "'\n");
            builder.Append("\t\t            ,'" + this.Page.ClientScript.GetWebResourceUrl(typeof(ToolResource), "Steema.TeeChart.Web.Images.scrollHzGt.gif") + "'\n");
            builder.Append("\t\t            ,'" + this.Page.ClientScript.GetWebResourceUrl(typeof(ToolResource), "Steema.TeeChart.Web.Images.scrollHzBack.gif") + "'\n");
            builder.Append("\t\t            ,'" + this.Page.ClientScript.GetWebResourceUrl(typeof(ToolResource), "Steema.TeeChart.Web.Images.scrollHzSlider.gif") + "');\n");
            builder.Append("\t\t\n");
            builder.Append("\t\twindow.onload = function()\n");
            builder.Append("\t\t{ \n");
            builder.Append("\t\t  initLayers();\n");
            builder.Append("\t\t  document.body.appendChild( rectangle(0,0,0,0,'" + zoomPenColor + "'));\n");
            builder.Append("\t\t} \n");
            builder.Append("\t\t\n");
            builder.Append("\t\tfunction rectangle(w,h,x,y,bCol) \n");
            builder.Append("\t\t{ \n");
            builder.Append("\t\t  var rect = document.createElement(\"div\"); \n");
            builder.Append("\t\t  var style = rect.style; \n");
            builder.Append("\t\t  style.zIndex = " + zoomCanvasIndex.ToString() + ";\n");
            if (zoomFillTransp == 0)
            {
                builder.Append("\t\t  style.backgroundColor = 'transparent'; \n");
                builder.Append("\t\t  style.opacity=1; \n");
                builder.Append("\t\t  style.filter='alpha(opacity=100)'; \n");
            }
            else
            {
                builder.Append("\t\t  style.backgroundColor = 'white'; \n");
                builder.Append("\t\t  style.opacity=" + ((zoomFillTransp == 100) ? "(1.0" : ("(0." + zoomFillTransp.ToString())) + "); \n");
                builder.Append("\t\t  style.filter='alpha(opacity=" + zoomFillTransp + ")'; \n");
            }
            builder.Append("\t\t  style.borderColor = bCol; \n");
            builder.Append("\t\t  style.visibility=\"hidden\"; \n");
            builder.Append("\t\t  style.position = \"absolute\"; \n");
            builder.Append("\t\t  style.borderStyle = \"solid\" \n");
            builder.Append("\t\t  style.borderWidth = '1px'; \n");
            builder.Append("\t\t  rect.setAttribute(\"id\", \"zoomdiv\");\n");
            if (zoomDragZoom)
            {
                builder.Append(" \t  rect.onmouseup = function (event) { activateContent(callingChart,false,event); } \n");
            }
            else
            {
                builder.Append("\t\t  rect.onmouseup = function (event) { activateContent(callingChart,event); } \n");
            }
            builder.Append("\t\t  rect.onmousemove = function (event) {  resizeObj(this,event); } \n");
            builder.Append("\t\t  return rect; \n");
            builder.Append("\t\t}\n");
            builder.Append("\n");
            builder.Append("\n");
            builder.Append("\t\t</script>\n");
            return builder.ToString();
        }

        public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            IEnumerator enumerator = postCollection.GetEnumerator();
            string str = "";
            if (postDataKey.LastIndexOf("$") != -1)
            {
                str = postDataKey.Substring(postDataKey.LastIndexOf("$") + 1, (postDataKey.Length - postDataKey.LastIndexOf("$")) - 1);
            }
            else
            {
                str = postDataKey;
            }
            while (str.IndexOf(":") != -1)
            {
                str = str.Substring(str.IndexOf(":") + 1);
            }
            while (enumerator.MoveNext() && (enumerator.Current != null))
            {
                string str2 = enumerator.Current.ToString();
                if (str2.LastIndexOf(".") != -1)
                {
                    str2 = str2.Substring(0, str2.LastIndexOf("."));
                }
                if ((postCollection["__EVENTTARGET"] == str) || (postCollection["__EVENTTARGET"] == postDataKey.Replace("$", "_")))
                {
                    if (postCollection["__EVENTARGUMENT"].Length > 0)
                    {
                        char[] separator = new char[] { ';' };
                        char[] chArray2 = new char[] { '$' };
                        foreach (string str3 in postCollection["__EVENTARGUMENT"].Split(separator))
                        {
                            if (str3.IndexOf("$") != -1)
                            {
                                if (str3.Split(chArray2)[0] == "xPos")
                                {
                                    this.clickedX = Convert.ToInt32(str3.Split(chArray2)[1]);
                                }
                                else if (str3.Split(chArray2)[0] == "yPos")
                                {
                                    this.clickedY = Convert.ToInt32(str3.Split(chArray2)[1]);
                                }
                            }
                        }
                        this.isObjectevent = true;
                    }
                    break;
                }
            }
            return false;
        }

        private void MakeShareFolder()
        {
            if (!Directory.Exists(Utils.ShareFolder() + @"\" + this.tmpFolder))
            {
                Directory.CreateDirectory(Utils.ShareFolder() + @"\" + this.tmpFolder);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.Chart.Height = Utils.Round(this.Height.Value);
            this.Chart.Width = Utils.Round(this.Width.Value);
            if (this.Config != null)
            {
                try
                {
                    this.Chart.Import.InternalLoadViewState(this.Chart.Import.DecodeBase64(this.Config), ref this.iChart);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.Page.RegisterRequiresPostBack(this);
            ImageExportFormat format = this.iChart.Export.Image.FromFormat(this.pictureFormat);
            format.Width = Utils.Round(this.Width.Value);
            format.Height = Utils.Round(this.Height.Value);
            if (this.isObjectevent)
            {
                if (format.Width <= 0)
                {
                    format.Width = 400;
                }
                if (format.Height <= 0)
                {
                    format.Height = 300;
                }
                this.Chart.Bitmap(format.Width, format.Height);
                MouseEventArgs args = new MouseEventArgs(MouseButtons.Left, 1, this.clickedX, this.clickedY, 0);
                this.Chart.DoMouseDown(false, args, Keys.None);
                this.isObjectevent = false;
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
        }

        public virtual void RaisePostDataChangedEvent()
        {
        }

        protected internal MemoryStream ReadFromFile(string chartID, string filePath, string extension)
        {
            string str = filePath;
            string url = str + chartID + "." + extension;
            XmlTextReader reader = new XmlTextReader(url) {
                WhitespaceHandling = WhitespaceHandling.None
            };
            string innerText = "";
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(url);
                foreach (XmlNode node in document.GetElementsByTagName(chartID))
                {
                    innerText = node.InnerText;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            reader.Close();
            document = null;
            return this.Chart.Import.DecodeBase64(innerText);
        }

        protected override void Render(HtmlTextWriter output)
        {
            int flag = 1;
            if ((this.Page.Request.Params["__EVENTARGUMENT"] != null) && (this.Page.Request.Params["__EVENTARGUMENT"].Length > 0))
            {
                flag = 0;
            }
            this.CreatePictureFile(output, flag);
        }

        protected void saveFileToImg(HttpContext Context, ImageExportFormat format, MemoryStream chartImg, string fileRoot, string fileName, bool scrolling)
        {
            if (this.tempChart == TempChartStyle.Session)
            {
                if (!scrolling)
                {
                    format.Save(chartImg);
                }
                Context.Session.Add(fileName, chartImg);
            }
            else if (this.tempChart == TempChartStyle.Httphandler)
            {
                if (!scrolling)
                {
                    format.Save(chartImg);
                }
                Context.Cache.Add(fileName, chartImg, null, DateTime.Now.AddSeconds(40.0), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
            else if (this.tempChart == TempChartStyle.Cache)
            {
                if (!scrolling)
                {
                    format.Save(chartImg);
                }
                this.Page.Cache.Add(fileName, chartImg, null, DateTime.Now.AddSeconds(40.0), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
            else
            {
                this.MakeShareFolder();
                fileName = fileRoot + "." + format.FileExtension;
                string str = this.designPath + @"\" + this.tmpFolder + @"\" + fileName;
                format.Save(str);
            }
        }

        void IChart.CheckBackground(object sender, MouseEventArgs e)
        {
            if (this.ClickBackground != null)
            {
                this.iChart.CancelMouse = true;
                ImageClickEventArgs args = new ImageClickEventArgs(e.X, e.Y);
                this.ClickBackground(sender, args);
                this.iChart.IClicked = this.iChart.CancelMouse;
            }
        }

        bool IChart.CheckClickSeries()
        {
            return (this.ClickSeries != null);
        }

        bool IChart.CheckGetAxisLabelAssigned()
        {
            return (this.GetAxisLabel != null);
        }

        void IChart.CheckTitle(Title ATitle, MouseEventArgs e, Keys Shift)
        {
            if (this.ClickTitle != null)
            {
                this.iChart.CancelMouse = true;
                ImageClickEventArgs args = new ImageClickEventArgs(e.X, e.Y);
                this.ClickTitle(ATitle, args);
                this.iChart.IClicked = this.iChart.CancelMouse;
            }
            if (!this.iChart.IClicked)
            {
                this.iChart.CheckZoomPanning(e, Shift);
            }
        }

        void IChart.DoAfterDraw()
        {
            if (this.AfterDraw != null)
            {
                this.iChart.graphics3D.DisposeObj();
                this.AfterDraw(this, this.iChart.graphics3D);
            }
        }

        bool IChart.DoAllowScroll(Axis a, double Delta, ref double Min, ref double Max)
        {
            return true;
        }

        void IChart.DoBeforeDraw()
        {
            if (this.BeforeDraw != null)
            {
                this.iChart.graphics3D.DisposeObj();
                this.BeforeDraw(this, this.iChart.graphics3D);
            }
        }

        void IChart.DoBeforeDrawAxes()
        {
            if (this.BeforeDrawAxes != null)
            {
                this.iChart.graphics3D.DisposeObj();
                this.BeforeDrawAxes(this, this.iChart.graphics3D);
            }
        }

        void IChart.DoBeforeDrawSeries()
        {
            if (this.BeforeDrawSeries != null)
            {
                this.iChart.graphics3D.DisposeObj();
                this.BeforeDrawSeries(this, this.iChart.graphics3D);
            }
        }

        void IChart.DoChartPrint(object sender, PrintPageEventArgs e)
        {
        }

        void IChart.DoClickAxis(Axis a, MouseEventArgs e)
        {
            if (this.ClickAxis != null)
            {
                this.iChart.CancelMouse = true;
                ImageClickEventArgs args = new ImageClickEventArgs(e.X, e.Y);
                this.ClickAxis(a, args);
            }
        }

        void IChart.DoClickLegend(object sender, MouseEventArgs e)
        {
            if (this.ClickLegend != null)
            {
                this.iChart.CancelMouse = true;
                ImageClickEventArgs args = new ImageClickEventArgs(e.X, e.Y);
                this.ClickLegend(sender, args);
                this.iChart.IClicked = this.iChart.CancelMouse;
            }
        }

        void IChart.DoClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            if (this.ClickSeries != null)
            {
                this.ClickSeries(this, s, valueIndex, e);
            }
        }

        void IChart.DoGetAxesChartRect(object sender, ref Rectangle rect)
        {
            if (this.GetAxesChartRect != null)
            {
                GetAxesChartRectEventArgs e = new GetAxesChartRectEventArgs(rect);
                this.GetAxesChartRect(sender, e);
                rect = e.AxesChartRect;
            }
        }

        void IChart.DoGetAxisLabel(object sender, Series s, int valueIndex, ref string labelText)
        {
            if (this.GetAxisLabel != null)
            {
                GetAxisLabelEventArgs e = new GetAxisLabelEventArgs(s, valueIndex, labelText);
                this.GetAxisLabel(sender, e);
                labelText = e.LabelText;
            }
        }

        void IChart.DoGetLegendPos(object sender, int index, ref int X, ref int Y, ref int XColor)
        {
            if (this.GetLegendPos != null)
            {
                GetLegendPosEventArgs e = new GetLegendPosEventArgs(index, X, Y, XColor);
                this.GetLegendPos(sender, e);
                X = e.X;
                Y = e.Y;
                XColor = e.XColor;
            }
        }

        void IChart.DoGetLegendRectangle(object sender, ref Rectangle rect)
        {
            if (this.GetLegendRect != null)
            {
                GetLegendRectEventArgs e = new GetLegendRectEventArgs(rect);
                this.GetLegendRect(sender, e);
                rect = e.Rectangle;
            }
        }

        void IChart.DoGetLegendSize(object sender, ref int size)
        {
        }

        void IChart.DoGetLegendText(object sender, LegendStyles LegendStyle, int Index, ref string Text)
        {
            if (this.GetLegendText != null)
            {
                GetLegendTextEventArgs e = new GetLegendTextEventArgs(LegendStyle, Index, Text);
                this.GetLegendText(sender, e);
                Text = e.Text;
            }
        }

        void IChart.DoGetNextAxisLabel(object sender, int labelIndex, ref double labelValue, ref bool doStop)
        {
            if (this.GetNextAxisLabel != null)
            {
                GetNextAxisLabelEventArgs e = new GetNextAxisLabelEventArgs(labelIndex, labelValue, doStop);
                this.GetNextAxisLabel(sender, e);
                labelValue = e.LabelValue;
                doStop = e.Stop;
            }
        }

        void IChart.DoInvalidate()
        {
            if (!this.refreshing)
            {
                this.refreshing = true;
                try
                {
                    if (base.Site != null)
                    {
                        IComponentChangeService service = (IComponentChangeService) base.Site.GetService(typeof(IComponentChangeService));
                        if (service != null)
                        {
                            service.OnComponentChanged(this, null, null, null);
                        }
                    }
                }
                finally
                {
                    this.refreshing = false;
                }
            }
        }

        void IChart.DoScroll(object sender, EventArgs e)
        {
        }

        void IChart.DoSetControlStyle()
        {
        }

        void IChart.DoUndoneZoom(object sender, EventArgs e)
        {
        }

        void IChart.DoZoomed(object sender, EventArgs e)
        {
        }

        void IChart.DrawBackColor(Graphics3D g)
        {
            if (this.Chart.Panel.BorderRound > 0)
            {
                Rectangle chartBounds = this.Chart.ChartBounds;
                chartBounds.Inflate(2, 2);
                Color color = g.Brush.Color;
                g.Brush.Color = this.BackColor;
                bool visible = g.Pen.Visible;
                g.Pen.Visible = false;
                g.Rectangle(chartBounds);
                g.Pen.Visible = visible;
                g.Brush.Color = color;
            }
        }

        object IChart.FindParentForm()
        {
            System.Web.UI.Page page = null;
            try
            {
                page = this.Parent.Page;
            }
            catch (Exception exception)
            {
                throw new Exception("IChart.FindParentForm()", exception.InnerException);
            }
            return page;
        }

        IContainer IChart.GetContainer()
        {
            if (base.Site != null)
            {
                return base.Site.Container;
            }
            return null;
        }

        System.Windows.Forms.Control IChart.GetControl()
        {
            return null;
        }

        Cursor IChart.GetCursor()
        {
            return null;
        }

        bool IChart.IsWebForm()
        {
            return true;
        }

        Point IChart.PointToScreen(Point p)
        {
            return p;
        }

        void IChart.RefreshControl()
        {
        }

        void IChart.SetChart(Steema.TeeChart.Chart c)
        {
            this.iChart = c;
        }

        void IChart.SetCursor(Cursor c)
        {
        }

        [Description("When True TeeChart's WebChart will respond to user mouseclick events.")]
        public bool AutoPostback
        {
            get
            {
                if (this.ViewState["AutoPostback"] != null)
                {
                    return (bool) this.ViewState["AutoPostback"];
                }
                return this.autoPostback;
            }
            set
            {
                this.ViewState["AutoPostback"] = value;
                this.autoPostback = value;
            }
        }

        [NotifyParentProperty(true), Description("Chart configuration."), Bindable(true), RefreshProperties(RefreshProperties.Repaint), Editor(typeof(WebChartEditor), typeof(UITypeEditor))]
        public Steema.TeeChart.Chart Chart
        {
            get
            {
                if (this.iChart.parent == null)
                {
                    this.iChart.parent = this;
                }
                return this.iChart;
            }
        }

        [DefaultValue((string) null), Browsable(false), PersistenceMode(PersistenceMode.Attribute)]
        public int ClickedX
        {
            get
            {
                return this.clickedX;
            }
        }

        [Browsable(false), DefaultValue((string) null), PersistenceMode(PersistenceMode.Attribute)]
        public int ClickedY
        {
            get
            {
                return this.clickedY;
            }
        }

        [PersistenceMode(PersistenceMode.Attribute), Browsable(false), DefaultValue((string) null)]
        public string Config
        {
            get
            {
                if (this.ViewState["Config"] != null)
                {
                    return (string) this.ViewState["Config"];
                }
                return this.config;
            }
            set
            {
                this.ViewState["Config"] = value;
                this.config = value;
            }
        }

        [Description("Sets location for GetChart.aspx if not housed at same location as project folder")]
        public string GetChartFile
        {
            get
            {
                if (this.ViewState["GetChartFile"] != null)
                {
                    return (string) this.ViewState["GetChartFile"];
                }
                return this.iGetChartFile;
            }
            set
            {
                this.ViewState["GetChartFile"] = value;
                this.iGetChartFile = value;
            }
        }

        [PersistenceMode(PersistenceMode.Attribute)]
        public override Unit Height
        {
            get
            {
                if (this.ViewState["Height"] != null)
                {
                    return (Unit) this.ViewState["Height"];
                }
                if (!base.Height.IsEmpty && (base.Height != 0))
                {
                    return base.Height;
                }
                return this.Chart.Height;
            }
            set
            {
                this.ViewState["Height"] = value;
                this.Chart.Height = Utils.Round(value.Value);
                base.Height = value;
            }
        }

        [Description("Chart Image Format"), DefaultValue(3)]
        public PictureFormats PictureFormat
        {
            get
            {
                return this.pictureFormat;
            }
            set
            {
                this.pictureFormat = value;
            }
        }

        [Description("When set to 'Session' TeeChart streams the Chart to a Session variable for direct retrieval from the WebForm (no temporary file). Requires script page 'GetChart'. See Documentation for details.")]
        public TempChartStyle TempChart
        {
            get
            {
                return this.tempChart;
            }
            set
            {
                this.tempChart = value;
            }
        }

        [PersistenceMode(PersistenceMode.Attribute)]
        public override Unit Width
        {
            get
            {
                if (this.ViewState["Width"] != null)
                {
                    return (Unit) this.ViewState["Width"];
                }
                if (!base.Width.IsEmpty && (base.Width != 0))
                {
                    return base.Width;
                }
                return this.Chart.Width;
            }
            set
            {
                this.ViewState["Width"] = value;
                this.Chart.Width = Utils.Round(value.Value);
                base.Width = value;
            }
        }

        public delegate void ClickEventHandler(object sender, ImageClickEventArgs e);

        internal sealed class Designer : ControlDesigner
        {
            public Designer()
            {
                this.AddVerbs();
            }

            private void aboutEvent(object sender, EventArgs e)
            {
                WebChart component = (WebChart) base.Component;
                AboutBox.ShowModal();
            }

            private void AddVerbs()
            {
                DesignTimeOptions.InitLanguage(true, true);
                this.Verbs.Add(new DesignerVerb(Texts.About, new EventHandler(this.aboutEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.Edit, new EventHandler(this.editorEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.Clear, new EventHandler(this.clearEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.ExportChart, new EventHandler(this.exportEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.ImportChart, new EventHandler(this.importEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.PrintPreview, new EventHandler(this.previewEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.Options, new EventHandler(this.optionsEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.OnlineSupport, new EventHandler(this.supportEvent)));
            }

            private void clearEvent(object sender, EventArgs e)
            {
                if (Utils.YesNo("Are you sure you want to reset your Chart to Default values?"))
                {
                    ((WebChart) base.Component).Clear();
                    base.Tag.SetDirty(true);
                    this.UpdateConfig();
                    this.UpdateDesignTimeHtml();
                }
            }

            private void editorEvent(object sender, EventArgs e)
            {
                WebChart component = (WebChart) base.Component;
                component.iChart.parent = component;
                ChartEditor.ShowModal(component.iChart);
                base.Tag.SetDirty(true);
                try
                {
                    this.UpdateConfig();
                    this.UpdateDesignTimeHtml();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }

            private void exportEvent(object sender, EventArgs e)
            {
                WebChart component = (WebChart) base.Component;
                ExportEditor.ShowModal(component.Chart);
            }

            public override string GetDesignTimeHtml()
            {
                WebChart component = (WebChart) base.Component;
                return component.CreateDesignPictureFile();
            }

            private void importEvent(object sender, EventArgs e)
            {
                WebChart component = (WebChart) base.Component;
                ImportEditor.ShowModal(component.Chart);
                this.UpdateConfig();
                this.UpdateDesignTimeHtml();
            }

            private void optionsEvent(object sender, EventArgs e)
            {
                using (DesignTimeOptions options = DesignTimeOptions.GetInstance())
                {
                    if (EditorUtils.ShowFormModal(options))
                    {
                        options.StoreSettings();
                    }
                }
            }

            private void previewEvent(object sender, EventArgs e)
            {
                WebChart component = (WebChart) base.Component;
                PrintPreview.ShowModal(component.Chart);
                base.RaiseComponentChanged(null, null, null);
            }

            private void supportEvent(object sender, EventArgs e)
            {
                Process.Start(Texts.SupportURL);
            }

            internal void UpdateConfig()
            {
                WebChart component = (WebChart) base.Component;
                string oldValue = "";
                component.Config = component.Chart.Export.GetBase64((MemoryStream) component.Chart.Export.InternalSaveViewState(component.Chart));
                base.Tag.SetDirty(true);
                PropertyDescriptor member = TypeDescriptor.GetProperties(component).Find("Config", true);
                base.RaiseComponentChanged(member, oldValue, component.Config);
            }
        }

        public delegate void SeriesEventHandler(object sender, Series s, int valueIndex, EventArgs e);

        internal sealed class WebChartEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                bool flag = ChartEditor.ShowModal((Chart) value);
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }
    }
}

