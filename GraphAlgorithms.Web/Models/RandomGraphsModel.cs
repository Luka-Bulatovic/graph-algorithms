using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GraphAlgorithms.Web.Models
{
    public class RandomGraphsModel
    {
        [DisplayName("Class")]
        public int GraphClassID { get; set; }
        public SelectList GraphClassList { get; set; }

        public RandomConnectedGraphModel RandomConnectedGraphModel { get; set; }
        public RandomUnicyclicBipartiteGraphModel RandomUnicyclicBipartiteGraphModel { get; set; }

        public RandomGraphsModel()
        {
            RandomConnectedGraphModel = new RandomConnectedGraphModel("RandomConnectedGraphModel");
            RandomUnicyclicBipartiteGraphModel = new RandomUnicyclicBipartiteGraphModel("RandomUnicyclicBipartiteGraphModel");
        }
    }
}
