using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TST;

namespace SqlStyleCop
{
    public class SchemaCaseChecker : ASTTraverseHandler
    {
        Dictionary<string, List<string>> TableSchemas { get; set; }
        Dictionary<string, string> TableLowers { get; set; }

        public SchemaCaseChecker()
            : base("SqlObjectIdentifier"
            , "Check the table and column names and whether they are exactly match to the database")
        {
            TableSchemas = DatabaseManager.GetTableSchemas();
            TableLowers = TableSchemas.Keys.ToDictionary(k => k.ToLower());
        }

        public override void HandleNode()
        {
            var schemaName = Node.Attributes["SchemaName"] != null ? Node.Attributes["SchemaName"].Value : null;
            var objectName = Node.Attributes["ObjectName"] != null ? Node.Attributes["ObjectName"].Value : null;

            string tableName = null;
            string colName = null;
            string tablePos = null;
            string colPos = null;

            if (schemaName != null)
            {
                tableName = ASTHelperMethod.GetTableNameIfHasAlias(Node.OwnerDocument, schemaName);
                colName = objectName;
                tablePos = Node.ChildNodes[0].Attributes["Location"].Value;
                colPos = Node.ChildNodes[1].Attributes["Location"].Value;
            }
            else if (objectName != null)
            {
                if (Node.LookUpForFirstNode("SqlTableRefExpression"
                    , "SqlSelectStarExpression") != null)
                {
                    tableName = ASTHelperMethod.GetTableNameIfHasAlias(Node.OwnerDocument, objectName);
                    tablePos = Node.ChildNodes[0].Attributes["Location"].Value;
                }
                else if (Node.LookUpForFirstNode("SqlColumnRefExpression") != null)
                {
                    colName = objectName;
                    colPos = Node.ChildNodes[0].Attributes["Location"].Value;
                }
            }

            if (tableName != null)
            {
                var isActualTable = ASTHelperMethod.GetTableDefinationNode(Node.OwnerDocument, tableName) != null;
                if (!isActualTable) return;

                var lowerTableName = tableName.ToLower();

                if (TableLowers.ContainsKey(lowerTableName))
                {
                    var isAlias = tableName != schemaName && tableName != objectName;
                    if (!isAlias && TableLowers[lowerTableName] != tableName)
                        WriteLog(string.Format("Table name [{0}] does not exactly match.", tableName), 1, tablePos);

                    var cols = TableSchemas[TableLowers[lowerTableName]];
                    var colDict = cols.ToDictionary(i => i.ToLower());

                    if (colName == null) return;
                    var lowerColName = colName.ToLower();
                    if (!colDict.ContainsKey(lowerColName))
                        WriteLog(string.Format("Table {0} does not contains the column {1}.", tableName, colName), 1, colPos);
                    else
                    {
                        if (colDict[lowerColName] != colName)
                            WriteLog(string.Format("Column name [{0}] does not exactly match.", colName), 1, colPos);
                    }
                }

            }
        }
    }
}
