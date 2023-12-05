using GraphAlgorithms.Service.DTO;

namespace GraphAlgorithms.Web.Models
{
    public class GraphDrawingModel
    {
        public int ID { get; set; }
        public GraphCanvasModel GraphCanvasModel { get; set; }

        public GraphDrawingModel(GraphDTO graph, bool showSaveButton = false, bool showSaveAsNewButton = false)
        {
            ID = graph.id;
            GraphCanvasModel = new GraphCanvasModel(graph, isEditable: true, showSaveButton: showSaveButton, showSaveAsNewButton: showSaveAsNewButton);
        }
    }
}
