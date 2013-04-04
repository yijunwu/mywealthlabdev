namespace Steema.TeeChart.Functions
{
    using System;

    public class Poly
    {
        public static int maxDegree = 20;
        private double[] phi;
        public double[] PolyCoeff;
        private int polyDegree = 5;

        public Poly()
        {
            if (this.PolyCoeff == null)
            {
                this.PolyCoeff = new double[maxDegree];
            }
            if (this.phi == null)
            {
                this.phi = new double[maxDegree];
            }
        }

        private void FKT(double x, ref double[] phi)
        {
            phi[0] = 1.0;
            for (int i = 1; i < this.polyDegree; i++)
            {
                phi[i] = phi[i - 1] * x;
            }
        }

        private double GaussFit(double error, ref double[][] pmtx, ref double[] y, ref double[] x)
        {
            double num5;
            int num = 0;
            for (int i = 0; i < (this.polyDegree - 1); i++)
            {
                int index = i;
                double num3 = Math.Abs(pmtx[i][i]);
                for (int k = i + 1; k < this.polyDegree; k++)
                {
                    if (Math.Abs(pmtx[k][i]) > num3)
                    {
                        num3 = Math.Abs(pmtx[k][i]);
                        index = k;
                    }
                }
                if (index != i)
                {
                    double num4;
                    for (int n = i; n < this.polyDegree; n++)
                    {
                        num4 = pmtx[i][n];
                        pmtx[i][n] = pmtx[index][n];
                        pmtx[index][n] = num4;
                    }
                    num4 = y[i];
                    y[i] = y[index];
                    y[index] = num4;
                    num++;
                }
                if (Math.Abs(pmtx[i][i]) < error)
                {
                    return 0.0;
                }
                for (int m = i + 1; m < this.polyDegree; m++)
                {
                    num5 = pmtx[m][i] / pmtx[i][i];
                    for (int num10 = i + 1; num10 < this.polyDegree; num10++)
                    {
                        pmtx[m][num10] -= num5 * pmtx[i][num10];
                    }
                    y[m] -= num5 * y[i];
                }
            }
            if (Math.Abs(pmtx[this.polyDegree - 1][this.polyDegree - 1]) < error)
            {
                return 0.0;
            }
            double num11 = 1.0;
            for (int j = this.polyDegree - 1; j >= 0; j--)
            {
                num5 = 0.0;
                for (int num13 = j + 1; num13 < this.polyDegree; num13++)
                {
                    num5 += pmtx[j][num13] * x[num13];
                }
                x[j] = (y[j] - num5) / pmtx[j][j];
                num11 *= pmtx[j][j];
            }
            if ((num % 2) == 1)
            {
                num11 *= -1.0;
            }
            return num11;
        }

        public double PolyEval(double x)
        {
            double num = 0.0;
            this.FKT(x, ref this.phi);
            for (int i = 0; i < this.polyDegree; i++)
            {
                num += this.PolyCoeff[i] * this.phi[i];
            }
            return num;
        }

        public void PolyFit(int numPoints, double[] x, double[] y)
        {
            double[] numArray = new double[maxDegree];
            double[][] numArray2 = new double[this.polyDegree][];
            for (int i = 0; i < this.polyDegree; i++)
            {
                numArray2[i] = new double[numPoints + 1];
                for (int k = 0; k <= numPoints; k++)
                {
                    numArray2[i][k] = 0.0;
                }
            }
            double[][] pmtx = new double[maxDegree][];
            for (int j = 0; j < maxDegree; j++)
            {
                pmtx[j] = new double[maxDegree];
            }
            try
            {
                for (int m = 0; m < numPoints; m++)
                {
                    this.FKT(x[m], ref this.phi);
                    for (int num5 = 0; num5 < this.PolyDegree; num5++)
                    {
                        numArray2[num5][m] = this.phi[num5];
                    }
                }
                for (int n = 0; n < this.polyDegree; n++)
                {
                    for (int num7 = 0; num7 < this.polyDegree; num7++)
                    {
                        pmtx[num7][n] = 0.0;
                        for (int num8 = 0; num8 < numPoints; num8++)
                        {
                            pmtx[num7][n] += numArray2[num7][num8] * numArray2[n][num8];
                        }
                        pmtx[n][num7] = pmtx[num7][n];
                    }
                }
                for (int num9 = 0; num9 < this.polyDegree; num9++)
                {
                    numArray[num9] = 0.0;
                    for (int num10 = 0; num10 < numPoints; num10++)
                    {
                        numArray[num9] += numArray2[num9][num10] * y[num10];
                    }
                }
                if (this.GaussFit(1E-15, ref pmtx, ref numArray, ref this.PolyCoeff) == 0.0)
                {
                    throw new ArithmeticException();
                }
            }
            finally
            {
                numArray2 = null;
                pmtx = null;
                numArray = null;
            }
        }

        public int PolyDegree
        {
            get
            {
                return this.polyDegree;
            }
            set
            {
                if (this.polyDegree != value)
                {
                    this.polyDegree = Math.Min(value, maxDegree);
                }
            }
        }
    }
}

