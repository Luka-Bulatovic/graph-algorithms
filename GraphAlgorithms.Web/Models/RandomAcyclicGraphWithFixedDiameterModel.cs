using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json;
using GraphAlgorithms.Shared.DTO;

namespace GraphAlgorithms.Web.Models
{
    public class RandomAcyclicGraphWithFixedDiameterModel : IRandomGraphParamsModel
    {
        [DisplayName("Nodes")]
        [Required]
        [Range(1, int.MaxValue)]
        public int Nodes { get; set; }

        [DisplayName("Diameter")]
        [Required]
        [Range(1, int.MaxValue)]
        public int Diameter { get; set; }

        public string PropertyNamePrefix { get; set; }

        public RandomAcyclicGraphWithFixedDiameterModel(string propertyNamePrefix)
        {
            PropertyNamePrefix = propertyNamePrefix;

            if (!string.IsNullOrEmpty(PropertyNamePrefix) && !PropertyNamePrefix.EndsWith('.'))
                PropertyNamePrefix = PropertyNamePrefix + '.';
        }

        public RandomGraphDataDTO GetDataDTO()
        {
            return new RandomGraphDataDTO
            {
                Nodes = Nodes,
                Diameter = Diameter
            };
        }

        public Shared.Shared.GraphClassEnum GetGraphClass()
        {
            return Shared.Shared.GraphClassEnum.AcyclicGraphWithFixedDiameter;
        }
    }
}
