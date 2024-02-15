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
        public readonly IGraphClassRepository graphClassRepository;

        public GraphConverter(IGraphClassRepository graphClassRepository)
        {
            this.graphClassRepository = graphClassRepository;
        }

        #region Conversion from GraphDrawingUpdateDTO
        public Graph GetGraphFromGraphDrawingUpdateDTO(GraphDrawingUpdateDTO graphDTO, bool calculateProperties = true, bool calculateClasses = false)
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
            if (calculateProperties)
                GraphEvaluator.CalculateGraphProperties(graph);

            // Calculate classes
            if(calculateClasses)
                GraphEvaluator.CalculateGraphClasses(graph);
            
            return graph;
        }

        public async Task<GraphEntity> GetGraphEntityFromGraphDrawingUpdateDTO(GraphDrawingUpdateDTO graphDTO)
        {
            Graph graph = GetGraphFromGraphDrawingUpdateDTO(graphDTO, calculateProperties: true, calculateClasses: true);

            GraphEntity graphEntity = await GetGraphEntityFromGraph(graph);

            return graphEntity;
        }
        #endregion

        #region Conversion from GraphEntity to GraphDTO and its helpers
        /// <summary>
        /// Used for converting Graph object to GraphDTO object for displaying purposes.
        /// It includes any graph Properties from Graph object, but NOT any Classes,
        /// as these are copied from GraphEntity directly by their names.
        /// </summary>
        /// <param name="graph"></param>
        /// <returns>GraphDTO</returns>
        private GraphDTO GetGraphDTOFromGraph(Graph graph)
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

        /// <summary>
        /// Used to convert GraphEntity object to Graph object. Will also include any Properties and Classes
        /// that were previously persisted.
        /// </summary>
        /// <param name="graphEntity"></param>
        /// <returns>Graph</returns>
        /// <exception cref="InvalidDataException"></exception>
        private Graph GetGraphFromGraphEntity(GraphEntity graphEntity)
        {
            string graphML = graphEntity.DataXML;
            var xdoc = XDocument.Parse(graphML);

            // Check basic XML structure
            if (xdoc.Root == null)
                throw new InvalidDataException(string.Format("Root node does not exist:\r\n{0}", graphML));

            var graphElement = xdoc.Root.Element(GraphEvaluator.GraphMLNamespace + "graph");
            if (graphElement == null)
                throw new InvalidDataException(string.Format("Graph node not found:\r\n{0}", graphML));

            bool isUndirected = true;
            XAttribute? edgeDefaultAttr = graphElement.Attribute("edgedefault");
            if (edgeDefaultAttr != null)
                isUndirected = edgeDefaultAttr.Value == "undirected";

            var nodeElements = graphElement.Elements(GraphEvaluator.GraphMLNamespace + "node");
            var edgeElements = graphElement.Elements(GraphEvaluator.GraphMLNamespace + "edge");

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
                var nodeLabelElement = nodeElement.Elements(GraphEvaluator.GraphMLNamespace + "data")
                                                    .Where(e => e.Attribute("key").Value == "d0")
                                                    .FirstOrDefault();

                var nodeColorElement = nodeElement.Elements(GraphEvaluator.GraphMLNamespace + "data")
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
            if (graphEntity.GraphClasses != null)
                graphDTO.classNames = string.Join(", ", graphEntity.GraphClasses.Select(c => c.Name));

            return graphDTO;
        }
        #endregion

        public async Task<GraphEntity> GetGraphEntityFromGraph(Graph graph)
        {
            GraphEntity graphEntity = new GraphEntity()
            {
                ID = graph.ID,
                Name = "",
                Order = graph.Nodes.Count,
                Size = graph.Edges.Count,
                DataXML = GraphEvaluator.GetGraphMLForGraph(graph),
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
    }
}
