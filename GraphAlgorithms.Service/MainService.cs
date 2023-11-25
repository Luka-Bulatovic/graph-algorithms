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
            Graph g = graphConverter.GetGraphFromGraphDTO(graphDTO);

            WienerIndexAlgorithm wie = new WienerIndexAlgorithm(g);
            wie.Run();

            return wie.WienerIndexValue;
        }

        public async Task<GraphDTO> GetGraphDTOByIDAsync(int id)
        {
            GraphEntity graphEntity = await graphRepository.GetByIdAsync(id);

            // Transform Graph Repository model into actual Graph object
            // TODO: We should change this and introduce some GraphEntityConverter, so that we get Graph from GraphEntity
            Graph graph = GraphMLConverter.GetGraphFromGraphML(graphEntity.ID, graphEntity.DataXML);

            // Transform Graph object into GraphDTO
            GraphDTO graphDTO = graphConverter.GetGraphDTOFromGraph(graph); //new GraphDTO(graph, 0); // ...

            return graphDTO;
        }

        public async Task StoreGraph(GraphDTO graphDTO)
        {
            GraphEntity graphEntity = graphConverter.GetGraphEntityFromGraphDTO(graphDTO);

            await graphRepository.SaveAsync(graphEntity);
        }
    }
}