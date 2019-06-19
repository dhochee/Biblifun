using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Biblifun.Common.Util
{
    public static class XmlHelper
    {
        // source: https://stackoverflow.com/a/14449850/773798
        public static string FormatXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
    }
}
