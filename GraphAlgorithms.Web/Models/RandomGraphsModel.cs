using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
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
        [Range(1, 1000000)]
        public int TotalNumberOfRandomGraphs { get; set; }

        [DisplayName("Number of top Graphs to store")]
        [Range(1, 10)]
        public int StoreTopNumberOfGraphs { get; set; }

        public RandomConnectedGraphModel RandomConnectedGraphModel { get; set; }
        public RandomUnicyclicBipartiteGraphModel RandomUnicyclicBipartiteGraphModel { get; set; }
        public RandomAcyclicGraphWithFixedDiameterModel RandomAcyclicGraphWithFixedDiameterModel { get; set; }

        public RandomGraphsModel()
        {
            RandomConnectedGraphModel = new RandomConnectedGraphModel("RandomConnectedGraphModel");
            RandomUnicyclicBipartiteGraphModel = new RandomUnicyclicBipartiteGraphModel("RandomUnicyclicBipartiteGraphModel");
            RandomAcyclicGraphWithFixedDiameterModel = new RandomAcyclicGraphWithFixedDiameterModel("RandomAcyclicGraphWithFixedDiameterModel");

            TotalNumberOfRandomGraphs = 10000;
            StoreTopNumberOfGraphs = 6;
        }

        public IRandomGraphParamsModel GetParamsModel()
        {
            List<IRandomGraphParamsModel> models = new List<IRandomGraphParamsModel>()
            {
                RandomConnectedGraphModel,
                RandomUnicyclicBipartiteGraphModel,
                RandomAcyclicGraphWithFixedDiameterModel
            };

            foreach(IRandomGraphParamsModel model in models)
            {
                if (model.GetGraphClass() == (Shared.Shared.GraphClassEnum)GraphClassID)
                    return model;
            }

            return null;
        }
    }
}
