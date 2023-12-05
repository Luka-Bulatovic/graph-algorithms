using GraphAlgorithms.Repository.Entities;

namespace GraphAlgorithms.Repository.Repositories
{
    public interface IGraphRepository
    {
        Task<List<GraphEntity>> GetAllAsync();
        Task<GraphEntity> GetByIdAsync(int id);
        Task SaveAsync(GraphEntity graph);
        Task<(List<GraphEntity>, int)> GetGraphsPaginatedAsync(int pageNumber, int pageSize);
    }
}
