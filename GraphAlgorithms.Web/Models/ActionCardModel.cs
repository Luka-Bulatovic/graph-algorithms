using GraphAlgorithms.Service.DTO;

namespace GraphAlgorithms.Web.Models
{
    public class ActionCardModel
    {
        private ActionDTO action;
        public ActionDTO Action => action;

        public ActionCardModel(ActionDTO action)
        {
            this.action = action;
        }
    }
}
