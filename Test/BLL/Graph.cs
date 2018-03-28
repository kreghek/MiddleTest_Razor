using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.BLL
{
    public class Graph
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<Node> Nodes { get; set; }
    }
}
