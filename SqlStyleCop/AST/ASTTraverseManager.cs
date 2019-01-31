using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TST;

namespace SqlStyleCop
{
    public class ASTTraverseManager
    {
        public List<ASTTraverseHandler> Handlers { get; set; }

        public ASTTraverseManager(List<ASTTraverseHandler> handlers)
        {
            Handlers = handlers;
        }

        public void RunHandlers(StyleCopContext context)
        {
            var doc = context.Doc;
            //var availableHandlers = 
            XmlTraverser.DSTXmlNode(doc, n =>
            {
                Handlers.FindAll(h => h.FilterNames.Contains(n.Name))
                    .ForEach(h =>
                    {
                        h.StyleCopContext = context;
                        h.Node = n;
                        h.HandleNode();
                    });
            });
        }
    }
}
