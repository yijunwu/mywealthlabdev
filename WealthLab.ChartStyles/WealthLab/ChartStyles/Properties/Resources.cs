namespace WealthLab.ChartStyles.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), CompilerGenerated, DebuggerNonUserCode]
    internal class Resources
    {
        private static CultureInfo cultureInfo_0;
        private static System.Resources.ResourceManager resourceManager_0;

        internal Resources()
        {
        }

        internal static Bitmap BarChartStyle
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("BarChartStyle", cultureInfo_0);
            }
        }

        internal static Bitmap CandleChartStyle
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("CandleChartStyle", cultureInfo_0);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return cultureInfo_0;
            }
            set
            {
                cultureInfo_0 = value;
            }
        }

        internal static Bitmap EquicandleChartStyle
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("EquicandleChartStyle", cultureInfo_0);
            }
        }

        internal static Bitmap EquivolumeChartStyle
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("EquivolumeChartStyle", cultureInfo_0);
            }
        }

        internal static Bitmap LineChartStyle
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("LineChartStyle", cultureInfo_0);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceManager_0, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("WealthLab.ChartStyles.Properties.Resources", typeof(Resources).Assembly);
                    resourceManager_0 = manager;
                }
                return resourceManager_0;
            }
        }
    }
}

