using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GraphAlgorithms.Web.Models
{
    public class RandomConnectedGraphModel
    {
        [DisplayName("Nodes")]
        [Required]
        [Range(1, int.MaxValue)]
        public int Nodes { get; set; }

        [DisplayName("Min. Edges Factor (%)")]
        [Required]
        [Range(1, 100)]
        public int MinEdgesFactor { get; set; }
    }
}
