using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Entities
{
    public class CustomGraphSetEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public string CreatedByID { get; set; }
        [ForeignKey("CreatedByID")]
        public UserEntity CreatedBy { get; set; }

        public ICollection<GraphEntity> Graphs { get; set; }
    }
}
