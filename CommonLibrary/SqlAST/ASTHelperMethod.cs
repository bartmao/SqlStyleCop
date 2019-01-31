using Microsoft.SqlServer.Management.SqlParser.Parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace TST
{
    public static class ASTHelperMethod
    {
        public static XmlDocument ParseAndGetAST(string scriptContent)
        {
            // Parse and generate the AST
            var rst = Parser.Parse(scriptContent);
            var fieldInfo = rst.GetType().GetField("sqlScript", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            var script = fieldInfo.GetValue(rst);
            var stream = new MemoryStream();
            var writer = XmlWriter.Create(stream);
            script.GetType().InvokeMember("WriteXml", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod
               , null, script, new object[] { writer });
            writer.Flush();
            writer.Close();
            stream.Position = 0;
            var doc = new XmlDocument();
            doc.Load(stream);

            // For debugging purpose
#if DEBUG
            if (Directory.Exists("c:\\doc"))
                doc.Save("c:\\doc\\1.xml");
#endif
            // Removes the comment sections
            var removeList = new List<XmlNode>();
            XmlTraverser.DSTXmlNode(doc.ChildNodes[1], n =>
            {
                if (n.Name == "#comment")
                    removeList.Add(n);
            });
            removeList.ForEach(n => n.ParentNode.RemoveChild(n));

            return doc;
        }

        public static Tuple<Point, Point> GetPosition(XmlNode node)
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

        public static List<XmlNode> GetTokensUnderNode(XmlNode node)
        {
            var parentPos = GetPosition(node);
            return node.OwnerDocument.SelectNodes("//Token").Cast<XmlNode>()
                .Where(n =>
                {
                    var pos = GetPosition(n);
                    return ComparePoint(pos.Item1, parentPos.Item1) >= 0
                        && ComparePoint(pos.Item2, parentPos.Item2) <= 0;
                })
                .ToList();
        }

        public static long ComparePoint(Point p1, Point p2)
        {
            var i1 = ((long)p1.X << 32) + p1.Y;
            var i2 = ((long)p2.X << 32) + p2.Y;
            return i1 - i2;
        }

        public static string GetTableNameIfHasAlias(XmlNode queryNode, string maybeAlias)
        {
            var node = queryNode.SelectSingleNode("//SqlTableRefExpression[@Alias='" + maybeAlias + "']");
            if (node == null) return maybeAlias;
            return node.Attributes["ObjectIdentifier"].Value;
        }

        public static XmlNode GetTableDefinationNode(XmlDocument doc, string tableName)
        {
            return doc.SelectSingleNode("//SqlTableRefExpression[@ObjectIdentifier='" + tableName + "']");
        }

        public static List<TableDefination> GetTableDefinations(XmlDocument doc)
        {
            var refTables = doc.SelectNodes("//SqlTableRefExpression")
                .Cast<XmlNode>()
                .Select(n => new TableDefination()
                {
                    TableName = n.Attributes["ObjectIdentifier"].Value,
                    DefineNodeName = n.Name,
                    Alias = n.Attributes["Alias"] == null ? string.Empty : n.Attributes["Alias"].Value,
                    LocationStr = n.Attributes["Location"].Value
                });

            var cteTables = doc.SelectNodes("//SqlCommonTableExpression")
                .Cast<XmlNode>()
                .Select(n => new TableDefination()
                {
                    TableName = n.Attributes["Name"].Value,
                    DefineNodeName = n.Name,
                    Alias = string.Empty,
                    LocationStr = n.Attributes["Location"].Value
                });

            var derivedTables = doc.SelectNodes("//SqlDerivedTableExpression")
                .Cast<XmlNode>()
                .Select(n => new TableDefination()
                {
                    TableName = n.Attributes["Alias"].Value,
                    DefineNodeName = n.Name,
                    Alias = n.Attributes["Alias"].Value,
                    LocationStr = n.Attributes["Location"].Value
                });

            return refTables.Union(cteTables).Union(derivedTables).ToList();
        }

        public static List<TableDefination> GetTableRefsInCurSelect(XmlNode node)
        {
            var refTables = node.SelectNodes(".//SqlTableRefExpression", "SqlQuerySpecification")
                .Cast<XmlNode>()
                .Select(n => new TableDefination()
                {
                    TableName = n.Attributes["ObjectIdentifier"].Value,
                    DefineNodeName = n.Name,
                    Alias = n.Attributes["Alias"] == null ? string.Empty : n.Attributes["Alias"].Value,
                    LocationStr = n.Attributes["Location"].Value
                });

            var derivedTables = node.SelectNodes(".//SqlDerivedTableExpression", "SqlQuerySpecification")
                .Cast<XmlNode>()
                .Select(n => new TableDefination()
                {
                    TableName = n.Attributes["Alias"].Value,
                    DefineNodeName = n.Name,
                    Alias = n.Attributes["Alias"].Value,
                    LocationStr = n.Attributes["Location"].Value
                });

            var varTables = node.SelectNodes(".//SqlTableVariableRefExpression", "SqlQuerySpecification")
                .Cast<XmlNode>()
                .Select(n => new TableDefination()
                {
                    TableName = n.Name,
                    DefineNodeName = n.Name,
                    Alias = n.Attributes["Alias"] == null ? string.Empty : n.Attributes["Alias"].Value,
                    LocationStr = n.Attributes["Location"].Value
                });

            return refTables.Union(varTables).Union(derivedTables).ToList();
        }

        public static List<XmlNode> GetTokensBetweenPoints(XmlDocument doc, Point p1, Point p2)
        {
            return doc.SelectNodes("//Token").Cast<XmlNode>()
                .Where(n =>
                {
                    var pos = GetPosition(n);
                    return ComparePoint(p1, pos.Item1) <= 0
                        && ComparePoint(p2, pos.Item2) >= 0;
                })
                .ToList();
        }

        /// <summary>
        /// TableName,ColumnName,AliasName
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<Tuple<string, string,string>> GetSelectFieldsDefinedInTables(XmlDocument doc)
        {
            var rst = new List<Tuple<string, string, string>>();
            var targetQuery = doc.SelectSingleNode(".//SqlCreateProcedureStatement/*/SqlSelectStatement[last()]/SqlSelectSpecification/SqlQuerySpecification");
            if (targetQuery == null) return rst;

            var refTables = GetTableRefsInCurSelect(targetQuery);
            var selectNode = targetQuery.SelectSingleNode(".//SqlSelectClause");

            selectNode.SelectNodes("./SqlSelectScalarExpression")
                .Cast<XmlNode>()
                .ToList()
                .ForEach(n => 
                {
                    string aliasName = "";
                    if (n.Attributes["Alias"] != null) aliasName = n.Attributes["Alias"].Value;

                    if (n.FirstChild.Name.Equals("SqlScalarRefExpression"))
                    {
                        var identifiers = n.SelectNodes(".//SqlIdentifier");
                        var schemaName = identifiers.Item(0).Attributes["Value"].Value;
                        var objName = identifiers.Item(1).Attributes["Value"].Value;
                        var tableName = GetTableNameIfHasAlias(targetQuery, schemaName);

                        rst.Add(Tuple.Create(tableName, objName, aliasName));
                    }
                    else if (n.FirstChild.Name.Equals("SqlColumnRefExpression")) 
                    {
                        var colName = n.FirstChild.Attributes["ColumnName"].Value;
                        var table = refTables.FirstOrDefault(t=>t.Type==1 && t.Columns.Value.ContainsIgnoreCase(colName));
                        var tableName = table == null ? string.Empty : table.TableName;

                        rst.Add(Tuple.Create(tableName, colName, aliasName));
                    }
                });

            return rst;
        }
    }


}
