namespace WealthLab.Extensions.Attribute
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Remoting.Contexts;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Soap;

    [Serializable, Synchronization, AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false)]
    public class ExtensionInfoAttribute : Attribute, IComparable
    {
        internal int _attributeVersion;
        internal string[] _deleteFiles;
        internal string _description;
        internal string _displayName;
        internal string _glyph100x100Name;
        internal string _glyph16x16Base64;
        internal string _glyph16x16Name;
        internal ExtensionHostApp _hostApp;
        internal ExtensionLicence _licence;
        internal string _licencePrice;
        internal string _maxDeveloperVersion;
        internal string _maxProVersion;
        internal string _minDeveloperVersion;
        internal string _minProVersion;
        internal string _postInstallBatch;
        internal string _postUninstallBatch;
        [OptionalField]
        internal string _postUpdateBatch;
        internal string _preInstallBatch;
        internal string _preUninstallBatch;
        [OptionalField]
        internal string _preUpdateBatch;
        internal string _publisher;
        internal string _publisherUrl;
        internal string _strongName;
        internal ExtensionType _type;
        internal string _version;

        internal ExtensionInfoAttribute()
        {
            this._attributeVersion = 1;
            this._hostApp = ExtensionHostApp.ProAndDeveloper;
        }

        public ExtensionInfoAttribute(ExtensionType type, string strongName, string displayName, string description, string version, string publisher, string glyph16x16Name, ExtensionLicence licence, string[] deleteFiles)
        {
            this._attributeVersion = 1;
            this._hostApp = ExtensionHostApp.ProAndDeveloper;
            this._type = type;
            this._strongName = strongName;
            this._displayName = displayName;
            this._description = description;
            this._version = version;
            this._publisher = publisher;
            this._glyph16x16Name = glyph16x16Name;
            this._licence = licence;
            this._deleteFiles = deleteFiles;
        }

        public int CompareTo(object obj)
        {
            return this.DisplayName.CompareTo((obj as ExtensionInfoAttribute).DisplayName);
        }

        public static Image ConvertBase64ToImage(string strBase64)
        {
            byte[] buffer = Convert.FromBase64String(strBase64);
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(buffer, 0, buffer.Length);
                return Image.FromStream(stream);
            }
        }

        internal static string ConvertStreamToBase64(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int) stream.Length);
            return Convert.ToBase64String(buffer);
        }

        public static ExtensionInfoAttribute DeserializeFromFile(string fileName)
        {
            SoapFormatter formatter = new SoapFormatter();
            object obj2 = null;
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                obj2 = formatter.Deserialize(stream);
            }
            return (ExtensionInfoAttribute) obj2;
        }

        public static ExtensionInfoAttribute DeserializeFromString(string soapString)
        {
            SoapFormatter formatter = new SoapFormatter();
            object obj2 = null;
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(soapString);
                    writer.Flush();
                    stream.Seek(0L, SeekOrigin.Begin);
                    obj2 = formatter.Deserialize(stream);
                }
            }
            return (ExtensionInfoAttribute) obj2;
        }

        public static ExtensionInfoAttribute GetAttributeFromAssembly(Assembly assembly)
        {
            try
            {
                return new AssemblyAnalysis().GetAttribute(assembly);
            }
            catch
            {
            }
            return null;
        }

        public static ExtensionInfoAttribute GetAttributeFromFile(string fileName, string attributeAssemblyDir)
        {
            AppDomain domain = AppDomain.CreateDomain("Attribute");
            ExtensionInfoAttribute extensionInfo = null;
            try
            {
                extensionInfo = ((AssemblyAnalysis) domain.CreateInstanceFromAndUnwrap(string.Concat(new object[] { attributeAssemblyDir, Path.DirectorySeparatorChar, Assembly.GetExecutingAssembly().GetName().Name, ".dll" }), typeof(AssemblyAnalysis).FullName)).GetExtensionInfo(fileName, attributeAssemblyDir);
            }
            finally
            {
                AppDomain.Unload(domain);
            }
            return extensionInfo;
        }

        public static Image GetGlyph(string fileName, string glyphName, string attributeAssemblyDir)
        {
            AppDomain domain = AppDomain.CreateDomain("Attribute");
            string str = null;
            try
            {
                str = ((AssemblyAnalysis) domain.CreateInstanceFromAndUnwrap(string.Concat(new object[] { attributeAssemblyDir, Path.DirectorySeparatorChar, Assembly.GetExecutingAssembly().GetName().Name, ".dll" }), typeof(AssemblyAnalysis).FullName)).GetGlyphBase64(fileName, glyphName, attributeAssemblyDir);
            }
            finally
            {
                AppDomain.Unload(domain);
            }
            if (str != null)
            {
                return ConvertBase64ToImage(str);
            }
            return null;
        }

        public void SerializeToFile(string fileName)
        {
            SoapFormatter formatter = new SoapFormatter();
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(stream, this);
            }
        }

        public string SerializeToString()
        {
            SoapFormatter formatter = new SoapFormatter();
            string str = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                using (StreamReader reader = new StreamReader(stream))
                {
                    stream.Seek(0L, SeekOrigin.Begin);
                    str = reader.ReadToEnd();
                }
            }
            return str;
        }

        public int AttributeVersion
        {
            get
            {
                return this._attributeVersion;
            }
        }

        public string[] DeleteFiles
        {
            get
            {
                return this._deleteFiles;
            }
        }

        public string Description
        {
            get
            {
                return this._description;
            }
        }

        public string DisplayName
        {
            get
            {
                return this._displayName;
            }
        }

        public string Glyph100x100Name
        {
            get
            {
                return this._glyph100x100Name;
            }
            set
            {
                this._glyph100x100Name = value;
            }
        }

        public string Glyph16x16Base64
        {
            get
            {
                return this._glyph16x16Base64;
            }
        }

        public string Glyph16x16Name
        {
            get
            {
                return this._glyph16x16Name;
            }
        }

        public ExtensionHostApp HostApp
        {
            get
            {
                return this._hostApp;
            }
            set
            {
                this._hostApp = value;
            }
        }

        public ExtensionLicence Licence
        {
            get
            {
                return this._licence;
            }
        }

        public string LicencePrice
        {
            get
            {
                return this._licencePrice;
            }
            set
            {
                this._licencePrice = value;
            }
        }

        public string MaxDeveloperVersion
        {
            get
            {
                return this._maxDeveloperVersion;
            }
            set
            {
                this._maxDeveloperVersion = value;
            }
        }

        public string MaxProVersion
        {
            get
            {
                return this._maxProVersion;
            }
            set
            {
                this._maxProVersion = value;
            }
        }

        public string MinDeveloperVersion
        {
            get
            {
                return this._minDeveloperVersion;
            }
            set
            {
                this._minDeveloperVersion = value;
            }
        }

        public string MinProVersion
        {
            get
            {
                return this._minProVersion;
            }
            set
            {
                this._minProVersion = value;
            }
        }

        public string PostInstallBatch
        {
            get
            {
                return this._postInstallBatch;
            }
            set
            {
                this._postInstallBatch = value;
            }
        }

        public string PostUninstallBatch
        {
            get
            {
                return this._postUninstallBatch;
            }
            set
            {
                this._postUninstallBatch = value;
            }
        }

        public string PostUpdateBatch
        {
            get
            {
                return this._postUpdateBatch;
            }
            set
            {
                this._postUpdateBatch = value;
            }
        }

        public string PreInstallBatch
        {
            get
            {
                return this._preInstallBatch;
            }
            set
            {
                this._preInstallBatch = value;
            }
        }

        public string PreUninstallBatch
        {
            get
            {
                return this._preUninstallBatch;
            }
            set
            {
                this._preUninstallBatch = value;
            }
        }

        public string PreUpdateBatch
        {
            get
            {
                return this._preUpdateBatch;
            }
            set
            {
                this._preUpdateBatch = value;
            }
        }

        public string Publisher
        {
            get
            {
                return this._publisher;
            }
        }

        public string PublisherUrl
        {
            get
            {
                return this._publisherUrl;
            }
            set
            {
                this._publisherUrl = value;
            }
        }

        public string StrongName
        {
            get
            {
                return this._strongName;
            }
        }

        public ExtensionType Type
        {
            get
            {
                return this._type;
            }
        }

        public string Version
        {
            get
            {
                return this._version;
            }
        }
    }
}

