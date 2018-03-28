namespace Test.BLL
{
    using System.Collections.Generic;

    public class TopoSorter
    {
        private readonly Dictionary<Node, TopoNodeData> _dataDict = new Dictionary<Node, TopoNodeData>();

        public Node[] Sort(Graph graph)
        {

            var time = 0;

            var result = new List<Node>();

            foreach (var node in graph.Nodes)
            {
                if (!_dataDict.TryGetValue(node, out var data))
                {
                    data = new TopoNodeData();
                    _dataDict.Add(node, data);
                }

                if (data.Color == NodeColor.White)
                {
                    Visit(node, result, ref time);
                }
            }

            return result.ToArray();
        }

        private void Visit(Node node, IList<Node> result, ref int time)
        {
            if (!_dataDict.TryGetValue(node, out var data))
            {
                data = new TopoNodeData();
                _dataDict.Add(node, data);
            }

            time++;
            data.D = time;
            data.Color = NodeColor.Gray;

            if (node.Nodes != null)
            {
                foreach (var v in node.Nodes)
                {
                    if (!_dataDict.TryGetValue(node, out var childData))
                    {
                        childData = new TopoNodeData();
                        _dataDict.Add(node, childData);
                    }


                    if (childData.Color == NodeColor.White)
                    {
                        childData.Pi = node;
                        Visit(v, result, ref time);
                    }
                }
            }

            data.Color = NodeColor.Black;
            time++;
            data.F = time;
            result.Insert(0, node);
        }

        private enum NodeColor {
            White,
            Gray,
            Black
        }

        private class TopoNodeData {
            public NodeColor Color { get; set; }
            public int D { get; set; }
            public int F { get; set; }
            public Node Pi { get; set; }
        }
    }
}
