namespace QWhale.SyntaxSettings
{
    using System;
    using System.Runtime.InteropServices;
    using System.Xml;

    public class SyntaxSettingsConverter
    {
        private UpdateInfo[] UpdateItems = new UpdateInfo[] { new UpdateInfo("HighlightUrls", "HighlightHyperText"), new UpdateInfo("xmlFontName", string.Empty), new UpdateInfo("xmlFontSize", string.Empty), new UpdateInfo("xmlFontStyle", string.Empty), new UpdateInfo("LexStyle", "Style"), new UpdateInfo("ReadOnly", "Readonly"), new UpdateInfo("Name", "Name", "braces", ""), new UpdateInfo("Name", "Name", "operators", ""), new UpdateInfo("Name", "Name", "functions", ""), new UpdateInfo("Name", "Name", "objects", ""), new UpdateInfo("Name", "Name", "newobjects", ""), new UpdateInfo("Name", "Name", "htmltags", "htmlparams") };

        private XmlNode DescendToNode(XmlNode root, string nodeText, bool nodeValue)
        {
            XmlNode nextSibling = root;
            XmlNode node2 = null;
            while (nextSibling != null)
            {
                if (!nodeValue && (nextSibling.Name == nodeText))
                {
                    return nextSibling;
                }
                if (nodeValue && (nextSibling.InnerText == nodeText))
                {
                    return nextSibling;
                }
                if (nextSibling.HasChildNodes)
                {
                    node2 = this.DescendToNode(nextSibling.FirstChild, nodeText, nodeValue);
                }
                if (node2 != null)
                {
                    if (!nodeValue && (node2.Name == nodeText))
                    {
                        return node2;
                    }
                    if (nodeValue && (node2.InnerText == nodeText))
                    {
                        return node2;
                    }
                }
                nextSibling = nextSibling.NextSibling;
            }
            return null;
        }

        public void UpdateSettings(ref XmlDocument doc)
        {
            XmlNode oldChild = null;
            bool nodeValue = true;
            oldChild = this.DescendToNode(doc.FirstChild, "ColorThemes", false);
            if (oldChild != null)
            {
                XmlElement newChild = doc.CreateElement("ColorThemes");
                int count = oldChild.ChildNodes.Count;
                for (int j = 0; j < count; j++)
                {
                    XmlNode node2 = oldChild.ChildNodes[0];
                    oldChild.RemoveChild(node2);
                    newChild.AppendChild(node2);
                }
                oldChild.AppendChild(newChild);
            }
            for (int i = 0; i < this.UpdateItems.Length; i++)
            {
                nodeValue = this.UpdateItems[i].OldTag == this.UpdateItems[i].NewTag;
                oldChild = this.DescendToNode(doc.FirstChild, nodeValue ? this.UpdateItems[i].OldText : this.UpdateItems[i].OldTag, nodeValue);
                if (!nodeValue)
                {
                    goto Label_0203;
                }
                if ((oldChild != null) && (oldChild.ParentNode != null))
                {
                    if (this.UpdateItems[i].NewText != string.Empty)
                    {
                        oldChild.InnerText = this.UpdateItems[i].NewText;
                    }
                    else if (oldChild.ParentNode.ParentNode != null)
                    {
                        oldChild.ParentNode.ParentNode.RemoveChild(oldChild.ParentNode);
                    }
                }
                continue;
            Label_0154:
                if ((oldChild != null) && (oldChild.ParentNode != null))
                {
                    if (this.UpdateItems[i].NewTag != string.Empty)
                    {
                        oldChild.ParentNode.InnerXml = oldChild.ParentNode.InnerXml.Replace(this.UpdateItems[i].OldTag, this.UpdateItems[i].NewTag);
                    }
                    else
                    {
                        oldChild.ParentNode.RemoveChild(oldChild);
                    }
                }
                oldChild = this.DescendToNode(doc.FirstChild, nodeValue ? this.UpdateItems[i].OldText : this.UpdateItems[i].OldTag, nodeValue);
            Label_0203:
                if (oldChild != null)
                {
                    goto Label_0154;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct UpdateInfo
        {
            public string OldTag;
            public string NewTag;
            public string OldText;
            public string NewText;
            public UpdateInfo(string oldTag, string newTag)
            {
                this.OldTag = oldTag;
                this.NewTag = newTag;
                this.OldText = string.Empty;
                this.NewText = string.Empty;
            }

            public UpdateInfo(string oldTag, string newTag, string oldText, string newText) : this(oldTag, newTag)
            {
                this.OldText = oldText;
                this.NewText = newText;
            }
        }
    }
}

