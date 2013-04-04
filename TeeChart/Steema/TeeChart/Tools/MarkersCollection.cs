namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Reflection;

    [Serializable, Editor(typeof(ToolsCollection.Editor), typeof(UITypeEditor))]
    public class MarkersCollection : ToolsCollection
    {
        public MarkersCollection(Chart c) : base(c)
        {
        }

        public Marker Add(string aText)
        {
            Marker marker = this[base.Add(typeof(Marker))];
            marker.Shape.Transparent = true;
            marker.Shape.Font.UsePrivateFont = true;
            marker.Shape.Font.Name = "DS-Digital";
            marker.Shape.Pen.Visible = false;
            marker.Shape.Shadow.Visible = false;
            marker.Text = aText;
            return marker;
        }

        public void SetParentChart(Chart chart)
        {
            base.chart = chart;
            foreach (Marker marker in this)
            {
                marker.Chart = chart;
            }
        }

        public Marker this[int index]
        {
            get
            {
                return (Marker) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

