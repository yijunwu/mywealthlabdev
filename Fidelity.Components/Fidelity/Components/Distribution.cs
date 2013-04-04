namespace Fidelity.Components
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Distribution), "Distribution")]
    public class Distribution : Component
    {
        private GridLines gridLines_0;
        private IContainer icontainer_0;
        private int int_0;
        private List<Bin> list_0;
        private List<double> list_1;

        public Distribution()
        {
            this.list_0 = new List<Bin>();
            this.list_1 = new List<double>();
            this.method_0();
        }

        public Distribution(IContainer container)
        {
            this.list_0 = new List<Bin>();
            this.list_1 = new List<double>();
            container.Add(this);
            this.method_0();
        }

        public void AddValue(double value)
        {
            this.list_1.Add(value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public void FillBins()
        {
            foreach (Bin bin in this.list_0)
            {
                bin.Count = 0;
                for (int i = 0; i < this.list_1.Count; i++)
                {
                    if ((this.list_1[i] > bin.Low) && (this.list_1[i] <= bin.High))
                    {
                        bin.Count++;
                    }
                }
            }
            this.list_1.Clear();
        }

        public void MakeBins()
        {
            this.list_0.Clear();
            if (this.list_1.Count != 0)
            {
                double num6 = this.list_1[0];
                double d = this.list_1[0];
                for (int i = 1; i < this.list_1.Count; i++)
                {
                    if (this.list_1[i] < num6)
                    {
                        num6 = this.list_1[i];
                    }
                    if (this.list_1[i] > d)
                    {
                        d = this.list_1[i];
                    }
                }
                if (double.IsInfinity(d))
                {
                    d = num6;
                }
                int binsDesired = this.BinsDesired;
                if (this.list_1.Count < binsDesired)
                {
                    binsDesired = this.list_1.Count;
                }
                this.gridLines_0.RangeMin = num6;
                this.gridLines_0.RangeMax = d;
                this.gridLines_0.LinesDesired = binsDesired;
                double num2 = this.gridLines_0.GridFirstValue - this.gridLines_0.GridIncrement;
                for (double j = num2 + this.gridLines_0.GridIncrement; j < (d + this.gridLines_0.GridIncrement); j += this.gridLines_0.GridIncrement)
                {
                    Bin item = new Bin {
                        Low = num2,
                        High = j
                    };
                    for (int k = 0; k < this.list_1.Count; k++)
                    {
                        if ((this.list_1[k] > num2) && (this.list_1[k] <= j))
                        {
                            item.Count++;
                        }
                    }
                    this.list_0.Add(item);
                    num2 = j;
                }
                this.list_1.Clear();
            }
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
            this.gridLines_0 = new GridLines(this.icontainer_0);
            this.gridLines_0.LinesDesired = 0;
            this.gridLines_0.RangeMax = 0.0;
            this.gridLines_0.RangeMin = 0.0;
            this.gridLines_0.WholeNumbersOnly = false;
        }

        public IList<Bin> Bins
        {
            get
            {
                return this.list_0.AsReadOnly();
            }
        }

        public int BinsDesired
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }
    }
}

