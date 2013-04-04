namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class PolyFitting : Function
    {
        private int factor;
        private int firstCalcPoint;
        private int firstPoint;
        private double iminyval;
        private int ipolyDegree;
        private int lastCalcPoint;
        private int lastPoint;
        private Poly p;

        public PolyFitting() : this(null)
        {
        }

        public PolyFitting(Chart c) : base(c)
        {
            this.ipolyDegree = 5;
            this.factor = 1;
            this.firstCalcPoint = -1;
            this.lastCalcPoint = -1;
            this.firstPoint = -1;
            this.lastPoint = -1;
            if (this.p == null)
            {
                this.p = new Poly();
            }
        }

        private void AddFittedPoints(Series src)
        {
            double num6;
            this.iminyval = base.ValueList(src).Minimum;
            double minimum = src.XValues.Minimum;
            int num2 = 0;
            if (this.firstCalcPoint != -1)
            {
                num2 = Math.Max(0, this.firstCalcPoint);
            }
            int num3 = src.Count - 1;
            if (this.lastCalcPoint != -1)
            {
                num3 = Math.Min(src.Count - 1, this.lastCalcPoint);
            }
            for (int i = num2; i < num3; i++)
            {
                double num4 = src.XValues[i];
                double num5 = (src.XValues[i + 1] - num4) / ((double) this.factor);
                for (int j = 0; j < this.factor; j++)
                {
                    num6 = num4 + (num5 * j);
                    base.Series.Add(num6, (double) (this.p.PolyEval(num6 - minimum) + this.iminyval));
                }
            }
            num6 = src.XValues[num3];
            base.Series.Add(num6, (double) (this.p.PolyEval(num6 - minimum) + this.iminyval));
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                Series s = (Series) source.GetValue(0);
                if (s.Count > 0)
                {
                    base.Series.Clear();
                    ValueList list = base.ValueList(s);
                    double minimum = s.XValues.Minimum;
                    this.iminyval = list.Minimum;
                    int num2 = 0;
                    if (this.firstCalcPoint != -1)
                    {
                        num2 = Math.Max(0, this.firstCalcPoint);
                    }
                    int num3 = s.Count - 1;
                    if (this.lastCalcPoint != -1)
                    {
                        num3 = Math.Min(s.Count - 1, this.lastCalcPoint);
                    }
                    int numPoints = (num3 - num2) + 1;
                    if (numPoints > 0)
                    {
                        double[] x = new double[numPoints];
                        double[] y = new double[numPoints];
                        try
                        {
                            for (int i = 0; i < numPoints; i++)
                            {
                                int num5 = i + num2;
                                x[i] = s.XValues[num5] - minimum;
                                y[i] = list[num5] - this.iminyval;
                            }
                            this.p.PolyFit(numPoints, x, y);
                            this.AddFittedPoints(s);
                        }
                        finally
                        {
                            x = null;
                            y = null;
                        }
                    }
                }
            }
        }

        public double Coefficient(int index)
        {
            if ((index >= 0) && (index < Poly.maxDegree))
            {
                return this.p.PolyCoeff[index];
            }
            return 0.0;
        }

        public override string Description()
        {
            return Texts.FunctionCurveFitting;
        }

        public double GetCurveYValue(Series s, double x)
        {
            return (this.p.PolyEval(x - s.XValues.Minimum) + this.iminyval);
        }

        [DefaultValue(1), Description("Multiplying factor.")]
        public int Factor
        {
            get
            {
                return this.factor;
            }
            set
            {
                base.SetIntegerProperty(ref this.factor, value);
            }
        }

        [Description("First point to be used in fitting."), DefaultValue(-1)]
        public int FirstCalcPoint
        {
            get
            {
                return this.firstCalcPoint;
            }
            set
            {
                base.SetIntegerProperty(ref this.firstCalcPoint, value);
            }
        }

        [Description("First point to be used in evaluating."), DefaultValue(-1)]
        public int FirstPoint
        {
            get
            {
                return this.firstPoint;
            }
            set
            {
                base.SetIntegerProperty(ref this.firstPoint, value);
            }
        }

        [Description("Last point to be used in fitting."), DefaultValue(-1)]
        public int LastCalcPoint
        {
            get
            {
                return this.lastCalcPoint;
            }
            set
            {
                base.SetIntegerProperty(ref this.lastCalcPoint, value);
            }
        }

        [DefaultValue(-1), Description("Last point to be used in evaluating.")]
        public int LastPoint
        {
            get
            {
                return this.lastPoint;
            }
            set
            {
                base.SetIntegerProperty(ref this.lastPoint, value);
            }
        }

        [Description("Defines fitted polynomial degree."), DefaultValue(5)]
        public int PolyDegree
        {
            get
            {
                return this.ipolyDegree;
            }
            set
            {
                if (value != this.ipolyDegree)
                {
                    this.ipolyDegree = Math.Min(value, Poly.maxDegree);
                    if (this.p != null)
                    {
                        this.p.PolyDegree = this.ipolyDegree;
                    }
                    base.Recalculate();
                }
            }
        }
    }
}

