using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TST;

namespace SqlStyleCop
{
    public abstract class TableFilterChecker : ASTTraverseHandler
    {
        protected List<TableDefination> ChkColumnRefTables { get; set; }

        public string ColumnName { get; set; }

        public string Filter { get; set; }

        public string TableName { get; set; }

        public TableFilterChecker(string columnName, string filter, string description, string limitTableName = null)
            : base("SqlQuerySpecification"
            , description)
        {
            ColumnName = columnName;
            Filter = filter;
            TableName = limitTableName;
        }

        public override void HandleNode()
        {
            ChkColumnRefTables = ASTHelperMethod.GetTableRefsInCurSelect(Node).Where(d =>
            {
                if (d.Type != 1) return false;
                return DatabaseManager.GetTableColumns(d.TableName).Contains(ColumnName);
            }).ToList();

            if (!string.IsNullOrEmpty(TableName))
            {
                ChkColumnRefTables.RemoveAll(t => !t.TableName.Equals(TableName, StringComparison.InvariantCultureIgnoreCase));
            }

            var boolComparsions = Node.SelectNodes(".//SqlComparisonBooleanExpression");
            foreach (XmlNode boolComparsion in boolComparsions) DoValidStatus(boolComparsion);

            if (ChkColumnRefTables.Count > 0)
                ChkColumnRefTables.ForEach(t =>
                {
                    WriteLog(string.Format("Table [{0}] does not contain the filter {1}", t.TableName, Filter), 2, t.LocationStr);
                });
        }

        private void DoValidStatus(XmlNode n)
        {
            var schemaName = DoValidInternal(n);
            if (!string.IsNullOrEmpty(schemaName))
            {
                var tableName = ASTHelperMethod.GetTableNameIfHasAlias(StyleCopContext.Doc, schemaName);
                var firstMatch = ChkColumnRefTables.FirstOrDefault(t => schemaName.Equals(t.Alias, StringComparison.InvariantCultureIgnoreCase));
                if (firstMatch == null)
                    firstMatch = ChkColumnRefTables.FirstOrDefault(t => t.TableName.Equals(tableName, StringComparison.InvariantCultureIgnoreCase));
                if (firstMatch != null)
                    ChkColumnRefTables.Remove(firstMatch);
            }
        }

        protected abstract string DoValidInternal(XmlNode boolComparsionNode);
    }
}
