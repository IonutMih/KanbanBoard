using KanbanBoard.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class DashboardModel
    {
        public List<Issue> issues { get; set; }
        public List<Project> projects { get; set; }
        public DashboardModel()
        {
            issues = new List<Issue>();

            projects = new List<Project>();
        }
    }
}
