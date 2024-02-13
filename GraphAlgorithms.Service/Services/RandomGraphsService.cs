using GraphAlgorithms.Core;
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

        public RandomGraphsService(
            IGraphConverter graphConverter,
            IGraphRepository graphRepository,
            IActionConverter actionConverter,
            IActionRepository actionRepository,
            IGraphClassRepository graphClassRepository)
        {
            this.graphConverter = graphConverter;
            this.graphRepository = graphRepository;
            this.actionConverter = actionConverter;
            this.actionRepository = actionRepository;
            this.graphClassRepository = graphClassRepository;
        }

        public async Task<ActionDTO> GenerateRandomConnectedGraphs(int numberOfNodes, double minEdgeFactor, int totalNumberOfRandomGraphs, int storeTopNumberOfGraphs)
        {
            RandomConnectedUndirectedGraphFactory factory = new(numberOfNodes, minEdgeFactor);

            return await GenerateRandomGraphs(factory, totalNumberOfRandomGraphs, storeTopNumberOfGraphs);
        }

        public async Task<ActionDTO> GenerateRandomUnicyclicBipartiteGraphs(int firstPartitionSize, int secondPartitionSize, int cycleLength, int totalNumberOfRandomGraphs, int storeTopNumberOfGraphs)
        {
            RandomUnicyclicBipartiteGraphFactory factory = new(firstPartitionSize, secondPartitionSize, cycleLength);

            return await GenerateRandomGraphs(factory, totalNumberOfRandomGraphs, storeTopNumberOfGraphs);
        }

        private async Task<ActionDTO> GenerateRandomGraphs(IGraphFactory factory, int totalNumberOfRandomGraphs, int storeTopNumberOfGraphs)
        {
            List<Graph> graphs = new();

            //      Generating graphs
            // Generate totalNumberOfGraphs Random Graphs
            for (int i = 0; i < totalNumberOfRandomGraphs; i++)
            {
                Graph graph = factory.CreateGraph();
                CalculateGraphProperties(graph);
                graphs.Add(graph);
            }

            // Take top storeNumberOfGraphs Graphs with largest Wiener index value
            graphs = graphs.OrderByDescending(graph => graph.GraphProperties.WienerIndex).ToList();
            graphs = graphs.GetRange(0, storeTopNumberOfGraphs);


            //      Persisting data
            // Create an ActionEntity and set its properties
            var actionEntity = new ActionEntity
            {
                ActionTypeID = (int)ActionTypeEnum.GenerateRandom,
                ForGraphClassID = (int)factory.GetGraphClass(),
                CreatedByID = 0, // Set the creator's ID,
                CreatedDate = DateTime.UtcNow
            };

            // Convert and store best Graphs to DB
            foreach (Graph graph in graphs)
            {
                CalculateGraphClasses(graph);

                GraphEntity graphEntity = graphConverter.GetGraphEntityFromGraph(graph);

                // Associate the ActionEntity with the GraphEntity
                graphEntity.Action = actionEntity;

                //// TODO: Move this all to converter??
                graphEntity.GraphClasses = new List<GraphClassEntity>();
                foreach (GraphClassEnum graphClass in graph.GraphClasses)
                {
                    // First we must find GraphClassEntity with corresponding ID
                    GraphClassEntity graphClassEntity = await graphClassRepository.GetGraphClassByIdAsync((int)graphClass);
                    if(graphClassEntity != null)
                        graphEntity.GraphClasses.Add(graphClassEntity);
                }

                await graphRepository.SaveAsync(graphEntity);
            }

            // Convert actionEntity to ActionDTO and return
            actionEntity = await actionRepository.GetByIdAsync(actionEntity.ID);
            return actionConverter.GetActionDTOFromActionEntity(actionEntity);
        }

        private void CalculateGraphProperties(Graph graph)
        {
            WienerIndexAlgorithm wienerAlgorithm = new WienerIndexAlgorithm(graph);
            wienerAlgorithm.Run();

            graph.GraphProperties.WienerIndex = wienerAlgorithm.WienerIndexValue;
        }

        private void CalculateGraphClasses(Graph graph)
        {
            if (graph == null || graph.Nodes == null || graph.Nodes.Count == 0)
                return;

            graph.GraphClasses.Clear();

            DepthFirstSearchAlgorithm dfsAlgorithm = new (graph, graph.Nodes[0]);
            BreadthFirstSearchAlgorithm bfsAlgorithm = new (graph, graph.Nodes[0]);

            List<IGraphClassifier> graphClassifiers = new List<IGraphClassifier>()
            {
                new ConnectedGraphClassifier(dfsAlgorithm),
                new TreeGraphClassifier(dfsAlgorithm),
                new BipartiteGraphClassifier(bfsAlgorithm),
                new UnicyclicGraphClassifier(dfsAlgorithm),
            };

            foreach(IGraphClassifier graphClassifier in graphClassifiers)
                if(graphClassifier.BelongsToClass())
                    graph.GraphClasses.Add(graphClassifier.GetGraphClass());
        }
    }
}
