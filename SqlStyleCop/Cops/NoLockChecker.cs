using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TST;

namespace SqlStyleCop
{
    public class NoLockChecker : ASTTraverseHandler
    {
        public NoLockChecker()
            : base("SqlTableRefExpression"
            , "Check whether the WITH(NOLOCK) is added when refer a table.")
        {
        }

        public override void HandleNode()
        {
            // No Lock should only in select specification.
            var inSpec = Node.LookUpForFirstNode("SqlUpdateSpecification", "SqlSelectSpecification", "SqlInsertSpecification");
            if (inSpec == null || inSpec.Name != "SqlSelectSpecification") return;

            var lockNode = GetTokensUnderNode().Find(n => n.Attributes["type"].Value == "TOKEN_ID"
                && n.InnerText.Equals("NOLOCK"
                , StringComparison.InvariantCultureIgnoreCase));
            if (lockNode == null)
            {
                var tbName = Node.Attributes["ObjectIdentifier"].Value;
                // Despite sys tables, temp tables, variable tables, simplely handling
                // Check CTE table TBD!
                if(tbName.StartsWith("SYS", StringComparison.InvariantCultureIgnoreCase)
                    || tbName.StartsWith("#")
                    || tbName.StartsWith("@")) return;
                WriteLog(string.Format("NOLOCK should be used when referencing the table [{0}]", tbName), 2);
            }
        }
    }
}
