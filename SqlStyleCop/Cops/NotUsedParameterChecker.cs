using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SqlStyleCop
{
    public class NotUsedParameterChecker : ASTTraverseHandler
    {
        public NotUsedParameterChecker()
            : base("SqlBatch", "Check whether any paramter is declared but never used.")
        {
        }

        public override void HandleNode()
        {
            var varDict = new Dictionary<string, string>();

            Node.SelectNodes(".//SqlInlineTableVariableDeclaration").Cast<XmlNode>()
                .Select(n => new KeyValuePair<string, string>(n.Attributes["Name"].Value, n.Attributes["Location"].Value))
                .ToList()
                .ForEach(n => varDict.Add(n.Key, n.Value));
            Node.SelectNodes(".//SqlVariableDeclaration").Cast<XmlNode>()
                .Select(n => new KeyValuePair<string, string>(n.Attributes["Name"].Value, n.Attributes["Location"].Value))
                .ToList()
                .ForEach(n => varDict.Add(n.Key, n.Value));
            Node.SelectNodes(".//SqlParameterDeclaration").Cast<XmlNode>()
                .Select(n => new KeyValuePair<string, string>(n.Attributes["Name"].Value, n.Attributes["Location"].Value))
                .ToList()
                .ForEach(n => varDict.Add(n.Key, n.Value));

            //Remove used
            Node.SelectNodes(".//SqlScalarVariableRefExpression").Cast<XmlNode>().ToList()
                .ForEach(n =>
                {
                    var varName = n.Attributes["VariableName"].Value;
                    var key = varDict.Keys.FirstOrDefault(k => k.ToLower() == varName.ToLower());
                    if (key != null)
                        varDict.Remove(key);
                });
            Node.SelectNodes(".//SqlTableVariableRefExpression").Cast<XmlNode>().ToList()
                .ForEach(n =>
                {
                    var varName = n.Attributes["Name"].Value;
                    var key = varDict.Keys.FirstOrDefault(k => k.ToLower() == varName.ToLower());
                    if (key != null)
                        varDict.Remove(key);
                });

            // Special case handle temporarily
            // Can not handle such below statement
            ///INSERT INTO @productLevels EXEC [MyRptSharedGetSAPProductLevel] @salesorg = @salesorg, 
            Node.SelectNodes(".//SqlNullInsertSource").Cast<XmlNode>()
                .ToList()
                .ForEach(n =>
                {
                    var tokens = ASTHelperMethod.GetTokensUnderNode(n);
                    tokens.ForEach(t =>
                    {
                        if (t.Attributes["type"].Value != "TOKEN_VARIABLE") return;
                        var key = varDict.Keys.FirstOrDefault(k => k.ToLower() == t.InnerText.ToLower());
                        if (key != null)
                            varDict.Remove(key);
                    });
                });


            foreach (var kv in varDict)
                WriteLog(string.Format("Parameter {0} is declared but never used.", kv.Key), 1, kv.Value);
        }
    }
}
