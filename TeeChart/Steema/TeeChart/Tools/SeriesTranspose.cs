namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(SeriesTranspose), "ToolsIcons.SeriesTranspose.bmp"), Description("Transpose series")]
    public class SeriesTranspose : Tool
    {
        public SeriesTranspose() : this(null)
        {
        }

        public SeriesTranspose(Chart c) : base(c)
        {
        }

        public void Transpose()
        {
            SeriesCollection seriess = base.Chart.Series;
            int count = seriess.Count;
            if (count > 0)
            {
                int num2 = seriess[0].Count;
                for (int i = 1; i < count; i++)
                {
                    if (seriess[i].Count > num2)
                    {
                        num2 = seriess[i].Count;
                    }
                }
                for (int j = 0; j < num2; j++)
                {
                    Series series = seriess.Chart.Series.Add(seriess[0].GetType());
                    for (int m = 0; m < count; m++)
                    {
                        ValueList mandatory = seriess[m].mandatory;
                        if (mandatory.Count > j)
                        {
                            if (seriess[m].IsNull(j))
                            {
                                series.Add(mandatory.Value[j], Color.Transparent);
                            }
                            else
                            {
                                series.Add(mandatory.Value[j]);
                            }
                        }
                        else
                        {
                            series.Add();
                        }
                    }
                }
                for (int k = 1; k <= count; k++)
                {
                    seriess[0].Dispose();
                }
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.SeriesTranspose;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.SeriesTransposeSummary;
            }
        }
    }
}

