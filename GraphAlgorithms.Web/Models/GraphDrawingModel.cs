using GraphAlgorithms.Core;
using GraphAlgorithms.Service.DTO;

namespace GraphAlgorithms.Web.Models
{
    public class GraphDrawingModel
    {
        public int ID { get; set; }
        public GraphCanvasModel CreateGraphCanvasModel { get; set; }
        public GraphCanvasModel EditGraphCanvasModel { get; set; }
        public AddToCustomSetModel CustomSetModel { get; set; }
        public bool IsViewOnly { get; set; }
        public bool IsEditing => ID > 0 && !IsViewOnly;

        // We probably don't need these params anymore
        public GraphDrawingModel(GraphDTO graph, bool isViewOnly = false)
        {
            ID = graph.id;
            IsViewOnly = isViewOnly;

            InitializeCanvasModels(graph);
            CustomSetModel = new AddToCustomSetModel();
        }

        private void InitializeCanvasModels(GraphDTO graph)
        {
            if(this.IsViewOnly)
            {
                CreateGraphCanvasModel = new GraphCanvasModel(graph, isEditable: false);
            }
            else if (this.IsEditing)
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
