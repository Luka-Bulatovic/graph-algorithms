using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Services
{
    public class GraphLibraryService : IGraphLibraryService
    {
        public readonly IGraphRepository graphRepository;
        public readonly IGraphConverter graphConverter;

        public GraphLibraryService(IGraphRepository graphRepository, IGraphConverter graphConverter)
        {
            this.graphRepository = graphRepository;
            this.graphConverter = graphConverter;
        }

        public async Task<List<GraphDTO>> GetGraphs()
        {
            List<GraphEntity> graphEntities = await graphRepository.GetAllAsync();

            return graphEntities.Select(graphEntity => graphConverter.GetGraphDTOFromGraphEntity(graphEntity))
                                .ToList();
        }

        public async Task<(List<GraphDTO>, int)> GetGraphsPaginated(int pageNumber, int pageSize, int actionID = 0, List<SearchParameter> searchParams = null, string sortBy = "")
        {
            (List<GraphEntity> graphEntities, int totalCount) = await graphRepository.GetGraphsPaginatedAsync(pageNumber, pageSize, actionID, searchParams, sortBy);

            List<GraphDTO> graphDTOs = graphEntities
                                        .Select(graphEntity => graphConverter.GetGraphDTOFromGraphEntity(graphEntity))
                                        .ToList();

            return (graphDTOs, totalCount);
        }
    }
}
