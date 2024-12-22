using GraphAlgorithms.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.DTO
{
    public class RandomGraphCriteriaDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public RandomGraphCriteriaDTO()
        {
            
        }

        public RandomGraphCriteriaDTO(RandomGraphCriteriaEntity entity) : this()
        {
            ID = entity.ID;
            Name = entity.Name;
        }
    }
}
