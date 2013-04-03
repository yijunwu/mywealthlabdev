namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public abstract class StrategyScorecard
    {
        protected StrategyScorecard()
        {
        }

        public virtual string GetFormatCode(string metric)
        {
            return "N2";
        }

        public abstract void PopulateScorecard(ListViewItem listViewItem_0, SystemPerformance performance);
        public void SetupListViewColumns(ListView listView_0, PositionSize positionSize_0, int fixedColumns)
        {
            IList<string> columnHeadersRawProfit;
            IList<string> columnTypesRawProfit;
            foreach (ListViewItem item in listView_0.Items)
            {
                for (int j = item.SubItems.Count - 1; j > (fixedColumns - 1); j--)
                {
                    item.SubItems.RemoveAt(j);
                }
                item.StateImageIndex = -1;
            }
            while (listView_0.Columns.Count > fixedColumns)
            {
                listView_0.Columns.RemoveAt(listView_0.Columns.Count - 1);
            }
            if (positionSize_0.RawProfitMode)
            {
                columnHeadersRawProfit = this.ColumnHeadersRawProfit;
                columnTypesRawProfit = this.ColumnTypesRawProfit;
            }
            else
            {
                columnHeadersRawProfit = this.ColumnHeadersPortfolioSim;
                columnTypesRawProfit = this.ColumnTypesPortfolioSim;
            }
            for (int i = 0; i < columnHeadersRawProfit.Count; i++)
            {
                listView_0.Columns.Add(columnHeadersRawProfit[i]);
                listView_0.Columns[i + fixedColumns].Tag = columnTypesRawProfit[i];
                if ((columnTypesRawProfit[i] == "N") || (columnTypesRawProfit[i] == "C"))
                {
                    listView_0.Columns[i + fixedColumns].TextAlign = HorizontalAlignment.Right;
                }
            }
        }

        public override string ToString()
        {
            return this.FriendlyName;
        }

        public abstract IList<string> ColumnHeadersPortfolioSim { get; }

        public abstract IList<string> ColumnHeadersRawProfit { get; }

        public abstract IList<string> ColumnTypesPortfolioSim { get; }

        public abstract IList<string> ColumnTypesRawProfit { get; }

        public abstract string FriendlyName { get; }
    }
}

