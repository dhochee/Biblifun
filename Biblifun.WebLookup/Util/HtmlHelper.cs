using Biblifun.Common.Util;
using HtmlAgilityPack;
using System.Text;

namespace Biblifun.WebLookup.Util
{
    public static class HtmlHelper
    {
        public static string ToFormattedHtml(this HtmlNodeCollection nodeCollection)
        {
            var sb = new StringBuilder();

            foreach(var node in nodeCollection)
            {
                var nodeHtml = node.ToFormattedHtml();
                if (!string.IsNullOrWhiteSpace(nodeHtml))
                {
                    sb.AppendLine(nodeHtml);
                }
            }

            return sb.ToString();
        }

        public static string ToFormattedHtml(this HtmlNode node)
        {
            return node.NodeType == HtmlNodeType.Element ?
                   XmlHelper.FormatXml(node.OuterHtml) :
                   node.InnerText.Trim();
        }
    }

}
