using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Services
{
    public class CustomGraphSetsService : ICustomGraphSetsService
    {
        public Task<(List<CustomGraphSetDTO>, int)> GetCustomGraphSetsPaginatedAsync(int pageNumber, int pageSize)
        {
            throw new Exception("NOT IMPLEMENTED");
        }
    }
}
