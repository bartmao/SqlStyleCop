using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace TST
{
    public class DatabaseManager
    {
        public static SqlConnection Connection;
        static Dictionary<string, List<string>> tableSchemas;

        static DatabaseManager()
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + "ConnectionStringFile.txt";

            if (File.Exists(filePath))
            {
                var constr = File.ReadAllText(filePath).Trim();
                if (string.IsNullOrEmpty(constr))
                    throw new InvalidDataException("The ConnectionString is NULL or Empty.");
                Connection = new SqlConnection(constr);
            }
            else
                throw new FileNotFoundException("Can not find the file that store database connection string.", filePath);
        }

        public static SPInfo GetSPInfo(string spName)
        {
            var ds = SqlHelper.ExecuteDataset(Connection, CommandType.Text
                , "select name, type_name(xusertype) type, length from syscolumns where id=object_id('" + spName + "') ORDER BY colid");
            if (ds.Tables.Count == 0) return null;

            var tb = ds.Tables[0];
            var spInfo = new SPInfo();
            var paraInfos = tb.Rows.Cast<DataRow>()
                .Select(r =>
                    new SPInfo.SPParaInfo()
                    {
                        Name = r["name"].ToString()
                        ,
                        DataType = (r["type"].ToString())
                        ,
                        DisplayName = HelperMethod.GetSuggestedName(r["name"].ToString())
                    }).ToList();

            var output = SqlHelper.ExecuteDataset(Connection, CommandType.Text
                , string.Format("exec {0} {1}", spName, string.Join(",", paraInfos.Select(p => p.Name + "=NULL")))).Tables[0];
            var fieldInfos = output.Columns.Cast<DataColumn>()
                .Select(c => new SPInfo.SPFieldInfo() { Name = HelperMethod.RemoveBlankSpaces(c.ColumnName), DataType = c.DataType.Name }).ToList();

            var contentRows = SqlHelper.ExecuteDataset(Connection, CommandType.Text, "sp_helptext "+ spName)
                .Tables[0].Rows.Cast<DataRow>();

            spInfo.PlainText = string.Concat(contentRows.Select(r => r[0].ToString()));
            spInfo.Name = spName;
            spInfo.ParaInfos = paraInfos;
            spInfo.FieldInfos = fieldInfos;
            spInfo.AST = ASTHelperMethod.ParseAndGetAST(spInfo.PlainText);

            return spInfo;
        }

        public static List<string> GetAllSpNames()
        {
            var ds = SqlHelper.ExecuteDataset(Connection, CommandType.Text
               , "SELECT name FROM sys.objects WHERE type in (N'P', N'PC') order by name");
            return ds.Tables[0].Rows.Cast<DataRow>().Select(r => r[0].ToString()).ToList();
        }

        public static Dictionary<string, List<string>> GetTableSchemas()
        {
            if (tableSchemas != null) return tableSchemas;
            tableSchemas = new Dictionary<string, List<string>>();
            var tables = SqlHelper.ExecuteDataset(Connection, CommandType.Text
              , @"select o.name as TableName, c.name as ColumnName from sysobjects o
                left join syscolumns c on o.id = c.id
                where o.xtype = 'U'").Tables[0];
            var objectIds = tables.Rows.Cast<DataRow>()
                .Select(r => Tuple.Create(r["TableName"].ToString(), r["ColumnName"].ToString()));
            foreach (var obj in objectIds)
                if (tableSchemas.ContainsKey(obj.Item1)) tableSchemas[obj.Item1].Add(obj.Item2);
                else tableSchemas.Add(obj.Item1, new List<string>() { obj.Item2 });

            return tableSchemas;
        }

        public static List<string> GetTableColumns(string tableName)
        {
            var tables = GetTableSchemas();
            var qualifiedTableName = tables.Keys.Cast<string>().FirstOrDefault(k => k.Equals(tableName, StringComparison.InvariantCultureIgnoreCase));
            if (qualifiedTableName != null)
                return tables[qualifiedTableName];
            return new List<string>();
        }
    }
}
