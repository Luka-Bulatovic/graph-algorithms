using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Converters
{
    public class ActionConverter : IActionConverter
    {
        public ActionDTO GetActionDTOFromActionEntity(ActionEntity actionEntity)
        {
            return new ActionDTO
            {
                ID = actionEntity.ID,
                ActionTypeName = actionEntity.ActionType.Name,
                ForGraphClassName = actionEntity.ForGraphClass != null 
                                        ? actionEntity.ForGraphClass.Name : "",
                CreatedByUserName = actionEntity.CreatedBy.UserName,
                CreatedDate = actionEntity.CreatedDate,
                CriteriaName = actionEntity.RandomGraphCriteria != null ? actionEntity.RandomGraphCriteria.Name : ""
            };
        }
    }
}
