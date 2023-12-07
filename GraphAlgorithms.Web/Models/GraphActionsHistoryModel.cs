using GraphAlgorithms.Service.DTO;
using System.Collections.Generic;

namespace GraphAlgorithms.Web.Models
{
    public class GraphActionsHistoryModel
    {
        public List<ActionDTO> Actions { get; set; }
        public PaginationModel PaginationInfo { get; set; }

        public GraphActionsHistoryModel()
        {
            PaginationInfo = new();
        }
    }
}
