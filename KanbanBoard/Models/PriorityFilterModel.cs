using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class PriorityFilterModel
    {
        public string priorityName { get; set; }
        public bool isChecked { get; set; }

        public PriorityFilterModel(string priorityName)
        {
            this.priorityName = priorityName;
            isChecked = false;
        }
    }
}
