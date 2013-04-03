namespace WealthLab.Visualizers.Extensions.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode, CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    internal class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        internal static Bitmap arrowb
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("arrowb", resourceCulture);
            }
        }

        internal static Bitmap arrowf
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("arrowf", resourceCulture);
            }
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

        internal static Bitmap printer
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("printer", resourceCulture);
            }
        }

        internal static Bitmap printer11
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("printer11", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("WealthLab.Visualizers.Extensions.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }
    }
}

