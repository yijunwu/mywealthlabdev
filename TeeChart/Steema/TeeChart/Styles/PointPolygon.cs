namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart.Drawing;
    using System;

    public class PointPolygon : TeeBase
    {
        private string attribs;
        private int[] coordinates;
        private string hRef;
        private bool includePoly;
        private PolygonStyle pointStyle;
        private string segmentTitle;
        private int valueIndex;

        public PointPolygon(int vIndex, int[] coords, PolygonStyle pStyle)
        {
            this.valueIndex = vIndex;
            this.coordinates = coords;
            this.pointStyle = pStyle;
        }

        public string Attributes
        {
            get
            {
                return this.attribs;
            }
            set
            {
                this.attribs = value;
            }
        }

        public int[] Coordinates
        {
            get
            {
                return this.coordinates;
            }
            set
            {
                this.coordinates = value;
            }
        }

        public string HREF
        {
            get
            {
                return this.hRef;
            }
            set
            {
                base.SetStringProperty(ref this.hRef, value);
            }
        }

        public bool IncludePolygon
        {
            get
            {
                return this.includePoly;
            }
            set
            {
                base.SetBooleanProperty(ref this.includePoly, value);
            }
        }

        public PolygonStyle PointStyle
        {
            get
            {
                return this.pointStyle;
            }
            set
            {
                if (this.pointStyle != value)
                {
                    this.pointStyle = value;
                }
            }
        }

        public string Title
        {
            get
            {
                return this.segmentTitle;
            }
            set
            {
                this.segmentTitle = value;
            }
        }

        public int ValueIndex
        {
            get
            {
                return this.valueIndex;
            }
            set
            {
                base.SetIntegerProperty(ref this.valueIndex, value);
            }
        }
    }
}

