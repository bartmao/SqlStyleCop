using System;
using System.Collections.Generic;
using System.Xml;

namespace TST
{
    /// <summary>
    /// Traverses the Xml nodes
    /// </summary>
    public static class XmlTraverser
    {
        /// <summary>
        /// Breadth-first visit XML node
        /// </summary>
        public static void BSTXmlNode(XmlNode node, Action<XmlNode> action)
        {
            var q = new Queue<XmlNode>();
            q.Enqueue(node);

            while (q.Count != 0) 
            {
                action(q.Dequeue());
                foreach (XmlNode child in node.ChildNodes)
                    q.Enqueue(child);
            }
        }

        /// <summary>
        /// Depth-first visit XML node
        /// </summary>
        public static void DSTXmlNode(XmlNode node, Action<XmlNode> action)
        {
            action(node);

            foreach (XmlNode child in node.ChildNodes)
            {
                DSTXmlNode(child, action);
            }
        }
    }
}
