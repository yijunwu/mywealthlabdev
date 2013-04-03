namespace WealthLabPro
{
    using Fidelity.Components;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Xml.Serialization;

    [ToolboxBitmap(typeof(QuickRefManager), "QuickRefManager")]
    public class QuickRefManager : Component
    {
        private IContainer icontainer_0;
        private static readonly ILog ilog_0 = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<QuickRefCategory> list_0;

        public QuickRefManager()
        {
            this.list_0 = new List<QuickRefCategory>();
            this.method_0();
        }

        public QuickRefManager(IContainer container)
        {
            this.list_0 = new List<QuickRefCategory>();
            container.Add(this);
            this.method_0();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public void LoadFromFile(string fileName)
        {
            FileStream stream = null;
            try
            {
                stream = File.OpenRead(fileName);
                XmlSerializer serializer = new XmlSerializer(this.list_0.GetType());
                this.list_0 = (List<QuickRefCategory>) serializer.Deserialize(stream);
                foreach (QuickRefCategory category in this.list_0)
                {
                    category.Entries.Sort();
                }
                this.list_0.Sort();
            }
            catch (Exception exception)
            {
                ilog_0.Debug("QuickRefManager.LoadFromFile() threw exception accesing " + fileName, exception);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
        }

        public void SaveToFile(string fileName)
        {
            FileStream stream = null;
            try
            {
                FileNameValidator.ValidateFileName(fileName);
                stream = File.Create(fileName);
                new XmlSerializer(this.list_0.GetType()).Serialize((Stream) stream, this.list_0);
            }
            catch (Exception exception)
            {
                ilog_0.Debug("QuickRefManager.SaveToFile() threw exception saving " + fileName, exception);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public List<QuickRefCategory> Categories
        {
            get
            {
                return this.list_0;
            }
        }
    }
}

