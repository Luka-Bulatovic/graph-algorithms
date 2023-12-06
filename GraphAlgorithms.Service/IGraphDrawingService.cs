using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service
{
    public interface IGraphDrawingService
    {
        public int GetWienerIndexValueForGraphFromDTO(GraphDTO graphDTO);
        public Task<GraphDTO> GetGraphDTOByIDAsync(int id);
        public Task StoreGraph(GraphDTO graphDTO);
    }
}