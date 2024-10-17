﻿using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Factories;
using GraphAlgorithms.Repository.Data;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Migrations;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using System.Collections.Concurrent;

namespace GraphAlgorithms.Service.Services
{
    public class GraphDrawingService : IGraphDrawingService
    {
        private readonly IGraphRepository graphRepository;
        private readonly IGraphClassRepository graphClassRepository;
        private readonly IGraphConverter graphConverter;

        public GraphDrawingService(IGraphRepository graphRepository, IGraphClassRepository graphClassRepository, IGraphConverter graphConverter)
        {
            this.graphRepository = graphRepository;
            this.graphClassRepository = graphClassRepository;
            this.graphConverter = graphConverter;
        }

        public async Task<GraphDTO> GetGraphDTOByIDAsync(int id)
        {
            GraphEntity graphEntity = await graphRepository.GetByIdAsync(id);

            GraphDTO graphDTO = graphConverter.GetGraphDTOFromGraphEntity(graphEntity);

            return graphDTO;
        }

        public async Task<GraphDTO> StoreGraph(GraphDrawingUpdateDTO graphDTO/*, int ActionTypeID = 1*/)
        {
            GraphEntity graphEntity = null;

            if (graphDTO.id == 0)
            {
                // Create new GraphEntity object with calculated Properties and Classes
                graphEntity = await graphConverter.GetGraphEntityFromGraphDrawingUpdateDTO(graphDTO);
                
                // Create an ActionEntity and set its properties
                var actionEntity = new ActionEntity
                {
                    ActionTypeID = (int)ActionTypeEnum.Draw,
                    CreatedByID = 0, // Set the creator's ID,
                    CreatedDate = DateTime.UtcNow
                };

                // Associate the ActionEntity with the GraphEntity
                graphEntity.Action = actionEntity;
            }
            else
            {
                graphEntity = await graphRepository.GetByIdAsync(graphDTO.id);

                Graph graph = graphConverter.GetGraphFromGraphDrawingUpdateDTO(graphDTO);

                // Update GraphEntity object
                graphEntity.Order = graph.Nodes.Count;
                graphEntity.Size = graph.Edges.Count;
                graphEntity.DataXML = GraphEvaluator.GetGraphMLForGraph(graph);
                graphEntity.WienerIndex = graph.GraphProperties.WienerIndex;

                // Update its GraphClasses
                graphEntity.GraphClasses.Clear();
                
                List<int> graphClassIDs = graph.GraphClasses.Select(gc => (int)gc).ToList();
                List<GraphClassEntity> graphClassEntities = await graphClassRepository.GetGraphClassesByIDsAsync(graphClassIDs);
                
                foreach(var graphClassEntity in graphClassEntities)
                    graphEntity.GraphClasses.Add(graphClassEntity);
            }

            // Save GraphEntity (along with the associated entities)
            var storedGraphEntity = await graphRepository.SaveAsync(graphEntity);

            return graphConverter.GetGraphDTOFromGraphEntity(storedGraphEntity);
        }

        public int CalculateWienerIndex(GraphDrawingUpdateDTO graphDTO)
        {
            Graph graph = graphConverter.GetGraphFromGraphDrawingUpdateDTO(graphDTO, calculateWienerIndexOnly: true);

            return graph.GraphProperties.WienerIndex;
        }
    }
}