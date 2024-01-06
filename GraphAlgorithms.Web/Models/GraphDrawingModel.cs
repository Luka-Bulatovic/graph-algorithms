using GraphAlgorithms.Core;
using GraphAlgorithms.Service.DTO;

namespace GraphAlgorithms.Web.Models
{
    public class GraphDrawingModel
    {
        public int ID { get; set; }
        public GraphCanvasModel CreateGraphCanvasModel { get; set; }
        public GraphCanvasModel EditGraphCanvasModel { get; set; }
        public bool IsEditing => ID > 0;

        // We probably don't need these params anymore
        public GraphDrawingModel(GraphDTO graph /*, bool showSaveButton = false, bool showSaveAsNewButton = false*/)
        {
            ID = graph.id;

            InitializeCanvasModels(graph);
        }

        private void InitializeCanvasModels(GraphDTO graph)
        {
            if (this.IsEditing)
            {
                CreateGraphCanvasModel = new GraphCanvasModel(graph, isEditable: false);
                EditGraphCanvasModel = new GraphCanvasModel(graph, isEditable: true, showSaveButton: true, showSaveAsNewButton: true, showCalculateButton: true, showHeader: false);
            }
            else
            {
                CreateGraphCanvasModel = new GraphCanvasModel(graph, isEditable: true, showSaveButton: true);
            }
        }
    }
}
