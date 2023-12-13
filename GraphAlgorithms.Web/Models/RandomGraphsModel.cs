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

        [DisplayName("Total number of generated Random Graphs")]
        [Range(1, 30000)]
        public int TotalNumberOfRandomGraphs { get; set; }

        [DisplayName("Number of top Graphs to store")]
        [Range(1, 10)]
        public int StoreTopNumberOfGraphs { get; set; }

        public RandomConnectedGraphModel RandomConnectedGraphModel { get; set; }
        public RandomUnicyclicBipartiteGraphModel RandomUnicyclicBipartiteGraphModel { get; set; }

        public RandomGraphsModel()
        {
            RandomConnectedGraphModel = new RandomConnectedGraphModel("RandomConnectedGraphModel");
            RandomUnicyclicBipartiteGraphModel = new RandomUnicyclicBipartiteGraphModel("RandomUnicyclicBipartiteGraphModel");

            TotalNumberOfRandomGraphs = 10000;
            StoreTopNumberOfGraphs = 6;
        }
    }
}
