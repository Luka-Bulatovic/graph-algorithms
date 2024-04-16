using GraphAlgorithms.Shared.DTO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace GraphAlgorithms.Web.Models
{
    public class RandomConnectedGraphModel : IRandomGraphParamsModel
    {
        [DisplayName("Nodes")]
        [Required]
        [Range(1, int.MaxValue)]
        public int Nodes { get; set; }

        [DisplayName("Min. Edges Factor (%)")]
        [Required]
        [Range(1, 100)]
        public int MinEdgesFactor { get; set; }

        public string PropertyNamePrefix { get; set; }

        public RandomConnectedGraphModel(string propertyNamePrefix)
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
                MinEdgesFactor = MinEdgesFactor
            };
        }

        public Shared.Shared.GraphClassEnum GetGraphClass()
        {
            return Shared.Shared.GraphClassEnum.ConnectedGraph;
        }
    }
}
