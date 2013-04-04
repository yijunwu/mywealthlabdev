namespace QWhale.Editor.Serialization
{
    using QWhale.Common;
    using System;
    using System.Xml.Serialization;

    public class XmlImport : FmtImport
    {
        public override bool Read()
        {
            if (base.edit != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XmlSyntaxEditInfo));
                try
                {
                    base.edit.SerializationInfo = (ISerializationInfo) serializer.Deserialize(base.reader);
                    return true;
                }
                catch (Exception exception)
                {
                    ErrorHandler.Error(exception);
                    return false;
                }
            }
            return false;
        }
    }
}

