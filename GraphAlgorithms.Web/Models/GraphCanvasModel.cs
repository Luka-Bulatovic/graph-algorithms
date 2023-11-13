using GraphAlgorithms.Service.DTO;
using System.Collections.Generic;
using System.Text.Json;

namespace GraphAlgorithms.Web.Models
{
    public class GraphCanvasModel
    {
        #region Properties
        public int ID { get; set; }
        public bool IsEditable { get; set; }
        public GraphDTO Graph { get; set; }
        public string NodesJSON
        {
            get
            {
                return Graph != null ?
                    JsonSerializer.Serialize(Graph.nodes) :
                    JsonSerializer.Serialize(new List<NodeDTO>());
            }
        }
        public string EdgesJSON
        {
            get
            {
                return Graph != null ?
                    JsonSerializer.Serialize(Graph.edges) :
                    JsonSerializer.Serialize(new List<EdgeDTO>());
            }
        }
        #endregion

        #region Constructors
        public GraphCanvasModel(int id, bool isEditable = false)
        {
            ID = id;
            IsEditable = isEditable;
        }
        #endregion
    }
}
