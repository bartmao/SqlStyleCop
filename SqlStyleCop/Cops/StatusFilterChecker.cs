using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SqlStyleCop
{
    public class StatusFilterChecker : TableFilterChecker
    {
        public StatusFilterChecker()
            : base("Status", "Status <> 'd'"
                , "Check whether the status <> 'd' filter is added for all referenced tables.")
        {
        }

        protected override string DoValidInternal(XmlNode boolComparsionNode)
        {
            if (boolComparsionNode.ChildNodes.Count != 2) return null;
            if (boolComparsionNode.Attributes["ComparisonOperator"].Value != "LessOrGreaterThan") return null;

            var objectNode = boolComparsionNode.SelectSingleNode(".//SqlObjectIdentifier");
            if (objectNode == null) return null;
            if (!objectNode.Attributes["ObjectName"].Value.Equals("Status", StringComparison.InvariantCultureIgnoreCase)) return null;

            var literalNode = boolComparsionNode.SelectSingleNode("./SqlLiteralExpression");
            if (!literalNode.Attributes["Value"].Value.Equals("d", StringComparison.InvariantCultureIgnoreCase)) return null;

            // Satisfied, remove table
            var schema = objectNode.Attributes["SchemaName"];
            if (schema == null && ChkColumnRefTables.Count == 1) return ChkColumnRefTables[0].TableName;
            else if (schema != null)
                return schema.Value;
            return null;
        }
    }
}
