using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Interfaces
{
    public interface IGraphDrawingService
    {
        public Task<GraphDTO> GetGraphDTOByIDAsync(int id);
        public Task<GraphDTO> StoreGraph(GraphDTO graphDTO);
        public int CalculateWienerIndex(GraphDTO graphDTO);
    }
}