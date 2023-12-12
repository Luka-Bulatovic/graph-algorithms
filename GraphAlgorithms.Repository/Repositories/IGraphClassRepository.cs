using GraphAlgorithms.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Repositories
{
    public interface IGraphClassRepository
    {
        Task<List<GraphClassEntity>> GetGraphClassesAsync();
    }
}
