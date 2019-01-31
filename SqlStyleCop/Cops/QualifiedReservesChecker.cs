using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SqlStyleCop
{
    public class QualifiedReservesChecker : ASTTraverseHandler
    {
        public QualifiedReservesChecker()
            : base("SqlBatch"
            , "Check whether all SQL reserved words is qualified. Like using LEFT OUTER JOIN instead of LEFT JOIN.")
        {
        }

        public override void HandleNode()
        {
            var leftJoinNodes = Node.SelectNodes("//SqlQualifiedJoinTableExpression[@JoinOperator='LeftOuterJoin']").Cast<XmlNode>();
            foreach(var node in leftJoinNodes)
            {
                var p1 = ASTHelperMethod.GetPosition(node.ChildNodes[0]).Item2; // left table, location Y
                var p2 = ASTHelperMethod.GetPosition(node.ChildNodes[1]).Item1;
                var tokens = ASTHelperMethod.GetTokensBetweenPoints(Node.OwnerDocument, p1, p2);
                if(tokens.Find(n=>n.Attributes["type"].Value == "TOKEN_OUTER") == null)
                    WriteLog("Use LFET OUTER JOIN instead of LEFT JOIN", pos: node.Attributes["Location"].Value);    
            }  
        }
    }
}
