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
    public class CustomGraphSetConverter : ICustomGraphSetConverter
    {
        public CustomGraphSetDTO GetCustomGraphSetDTOFromEntity(CustomGraphSetEntity entity)
        {
            return new CustomGraphSetDTO()
            {
                ID = entity.ID,
                CreatedDate = entity.CreatedDate,
                Name = entity.Name
            };
        }
    }
}
