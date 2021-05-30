using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models.DataBase
{
    public class Project
    {
        public int ID { get; set; }

        [MaxLength(100)]
        [Required]
        public string ProjectName { get; set; }

        public List<Issue> Tasks { get; set; }
        public Priority Priority { get; set; }

        public Project()
        {
            Tasks = new List<Issue>();
        }
        public Project(string ProjectName)
        {
            this.Tasks = new List<Issue>();
            this.ProjectName = ProjectName;
        }

    }
}
