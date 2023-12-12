using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Service.DTO;

namespace GraphAlgorithms.Service.Interfaces
{
    public interface IActionConverter
    {
        public ActionDTO GetActionDTOFromActionEntity(ActionEntity actionEntity);
    }
}