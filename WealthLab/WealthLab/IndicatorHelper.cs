namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public abstract class IndicatorHelper
    {
        private List<string> list_0 = new List<string>();

        protected IndicatorHelper()
        {
        }

        public bool CanDropOnIndicator
        {
            get
            {
                if (this.ParameterDefaultValues.Count == 0)
                {
                    return false;
                }
                return (this.ParameterDefaultValues[0].GetType() == typeof(CoreDataSeries));
            }
        }

        public virtual Color DefaultBandColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public abstract Color DefaultColor { get; }

        public virtual LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
            }
        }

        public virtual int DefaultWidth
        {
            get
            {
                return 1;
            }
        }

        public abstract string Description { get; }

        public virtual Bitmap Glyph
        {
            get
            {
                return null;
            }
        }

        public abstract Type IndicatorType { get; }

        public virtual bool IsOscillator
        {
            get
            {
                return false;
            }
        }

        public virtual Color OscillatorOverboughtColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public virtual double OscillatorOverboughtValue
        {
            get
            {
                return 0.0;
            }
        }

        public virtual Color OscillatorOversoldColor
        {
            get
            {
                return Color.Red;
            }
        }

        public virtual double OscillatorOversoldValue
        {
            get
            {
                return 0.0;
            }
        }

        public abstract IList<object> ParameterDefaultValues { get; }

        public abstract IList<string> ParameterDescriptions { get; }

        public List<string> ParameterDisplayNames
        {
            get
            {
                return this.list_0;
            }
        }

        public virtual Type PartnerBandIndicatorType
        {
            get
            {
                return null;
            }
        }

        public virtual string TargetPane
        {
            get
            {
                return "";
            }
        }

        public virtual string URL
        {
            get
            {
                return "";
            }
        }
    }
}

