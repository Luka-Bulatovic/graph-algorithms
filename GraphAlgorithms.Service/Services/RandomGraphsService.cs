﻿using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Factories;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Services
{
    public class RandomGraphsService : IRandomGraphsService
    {
        public readonly IGraphConverter graphConverter;
        public readonly IActionConverter actionConverter;
        public readonly IGraphRepository graphRepository;
        public readonly IActionRepository actionRepository;

        public RandomGraphsService(
            IGraphConverter graphConverter,
            IGraphRepository graphRepository,
            IActionConverter actionConverter,
            IActionRepository actionRepository)
        {
            this.graphConverter = graphConverter;
            this.graphRepository = graphRepository;
            this.actionConverter = actionConverter;
            this.actionRepository = actionRepository;
        }

        public async Task<ActionDTO> GenerateRandomConnectedGraphs(int numberOfNodes, double minEdgeFactor)
        {
            int totalNumberOfGraphs = 10000;
            int storeNumberOfGraphs = 6;
            RandomConnectedUndirectedGraphFactory factory = new(numberOfNodes, minEdgeFactor);

            List<Graph> graphs = new();



            //      Generating graphs
            // Generate totalNumberOfGraphs Random Graphs
            for (int i = 0; i < totalNumberOfGraphs; i++)
            {
                Graph graph = factory.CreateGraph();

                WienerIndexAlgorithm wienerAlgorithm = new WienerIndexAlgorithm(graph);
                wienerAlgorithm.Run();

                graph.GraphProperties.WienerIndex = wienerAlgorithm.WienerIndexValue;

                graphs.Add(graph);
            }

            // Take top storeNumberOfGraphs Graphs with largest Wiener index value
            graphs = graphs.OrderByDescending(graph => graph.GraphProperties.WienerIndex).ToList();
            graphs = graphs.GetRange(0, storeNumberOfGraphs);




            //      Persisting data
            // Create an ActionEntity and set its properties
            var actionEntity = new ActionEntity
            {
                ActionTypeID = (int)ActionTypeEnum.GenerateRandom,
                CreatedByID = 0, // Set the creator's ID,
                CreatedDate = DateTime.UtcNow
            };

            // Convert and store best Graphs to DB
            foreach (var graph in graphs)
            {
                GraphEntity graphEntity = graphConverter.GetGraphEntityFromGraph(graph);
                
                // Associate the ActionEntity with the GraphEntity
                graphEntity.Action = actionEntity;

                await graphRepository.SaveAsync(graphEntity);
            }

            // Convert actionEntity to ActionDTO and return
            actionEntity = await actionRepository.GetByIdAsync(actionEntity.ID);
            return actionConverter.GetActionDTOFromActionEntity(actionEntity);
        }
    }
}
