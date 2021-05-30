using KanbanBoard.Data;
using KanbanBoard.Models;
using KanbanBoard.Models.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public IssueController(AppDbContext context,
                                UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(int ?id)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "ManagerAccess")]
        public IActionResult Create()
        {
            CreateIssueModel model = new CreateIssueModel();
            model.Priorities = _context.Priorities.ToList();
            model.Projects = _context.Projects.ToList();
            model.Users = _userManager.Users.OrderBy(u=>u.UserName).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var issue = await _context.Issues.Include(u => u.AssignedUser)
                                        .Include(p => p.Project)
                                        .Include(p => p.Priority)
                                        .Include(s => s.State)
                                        .FirstOrDefaultAsync(i => i.ID == id);
            if (issue != null)
            {
                return View(issue);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Policy = "ManagerAccess")]
        public async Task<IActionResult> Create(Issue issue,string Project,string AssignedUser,string Priority)
        {
            if(Project==null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                issue.ID = _context.Issues.Max(e => e.ID) + 1;
            }
            catch(Exception exp)
            {
                issue.ID = 1;
            }
            issue.AssignedUser = _userManager.Users.FirstOrDefault(u => u.UserName == AssignedUser);
            issue.Project = _context.Projects.FirstOrDefault(p => p.ProjectName == Project);
            issue.Priority = _context.Priorities.FirstOrDefault(p => p.Name == Priority);
            issue.StartDate = DateTime.Now;
            issue.State = _context.KanbanFlag.FirstOrDefault(k => k.Name == "Backlog");

            await _context.Issues.AddAsync(issue);
            _context.Database.OpenConnection();
            try
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Issues ON;");
                await _context.SaveChangesAsync();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Issues OFF;");
            }
            finally
            {
                _context.Database.CloseConnection();
            }
            return RedirectToAction("Create");
        }

        [HttpPost]
        [Authorize(Policy = "ManagerAccess")]
        public async Task<IActionResult> Edit(int? id, Issue issue,
                                string Project, string AssignedUser,
                                string Priority,string State)
        {
            var currentIssue = _context.Issues.FirstOrDefault(i => i.ID == id);

            if(currentIssue!=null)
            {
                currentIssue.Summary = issue.Summary;
                currentIssue.CloseDate = issue.CloseDate;
                currentIssue.EstimatedEffort = issue.EstimatedEffort;
                currentIssue.RequestedCloseDate = issue.RequestedCloseDate;
                currentIssue.Project = _context.Projects.FirstOrDefault(p => p.ProjectName == Project);
                currentIssue.AssignedUser = _userManager.Users.FirstOrDefault(u => u.UserName == AssignedUser);
                currentIssue.Priority = _context.Priorities.FirstOrDefault(p => p.Name == Priority);
                currentIssue.State = _context.KanbanFlag.FirstOrDefault(s => s.Name == State);

                await _context.SaveChangesAsync();
            }

            return View("Index");
        }

        [HttpGet]
        [Authorize(Policy = "ManagerAccess")]
        public IActionResult Edit(int ?id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var issue = _context.Issues.Include(u => u.AssignedUser)
                                        .Include(p => p.Project)
                                        .Include(p => p.Priority)
                                        .Include(s => s.State)
                                        .FirstOrDefault(i => i.ID == id);

            if (issue != null)
            {
                EditIssueModel model = new EditIssueModel();
                model.Priorities = _context.Priorities.ToList();
                model.Projects = _context.Projects.ToList();
                model.States = _context.KanbanFlag.ToList();
                model.Users = _userManager.Users.OrderBy(u => u.UserName).ToList();
                model.currentIssue = issue;

                model.Priorities.Remove(model.currentIssue.Priority);
                model.Projects.Remove(model.currentIssue.Project);
                model.Users.Remove(model.currentIssue.AssignedUser);
                model.States.Remove(model.currentIssue.State);
                model.currentIssue = issue;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
