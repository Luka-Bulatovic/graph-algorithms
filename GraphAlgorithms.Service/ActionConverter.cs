using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service
{
    public class ActionConverter : IActionConverter
    {
        public ActionDTO GetActionDTOFromActionEntity(ActionEntity actionEntity)
        {
            return new ActionDTO
            {
                ID = actionEntity.ID,
                ActionTypeName = actionEntity.ActionType.Name,
                CreatedByUserName = "System", // TODO
                CreatedDate = actionEntity.CreatedDate
            };
        }
    }
}
