using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TST
{
    /// <summary>
    /// Extensions of XML library
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Select nodes by <para>xpath</para> until encounter the <para>enNode</para>
        /// </summary>
        public static IEnumerable<XmlNode> SelectNodes(this XmlNode node, string xpath, string endNode)
        {
            var allNodes = node.SelectNodes(xpath).Cast<XmlNode>();
            return allNodes.Where(n => 
            {
                var parent = n.LookUpForFirstNode(endNode);
                if (parent == node || parent == null) return true;
                return false;
            });
        }

        /// <summary>
        /// Look up from <para>node</para> until find node that its name is in <para>nodeNames</para>
        /// </summary>
        public static XmlNode LookUpForFirstNode(this XmlNode node, params string[] nodeNames)
        {
            while (node.ParentNode != null)
                if (nodeNames.Contains(node.ParentNode.Name)) return node.ParentNode;
                else node = node.ParentNode;

            return null;
        }
    }
}
