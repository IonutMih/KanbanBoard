using KanbanBoard.Models.DataBase;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class EditIssueModel
    {
        public Issue currentIssue { get; set; }

        public List<IdentityUser> Users { get; set; }
        public List<Priority> Priorities { get; set; }
        public List<Project> Projects { get; set; }

        public List<KanbanFlag> States { get; set; }
        public EditIssueModel()
        {
            Users = new List<IdentityUser>();
            Priorities = new List<Priority>();
            Projects = new List<Project>();
            States = new List<KanbanFlag>();
        }

    }
}
