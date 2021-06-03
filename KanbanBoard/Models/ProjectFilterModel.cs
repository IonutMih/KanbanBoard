using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class ProjectFilterModel
    {
        public string projectName { get; set; }
        public bool isChecked { get; set; }

        public ProjectFilterModel(string projectName)
        {
            this.projectName = projectName;
            isChecked = false;
        }
    }
}
