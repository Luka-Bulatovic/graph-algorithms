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
    public class GraphPropertyConverter : IGraphPropertyConverter
    {
        public GraphPropertyDTO GetGraphPropertyDTOFromGraphPropertyEntity(GraphPropertyEntity graphPropertyEntity)
        {
            return new GraphPropertyDTO()
            {
                ID = graphPropertyEntity.ID,
                Name = graphPropertyEntity.Name,
            };
        }
    }
}
