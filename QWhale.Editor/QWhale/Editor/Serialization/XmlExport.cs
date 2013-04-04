namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using System;
    using System.Xml.Serialization;

    public class XmlExport : FmtExport
    {
        public override bool Write()
        {
            bool flag = base.edit != null;
            if (flag)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XmlSyntaxEditInfo));
                try
                {
                    serializer.Serialize(base.writer, base.edit.SerializationInfo);
                }
                catch (Exception exception)
                {
                    base.writer.Flush();
                    ErrorHandler.Error(exception);
                }
            }
            return flag;
        }
    }
}

