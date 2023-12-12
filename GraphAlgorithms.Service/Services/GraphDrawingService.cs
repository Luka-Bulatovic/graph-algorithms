using GraphAlgorithms.Core;
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

        public async Task StoreGraph(GraphDTO graphDTO/*, int ActionTypeID = 1*/)
        {
            GraphEntity graphEntity = graphConverter.GetGraphEntityFromGraphDTO(graphDTO);

            // We probably don't need ActionTypeID as this should always be for drawing, so enum value is enough

            // Create an ActionEntity and set its properties
            var actionEntity = new ActionEntity
            {
                ActionTypeID = (int)ActionTypeEnum.Draw,
                CreatedByID = 0, // Set the creator's ID,
                CreatedDate = DateTime.UtcNow
            };

            // Associate the ActionEntity with the GraphEntity
            graphEntity.Action = actionEntity;

            // TODO: Add some stuff here that should be passed as parameters to this method
            // That stuff, such as ActionTypeID etc, should be added to graphEntity before persisting it

            // Save GraphEntity (along with the associated ActionEntity)
            await graphRepository.SaveAsync(graphEntity);
        }
    }
}