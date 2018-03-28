using System.Collections.Generic;

namespace Test.BLL
{
    public class Node
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<Node> Nodes { get; set; }
    }
}
