namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Web;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Drawing;
    using System.Web;

    [Description("Provides clientside Chart zoom functionality for WebCharts on a WebForm"), DesignTimeVisible(false), ToolboxBitmap(typeof(ZoomTool), "ToolsIcons.ZoomTool.bmp")]
    public class ZoomTool : Tool
    {
        private string customVariables;
        private HttpRequest request;
        private ZoomAttributes zoomAtts;
        private int zoomCanvasIndex;
        private int zoomFillTransp;
        private Color zoomPenColor;

        public ZoomTool() : this(null)
        {
        }

        public ZoomTool(Chart c) : base(c)
        {
            this.zoomCanvasIndex = 0x1f3;
            this.zoomFillTransp = 30;
            this.zoomPenColor = Color.Red;
            this.customVariables = "";
            this.zoomAtts = new ZoomAttributes();
            this.zoomAtts.Attribs = "";
            this.zoomAtts.DragZoom = true;
            this.zoomAtts.CanvasIndex = 0;
            this.zoomAtts.FillTransp = 30;
            this.zoomAtts.PenColor = "";
            this.zoomAtts.ImageStr = "";
        }

        protected override void Assign(Tool t)
        {
            base.Assign(t);
            ZoomTool tool = t as ZoomTool;
            tool.CustomVariables = this.CustomVariables;
            tool.ZoomCanvasIndex = this.ZoomCanvasIndex;
            tool.ZoomPenColor = this.ZoomPenColor;
        }

        private void CheckExistingZoom(ArrayList zoomState)
        {
            if (zoomState != null)
            {
                this.SetSavedZoom(zoomState);
            }
        }

        private ArrayList DoZoom(ArrayList zoomState)
        {
            this.CheckExistingZoom(zoomState);
            ArrayList list = new ArrayList();
            if (this.request.RequestType == "POST")
            {
                int num = Convert.ToInt32(this.request.Params["x0"]);
                int num2 = Convert.ToInt32(this.request.Params["y0"]);
                int num3 = Convert.ToInt32(this.request.Params["x1"]);
                int num4 = Convert.ToInt32(this.request.Params["y1"]);
                return this.Zoom(new Rectangle(num, num2, num3 - num, num4 - num2));
            }
            int x = Convert.ToInt32(this.request.QueryString["x0"]);
            int y = Convert.ToInt32(this.request.QueryString["y0"]);
            int num7 = Convert.ToInt32(this.request.QueryString["x1"]);
            int num8 = Convert.ToInt32(this.request.QueryString["y1"]);
            return this.Zoom(new Rectangle(x, y, num7 - x, num8 - y));
        }

        private ArrayList DoZoom(ArrayList zoomState, Hashtable argumentList)
        {
            this.CheckExistingZoom(zoomState);
            ArrayList list = new ArrayList();
            int x = Convert.ToInt32(argumentList["x0"]);
            int y = Convert.ToInt32(argumentList["y0"]);
            int num3 = Convert.ToInt32(argumentList["x1"]);
            int num4 = Convert.ToInt32(argumentList["y1"]);
            return this.Zoom(new Rectangle(x, y, num3 - x, num4 - y));
        }

        internal string GetColorString()
        {
            string str = string.Format("{0:x}", this.zoomPenColor.R);
            if (str.Length < 2)
            {
                str = "0" + str;
            }
            string str2 = string.Format("{0:x}", this.zoomPenColor.G);
            if (str2.Length < 2)
            {
                str2 = "0" + str2;
            }
            string str3 = string.Format("{0:x}", this.zoomPenColor.B);
            if (str3.Length < 2)
            {
                str3 = "0" + str3;
            }
            return ("#" + str + str2 + str3);
        }

        protected Hashtable getZoomArgs(HttpRequest r)
        {
            Hashtable hashtable = new Hashtable();
            if ((r.Params["__EVENTARGUMENT"] != null) && (r.Params["__EVENTARGUMENT"].Length > 0))
            {
                char[] separator = new char[] { ';' };
                char[] chArray2 = new char[] { '$' };
                foreach (string str in r.Params["__EVENTARGUMENT"].Split(separator))
                {
                    if (str.IndexOf("$") != -1)
                    {
                        hashtable.Add(str.Split(chArray2)[0], str.Split(chArray2)[1]);
                    }
                }
            }
            return hashtable;
        }

        protected internal bool IsChart(NameValueCollection postCollection)
        {
            if (postCollection != null)
            {
                return false;
            }
            return true;
        }

        protected internal void setAttributes(string id)
        {
            this.zoomAtts.PenColor = this.GetColorString();
            this.zoomAtts.CanvasIndex = this.ZoomCanvasIndex;
            this.zoomAtts.FillTransp = this.ZoomFillTransparency;
            this.zoomAtts.Attribs = " onclick=\"activateContent('" + id + "',event)\" onmousemove=\"zoomRect('" + id + "',event)\" ";
        }

        public ArrayList SetCurrentZoom(HttpRequest r, ArrayList zoomState)
        {
            WebChart parent = (WebChart) base.Chart.parent;
            this.request = r;
            if (this.request.RequestType == "POST")
            {
                zoomState = this.SetCurrentZoom(r, this.getZoomArgs(r), zoomState);
                return zoomState;
            }
            string str = this.request.QueryString["zoom"];
            if ((str != null) && ((this.request.QueryString["chart"] == parent.ClientID) || (this.request.Params["chart"] == parent.ClientID)))
            {
                if (str == "0")
                {
                    parent.Chart.Zoom.Undo();
                    zoomState = null;
                }
                else
                {
                    zoomState = this.DoZoom(zoomState);
                }
            }
            this.CheckExistingZoom(zoomState);
            return zoomState;
        }

        public ArrayList SetCurrentZoom(HttpRequest r, ArrayList zoomState, NameValueCollection postCollection)
        {
            if (this.IsChart(postCollection))
            {
                return this.SetCurrentZoom(r, zoomState);
            }
            if (zoomState != null)
            {
                this.CheckExistingZoom(zoomState);
            }
            return zoomState;
        }

        public ArrayList SetCurrentZoom(HttpRequest r, Hashtable argumentList, ArrayList zoomState)
        {
            WebChart parent = (WebChart) base.Chart.parent;
            this.request = r;
            string str = null;
            if (argumentList["zoom"] != null)
            {
                str = argumentList["zoom"].ToString();
            }
            if ((str != null) && (argumentList["chart"].ToString() == parent.ClientID))
            {
                if (str == "0")
                {
                    parent.Chart.Zoom.Undo();
                    zoomState = null;
                }
                else
                {
                    zoomState = this.DoZoom(zoomState, argumentList);
                }
            }
            this.CheckExistingZoom(zoomState);
            return zoomState;
        }

        public ArrayList SetCurrentZoom(HttpRequest r, string args, ArrayList zoomState)
        {
            Hashtable argumentList = new Hashtable();
            if ((args != null) && (args.Length > 0))
            {
                char[] separator = new char[] { '&' };
                char[] chArray2 = new char[] { '=' };
                foreach (string str in args.Split(separator))
                {
                    if (str.IndexOf("=") != -1)
                    {
                        argumentList.Add(str.Split(chArray2)[0], str.Split(chArray2)[1]);
                    }
                }
                this.SetCurrentZoom(r, argumentList, zoomState);
                return zoomState;
            }
            this.SetCurrentZoom(r, zoomState);
            return zoomState;
        }

        public void SetSavedZoom(ArrayList zoomedState)
        {
            PointDouble num = (PointDouble) zoomedState[0];
            PointDouble num2 = (PointDouble) zoomedState[0];
            base.Chart.Axes.Left.SetMinMax(num.X, num2.Y);
            PointDouble num3 = (PointDouble) zoomedState[3];
            PointDouble num4 = (PointDouble) zoomedState[3];
            base.Chart.Axes.Bottom.SetMinMax(num3.X, num4.Y);
            PointDouble num5 = (PointDouble) zoomedState[1];
            PointDouble num6 = (PointDouble) zoomedState[1];
            base.Chart.Axes.Top.SetMinMax(num5.X, num6.Y);
            PointDouble num7 = (PointDouble) zoomedState[2];
            PointDouble num8 = (PointDouble) zoomedState[2];
            base.Chart.Axes.Right.SetMinMax(num7.X, num8.Y);
        }

        public ArrayList Zoom(Rectangle r)
        {
            if ((base.Chart.parent != null) && (base.Chart.parent is WebChart))
            {
                base.Chart.Width = Utils.Round(((WebChart) base.Chart.parent).Width.Value);
                base.Chart.Height = Utils.Round(((WebChart) base.Chart.parent).Height.Value);
            }
            base.Chart.Bitmap();
            base.Chart.Zoom.ZoomRect(r);
            ArrayList list = new ArrayList();
            list.Add(new PointDouble(base.Chart.Axes.Left.Minimum, base.Chart.Axes.Left.Maximum));
            list.Add(new PointDouble(base.Chart.Axes.Top.Minimum, base.Chart.Axes.Top.Maximum));
            list.Add(new PointDouble(base.Chart.Axes.Right.Minimum, base.Chart.Axes.Right.Maximum));
            list.Add(new PointDouble(base.Chart.Axes.Bottom.Minimum, base.Chart.Axes.Bottom.Maximum));
            return list;
        }

        public ZoomAttributes Attributes
        {
            get
            {
                return this.zoomAtts;
            }
            set
            {
                this.zoomAtts = value;
            }
        }

        public string CustomVariables
        {
            get
            {
                return this.customVariables;
            }
            set
            {
                if (value != this.customVariables)
                {
                    this.customVariables = value;
                }
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.ZoomTool;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.ZoomToolSummary;
            }
        }

        public int ZoomCanvasIndex
        {
            get
            {
                return this.zoomCanvasIndex;
            }
            set
            {
                if (value != this.zoomCanvasIndex)
                {
                    this.zoomCanvasIndex = value;
                }
            }
        }

        public int ZoomFillTransparency
        {
            get
            {
                return this.zoomFillTransp;
            }
            set
            {
                this.zoomFillTransp = value;
            }
        }

        public Color ZoomPenColor
        {
            get
            {
                return this.zoomPenColor;
            }
            set
            {
                if (value != this.zoomPenColor)
                {
                    this.zoomPenColor = value;
                }
            }
        }
    }
}

