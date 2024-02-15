using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Classifiers;
using GraphAlgorithms.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GraphAlgorithms.Core
{
    public class GraphEvaluator
    {
        public static XNamespace GraphMLNamespace = "http://graphml.graphdrawing.org/xmlns";

        #region Utils
        public static void CalculateWienerIndex(Graph graph)
        {
            if (graph == null)
                return;

            WienerIndexAlgorithm wienerAlg = new WienerIndexAlgorithm(graph);
            wienerAlg.Run();

            graph.GraphProperties.WienerIndex = wienerAlg.WienerIndexValue;
        }

        private static List<IGraphClassifier> GetGraphClassifiersList(Graph graph)
        {
            DepthFirstSearchAlgorithm dfsAlgorithm = new(graph, graph.Nodes[0]);
            BreadthFirstSearchAlgorithm bfsAlgorithm = new(graph, graph.Nodes[0]);

            List<IGraphClassifier> graphClassifiers = new List<IGraphClassifier>()
            {
                new ConnectedGraphClassifier(dfsAlgorithm),
                new TreeGraphClassifier(dfsAlgorithm),
                new BipartiteGraphClassifier(bfsAlgorithm),
                new UnicyclicGraphClassifier(dfsAlgorithm),
            };
            return graphClassifiers;
        }
        #endregion

        #region Main Methods
        public static void CalculateGraphProperties(Graph graph)
        {
            if (graph == null)
                return;

            CalculateWienerIndex(graph);
        }

        public static void CalculateGraphClasses(Graph graph)
        {
            if (graph == null || graph.Nodes == null || graph.Nodes.Count == 0)
                return;

            graph.GraphClasses.Clear();

            List<IGraphClassifier> graphClassifiers = GetGraphClassifiersList(graph);

            foreach (IGraphClassifier graphClassifier in graphClassifiers)
                if (graphClassifier.BelongsToClass())
                    graph.GraphClasses.Add(graphClassifier.GetGraphClass());
        }

        public static string GetGraphMLForGraph(Graph g)
        {
            var xdoc = new XDocument();
            var root = new XElement(GraphMLNamespace + "graphml");

            root.Add(new XElement(GraphMLNamespace + "key", new XAttribute("id", "d0"), new XAttribute("for", "node"), new XAttribute("attr.name", "label"), new XAttribute("attr.type", "string")));
            root.Add(new XElement(GraphMLNamespace + "key", new XAttribute("id", "d1"), new XAttribute("for", "node"), new XAttribute("attr.name", "color"), new XAttribute("attr.type", "string")));
            //root.Add(new XElement("key", new XAttribute("id", "d2"), new XAttribute("for", "edge"), new XAttribute("attr.name", "weight"), new XAttribute("attr.type", "double")));

            var graphElement = new XElement(GraphMLNamespace + "graph",
                new XAttribute("id", "G"),
                new XAttribute("edgedefault", g.IsUndirected ? "undirected" : "directed"));

            foreach (Node node in g.Nodes)
            {
                var nodeElement = new XElement(GraphMLNamespace + "node", new XAttribute("id", node.Index));
                nodeElement.Add(new XElement(GraphMLNamespace + "data", new XAttribute("key", "d0"), node.Label));
                nodeElement.Add(new XElement(GraphMLNamespace + "data", new XAttribute("key", "d1"), node.NodeProperties.Color));
                graphElement.Add(nodeElement);
            }

            foreach (Edge edge in g.Edges)
            {
                var edgeElement = new XElement(GraphMLNamespace + "edge",
                    new XAttribute("source", edge.GetSrcNodeIndex()),
                    new XAttribute("target", edge.GetDestNodeIndex()));
                //edgeElement.Add(new XElement("data", new XAttribute("key", "d2"), edge.Weight));
                graphElement.Add(edgeElement);
            }

            root.Add(graphElement);
            xdoc.Add(root);

            var sb = new StringBuilder();
            xdoc.Save(new StringWriter(sb));
            return sb.ToString();
        }
        #endregion
    }
}
