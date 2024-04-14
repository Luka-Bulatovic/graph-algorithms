using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GraphAlgorithms.Web.Models
{
    public class RandomAcyclicGraphWithFixedDiameterModel
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
    }
}
