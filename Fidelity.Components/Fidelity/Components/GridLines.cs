namespace Fidelity.Components
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(GridLines), "GridLines")]
    public class GridLines : Component
    {
        private bool bool_0;
        private bool bool_1;
        private double double_0;
        private double double_1;
        private double double_2;
        private double double_3;
        private IContainer icontainer_0;
        private int int_0;
        private int int_1;

        public GridLines()
        {
            this.int_1 = 2;
            this.method_1();
        }

        public GridLines(IContainer container)
        {
            this.int_1 = 2;
            container.Add(this);
            this.method_1();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void method_0()
        {
            double d = this.double_1 - this.double_0;
            if (!double.IsInfinity(d))
            {
                this.double_3 = 1.0;
                this.double_2 = this.double_0;
                if ((d > 0.0) && (this.int_0 > 0))
                {
                    double num2 = d / ((double) this.int_0);
                    if (num2 > 0.0)
                    {
                        int num3 = 0;
                        int num4 = 0;
                        while (num2 < 1.0)
                        {
                            num2 *= 10.0;
                            num3++;
                        }
                        if (this.bool_1)
                        {
                            while (num3 > this.int_1)
                            {
                                num2 /= 10.0;
                                num3--;
                            }
                        }
                        while (num2 > 10.0)
                        {
                            num2 /= 10.0;
                            num4++;
                        }
                        if (num2 < 1.0)
                        {
                            num2 = 1.0;
                        }
                        else if (num2 <= 2.0)
                        {
                            num2 = 2.0;
                        }
                        else if (num2 <= 2.5)
                        {
                            num2 = 2.5;
                        }
                        else if (num2 <= 5.0)
                        {
                            num2 = 5.0;
                        }
                        else
                        {
                            num2 = 10.0;
                        }
                        while (num3 > 0)
                        {
                            num2 /= 10.0;
                            num3--;
                        }
                        while (num4 > 0)
                        {
                            num2 *= 10.0;
                            num4--;
                        }
                        this.double_2 = ((int) (this.double_0 / num2)) * num2;
                        this.double_3 = num2;
                        if (this.bool_0)
                        {
                            this.double_3 = (int) this.double_3;
                            if (this.double_3 < 1.0)
                            {
                                this.double_3 = 1.0;
                            }
                        }
                    }
                }
            }
        }

        private void method_1()
        {
            this.icontainer_0 = new Container();
        }

        public int Decimals
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
                this.bool_1 = true;
                this.method_0();
            }
        }

        public double GridFirstValue
        {
            get
            {
                return this.double_2;
            }
        }

        public double GridIncrement
        {
            get
            {
                return this.double_3;
            }
        }

        public int LinesDesired
        {
            get
            {
                return this.int_0;
            }
            set
            {
                if (value <= 0)
                {
                    this.int_0 = 1;
                }
                else
                {
                    this.int_0 = value;
                }
                this.method_0();
            }
        }

        public double RangeMax
        {
            get
            {
                return this.double_1;
            }
            set
            {
                this.double_1 = value;
                this.method_0();
            }
        }

        public double RangeMin
        {
            get
            {
                return this.double_0;
            }
            set
            {
                this.double_0 = value;
                this.method_0();
            }
        }

        public bool WholeNumbersOnly
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
                this.method_0();
            }
        }
    }
}

