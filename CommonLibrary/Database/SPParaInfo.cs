using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TST
{
    /// <summary>
    /// Information of store procedure
    /// </summary>
    public class SPInfo
    {
        /// <summary>
        /// SP Name
        /// </summary>
        public string Name { get; set; }

        public string PlainText { get; set; }

        /// <summary>
        /// SP paramters
        /// </summary>
        public List<SPParaInfo> ParaInfos { get; set; }

        /// <summary>
        /// SP fields
        /// </summary>
        public List<SPFieldInfo> FieldInfos { get; set; }

        public XmlDocument AST { get; set; }

        /// <summary>
        /// Represents a parameter of store procedure
        /// </summary>
        public class SPParaInfo
        {
            public SPParaInfo()
            {
                UseTemplate = true;
            }
            public string Name { get; set; }

            [Obsolete]
            public string DisplayName { get; set; }

            public string DataType { get; set; }

            [Obsolete]
            public bool UseTemplate { get; set; }

            [Obsolete]
            public bool IsDropdown { get; set; }
        }

        /// <summary>
        /// Represents a return field of store procedure
        /// </summary>
        public class SPFieldInfo
        {
            public string Name { get; set; }

            public string DataType { get; set; }
        }
    }
}
