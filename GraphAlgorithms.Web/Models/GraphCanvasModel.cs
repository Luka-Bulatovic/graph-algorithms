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
        public bool ShowEditButton { get; set; }
        public bool ShowSaveButton { get; set; }
        public bool ShowSaveAsNewButton { get; set; }
        public bool ShowCalculateButton { get; set; }
        public bool ShowHeader { get; set; }

        private GraphDTO graph;
        public GraphDTO Graph => graph;
        
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
        public GraphCanvasModel(GraphDTO graph, bool isEditable = false, bool showEditButton = false, bool showSaveButton = false, bool showSaveAsNewButton = false, bool showCalculateButton = false, bool showHeader = true)
        {
            ID = graph.id;
            this.graph = graph;
            
            IsEditable = isEditable;
            
            ShowEditButton = showEditButton;
            ShowSaveButton = showSaveButton;
            ShowSaveAsNewButton = showSaveAsNewButton;
            ShowCalculateButton = showCalculateButton;
            
            ShowHeader = showHeader;
        }
        #endregion
    }
}
