using System.ComponentModel;

namespace GraphAlgorithms.Web.Models
{
    public class RandomUnicyclicBipartiteGraphModel
    {
        [DisplayName("Partition 1 Size")]
        public int FirstPartitionSize { get; set; }
        
        [DisplayName("Partition 2 Size")]
        public int SecondPartitionSize { get; set; }

        [DisplayName("Cycle Length")]
        public int CycleLength { get; set; }
    }
}
