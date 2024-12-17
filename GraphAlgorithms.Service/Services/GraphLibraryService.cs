using GraphAlgorithms.Core;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Shared;

namespace GraphAlgorithms.Service.Services
{
    public class GraphLibraryService : IGraphLibraryService
    {
        public readonly IGraphRepository graphRepository;
        public readonly IGraphConverter graphConverter;
        public readonly GraphEvaluator graphEvaluator;
        
        public GraphLibraryService(
            IGraphRepository graphRepository, 
            IGraphConverter graphConverter, 
            GraphEvaluator graphEvaluator
            )
        {
            this.graphRepository = graphRepository;
            this.graphConverter = graphConverter;
            this.graphEvaluator = graphEvaluator;
        }

        public async Task<string> ExportGraph(GraphDrawingUpdateDTO graphDTO, string rootFolder)
        {
            Graph graph = graphConverter.GetGraphFromGraphDrawingUpdateDTO(graphDTO);
            string graphML = graphEvaluator.GetGraphMLForGraph(graph);

            Guid guid = Guid.NewGuid();
            string virtualPath = "Temp/" + guid.ToString() + ".graphml";
            string filePath = Path.Combine(rootFolder, virtualPath);
            await File.WriteAllTextAsync(filePath, graphML);

            return virtualPath;
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
