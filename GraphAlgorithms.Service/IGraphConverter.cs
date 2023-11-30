using GraphAlgorithms.Core;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service
{
    public interface IGraphConverter
    {
        public Graph GetGraphFromGraphDTO(GraphDTO graphDTO);
        public GraphDTO GetGraphDTOFromGraph(Graph graph);
        public Graph GetGraphFromGraphEntity(GraphEntity graphEntity);
        public GraphDTO GetGraphDTOFromGraphEntity(GraphEntity graphEntity);
        public GraphEntity GetGraphEntityFromGraphDTO(GraphDTO graphDTO);
    }
}
