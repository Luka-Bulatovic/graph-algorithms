﻿using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Interfaces
{
    public interface IGraphActionsService
    {
        Task<(List<ActionDTO>, int)> GetActionsPaginated(int pageNumber, int pageSize);
    }
}
