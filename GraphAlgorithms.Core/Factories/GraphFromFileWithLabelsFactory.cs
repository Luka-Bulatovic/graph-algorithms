using GraphAlgorithms.Core.Interfaces;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Core.Factories
{
    public class GraphFromFileWithLabelsFactory : IGraphFactory
    {
        string fileName;

        public GraphFromFileWithLabelsFactory(string fileName)
        {
            this.fileName = fileName;
        }

        public Graph CreateGraph()
        {
            string[] lines = System.IO.File.ReadAllLines(@"TestGraphInputs/" + fileName);
            int n = Int32.Parse(lines[0]);
            int m = lines.Length - 1;

            Graph g = new Graph(0, n, isUndirected: false);

            int currNodeIndex = 0;

            for (int i = 1; i <= m; i++)
            {
                string line = lines[i];

                string startNodeLabel = line.Split('\t')[0];
                string endNodeLabel = line.Split('\t')[1];

                Node? startNode = g.Nodes.Where(n => n.Label == startNodeLabel).FirstOrDefault();
                Node? endNode = g.Nodes.Where(n => n.Label == endNodeLabel).FirstOrDefault();

                int startNodeIndex = startNode != null ? startNode.Index : currNodeIndex++;
                int endNodeIndex = endNode != null ? endNode.Index : currNodeIndex++;

                if (startNode == null)
                {
                    startNode = new Node(startNodeIndex, startNodeLabel);
                    g.AddNode(startNode);
                }

                if (endNode == null)
                {
                    endNode = new Node(endNodeIndex, endNodeLabel);
                    g.AddNode(endNode);
                }

                g.ConnectNodes(startNode, endNode);
            }

            return g;
        }

        public GraphClassEnum GetGraphClass()
        {
            return GraphClassEnum.ConnectedGraph;
        }
    }
}
