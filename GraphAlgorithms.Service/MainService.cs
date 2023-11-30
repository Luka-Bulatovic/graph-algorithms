using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Factories;
using GraphAlgorithms.Repository.Data;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.DTO;
using System.Collections.Concurrent;

namespace GraphAlgorithms.Service
{
    public class MainService : IMainService
    {
        private readonly IGraphRepository graphRepository;
        private readonly IGraphConverter graphConverter;

        // So far, this is for testing only, so we put all in MainService
        public MainService(IGraphRepository graphRepository, IGraphConverter graphConverter)
        {
            this.graphRepository = graphRepository;
            this.graphConverter = graphConverter;
        }

        public List<GraphDTO> GetBestUnicyclicBipartiteGraphs(int p, int q, int k)
        {
            List<GraphDTO> bestGraphs = new List<GraphDTO>();
            int numberOfRandomGraphs = 3000;
            int numberOfBestGraphs = 10;

            ConcurrentBag<WienerIndexAlgorithm> graphs = new ConcurrentBag<WienerIndexAlgorithm>();
            Parallel.For(0, numberOfRandomGraphs, i =>
            {
                var factory = new RandomUnicyclicBipartiteGraphFactory(p, q, k);
                Graph g = factory.CreateGraph();
                WienerIndexAlgorithm wie = new WienerIndexAlgorithm(g);
                wie.Run();
                graphs.Add(wie);
                // graphs[i].Run();
            });

            var graphsList = graphs.ToList();
            graphsList.Sort((x, y) => { return y.WienerIndexValue - x.WienerIndexValue; });

            for (int i = 0; i < numberOfBestGraphs; i++)
            {
                GraphDTO graphDTO = graphConverter.GetGraphDTOFromGraph(graphsList[i].G);
                /*
                 * TODO: Make GraphProperties class that contains properties such as Wiener Index. 
                 * Add its instance to Graph Core class, so that we can convert it alltogether in GraphDTOConverter and 
                 * not do the below line manually.
                */ 
                graphDTO.score = graphsList[i].WienerIndexValue;
                bestGraphs.Add(graphDTO);
            }

            //string graphml = GraphMLConverter.GetGraphMLForGraph(graphsList[0].G);
            //Graph reconstructedGraph = GraphMLConverter.GetGraphFromGraphML(graphml);

            return bestGraphs;
        }

        public int GetWienerIndexValueForGraphFromDTO(GraphDTO graphDTO)
        {
            Graph graph = graphConverter.GetGraphFromGraphDTO(graphDTO);

            WienerIndexAlgorithm wIdxAlgorithm = new WienerIndexAlgorithm(graph);
            wIdxAlgorithm.Run();

            return wIdxAlgorithm.WienerIndexValue;
        }

        public async Task<GraphDTO> GetGraphDTOByIDAsync(int id)
        {
            GraphEntity graphEntity = await graphRepository.GetByIdAsync(id);

            GraphDTO graphDTO = graphConverter.GetGraphDTOFromGraphEntity(graphEntity);

            return graphDTO;
        }

        public async Task StoreGraph(GraphDTO graphDTO, int ActionTypeID = 1)
        {
            GraphEntity graphEntity = graphConverter.GetGraphEntityFromGraphDTO(graphDTO);
            graphEntity.ActionTypeID = ActionTypeID;
            
            // TODO: Add some stuff here that should be passed as parameters to this method
            // That stuff, such as ActionTypeID etc, should be added to graphEntity before persisting it

            await graphRepository.SaveAsync(graphEntity);
        }
    }
}