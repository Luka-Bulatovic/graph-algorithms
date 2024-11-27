﻿using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Classifiers;
using GraphAlgorithms.Core.Factories;
using GraphAlgorithms.Core.Interfaces;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Service.Services
{
    public class RandomGraphsService : IRandomGraphsService
    {
        public readonly IGraphConverter graphConverter;
        public readonly IActionConverter actionConverter;
        public readonly IGraphRepository graphRepository;
        public readonly IActionRepository actionRepository;
        public readonly IGraphClassRepository graphClassRepository;
        public readonly IGraphPropertyConverter graphPropertyConverter;
        public readonly GraphEvaluator graphEvaluator;
        public readonly RandomGraphsGenerator randomGraphsGenerator;

        public RandomGraphsService(
            IGraphConverter graphConverter,
            IGraphRepository graphRepository,
            IActionConverter actionConverter,
            IActionRepository actionRepository,
            IGraphClassRepository graphClassRepository,
            IGraphPropertyConverter graphPropertyConverter,
            GraphEvaluator graphEvaluator,
            RandomGraphsGenerator randomGraphsGenerator)
        {
            this.graphConverter = graphConverter;
            this.graphRepository = graphRepository;
            this.actionConverter = actionConverter;
            this.actionRepository = actionRepository;
            this.graphClassRepository = graphClassRepository;
            this.graphPropertyConverter = graphPropertyConverter;
            this.graphEvaluator = graphEvaluator;
            this.randomGraphsGenerator = randomGraphsGenerator;
        }

        public async Task<List<GraphPropertyDTO>> GetGraphClassProperties(GraphClassEnum graphClass)
        {
            List<GraphPropertyEntity> propertyEntities = 
                await graphClassRepository.GetRandomGenerationPropertiesForGraphClassAsync((int)graphClass);

            return propertyEntities
                .Select(pe => graphPropertyConverter.GetGraphPropertyDTOFromGraphPropertyEntity(pe))
                .ToList();
        }

        public async Task<ActionDTO> StoreGeneratedGraphs(List<Graph> graphs, GraphClassEnum graphClass)
        {
            //      Persisting data
            // Create an ActionEntity and set its properties
            var actionEntity = new ActionEntity
            {
                ActionTypeID = (int)ActionTypeEnum.GenerateRandom,
                ForGraphClassID = (int)graphClass,
                CreatedByID = 0, // Set the creator's ID,
                CreatedDate = DateTime.UtcNow
            };

            // Convert and store best Graphs to DB
            foreach (Graph graph in graphs)
            {
                graphEvaluator.CalculateGraphPropertiesAndClasses(graph);

                GraphEntity graphEntity = await graphConverter.GetGraphEntityFromGraph(graph);

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
