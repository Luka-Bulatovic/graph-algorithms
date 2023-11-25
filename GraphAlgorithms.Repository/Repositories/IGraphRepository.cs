using GraphAlgorithms.Repository.Entities;

namespace GraphAlgorithms.Repository.Repositories
{
    public interface IGraphRepository
    {
        Task<IEnumerable<GraphEntity>> GetAllAsync();
        Task<GraphEntity> GetByIdAsync(int id);
        Task SaveAsync(GraphEntity graph);
    }
}
