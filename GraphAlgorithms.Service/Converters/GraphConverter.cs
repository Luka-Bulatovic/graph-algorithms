using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Migrations;
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
        public readonly GraphEvaluator graphEvaluator;

        public GraphConverter(IGraphClassRepository graphClassRepository, GraphEvaluator graphEvaluator)
        {
            this.graphClassRepository = graphClassRepository;
            this.graphEvaluator = graphEvaluator;
        }

        #region Conversion from GraphDrawingUpdateDTO
        public Graph GetGraphFromGraphDrawingUpdateDTO(GraphDrawingUpdateDTO graphDTO, bool calculateWienerIndexOnly = false)
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

            if (calculateWienerIndexOnly)
                graphEvaluator.CalculateWienerIndex(graph);
            else
                graphEvaluator.CalculateGraphPropertiesAndClasses(graph);
            
            return graph;
        }

        public async Task<GraphEntity> GetGraphEntityFromGraphDrawingUpdateDTO(GraphDrawingUpdateDTO graphDTO)
        {
            Graph graph = GetGraphFromGraphDrawingUpdateDTO(graphDTO);

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
            Graph graph = graphEvaluator.GetGraphFromGraphML(graphEntity.ID, graphML);

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
                DataXML = graphEvaluator.GetGraphMLForGraph(graph),
                WienerIndex = graph.GraphProperties.WienerIndex,
                GraphPropertyValues = new List<GraphPropertyValueEntity>()
            };
            
            // Fetch and link Graph Classes to entity
            if(graph.GraphClasses.Count > 0)
            {
                List<int> graphClassIDs = graph.GraphClasses.Select(gc => (int)gc).ToList();
                List<GraphClassEntity> graphClassEntities = await graphClassRepository.GetGraphClassesByIDsAsync(graphClassIDs);
                graphEntity.GraphClasses = graphClassEntities;
            }

            // Link Graph Properties with corresponding values to entity
            var propertyMappings = graph.GraphProperties.PropertyMappings;
            foreach (var propertyMapping in propertyMappings)
            {
                if (propertyMapping.Value.Getter() == null)
                    continue;

                string propertyValueStr = "";
                if (propertyMapping.Value.Type == typeof(int) 
                    && (int)propertyMapping.Value.Getter() != 0)
                {
                    propertyValueStr = ((int)propertyMapping.Value.Getter()).ToString();
                }
                // TODO: Add cases for some more data types here as needed


                // If there is a value, link GraphPropertyValueEntity to Graph
                if (propertyValueStr.Length > 0) 
                {
                    GraphPropertyValueEntity newPropertyValue = new()
                    {
                        Graph = graphEntity,
                        GraphPropertyID = (int)propertyMapping.Key,
                        PropertyValue = ((int)propertyMapping.Value.Getter()).ToString()
                    };

                    graphEntity.GraphPropertyValues.Add(newPropertyValue);
                }
            }

            return graphEntity;
        }
    }
}
