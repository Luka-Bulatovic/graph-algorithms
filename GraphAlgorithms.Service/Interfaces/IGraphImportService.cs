using GraphAlgorithms.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Interfaces
{
    public interface IGraphImportService
    {
        public Task<GraphEntity> ImportFromFile(string filePath);
    }
}
