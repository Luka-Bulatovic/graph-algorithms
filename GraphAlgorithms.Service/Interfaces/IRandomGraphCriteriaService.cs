using GraphAlgorithms.Service.DTO;


namespace GraphAlgorithms.Service.Interfaces
{
    public interface IRandomGraphCriteriaService
    {
        Task<List<RandomGraphCriteriaDTO>> GetAllAsync();
    }
}
