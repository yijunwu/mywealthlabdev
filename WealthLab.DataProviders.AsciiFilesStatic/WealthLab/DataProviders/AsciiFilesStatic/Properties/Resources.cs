namespace WealthLab.DataProviders.AsciiFilesStatic.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), CompilerGenerated]
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

        internal static Bitmap folder_page_white
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("folder_page_white", resourceCulture);
            }
        }

        internal static Bitmap help
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("help", resourceCulture);
            }
        }

        internal static Bitmap notepad
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("notepad", resourceCulture);
            }
        }

        internal static Bitmap page_white_text
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("page_white_text", resourceCulture);
            }
        }

        internal static Bitmap Refresh
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("Refresh", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("WealthLab.DataProviders.AsciiFilesStatic.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static Icon TextFiles
        {
            get
            {
                return (Icon) ResourceManager.GetObject("TextFiles", resourceCulture);
            }
        }
    }
}

