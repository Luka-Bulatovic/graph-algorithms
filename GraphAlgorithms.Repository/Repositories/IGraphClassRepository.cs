using GraphAlgorithms.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Repositories
{
    public interface IGraphClassRepository
    {
        Task<GraphClassEntity> GetGraphClassByIdAsync(int id);
        Task<List<GraphClassEntity>> GetGraphClassesAsync();
        Task<List<GraphClassEntity>> GetClassifiableGraphClassesAsync();
        Task<List<GraphClassEntity>> GetGraphClassesByIDsAsync(List<int> ids);
        Task<List<GraphClassEntity>> GetGraphClassesForGeneratingRandomGraphsAsync();
        Task<List<GraphPropertyEntity>> GetRandomGenerationPropertiesForGraphClassAsync(int id);
    }
}
