namespace WealthLab.Properties
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

        internal static Bitmap CrossHair
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("CrossHair", cultureInfo_0);
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

        internal static Bitmap FundamentalItem
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("FundamentalItem", cultureInfo_0);
            }
        }

        internal static Bitmap LongEntry
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("LongEntry", cultureInfo_0);
            }
        }

        internal static Bitmap LongExitLoss
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("LongExitLoss", cultureInfo_0);
            }
        }

        internal static Bitmap LongExitProfit
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("LongExitProfit", cultureInfo_0);
            }
        }

        internal static string MultiPosition
        {
            get
            {
                return ResourceManager.GetString("MultiPosition", cultureInfo_0);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceManager_0, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("WealthLab.Properties.Resources", typeof(Resources).Assembly);
                    resourceManager_0 = manager;
                }
                return resourceManager_0;
            }
        }

        internal static Bitmap ShortEntry
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ShortEntry", cultureInfo_0);
            }
        }

        internal static Bitmap ShortExitLoss
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ShortExitLoss", cultureInfo_0);
            }
        }

        internal static Bitmap ShortExitProfit
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ShortExitProfit", cultureInfo_0);
            }
        }

        internal static string SinglePosition
        {
            get
            {
                return ResourceManager.GetString("SinglePosition", cultureInfo_0);
            }
        }

        internal static Bitmap Sphere
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("Sphere", cultureInfo_0);
            }
        }

        internal static Bitmap wl_index
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("wl_index", cultureInfo_0);
            }
        }
    }
}

