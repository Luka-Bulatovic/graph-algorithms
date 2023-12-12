using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Converters
{
    public class GraphClassConverter : IGraphClassConverter
    {
        public GraphClassDTO GetGraphClassDTOFromGraphClassEntity(GraphClassEntity graphClassEntity)
        {
            return new GraphClassDTO
            {
                ID = graphClassEntity.ID,
                Name = graphClassEntity.Name
            };
        }
    }
}
