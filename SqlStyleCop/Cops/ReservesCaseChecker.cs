using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlStyleCop
{
    public class ReservesCaseChecker : ASTTraverseHandler
    {
        public ReservesCaseChecker()
            : base("SqlBatch"
            , "Check whether all SQL reserved words is UPPERCASE.")
        {
        }

        public override void HandleNode()
        {
            var tokens = GetTokensUnderNode();
            foreach (var token in tokens)
            {
                // Could miss something :(, more coding TBD.
                var typ = token.Attributes["type"].Value;
                if (!typ.StartsWith("TOKEN_") 
                    || typ == "TOKEN_STRING" 
                    || typ == "TOKEN_VARIABLE"
                    || typ == "TOKEN_s_AW_ID") continue;
                else if (typ == "TOKEN_ID"
                    && !token.InnerText.Equals("JOIN", StringComparison.InvariantCultureIgnoreCase))
                    continue;
                else if (token.InnerText.ToUpper() != token.InnerText)
                    WriteLog(string.Format("Reserved word [{0}] should be UPPERCASE", token.InnerText), pos: token.Attributes["location"].Value);
            }
        }
    }
}
