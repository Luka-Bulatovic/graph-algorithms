using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GraphAlgorithms.Core
{
    public static class GraphMLConverter
    {
        public static XNamespace Namespace = "http://graphml.graphdrawing.org/xmlns";

        public static string GetGraphMLForGraph(Graph g)
        {
            var xdoc = new XDocument();
            var root = new XElement(GraphMLConverter.Namespace + "graphml");

            root.Add(new XElement(GraphMLConverter.Namespace + "key", new XAttribute("id", "d0"), new XAttribute("for", "node"), new XAttribute("attr.name", "label"), new XAttribute("attr.type", "string")));
            root.Add(new XElement(GraphMLConverter.Namespace + "key", new XAttribute("id", "d1"), new XAttribute("for", "node"), new XAttribute("attr.name", "color"), new XAttribute("attr.type", "string")));
            //root.Add(new XElement("key", new XAttribute("id", "d2"), new XAttribute("for", "edge"), new XAttribute("attr.name", "weight"), new XAttribute("attr.type", "double")));

            var graphElement = new XElement(GraphMLConverter.Namespace + "graph",
                new XAttribute("id", "G"),
                new XAttribute("edgedefault", g.IsUndirected ? "undirected" : "directed"));

            foreach (Node node in g.Nodes)
            {
                var nodeElement = new XElement(GraphMLConverter.Namespace + "node", new XAttribute("id", node.Index));
                nodeElement.Add(new XElement(GraphMLConverter.Namespace + "data", new XAttribute("key", "d0"), node.Label));
                graphElement.Add(nodeElement);
            }

            foreach (Edge edge in g.Edges)
            {
                var edgeElement = new XElement(GraphMLConverter.Namespace + "edge",
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
    
        public static Graph GetGraphFromGraphML(int id, string graphML)
        {
            var xdoc = XDocument.Parse(graphML);

            // Check basic XML structure
            if(xdoc.Root == null)
                throw new InvalidDataException(string.Format("Root node does not exist:\r\n{0}", graphML));

            var graphElement = xdoc.Root.Element(GraphMLConverter.Namespace + "graph");
            if (graphElement == null)
                throw new InvalidDataException(string.Format("Graph node not found:\r\n{0}", graphML));

            bool isUndirected = true;
            XAttribute? edgeDefaultAttr = graphElement.Attribute("edgedefault");
            if (edgeDefaultAttr != null)
                isUndirected = edgeDefaultAttr.Value == "undirected";

            var nodeElements = graphElement.Elements(GraphMLConverter.Namespace + "node");
            var edgeElements = graphElement.Elements(GraphMLConverter.Namespace + "edge");

            // Check if nodes and Edges are in valid format
            if(nodeElements.Any(n => n.Attribute("id") == null))
                throw new InvalidDataException(string.Format("Each node must contain id attribute:\r\n{0}", graphML));
            if(edgeElements.Any(e => e.Attribute("source") == null || e.Attribute("target") == null))
                throw new InvalidDataException(string.Format("Each edge must contain source and target attributes:\r\n{0}", graphML));

            // Create Graph
            Graph graph = new Graph(id, nodeElements.Count(), isUndirected);

            foreach (var nodeElement in nodeElements)
            {
                var nodeIndex = nodeElement.Attribute("id").Value;
                var nodeLabel = nodeElement.Element(GraphMLConverter.Namespace + "data")?.Value;

                Node node = new Node(int.Parse(nodeIndex), nodeLabel);

                graph.Nodes.Add(node);
            }

            foreach (var edgeElement in edgeElements)
            {
                var sourceIndex = int.Parse(edgeElement.Attribute("source").Value);
                var targetIndex = int.Parse(edgeElement.Attribute("target").Value);
                // double weight = Convert.ToDouble(edgeElement.Element("data")?.Value); // Edge weights

                graph.ConnectNodes(graph.GetNode(sourceIndex), graph.GetNode(targetIndex));
            }

            return graph;
        }

    }
}
