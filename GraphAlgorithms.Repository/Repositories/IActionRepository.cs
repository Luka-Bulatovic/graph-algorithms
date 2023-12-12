using GraphAlgorithms.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Repositories
{
    public interface IActionRepository
    {
        Task<(List<ActionEntity> actions, int totalCount)> GetActionsPaginatedAsync(int pageNumber, int pageSize);
        Task<ActionEntity> GetByIdAsync(int id);
    }
}
