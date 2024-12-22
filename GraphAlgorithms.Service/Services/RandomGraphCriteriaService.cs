using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Services
{
    public class RandomGraphCriteriaService : IRandomGraphCriteriaService
    {
        private readonly IRandomGraphCriteriaRepository randomGraphCriteriaRepository;
        public RandomGraphCriteriaService(IRandomGraphCriteriaRepository randomGraphCriteriaRepository)
        {
            this.randomGraphCriteriaRepository = randomGraphCriteriaRepository;
        }

        public async Task<List<RandomGraphCriteriaDTO>> GetAllAsync()
        {
            var allEntities = await randomGraphCriteriaRepository.GetAllAsync();
            return allEntities.Select(e => new RandomGraphCriteriaDTO(e)).ToList();
        }
    }
}
