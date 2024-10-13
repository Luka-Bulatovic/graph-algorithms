using GraphAlgorithms.Core;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Shared.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static GraphAlgorithms.Shared.Shared;

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

        public Dictionary<GraphPropertyEnum, FieldMetadata> PropertiesMetadata { get; set; }

        //public RandomConnectedGraphModel RandomConnectedGraphModel { get; set; }
        //public RandomUnicyclicBipartiteGraphModel RandomUnicyclicBipartiteGraphModel { get; set; }
        //public RandomAcyclicGraphWithFixedDiameterModel RandomAcyclicGraphWithFixedDiameterModel { get; set; }

        public RandomGraphDataDTO Data { get; set; }

        public RandomGraphsModel()
        {
            //RandomConnectedGraphModel = new RandomConnectedGraphModel("RandomConnectedGraphModel");
            //RandomUnicyclicBipartiteGraphModel = new RandomUnicyclicBipartiteGraphModel("RandomUnicyclicBipartiteGraphModel");
            //RandomAcyclicGraphWithFixedDiameterModel = new RandomAcyclicGraphWithFixedDiameterModel("RandomAcyclicGraphWithFixedDiameterModel");

            TotalNumberOfRandomGraphs = 10000;
            StoreTopNumberOfGraphs = 6;
            Data = new RandomGraphDataDTO();
        }

        private Dictionary<GraphPropertyEnum, FieldMetadata> GetAllPropertiesMetadata()
        {
            return new Dictionary<GraphPropertyEnum, FieldMetadata>()
            {
                { GraphPropertyEnum.Order, new FieldMetadata("Nodes", () => Data.Nodes, value => Data.Nodes = (int)value, typeof(int)) },
                { GraphPropertyEnum.MinSizeCoef, new FieldMetadata("MinEdgesFactor", () => Data.MinEdgesFactor, value => Data.MinEdgesFactor = (int)value, typeof(int)) },

                // TODO: Add more here as more are defined
            };
        }

        public void InitializeMetadataForProperties(List<GraphPropertyDTO> properties)
        {
            this.PropertiesMetadata = new Dictionary<GraphPropertyEnum, FieldMetadata>();
            Dictionary<GraphPropertyEnum, FieldMetadata> allPropertiesMetadata = GetAllPropertiesMetadata();

            foreach(var property in properties)
            {
                if(allPropertiesMetadata.ContainsKey((GraphPropertyEnum)property.ID))
                    this.PropertiesMetadata[(GraphPropertyEnum)property.ID] = allPropertiesMetadata[(GraphPropertyEnum)property.ID];
            }
        }

        //public IRandomGraphParamsModel GetParamsModel()
        //{
        //    List<IRandomGraphParamsModel> models = new List<IRandomGraphParamsModel>()
        //    {
        //        RandomConnectedGraphModel,
        //        RandomUnicyclicBipartiteGraphModel,
        //        RandomAcyclicGraphWithFixedDiameterModel
        //    };

        //    foreach(IRandomGraphParamsModel model in models)
        //    {
        //        if (model.GetGraphClass() == (Shared.Shared.GraphClassEnum)GraphClassID)
        //            return model;
        //    }

        //    return null;
        //}
    }
}
