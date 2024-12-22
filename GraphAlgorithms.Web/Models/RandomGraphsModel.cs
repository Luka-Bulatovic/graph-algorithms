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

        // Criteria for generating random graphs
        [DisplayName("Criteria")]
        public int CriteriaID { get; set; }
        public SelectList Criteria { get; set; }

        public Dictionary<GraphPropertyEnum, FieldMetadata> PropertiesMetadata { get; set; }

        public RandomGraphDataDTO Data { get; set; }

        public RandomGraphsModel()
        {
            TotalNumberOfRandomGraphs = 10000;
            StoreTopNumberOfGraphs = 6;
            Data = new RandomGraphDataDTO();
        }

        public RandomGraphsModel(int graphClassID, SelectList graphClassList, SelectList criteria) : this()
        {
            GraphClassID = graphClassID;
            GraphClassList = graphClassList;
            Criteria = criteria;
            CriteriaID = 2; // By default, max Wiener Index
        }

        private Dictionary<GraphPropertyEnum, FieldMetadata> GetAllPropertiesMetadata()
        {
            return new Dictionary<GraphPropertyEnum, FieldMetadata>()
            {
                { 
                    GraphPropertyEnum.Order, 
                    new FieldMetadata(
                        "Nodes", 
                        () => Data.Nodes, 
                        value => Data.Nodes = (int)value, 
                        typeof(int),
                        isRequired: true,
                        minValue: 1,
                        maxValue: 200
                        ) 
                },
                { 
                    GraphPropertyEnum.MinSizeCoef, 
                    new FieldMetadata(
                        "MinEdgesFactor", 
                        () => Data.MinEdgesFactor, 
                        value => Data.MinEdgesFactor = (int)value, 
                        typeof(int),
                        isRequired: true,
                        minValue: 1,
                        maxValue: 100
                        ) 
                },
                
                { 
                    GraphPropertyEnum.FirstBipartitionSize, 
                    new FieldMetadata(
                        "FirstPartitionSize", 
                        () => Data.FirstPartitionSize, 
                        value => Data.FirstPartitionSize = (int)value, 
                        typeof(int),
                        isRequired: true,
                        minValue: 1,
                        maxValue: 100
                        ) 
                },
                { 
                    GraphPropertyEnum.SecondBipartitionSize, 
                    new FieldMetadata(
                        "SecondPartitionSize", 
                        () => Data.SecondPartitionSize, 
                        value => Data.SecondPartitionSize = (int)value, 
                        typeof(int),
                        isRequired: true,
                        minValue: 1,
                        maxValue: 100
                        ) 
                },
                { 
                    GraphPropertyEnum.CycleLength, 
                    new FieldMetadata(
                        "CycleLength", 
                        () => Data.CycleLength, 
                        value => Data.CycleLength = (int)value, 
                        typeof(int),
                        isRequired: true,
                        minValue: 3,
                        maxValue: 100,
                        isEven: true,
                        lessThanOrEqualToPropertyNames: new string[] { "FirstPartitionSize", "SecondPartitionSize" }
                        ) 
                },

                { 
                    GraphPropertyEnum.Diameter, 
                    new FieldMetadata(
                        "Diameter", 
                        () => Data.Diameter, 
                        value => Data.Diameter = (int)value, 
                        typeof(int),
                        isRequired: true,
                        minValue: 1,
                        maxValue: 100
                        ) 
                },

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
    }
}
