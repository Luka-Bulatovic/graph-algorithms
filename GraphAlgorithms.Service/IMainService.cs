using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service
{
    public interface IMainService
    {
        public List<GraphDTO> GetBestUnicyclicBipartiteGraphs(int p, int q, int k);
        public int GetWienerIndexValueForGraphFromDTO(List<NodeDTO> nodes, List<EdgeDTO> edges);
        public Task<GraphDTO> GetGraphDTOByIDAsync(int id);
    }
}