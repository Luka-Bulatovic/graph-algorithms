using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Service.DTO;
using System.Text;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace GraphAlgorithms.Service
{
    public class GraphConverter : IGraphConverter
    {
        public readonly XNamespace Namespace = "http://graphml.graphdrawing.org/xmlns";

        public Graph GetGraphFromGraphDTO(GraphDTO graphDTO)
        {
            Graph graph = new Graph(graphDTO.id, graphDTO.nodes.Count);

            for (int i = 0; i < graphDTO.nodes.Count; i++)
                graph.AddNode(new Node(graphDTO.nodes[i].id, graphDTO.nodes[i].label));

            for (int i = 0; i < graphDTO.edges.Count; i++)
            {
                int fromIndex = graphDTO.edges[i].from;
                int toIndex = graphDTO.edges[i].to;

                Node fromNode = graph.GetNode(fromIndex);
                Node toNode = graph.GetNode(toIndex);

                graph.ConnectNodes(fromNode, toNode);
            }

            return graph;
        }

        public GraphDTO GetGraphDTOFromGraph(Graph graph)
        {
            GraphDTO graphDTO = new GraphDTO();

            graphDTO.id = graph.ID;

            foreach (Node node in graph.Nodes)
                graphDTO.nodes.Add(new NodeDTO(node));

            foreach (Node node in graph.Nodes)
            {
                List<Edge> adjEdges = graph.GetAdjacentEdges(node);

                foreach (Edge e in adjEdges)
                {
                    if (node.Index < e.GetDestNodeIndex())
                        graphDTO.edges.Add(new EdgeDTO(e));
                }
            }

            // TODO: Here, we will add mapping for WienerIndex and some more properties in future (other indices)

            return graphDTO;
        }

        public Graph GetGraphFromGraphEntity(GraphEntity graphEntity)
        {
            string graphML = graphEntity.DataXML;
            var xdoc = XDocument.Parse(graphML);

            // Check basic XML structure
            if (xdoc.Root == null)
                throw new InvalidDataException(string.Format("Root node does not exist:\r\n{0}", graphML));

            var graphElement = xdoc.Root.Element(this.Namespace + "graph");
            if (graphElement == null)
                throw new InvalidDataException(string.Format("Graph node not found:\r\n{0}", graphML));

            bool isUndirected = true;
            XAttribute? edgeDefaultAttr = graphElement.Attribute("edgedefault");
            if (edgeDefaultAttr != null)
                isUndirected = edgeDefaultAttr.Value == "undirected";

            var nodeElements = graphElement.Elements(this.Namespace + "node");
            var edgeElements = graphElement.Elements(this.Namespace + "edge");

            // Check if nodes and Edges are in valid format
            if (nodeElements.Any(n => n.Attribute("id") == null))
                throw new InvalidDataException(string.Format("Each node must contain id attribute:\r\n{0}", graphML));
            if (edgeElements.Any(e => e.Attribute("source") == null || e.Attribute("target") == null))
                throw new InvalidDataException(string.Format("Each edge must contain source and target attributes:\r\n{0}", graphML));

            // Create Graph
            Graph graph = new Graph(graphEntity.ID, nodeElements.Count(), isUndirected);

            foreach (var nodeElement in nodeElements)
            {
                var nodeIndex = nodeElement.Attribute("id").Value;
                var nodeLabel = nodeElement.Element(this.Namespace + "data")?.Value;

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

            // TODO: Here, we will add more stuff for mapping GraphProperties

            return graph;
        }

        public GraphEntity GetGraphEntityFromGraphDTO(GraphDTO graphDTO)
        {
            Graph graph = GetGraphFromGraphDTO(graphDTO);

            WienerIndexAlgorithm wienerAlg = new WienerIndexAlgorithm(graph);
            wienerAlg.Run();

            return new GraphEntity()
            {
                ID = graph.ID,
                Name = "",
                Order = graphDTO.nodes.Count,
                Size = graphDTO.edges.Count,
                DataXML = GetGraphMLForGraph(graph),
                WienerIndex = wienerAlg.WienerIndexValue
            };
        }

        public GraphDTO GetGraphDTOFromGraphEntity(GraphEntity graphEntity)
        {
            // Transform Graph Repository model into actual Graph object
            Graph graph = GetGraphFromGraphEntity(graphEntity);

            // Transform Graph object into GraphDTO
            GraphDTO graphDTO = GetGraphDTOFromGraph(graph);

            // Add additional data to GraphDTO that are stored to DB and not contained in Graph object
            graphDTO.actionTypeName = graphEntity.ActionType.Name;
            graphDTO.createdDate = graphEntity.CreatedDate;

            return graphDTO;
        }

        private string GetGraphMLForGraph(Graph g)
        {
            var xdoc = new XDocument();
            var root = new XElement(this.Namespace + "graphml");

            root.Add(new XElement(this.Namespace + "key", new XAttribute("id", "d0"), new XAttribute("for", "node"), new XAttribute("attr.name", "label"), new XAttribute("attr.type", "string")));
            root.Add(new XElement(this.Namespace + "key", new XAttribute("id", "d1"), new XAttribute("for", "node"), new XAttribute("attr.name", "color"), new XAttribute("attr.type", "string")));
            //root.Add(new XElement("key", new XAttribute("id", "d2"), new XAttribute("for", "edge"), new XAttribute("attr.name", "weight"), new XAttribute("attr.type", "double")));

            var graphElement = new XElement(this.Namespace + "graph",
                new XAttribute("id", "G"),
                new XAttribute("edgedefault", g.IsUndirected ? "undirected" : "directed"));

            foreach (Node node in g.Nodes)
            {
                var nodeElement = new XElement(this.Namespace + "node", new XAttribute("id", node.Index));
                nodeElement.Add(new XElement(this.Namespace + "data", new XAttribute("key", "d0"), node.Label));
                graphElement.Add(nodeElement);
            }

            foreach (Edge edge in g.Edges)
            {
                var edgeElement = new XElement(this.Namespace + "edge",
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
    }
}
