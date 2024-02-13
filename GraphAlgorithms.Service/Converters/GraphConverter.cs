using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using System.Text;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace GraphAlgorithms.Service.Converters
{
    public class GraphConverter : IGraphConverter
    {
        public readonly XNamespace Namespace = "http://graphml.graphdrawing.org/xmlns";

        public readonly IGraphClassRepository graphClassRepository;

        public GraphConverter(IGraphClassRepository graphClassRepository)
        {
            this.graphClassRepository = graphClassRepository;
        }

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

            // Calculate Graph properties
            GraphEvaluator.CalculateGraphProperties(graph);

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
            graphDTO.score = graph.GraphProperties.WienerIndex;

            return graphDTO;
        }

        public Graph GetGraphFromGraphEntity(GraphEntity graphEntity)
        {
            string graphML = graphEntity.DataXML;
            var xdoc = XDocument.Parse(graphML);

            // Check basic XML structure
            if (xdoc.Root == null)
                throw new InvalidDataException(string.Format("Root node does not exist:\r\n{0}", graphML));

            var graphElement = xdoc.Root.Element(Namespace + "graph");
            if (graphElement == null)
                throw new InvalidDataException(string.Format("Graph node not found:\r\n{0}", graphML));

            bool isUndirected = true;
            XAttribute? edgeDefaultAttr = graphElement.Attribute("edgedefault");
            if (edgeDefaultAttr != null)
                isUndirected = edgeDefaultAttr.Value == "undirected";

            var nodeElements = graphElement.Elements(Namespace + "node");
            var edgeElements = graphElement.Elements(Namespace + "edge");

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
                var nodeLabelElement = nodeElement.Elements(Namespace + "data")
                                                    .Where(e => e.Attribute("key").Value == "d0")
                                                    .FirstOrDefault();

                var nodeColorElement = nodeElement.Elements(Namespace + "data")
                                                    .Where(e => e.Attribute("key").Value == "d1")
                                                    .FirstOrDefault();

                var nodeLabel = nodeLabelElement.Value;
                
                Node node = new Node(int.Parse(nodeIndex), nodeLabel);

                if (nodeColorElement != null)
                {
                    var nodeColor = nodeColorElement.Value;
                    node.NodeProperties.Color = nodeColor;
                }

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
            graph.GraphProperties.WienerIndex = graphEntity.WienerIndex;

            // Graph Classes
            if(graphEntity.GraphClasses != null)
            {
                foreach(GraphClassEntity graphClass in graphEntity.GraphClasses)
                {
                    graph.GraphClasses.Add((Shared.Shared.GraphClassEnum)graphClass.ID);
                }
            }

            return graph;
        }

        public async Task<GraphEntity> GetGraphEntityFromGraph(Graph graph)
        {
            GraphEntity graphEntity = new GraphEntity()
            {
                ID = graph.ID,
                Name = "",
                Order = graph.Nodes.Count,
                Size = graph.Edges.Count,
                DataXML = GetGraphMLForGraph(graph),
                WienerIndex = graph.GraphProperties.WienerIndex
            };
            
            // Fetch and link Graph Classes to entity
            if(graph.GraphClasses.Count > 0)
            {
                List<int> graphClassIDs = graph.GraphClasses.Select(gc => (int)gc).ToList();
                List<GraphClassEntity> graphClassEntities = await graphClassRepository.GetGraphClassesByIDsAsync(graphClassIDs);
                graphEntity.GraphClasses = graphClassEntities;
            }

            return graphEntity;
        }

        public async Task<GraphEntity> GetGraphEntityFromGraphDTO(GraphDTO graphDTO)
        {
            Graph graph = GetGraphFromGraphDTO(graphDTO);

            // Calculate classes
            GraphEvaluator.CalculateGraphClasses(graph);

            GraphEntity graphEntity = await GetGraphEntityFromGraph(graph);

            return graphEntity;
        }

        public GraphDTO GetGraphDTOFromGraphEntity(GraphEntity graphEntity)
        {
            // Transform Graph Repository model into actual Graph object
            Graph graph = GetGraphFromGraphEntity(graphEntity);

            // Transform Graph object into GraphDTO
            GraphDTO graphDTO = GetGraphDTOFromGraph(graph);

            // Add additional data to GraphDTO that are stored to DB and not contained in Graph object
            graphDTO.actionTypeName = graphEntity.Action.ActionType.Name;
            graphDTO.actionForGraphClassName = graphEntity.Action.ForGraphClass != null 
                                            ? graphEntity.Action.ForGraphClass.Name : "";
            graphDTO.createdDate = graphEntity.CreatedDate;

            // Graph Classes
            if(graphEntity.GraphClasses != null)
                graphDTO.classNames = string.Join(", ", graphEntity.GraphClasses.Select(c => c.Name));

            return graphDTO;
        }

        private string GetGraphMLForGraph(Graph g)
        {
            var xdoc = new XDocument();
            var root = new XElement(Namespace + "graphml");

            root.Add(new XElement(Namespace + "key", new XAttribute("id", "d0"), new XAttribute("for", "node"), new XAttribute("attr.name", "label"), new XAttribute("attr.type", "string")));
            root.Add(new XElement(Namespace + "key", new XAttribute("id", "d1"), new XAttribute("for", "node"), new XAttribute("attr.name", "color"), new XAttribute("attr.type", "string")));
            //root.Add(new XElement("key", new XAttribute("id", "d2"), new XAttribute("for", "edge"), new XAttribute("attr.name", "weight"), new XAttribute("attr.type", "double")));

            var graphElement = new XElement(Namespace + "graph",
                new XAttribute("id", "G"),
                new XAttribute("edgedefault", g.IsUndirected ? "undirected" : "directed"));

            foreach (Node node in g.Nodes)
            {
                var nodeElement = new XElement(Namespace + "node", new XAttribute("id", node.Index));
                nodeElement.Add(new XElement(Namespace + "data", new XAttribute("key", "d0"), node.Label));
                nodeElement.Add(new XElement(Namespace + "data", new XAttribute("key", "d1"), node.NodeProperties.Color));
                graphElement.Add(nodeElement);
            }

            foreach (Edge edge in g.Edges)
            {
                var edgeElement = new XElement(Namespace + "edge",
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
