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
    public class GraphDrawingService : IGraphDrawingService
    {
        private readonly IGraphRepository graphRepository;
        private readonly IGraphConverter graphConverter;

        public GraphDrawingService(IGraphRepository graphRepository, IGraphConverter graphConverter)
        {
            this.graphRepository = graphRepository;
            this.graphConverter = graphConverter;
        }

        public int GetWienerIndexValueForGraphFromDTO(GraphDTO graphDTO)
        {
            Graph graph = graphConverter.GetGraphFromGraphDTO(graphDTO);

            return graph.GraphProperties.WienerIndex;
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