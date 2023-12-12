using GraphAlgorithms.Web.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GraphAlgorithms.Web.Models
{
    public class RandomUnicyclicBipartiteGraphModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        [DisplayName("Partition 1 Size")]
        public int FirstPartitionSize { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [DisplayName("Partition 2 Size")]
        public int SecondPartitionSize { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [LessThanOrEqualToProperties(
            new string[] { "FirstPartitionSize", "SecondPartitionSize" },
            "Value must be less than or equal to Partition 1 and 2 Sizes."
        )]
        [DisplayName("Cycle Length")]
        public int CycleLength { get; set; }

        public string PropertyNamePrefix { get; set; }

        public RandomUnicyclicBipartiteGraphModel(string propertyNamePrefix)
        {
            PropertyNamePrefix = propertyNamePrefix;
        }
    }
}
