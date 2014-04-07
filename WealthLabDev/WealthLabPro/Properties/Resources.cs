namespace WealthLabPro.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode]
    internal class Resources
    {
        private static CultureInfo cultureInfo_0;
        private static System.Resources.ResourceManager resources;

        internal Resources()
        {
        }

        internal static Icon Accounts
        {
            get
            {
                return (Icon) ResourceManager.GetObject("Accounts", cultureInfo_0);
            }
        }

        internal static Icon Chart
        {
            get
            {
                return (Icon) ResourceManager.GetObject("Chart", cultureInfo_0);
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

        internal static Icon DataSources
        {
            get
            {
                return (Icon) ResourceManager.GetObject("DataSources", cultureInfo_0);
            }
        }

        internal static string DotNetReferences
        {
            get
            {
                return ResourceManager.GetString("DotNetReferences", cultureInfo_0);
            }
        }

        internal static Icon Futures
        {
            get
            {
                return (Icon) ResourceManager.GetObject("Futures", cultureInfo_0);
            }
        }

        internal static Icon Home
        {
            get
            {
                return (Icon) ResourceManager.GetObject("Home", cultureInfo_0);
            }
        }

        internal static Icon Icon_0
        {
            get
            {
                return (Icon) ResourceManager.GetObject("wlp", cultureInfo_0);
            }
        }

        internal static UnmanagedMemoryStream kerchunk
        {
            get
            {
                return ResourceManager.GetStream("kerchunk", cultureInfo_0);
            }
        }

        internal static UnmanagedMemoryStream Metronome2
        {
            get
            {
                return ResourceManager.GetStream("Metronome2", cultureInfo_0);
            }
        }

        internal static Icon Orders
        {
            get
            {
                return (Icon) ResourceManager.GetObject("Orders", cultureInfo_0);
            }
        }

        internal static string QuickRefPre
        {
            get
            {
                return ResourceManager.GetString("QuickRefPre", cultureInfo_0);
            }
        }

        internal static Icon Quotes
        {
            get
            {
                return (Icon) ResourceManager.GetObject("Quotes", cultureInfo_0);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resources, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("WealthLabPro.Properties.Resources", typeof(Resources).Assembly);
                    resources = manager;
                }
                return resources;
            }
        }

        internal static UnmanagedMemoryStream shortbep
        {
            get
            {
                return ResourceManager.GetStream("shortbep", cultureInfo_0);
            }
        }

        internal static Bitmap Slider
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("Slider", cultureInfo_0);
            }
        }

        internal static Icon StrategyCenter
        {
            get
            {
                return (Icon) ResourceManager.GetObject("StrategyCenter", cultureInfo_0);
            }
        }

        internal static string StrategyTemplate
        {
            get
            {
                return ResourceManager.GetString("StrategyTemplate", cultureInfo_0);
            }
        }

        internal static Bitmap Streaming
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("Streaming", cultureInfo_0);
            }
        }

        internal static Bitmap StreamingDisconnect
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("StreamingDisconnect", cultureInfo_0);
            }
        }

        internal static Bitmap StreamingSymbolsOff
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("StreamingSymbolsOff", cultureInfo_0);
            }
        }

        internal static Bitmap StreamingSymbolsOn
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("StreamingSymbolsOn", cultureInfo_0);
            }
        }

        internal static string ThirdPartyContentWarning
        {
            get
            {
                return ResourceManager.GetString("ThirdPartyContentWarning", cultureInfo_0);
            }
        }

        internal static Bitmap wlp_push
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("wlp_push", cultureInfo_0);
            }
        }
    }
}

