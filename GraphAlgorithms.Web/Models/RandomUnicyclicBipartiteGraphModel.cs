using GraphAlgorithms.Shared.DTO;
using GraphAlgorithms.Web.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

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
        [EvenValue("Value must be even.")]
        [DisplayName("Cycle Length")]
        public int CycleLength { get; set; }

        public string PropertyNamePrefix { get; set; }

        public RandomUnicyclicBipartiteGraphModel(string propertyNamePrefix)
        {
            PropertyNamePrefix = propertyNamePrefix;

            if (!string.IsNullOrEmpty(PropertyNamePrefix) && !PropertyNamePrefix.EndsWith('.'))
                PropertyNamePrefix = PropertyNamePrefix + '.';
        }

        public RandomGraphDataDTO GetDataDTO()
        {
            return new RandomGraphDataDTO
            {
                FirstPartitionSize = FirstPartitionSize,
                SecondPartitionSize = SecondPartitionSize,
                CycleLength = CycleLength
            };
        }
    }
}
