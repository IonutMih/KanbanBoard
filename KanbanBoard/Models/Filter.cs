using KanbanBoard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class Filter
    {
        public List<string> projectFilter { get; set; }

        public List<string> priorityFilter { get; set; }

        public string selectedUser { get; set; }
        public Filter()
        {
            projectFilter = new List<string>();
            priorityFilter = new List<string>();
            selectedUser = "";
        }

        
    }
}
