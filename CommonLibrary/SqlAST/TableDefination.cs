using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TST
{
    public class TableDefination
    {
        public TableDefination()
        {
            Columns = new Lazy<List<string>>(() => 
            {
                if (Type != 1 || string.IsNullOrEmpty(TableName)) return new List<string>();
                return DatabaseManager.GetTableColumns(TableName);
            });
        }

        public string TableName { get; set; }

        public string DefineNodeName { get; set; }

        public string Alias { get; set; }

        public string LocationStr { get; set; }

        public Lazy<List<string>> Columns { get; set; }

        public static TableDefination CreateTableDefination(XmlNode n)
        {
            switch (n.Name)
            {
                case "SqlTableRefExpression":
                    return new TableDefination()
                    {
                        TableName = n.Attributes["ObjectIdentifier"].Value,
                        DefineNodeName = n.Name,
                        Alias = n.Attributes["Alias"] == null ? string.Empty : n.Attributes["Alias"].Value,
                        LocationStr = n.Attributes["Location"].Value
                    };
                case "SqlCommonTableExpression":
                    return new TableDefination()
                    {
                        TableName = n.Attributes["Name"].Value,
                        DefineNodeName = n.Name,
                        Alias = string.Empty,
                        LocationStr = n.Attributes["Location"].Value
                    };
                case "SqlDerivedTableExpression":
                    return new TableDefination()
                    {
                        TableName = n.Attributes["Alias"].Value,
                        DefineNodeName = n.Name,
                        Alias = n.Attributes["Alias"].Value,
                        LocationStr = n.Attributes["Location"].Value
                    };
                default:
                    return null;
            }
        }

        public int Type
        {
            get
            {
                switch (DefineNodeName)
                {
                    case "SqlTableRefExpression":
                        return 1;
                    case "SqlCommonTableExpression":
                        return 2;
                    case "SqlDerivedTableExpression":
                        return 3;
                    case "SqlTableVariableRefExpression":
                        return 4;
                    default:
                        return -1;
                }
            }
        }
    }
}
