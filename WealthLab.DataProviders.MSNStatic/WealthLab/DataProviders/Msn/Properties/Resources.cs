namespace WealthLab.DataProviders.Msn.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode]
    internal class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static Bitmap Dividend
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("Dividend", resourceCulture);
            }
        }

        internal static Bitmap MSN
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("MSN", resourceCulture);
            }
        }

        internal static Bitmap MSNFund
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("MSNFund", resourceCulture);
            }
        }

        internal static Bitmap MSNFundEM
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("MSNFundEM", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("WealthLab.DataProviders.Msn.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static Bitmap Split
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("Split", resourceCulture);
            }
        }
    }
}

