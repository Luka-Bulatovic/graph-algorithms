﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Entities
{
    public class ActionEntity
    {
        public int ID { get; set; }
        
        [ForeignKey("ActionType")]
        public int ActionTypeID { get; set; }

        [ForeignKey("ForGraphClass")]
        public int? ForGraphClassID { get; set; }

        public int CreatedByID { get; set; }
        public DateTime CreatedDate { get; set; }

        public ActionTypeEntity ActionType { get; set; }
        public GraphClassEntity? ForGraphClass { get; set; }
        public ICollection<GraphEntity> Graphs { get; set; }
    }
}
