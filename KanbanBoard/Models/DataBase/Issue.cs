using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models.DataBase
{
    public class Issue
    {
        public int ID { get; set; }
        
        [MaxLength(1000)]
        public string Summary { get; set; }
        [MaxLength(20)]
        public string EstimatedEffort { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CloseDate { get; set; }
        [Required]
        public DateTime RequestedCloseDate { get; set; }

        public IdentityUser AssignedUser { get; set; }
        public KanbanFlag State { get; set; }
        public Project Project { get; set; }
        public Priority Priority { get; set; }

    }
}
