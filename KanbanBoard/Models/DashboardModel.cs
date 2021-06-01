using KanbanBoard.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class DashboardModel
    {
        public List<Issue> issuesInBacklog { get; set; }

        public DashboardModel()
        {
            issuesInBacklog = new List<Issue>();
        }
    }
}
