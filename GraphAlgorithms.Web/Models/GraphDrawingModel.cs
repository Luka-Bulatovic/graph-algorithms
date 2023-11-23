using GraphAlgorithms.Service.DTO;

namespace GraphAlgorithms.Web.Models
{
    public class GraphDrawingModel
    {
        public int ID { get; set; }
        public GraphCanvasModel GraphCanvasModel { get; set; }

        public GraphDrawingModel(int id, bool showSaveButton = false, bool showSaveAsNewButton = false)
        {
            ID = id;
            GraphCanvasModel = new GraphCanvasModel(id, isEditable: true, showSaveButton: showSaveButton, showSaveAsNewButton: showSaveAsNewButton);
        }

        public void SetCanvasGraph(GraphDTO graph)
        {
            this.GraphCanvasModel.SetGraph(graph);
        }
    }
}
