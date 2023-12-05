using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service
{
    public interface IGraphLibraryService
    {
        public Task<List<GraphDTO>> GetGraphs();
        public Task<(List<GraphDTO>, int)> GetGraphsPaginated(int pageNumber, int pageSize);
    }
}
