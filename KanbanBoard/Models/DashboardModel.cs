using KanbanBoard.Data;
using KanbanBoard.Models.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class DashboardModel
    {
        public AppDbContext _context { get; set; }
        public UserManager<IdentityUser> _userManager { get; set; }

        public List<ProjectFilterModel> allProjects { get; set; }
        public List<PriorityFilterModel> allPriorities { get; set; }

        public List<Issue> issues { get; set; }
        public List<string> userNames { get; set; }
        public List<Project> projects { get; set; }
        public List<Priority> priorities { get; set; }



        [BindProperty(SupportsGet = true)]
        public Filter filter { get; set; }
        public DashboardModel()
        {
            allProjects = new List<ProjectFilterModel>();

            allPriorities = new List<PriorityFilterModel>();

            userNames = new List<string>();

            filter = new Filter();

            issues = new List<Issue>();

            projects = new List<Project>();
        }

        public void ApplyFilters()
        {
            this.issues = _context.Issues.Include(u => u.AssignedUser)
                           .Include(p => p.Project)
                           .Include(p => p.Priority)
                           .Include(s => s.State)
                           .ToList();
            this.projects = _context.Projects.Include(u => u.Tasks)
                                .Include(p => p.Priority)
                                .ToList();

            List<string> projectNameList = _context.Projects.Select(p => p.ProjectName).ToList();

            foreach (string pr in projectNameList)
            {
                this.allProjects.Add(new ProjectFilterModel(pr));
            }

            this.userNames = _userManager.Users.Select(u => u.UserName).ToList();
            this.userNames.Sort();

            ApplyFilterProject();
            ApplyFilterPriority();
            ApplyFilterAssignedUser();
        }

        public void ApplyFilterProject()
        {
            if (this.filter.projectFilter.Count != 0)
            {
                this.issues = this.issues.Where(i => filter.projectFilter
                                                .Contains(i.Project.ProjectName))
                                                .ToList();

                this.projects = this.projects.Where(p => this.filter.projectFilter
                                                 .Contains(p.ProjectName))
                                                .ToList();

                List<string> projectNameList = _context.Projects.Select(p => p.ProjectName).ToList();

                foreach (string pr in projectNameList)
                {
                    this.allProjects.Add(new ProjectFilterModel(pr));
                    if (this.projects.Any(p => p.ProjectName == pr))
                    {
                        this.allProjects.Last().isChecked = true;
                    }
                }
            }
        }

        public void ApplyFilterPriority()
        {
            if (this.filter.priorityFilter.Count != 0)
            {
                foreach (string project in this.filter.projectFilter)
                {
                    this.projects.FirstOrDefault(p => p.ProjectName == project).Tasks = this.projects.FirstOrDefault(p => p.ProjectName == project)
                                                                                            .Tasks.Where(i => this.filter.priorityFilter
                                                                                            .Contains(i.Priority.Name))
                                                                                            .ToList();
                }

            }

            this.priorities = _context.Priorities.Where(p => this.filter.priorityFilter.Contains(p.Name)).ToList();

            List<string> priorityNameList = _context.Priorities.Select(p => p.Name).ToList();

            foreach (string pr in priorityNameList)
            {
                this.allPriorities.Add(new PriorityFilterModel(pr));
                if (this.priorities.Any(p => p.Name == pr))
                {
                    this.allPriorities.Last().isChecked = true;
                }
            }
        }

        public void ApplyFilterAssignedUser()
        {
            if (this.filter.selectedUser != null && this.filter.selectedUser != "")
            {
                foreach (var project in this.projects)
                {
                    project.Tasks = project.Tasks.Where(i => i.AssignedUser.UserName == this.filter.selectedUser).ToList();
                }
                this.issues = this.issues.Where(i => i.AssignedUser.UserName == this.filter.selectedUser).ToList();
            }
        }
    }
}
