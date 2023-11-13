namespace GraphAlgorithms.Web.Models
{
    public class GraphDrawingModel
    {
        public int ID { get; set; }
        public GraphCanvasModel GraphCanvasModel { get; set; }

        public GraphDrawingModel(int id)
        {
            ID = id;
            GraphCanvasModel = new GraphCanvasModel(id);
        }
    }
}
