using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models.DataBase
{
    public class Priority
    {
        public int ID { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

    }
}
