using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Shared;

namespace GraphAlgorithms.Repository.Repositories
{
    public interface IGraphRepository
    {
        Task<List<GraphEntity>> GetAllAsync();
        Task<GraphEntity> GetByIdAsync(int id);
        Task<GraphEntity> SaveAsync(GraphEntity graph);
        Task<(List<GraphEntity>, int)> GetGraphsPaginatedAsync(int pageNumber, int pageSize, int actionID, List<SearchParameter> searchParams, string sortBy);
    }
}
