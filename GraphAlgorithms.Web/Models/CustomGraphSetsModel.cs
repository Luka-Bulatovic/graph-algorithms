using GraphAlgorithms.Service.DTO;
using System.Collections.Generic;

namespace GraphAlgorithms.Web.Models
{
    public class CustomGraphSetsModel
    {
        public List<CustomGraphSetDTO> CustomGraphSets { get; }
        public PaginationModel PaginationInfo { get; }

        public CustomGraphSetsModel(List<CustomGraphSetDTO> customGraphSets, PaginationModel paginationInfo)
        {
            this.CustomGraphSets = customGraphSets;
            this.PaginationInfo = paginationInfo;
        }
    }
}
