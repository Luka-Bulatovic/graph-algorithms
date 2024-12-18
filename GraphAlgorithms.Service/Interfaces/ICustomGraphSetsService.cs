using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Interfaces
{
    public interface ICustomGraphSetsService
    {
        Task<(List<CustomGraphSetDTO>, int)> GetCustomGraphSetsPaginatedAsync(int pageNumber, int pageSize);
    }
}
