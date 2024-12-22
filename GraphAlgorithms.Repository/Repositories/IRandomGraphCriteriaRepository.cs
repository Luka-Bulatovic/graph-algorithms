using GraphAlgorithms.Repository.Entities;

namespace GraphAlgorithms.Repository.Repositories
{
    public interface IRandomGraphCriteriaRepository
    {
        Task<List<RandomGraphCriteriaEntity>> GetAllAsync();
    }
}
