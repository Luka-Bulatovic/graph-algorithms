using GraphAlgorithms.Repository.Models;

namespace GraphAlgorithms.Repository.Repositories
{
    public interface IGraphRepository
    {
        Task<IEnumerable<Graph>> GetAllAsync();
        Task<Graph> GetByIdAsync(int id);
        Task AddAsync(Graph graph);
    }
}
