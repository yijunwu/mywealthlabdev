namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Description("Axis Labels properties."), Editor(typeof(AxisLabels.Editor), typeof(UITypeEditor))]
    public class AxisLabels : TextShape
    {
        private AxisLabelAlign align;
        internal Axis axis;
        protected internal string axisvaluesformat;
        protected internal bool bExponent;
        protected internal bool bOnAxis;
        private int customSize;
        private bool exactDateTime;
        private bool FLabelsAlternate;
        protected internal int iAngle;
        protected internal int isEAxisValueFormat;
        protected internal int iSeparation;
        protected internal AxisLabelStyle iStyle;
        private AxisLabelsItems items;
        protected internal ArrayList labelPos;
        private bool labelsAsLocalTime;
        private bool multiline;
        protected internal int position;
        private bool roundfirstlabel;
        protected internal string sDatetimeformat;

        [Description("Accesses the Label characteristics of Axis Labels.")]
        public AxisLabels(Axis a) : base(a.chart)
        {
            this.bOnAxis = true;
            this.iSeparation = 10;
            this.axisvaluesformat = "#,##0.###";
            this.sDatetimeformat = "";
            this.exactDateTime = true;
            this.roundfirstlabel = true;
            this.isEAxisValueFormat = -1;
            this.axis = a;
            base.bTransparent = true;
            this.items = new AxisLabelsItems(a);
            this.labelPos = new ArrayList();
        }

        public int Clicked(int X, int Y)
        {
            int num = -1;
            if (this.labelPos.Count > 0)
            {
                for (int i = 0; i < this.labelPos.Count; i++)
                {
                    Rectangle rectangle = (Rectangle) this.labelPos[i];
                    if (rectangle.Contains(X, Y))
                    {
                        num = (this.labelPos.Count - i) - 1;
                    }
                }
            }
            return num;
        }

        private int InternalLabelSize(double value, bool isWidth)
        {
            bool flag;
            int num;
            int num2 = base.chart.MultiLineTextWidth(this.LabelValue(value), out num);
            if (isWidth)
            {
                flag = (this.iAngle == 90) || (this.iAngle == 270);
            }
            else
            {
                flag = (this.iAngle == 0) || (this.iAngle == 180);
            }
            if (flag)
            {
                num2 = base.chart.graphics3D.FontHeight * num;
            }
            return num2;
        }

        private int isExponential()
        {
            for (string str = this.axisvaluesformat.ToLower(); str.IndexOf('e') != -1; str = str.Substring(str.IndexOf('e') + 1))
            {
                if (str.Length == 1)
                {
                    return str.IndexOf('e');
                }
                if ((str.Length > (str.IndexOf('e') + 1)) && ((char.IsDigit(str[str.IndexOf('e') + 1]) || (str[str.IndexOf('e') + 1] == '-')) || (str[str.IndexOf('e') + 1] == '+')))
                {
                    return str.IndexOf('e');
                }
            }
            return -1;
        }

        public int LabelHeight(double value)
        {
            return this.InternalLabelSize(value, false);
        }

        [Description("Returns the corresponding text representation of the Value parameter.")]
        public string LabelValue(double value)
        {
            string str;
            if (this.axis.iAxisDateTime)
            {
                if ((value >= -657435.0) && (value <= 2958466.0))
                {
                    try
                    {
                        if (this.labelsAsLocalTime)
                        {
                            value = Utils.DateTime(Utils.DateTime(value).ToLocalTime());
                        }
                        if (this.sDatetimeformat.Length == 0)
                        {
                            str = Utils.DateTime(value).ToString(this.axis.DateTimeDefaultFormat(this.axis.iRange));
                        }
                        else
                        {
                            str = Utils.DateTime(value).ToString(this.sDatetimeformat);
                        }
                    }
                    catch
                    {
                        str = Utils.DateTime(value).ToString(this.axis.DateTimeDefaultFormat(this.axis.iRange));
                    }
                }
                else
                {
                    str = "";
                }
            }
            else
            {
                try
                {
                    if (this.isEAxisValueFormat != -1)
                    {
                        string str2 = this.axis.labelIncr.ToString();
                        if (((str2.ToUpper().IndexOf("E") != -1) && (str2.Length > (str2.ToUpper().IndexOf("E") + 1))) && (str2[str2.ToUpper().IndexOf("E") + 1] == '-'))
                        {
                            string str3 = str2.Substring(str2.IndexOf("E") + 2);
                            value = Convert.ToDouble(string.Format("{0:F" + str3 + "}", value));
                        }
                        str = value.ToString(this.axisvaluesformat);
                    }
                    else
                    {
                        str = value.ToString(this.axisvaluesformat);
                    }
                }
                catch
                {
                    str = value.ToString("#,##0.###");
                }
            }
            if (base.chart.parent != null)
            {
                base.chart.parent.DoGetAxisLabel(this.axis, null, -1, ref str);
            }
            if (this.multiline)
            {
                this.SplitInLines(ref str, " ");
            }
            return str;
        }

        [Description("Returns the Axis Label width of the Value parameter.")]
        public int LabelWidth(double value)
        {
            return this.InternalLabelSize(value, true);
        }

        protected override bool ShouldSerializeTransparent()
        {
            return !base.bTransparent;
        }

        [Description("Sets String to be used for new line")]
        public void SplitInLines(ref string s, string separator)
        {
            int index;
            do
            {
                index = s.IndexOf(separator, 0);
                if (index > 0)
                {
                    s = s.Remove(index, separator.Length);
                    s = s.Insert(index, Convert.ToString('\n'));
                }
            }
            while (index != -1);
        }

        [DefaultValue(0), Description("Sets position of Labels on an Axis")]
        public AxisLabelAlign Align
        {
            get
            {
                return this.align;
            }
            set
            {
                if (this.align != value)
                {
                    this.align = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Sets the axis labels to be drawn in two rows or columns."), DefaultValue(false)]
        public bool Alternate
        {
            get
            {
                return this.FLabelsAlternate;
            }
            set
            {
                base.SetBooleanProperty(ref this.FLabelsAlternate, value);
            }
        }

        [Description("Sets rotation degree applied to each Axis Label."), DefaultValue(0)]
        public int Angle
        {
            get
            {
                return this.iAngle;
            }
            set
            {
                base.SetIntegerProperty(ref this.iAngle, Convert.ToInt32(decimal.Remainder(value, 360M)));
            }
        }

        [Description("Changes spacing of axis labels between the ticks and the title."), DefaultValue(0)]
        public int CustomSize
        {
            get
            {
                return this.customSize;
            }
            set
            {
                base.SetIntegerProperty(ref this.customSize, value);
            }
        }

        [Description("Standard DateTime formatting string specifier used to draw the axis labels."), DefaultValue("")]
        public string DateTimeFormat
        {
            get
            {
                return this.sDatetimeformat;
            }
            set
            {
                base.SetStringProperty(ref this.sDatetimeformat, value);
            }
        }

        [Description("Sets Axis Labels in exact DateTime steps."), DefaultValue(true)]
        public bool ExactDateTime
        {
            get
            {
                return this.exactDateTime;
            }
            set
            {
                base.SetBooleanProperty(ref this.exactDateTime, value);
            }
        }

        [DefaultValue(false), Description("Sets Axis Labels in exponent format with super-script fonts.")]
        public bool Exponent
        {
            get
            {
                return this.bExponent;
            }
            set
            {
                base.SetBooleanProperty(ref this.bExponent, value);
            }
        }

        [Description("Contains the custom labels")]
        public AxisLabelsItems Items
        {
            get
            {
                return this.items;
            }
        }

        [DefaultValue(false), Description("When true, displays DateTime Labels in Local Time rather than UTC.")]
        public bool LabelsAsLocalTime
        {
            get
            {
                return this.labelsAsLocalTime;
            }
            set
            {
                base.SetBooleanProperty(ref this.labelsAsLocalTime, value);
            }
        }

        [Description("Breaks DateTime Labels on occurence of a space."), DefaultValue(false)]
        public bool MultiLine
        {
            get
            {
                return this.multiline;
            }
            set
            {
                base.SetBooleanProperty(ref this.multiline, value);
            }
        }

        [DefaultValue(true), Description("Shows Labels at Axis Minimum and Maximum positions or not")]
        public bool OnAxis
        {
            get
            {
                return this.bOnAxis;
            }
            set
            {
                base.SetBooleanProperty(ref this.bOnAxis, value);
            }
        }

        [Description("Axis labels automatically rounded to the nearest magnitude."), DefaultValue(true)]
        public bool RoundFirstLabel
        {
            get
            {
                return this.roundfirstlabel;
            }
            set
            {
                base.SetBooleanProperty(ref this.roundfirstlabel, value);
            }
        }

        [Description("Sets minimum distance between Axis Labels as percentage."), DefaultValue(10)]
        public int Separation
        {
            get
            {
                return this.iSeparation;
            }
            set
            {
                if (value < 0)
                {
                    throw new TeeChartException(Texts.AxisLabelSep);
                }
                base.SetIntegerProperty(ref this.iSeparation, value);
            }
        }

        [Description("Sets the style of the labels."), DefaultValue(0)]
        public AxisLabelStyle Style
        {
            get
            {
                return this.iStyle;
            }
            set
            {
                if (this.iStyle != value)
                {
                    this.iStyle = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue("#,##0.###"), Description("Sets formatting string to be applied to Axis Labels.")]
        public string ValueFormat
        {
            get
            {
                return this.axisvaluesformat;
            }
            set
            {
                base.SetStringProperty(ref this.axisvaluesformat, value);
                this.isEAxisValueFormat = this.isExponential();
            }
        }

        internal sealed class Editor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                AxisLabels labels = (AxisLabels) value;
                using (AxisLabelsEditor editor = new AxisLabelsEditor(null))
                {
                    editor.SetAxis(labels.axis);
                    bool flag = EditorUtils.ShowFormModal(editor);
                    if ((context != null) && flag)
                    {
                        context.OnComponentChanged();
                    }
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

