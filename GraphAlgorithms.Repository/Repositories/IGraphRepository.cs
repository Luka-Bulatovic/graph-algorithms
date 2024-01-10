using GraphAlgorithms.Repository.Entities;

namespace GraphAlgorithms.Repository.Repositories
{
    public interface IGraphRepository
    {
        Task<List<GraphEntity>> GetAllAsync();
        Task<GraphEntity> GetByIdAsync(int id);
        Task<GraphEntity> SaveAsync(GraphEntity graph);
        Task<(List<GraphEntity>, int)> GetGraphsPaginatedAsync(int pageNumber, int pageSize);
        Task<(List<GraphEntity>, int)> GetGraphsForActionPaginatedAsync(int actionID, int pageNumber, int pageSize);
    }
}
