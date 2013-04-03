namespace WealthLab.ChartStyles.Trending.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    internal class Resources
    {
        private static CultureInfo cultureInfo_0;
        private static System.Resources.ResourceManager resourceManager_0;

        internal Resources()
        {
        }

        internal static Bitmap ChartStyleKagi
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ChartStyleKagi", cultureInfo_0);
            }
        }

        internal static Bitmap ChartStyleLineBreak
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ChartStyleLineBreak", cultureInfo_0);
            }
        }

        internal static Bitmap ChartStylePnF
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ChartStylePnF", cultureInfo_0);
            }
        }

        internal static Bitmap ChartStyleRenko
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ChartStyleRenko", cultureInfo_0);
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

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceManager_0, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("WealthLab.ChartStyles.Trending.Properties.Resources", typeof(Resources).Assembly);
                    resourceManager_0 = manager;
                }
                return resourceManager_0;
            }
        }
    }
}

