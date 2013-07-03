using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JQSQL.Core
{
    public class JElement
    {
        public string Name { get; set; }
        public bool HasIndexer { get; set; }
        public int? IndexNo { get; set; }
        public JElement Parent { get; set; }
        public JElement Child { get; set; }
    }
}
