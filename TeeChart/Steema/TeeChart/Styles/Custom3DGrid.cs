namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Custom3DGrid : Custom3DPalette
    {
        protected PCellsRow gridIndex;
        protected bool iGridIndexFilled;
        protected bool iInGallery;
        protected internal int iNextXCell;
        protected internal int iNextZCell;
        protected internal int iNumXValues;
        protected internal int iNumZValues;
        private bool irregularGrid;
        private const int MaxAllowedCells = 0x4e20;
        public bool ReuseGridIndex;
        protected internal int valueIndex0;
        protected internal int valueIndex1;
        protected internal int valueIndex2;
        protected internal int valueIndex3;

        public event GetYValueEventHandler GetYValue;

        public Custom3DGrid() : this(null)
        {
        }

        public Custom3DGrid(Chart c) : base(c)
        {
            this.gridIndex = new PCellsRow();
            this.iNumXValues = 10;
            this.iNumZValues = 10;
            this.gridIndex.Capacity = 0x4e20;
            for (int i = 0; i < 0x4e20; i++)
            {
                this.gridIndex.Add(null);
            }
        }

        protected override void AddSampleValues(int numValues)
        {
            if (numValues > 0)
            {
                bool iInGallery = this.iInGallery;
                this.iInGallery = true;
                try
                {
                    int numX = Math.Min(0x4e20, numValues);
                    this.CreateValues(numX, numX);
                }
                finally
                {
                    this.iInGallery = iInGallery;
                }
            }
        }

        protected override void AddValues(Array source)
        {
            if (source.GetValue(0) is Custom3DGrid)
            {
                Custom3DGrid grid = (Custom3DGrid) source.GetValue(0);
                this.iNumXValues = grid.NumXValues;
                this.iNumZValues = grid.NumZValues;
            }
            base.AddValues(source);
            this.FillGridIndex();
            this.Invalidate();
        }

        internal bool CanCreateValues()
        {
            if (!base.DesignMode && !this.iInGallery)
            {
                return (this.GetYValue != null);
            }
            return true;
        }

        public override void Clear()
        {
            base.Clear();
            base.vxValues.Order = ValueListOrder.None;
            this.InitGridIndex(this.NumXValues, this.NumZValues);
        }

        private void ClearGridIndex()
        {
            for (int i = 0; i < 0x4e20; i++)
            {
                CellsRow row = this.gridIndex[i];
                if (row != null)
                {
                    for (int j = 0; j < row.Count; j++)
                    {
                        row[j] = -1;
                    }
                }
            }
            this.iGridIndexFilled = false;
        }

        public void CreateValues(int numX, int numZ)
        {
            if (this.CanCreateValues())
            {
                this.iNumXValues = numX;
                this.iNumZValues = numZ;
                base.BeginUpdate();
                this.Clear();
                for (int i = 1; i <= numZ; i++)
                {
                    for (int j = 1; j <= numX; j++)
                    {
                        base.Add((double) j, this.GetXZValue(j, i), (double) i);
                    }
                }
                base.EndUpdate();
                base.CreateDefaultPalette(base.iPaletteSteps);
            }
        }

        protected internal override void DoBeforeDrawChart()
        {
            base.DoBeforeDrawChart();
            if (!this.ReuseGridIndex && (base.Count > 0))
            {
                this.FillGridIndex();
            }
        }

        protected void DoGetYValue(int x, int z, ref double Value)
        {
            if (this.GetYValue != null)
            {
                GetYValueEventArgs e = new GetYValueEventArgs(x, z, Value);
                this.GetYValue(this, e);
                Value = e.Value;
            }
        }

        protected internal bool ExistFourGridIndex(int x, int z)
        {
            if (((x < this.gridIndex.Capacity) && ((x + this.iNextXCell) < this.gridIndex.Capacity)) && ((this.gridIndex[x] != null) && (this.gridIndex[x + this.iNextXCell] != null)))
            {
                this.valueIndex0 = this[x, z];
                if (this.valueIndex0 > -1)
                {
                    this.valueIndex1 = this[x + this.iNextXCell, z];
                    if (this.valueIndex1 > -1)
                    {
                        this.valueIndex2 = this[x + this.iNextXCell, z + this.iNextZCell];
                        if (this.valueIndex2 > -1)
                        {
                            this.valueIndex3 = this[x, z + this.iNextZCell];
                            return (this.valueIndex3 > -1);
                        }
                    }
                }
            }
            return false;
        }

        public void FillGridIndex()
        {
            this.FillGridIndex(0);
        }

        public void FillGridIndex(int StartIndex)
        {
            int num3;
            int num4;
            double minimum = base.vxValues.Minimum;
            double minZ = base.vzValues.Minimum;
            if (this.irregularGrid)
            {
                this.FillIrregularGrid(StartIndex, out num3, out num4, minimum, minZ);
            }
            else
            {
                this.FillRegularGrid(StartIndex, out num3, out num4, minimum, minZ);
            }
            if (num3 != this.iNumXValues)
            {
                this.iNumXValues = num3;
            }
            if (num4 != this.iNumZValues)
            {
                this.iNumZValues = num4;
            }
            this.iGridIndexFilled = true;
        }

        private void FillIrregularGrid(int startIndex, out int xCount, out int zCount, double minX, double minZ)
        {
            double[] values = new double[0x4e20];
            double[] numArray2 = new double[0x4e20];
            xCount = 1;
            values[0] = minX;
            zCount = 1;
            numArray2[0] = minZ;
            for (int i = startIndex; i < base.Count; i++)
            {
                this.SearchValue(ref xCount, values, base.vxValues[i]);
                this.SearchValue(ref zCount, numArray2, base.vzValues[i]);
            }
            this.SortValues(xCount, values);
            this.SortValues(zCount, numArray2);
            this.InitGridIndex(Math.Max(this.NumXValues + 1, xCount), Math.Max(this.NumZValues + 1, zCount));
            for (int j = startIndex; j < base.Count; j++)
            {
                this.SetGridIndex(j, xCount, zCount, values, numArray2);
            }
        }

        private void FillRegularGrid(int startIndex, out int xCount, out int zCount, double minX, double minZ)
        {
            int num = Utils.Round(minX) - 1;
            int num2 = Utils.Round(minZ) - 1;
            xCount = Utils.Round(base.XValues.Maximum) - num;
            zCount = Utils.Round(base.ZValues.Maximum) - num2;
            this.InitGridIndex(xCount, zCount);
            for (int i = startIndex; i < base.Count; i++)
            {
                this[Utils.Round(base.XValues[i]) - num, Utils.Round(base.ZValues[i]) - num2] = i;
            }
        }

        private double GetXZValue(int x, int z)
        {
            if (this.GetYValue != null)
            {
                double num = 0.0;
                GetYValueEventArgs e = new GetYValueEventArgs(x, z, num);
                this.GetYValue(this, e);
                return e.Value;
            }
            if (!base.DesignMode && !this.iInGallery)
            {
                return 0.0;
            }
            return (((0.5 * Math.Pow(Math.Cos(((double) x) / (this.iNumXValues * 0.2)), 2.0)) + Math.Pow(Math.Cos(((double) z) / (this.iNumZValues * 0.2)), 2.0)) - Math.Cos(((double) z) / (this.iNumZValues * 0.5)));
        }

        private void InitGridIndex()
        {
            if (!this.ReuseGridIndex)
            {
                this.ClearGridIndex();
            }
        }

        private void InitGridIndex(int xCount, int zCount)
        {
            if (!this.ReuseGridIndex)
            {
                for (int i = 0; i <= xCount; i++)
                {
                    CellsRow row = this.gridIndex[i];
                    if (row != null)
                    {
                        for (int j = 0; j < row.Count; j++)
                        {
                            row[j] = -1;
                        }
                    }
                }
            }
        }

        private void InternalSetGridIndex(int x, int z, int value)
        {
            if (this.gridIndex[x] == null)
            {
                this.gridIndex[x] = new CellsRow();
                for (int i = 0; i < 0x4e20; i++)
                {
                    this.gridIndex[x].Add(-1);
                }
            }
            this.gridIndex[x][z] = value;
        }

        internal override bool IsValidSeriesSource(Series value)
        {
            return (value is Custom3DGrid);
        }

        protected internal override int NumSampleValues()
        {
            return this.iNumXValues;
        }

        private void ReCreateValues()
        {
            this.CreateValues(this.iNumXValues, this.iNumZValues);
        }

        private int SearchSorted(int aCount, double[] values, double aValue)
        {
            int index = -1;
            int num2 = 0;
            int num3 = aCount - 1;
            while (num2 <= num3)
            {
                index = (num2 + num3) >> 1;
                if (values[index] < aValue)
                {
                    num2 = index + 1;
                }
                else
                {
                    if (values[index] == aValue)
                    {
                        return index;
                    }
                    num3 = index - 1;
                }
            }
            return -1;
        }

        private void SearchValue(ref int aCount, double[] values, double aValue)
        {
            if (this.SearchSorted(aCount, values, aValue) == -1)
            {
                values[aCount] = aValue;
                aCount++;
            }
        }

        private void SetGridIndex(int Index, int XCount, int ZCount, double[] XVals, double[] ZVals)
        {
            int index = 0;
        Label_0002:
            if (XVals[index] == base.XValues[Index])
            {
                int num2 = 0;
                do
                {
                    if (ZVals[num2] == base.ZValues[Index])
                    {
                        this[1 + index, 1 + num2] = Index;
                        return;
                    }
                    num2++;
                }
                while (num2 < ZCount);
            }
            else
            {
                index++;
                if (index < XCount)
                {
                    goto Label_0002;
                }
            }
        }

        private void SetNumZValues(int value)
        {
            if (value != this.iNumZValues)
            {
                this.iNumZValues = value;
                this.ReCreateValues();
            }
        }

        private void SortValues(int aCount, double[] values)
        {
            for (int i = 1; i < (aCount - 1); i++)
            {
                double num2 = values[i];
                for (int j = i + 1; j < aCount; j++)
                {
                    if (values[j] < num2)
                    {
                        double num3 = values[j];
                        values[j] = num2;
                        values[i] = num3;
                    }
                }
            }
        }

        private int ValuePosition(int aCount, double[] values, double aValue)
        {
            int index = 0;
            while ((aValue != values[index]) && (index < aCount))
            {
                index++;
            }
            index++;
            return index;
        }

        [Description("Determine if X and Z values are equi-distant or not."), DefaultValue(false)]
        public bool IrregularGrid
        {
            get
            {
                return this.irregularGrid;
            }
            set
            {
                base.SetBooleanProperty(ref this.irregularGrid, value);
            }
        }

        public int this[int x, int z]
        {
            get
            {
                int num = x;
                int num2 = z;
                if (this.gridIndex[num] == null)
                {
                    return -1;
                }
                return this.gridIndex[num][num2];
            }
            set
            {
                int num = x;
                int num2 = z;
                if (((num >= 0) && (num < 0x4e20)) && ((num2 >= 0) && (num2 < 0x4e20)))
                {
                    this.InternalSetGridIndex(num, num2, value);
                }
            }
        }

        [DefaultValue(10), Description("Determines the Surface's horizontal size in number of points.")]
        public int NumXValues
        {
            get
            {
                return this.iNumXValues;
            }
            set
            {
                if (this.iNumXValues != value)
                {
                    this.iNumXValues = value;
                    this.ReCreateValues();
                }
            }
        }

        [DefaultValue(10), Description("Determines the Surface's depth size in number of points.")]
        public int NumZValues
        {
            get
            {
                return this.iNumZValues;
            }
            set
            {
                if (this.iNumZValues != value)
                {
                    this.iNumZValues = value;
                    this.ReCreateValues();
                }
            }
        }

        protected sealed class CellsRow : List<int>
        {
            public CellsRow() : base(0x4e20)
            {
            }
        }

        public class GetYValueEventArgs : EventArgs
        {
            private double lValue;
            private readonly int x;
            private readonly int z;

            public GetYValueEventArgs(int X, int Z, double Value)
            {
                this.x = X;
                this.z = Z;
                this.lValue = Value;
            }

            public double Value
            {
                get
                {
                    return this.lValue;
                }
                set
                {
                    this.lValue = value;
                }
            }

            public int X
            {
                get
                {
                    return this.x;
                }
            }

            public int Z
            {
                get
                {
                    return this.z;
                }
            }
        }

        public delegate void GetYValueEventHandler(Series sender, Custom3DGrid.GetYValueEventArgs e);

        protected sealed class PCellsRow : List<Custom3DGrid.CellsRow>
        {
        }
    }
}

