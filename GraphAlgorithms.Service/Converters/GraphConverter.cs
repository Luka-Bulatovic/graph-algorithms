using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Migrations;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace GraphAlgorithms.Service.Converters
{
    public class GraphConverter : IGraphConverter
    {
        public readonly IGraphClassRepository graphClassRepository;
        public readonly IGraphRepository graphRepository;
        public readonly GraphEvaluator graphEvaluator;

        public GraphConverter(IGraphClassRepository graphClassRepository, GraphEvaluator graphEvaluator, IGraphRepository graphRepository)
        {
            this.graphClassRepository = graphClassRepository;
            this.graphEvaluator = graphEvaluator;
            this.graphRepository = graphRepository;
        }

        #region Conversion from GraphDrawingUpdateDTO
        public Graph GetGraphFromGraphDrawingUpdateDTO(GraphDrawingUpdateDTO graphDTO, bool calculateWienerIndexOnly = false)
        {
            Graph graph = new Graph(graphDTO.id, graphDTO.nodes.Count);

            for (int i = 0; i < graphDTO.nodes.Count; i++)
                graph.AddNode(new Node(graphDTO.nodes[i].id, graphDTO.nodes[i].label, graphDTO.nodes[i].x.Value, graphDTO.nodes[i].y.Value));

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

        public async Task<GraphEntity> GetUpdatedGraphEntityFromGraphDrawingUpdateDTO(GraphDrawingUpdateDTO graphDTO)
        {
            Graph graph = GetGraphFromGraphDrawingUpdateDTO(graphDTO);
            GraphEntity newGraphEntity = await GetGraphEntityFromGraph(graph);

            GraphEntity graphEntity = await graphRepository.GetByIdAsync(graphDTO.id);
            // Copy new data
            graphEntity.Order = newGraphEntity.Order;
            graphEntity.Size = newGraphEntity.Size;
            graphEntity.WienerIndex = newGraphEntity.WienerIndex;
            graphEntity.DataXML = newGraphEntity.DataXML;
            graphEntity.GraphClasses = newGraphEntity.GraphClasses;
            graphEntity.GraphPropertyValues = newGraphEntity.GraphPropertyValues;

            return graphEntity;
        }
        #endregion

        #region Conversion from GraphEntity to GraphDTO and its helpers
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

            // Map Graph Properties from Entity
            var propertyMappings = graph.GraphProperties.PropertyMappings;
            foreach(var graphPropertyValue in graphEntity.GraphPropertyValues)
            {
                var propertyMapping = propertyMappings
                                        .Where(pm => 
                                            (int)pm.Key == graphPropertyValue.GraphPropertyID)
                                        .First();

                propertyMapping.Value.Setter(Convert.ChangeType(graphPropertyValue.PropertyValue, propertyMapping.Value.Type));
            }

            // Map Graph Classes from Entity
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
            // First, transform GraphEntity into actual Graph object
            Graph graph = GetGraphFromGraphEntity(graphEntity);

            // Create GraphDTO
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

            graphDTO.score = graph.GraphProperties.WienerIndex;

            graphDTO.properties = graph.GraphProperties;

            // Graph Classes
            if (graphEntity.GraphClasses != null && graphEntity.GraphClasses.Count > 0)
            {
                graphDTO.classNames = string.Join(", ", graphEntity.GraphClasses.Select(c => c.Name));
            }

            // Some additional data
            graphDTO.actionTypeName = graphEntity.Action.ActionType.Name;
            graphDTO.actionForGraphClassName = graphEntity.Action.ForGraphClass != null
                                            ? graphEntity.Action.ForGraphClass.Name : "";
            graphDTO.createdDate = graphEntity.CreatedDate;

            // Set node positions if nodes have defined positions
            if(graph.Nodes.Any(n => n.NodeProperties.PlanePosX > 0 || n.NodeProperties.PlanePosY > 0))
            {
                for(int i = 0; i < graph.Nodes.Count; i++)
                {
                    graphDTO.nodes[i].x = graph.Nodes[i].NodeProperties.PlanePosX;
                    graphDTO.nodes[i].y = graph.Nodes[i].NodeProperties.PlanePosY;
                }
            }

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
                var value = propertyMapping.Value.Getter();
                if (value == null)
                    continue;

                string propertyValueStr = Convert.ToString(value);

                // If there is a value, link GraphPropertyValueEntity to Graph
                if (propertyValueStr.Length > 0) 
                {
                    GraphPropertyValueEntity newPropertyValue = new()
                    {
                        Graph = graphEntity,
                        GraphPropertyID = (int)propertyMapping.Key,
                        PropertyValue = propertyValueStr
                    };

                    graphEntity.GraphPropertyValues.Add(newPropertyValue);
                }
            }

            return graphEntity;
        }
    }
}
