using KanbanBoard.Models.DataBase;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class CreateIssueModel
    {
        public List<IdentityUser> Users { get; set; }
        public List<Priority> Priorities { get; set; }
        public List<Project> Projects { get; set; }

        public CreateIssueModel()
        {
            Users = new List<IdentityUser>();
            Priorities = new List<Priority>();
            Projects = new List<Project>();
        }

    }
}
