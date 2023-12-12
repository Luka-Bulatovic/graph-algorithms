using GraphAlgorithms.Core;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Interfaces
{
    public interface IGraphConverter
    {
        public Graph GetGraphFromGraphDTO(GraphDTO graphDTO);
        public Graph GetGraphFromGraphEntity(GraphEntity graphEntity);
        public GraphDTO GetGraphDTOFromGraph(Graph graph);
        public GraphDTO GetGraphDTOFromGraphEntity(GraphEntity graphEntity);
        public GraphEntity GetGraphEntityFromGraphDTO(GraphDTO graphDTO);
        public GraphEntity GetGraphEntityFromGraph(Graph graph);
    }
}
