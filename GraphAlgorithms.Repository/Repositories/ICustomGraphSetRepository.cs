using GraphAlgorithms.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Repositories
{
    public interface ICustomGraphSetRepository
    {
        Task<(List<CustomGraphSetEntity> customGraphSets, int totalCount)> GetCustomGraphSetsPaginatedAsync(int pageNumber, int pageSize);
        Task<CustomGraphSetEntity> GetByIdAsync(int id);
    }
}
