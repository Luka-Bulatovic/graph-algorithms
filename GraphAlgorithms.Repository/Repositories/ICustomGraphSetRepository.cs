using GraphAlgorithms.Repository.Entities;

namespace GraphAlgorithms.Repository.Repositories
{
    public interface ICustomGraphSetRepository
    {
        Task<(List<CustomGraphSetEntity> customGraphSets, int totalCount)> GetCustomGraphSetsPaginatedAsync(string userID, int pageNumber, int pageSize);
        Task<CustomGraphSetEntity> GetByIdAsync(int id);
        Task<CustomGraphSetEntity> Create(string customGraphSetName, string userID, List<GraphEntity> graphs);
        Task<CustomGraphSetEntity> AddGraphsToSetAsync(CustomGraphSetEntity customGraphSet, List<GraphEntity> graphs);
        Task<List<CustomGraphSetEntity>> GetAllCustomSetsForUserAsync(string userID);
    }
}
