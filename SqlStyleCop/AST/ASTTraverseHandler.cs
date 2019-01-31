using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SqlStyleCop
{
    /// <summary>
    /// Base class to handler AST nodes.
    /// </summary>
    public abstract class ASTTraverseHandler
    {
        public string NodeName { get { return Node != null ? Node.Name : string.Empty; } }

        public XmlNode Node { get; set; }

        public List<string> FilterNames { get; set; }

        public string Description { get; set; }

        public string TypeName { get { return this.GetType().Name; } }

        public StyleCopContext StyleCopContext { get; set; }

        public string Position
        {
            get
            {
                var attr = Node.Attributes["Location"] ?? Node.Attributes["location"];
                if (attr == null) return "(-1,-1)";
                return attr.Value;
            }
        }

        public ASTTraverseHandler(string filterName, string description = "")
        {
            FilterNames = new List<string>() { filterName };
            Description = description;
        }

        public ASTTraverseHandler(List<string> filterNames, string description = "")
        {
            FilterNames = filterNames;
            Description = description;
        }

        public abstract void HandleNode();

        protected static Tuple<Point, Point> GetPosition(XmlNode node)
        {
            var attr = node.Attributes["Location"] ?? node.Attributes["location"];
            if (attr == null) return Tuple.Create(Point.Empty, Point.Empty);
            var pos = attr.Value;
            var arr = pos.Split(',').Select(s => s.Trim()).ToList();
            var pos1 = new Point(int.Parse(arr[0].Substring(arr[0].LastIndexOf('(') + 1))
                , int.Parse(arr[1].Substring(0, arr[1].Length - 1)));
            var pos2 = new Point(int.Parse(arr[2].Substring(1, arr[2].Length - 1))
                , int.Parse(arr[3].Substring(0, arr[3].Length - 2)));

            return Tuple.Create(pos1, pos2);
        }

        protected List<XmlNode> GetTokensUnderNode()
        {
            var parentPos = GetPosition(Node);
            return Node.OwnerDocument.SelectNodes("//Token").Cast<XmlNode>()
                .Where(n =>
                {
                    var pos = GetPosition(n);
                    return ComparePoint(pos.Item1, parentPos.Item1) >= 0
                        && ComparePoint(pos.Item2, parentPos.Item2) <= 0;
                })
                .ToList();
        }

        protected static long ComparePoint(Point p1, Point p2)
        {
            var i1 = ((long)p1.X << 32) + p1.Y;
            var i2 = ((long)p2.X << 32) + p2.Y;
            return i1 - i2;
        }

        protected void WriteLog(string content, int level = 1, string pos = null)
        {
            StyleCopContext.WriteLog(pos ?? Position, content, level);
        }
    }
}
