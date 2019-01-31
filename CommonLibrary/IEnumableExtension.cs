using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TST
{
    public static class IEnumableExtension
    {
        public static bool ContainsIgnoreCase(this IEnumerable<string> source, string target) 
        {
            foreach(var i in source)
            {
                if (i.Equals(target, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
