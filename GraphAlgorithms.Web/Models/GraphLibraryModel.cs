using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;

namespace GraphAlgorithms.Web.Models
{
    public class GraphLibraryModel
    {
        public List<GraphDTO> Graphs { get; set; }

        public int ItemsPerRow = 3;
        public int ItemsPerPage { get; }

        public GraphLibraryModel(int itemsPerPage = 9)
        {
            if (itemsPerPage % ItemsPerRow > 0)
                throw new ArgumentException(string.Format("ItemsPerPage must be multiple of {0}", ItemsPerRow));

            ItemsPerPage = itemsPerPage;
        }
    }
}
