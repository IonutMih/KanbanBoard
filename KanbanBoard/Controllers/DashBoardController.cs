using KanbanBoard.Data;
using KanbanBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
        private readonly AppDbContext _context;

        public DashBoardController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        public async Task Actualize(string item, string to)
        {
            int ID = Int32.Parse(item.Split('-').Last());
            string ProjectNam = item.Split('-')[0];
            string movedTo = to.Split('-').Last();

            var newState = _context.KanbanFlag.FirstOrDefault(k => k.Name == movedTo);

            if (newState != null)
            {
                var issue = _context.Issues.FirstOrDefault(i => i.ID == ID);
                issue.State = newState;
                await _context.SaveChangesAsync();
            }
        }
        public IActionResult Index()
        {

            DashboardModel model = new DashboardModel();

            List<string> projectNameList = _context.Projects.Select(p => p.ProjectName).ToList();

            foreach (string pr in projectNameList)
            {
                model.allProjects.Add(new ProjectFilterModel(pr));
            }
            foreach (string pr in _context.Priorities.Select(p=>p.Name).ToList())
            {
                model.allPriorities.Add(new PriorityFilterModel(pr));
            }

            model.issues = _context.Issues.Include(u => u.AssignedUser)
                            .Include(p => p.Project)
                            .Include(p => p.Priority)
                            .Include(s => s.State).ToList();

            model.projects = _context.Projects.Include(u => u.Tasks)
                            .Include(p => p.Priority).ToList();


            return View(model);
        }
        [HttpPost]
        public IActionResult Index(DashboardModel model)
        {
            if(model.filter.priorityFilter.Count==0&&model.filter.projectFilter.Count==0)
            {
                return RedirectToAction("Index");
            }
            //TODO: vezi cand selectezi ceva si dupa deselectezi si dai filtrare
            model.ApplyFilter(_context);

            return View(model);
        }

        public async Task<IActionResult> MoveIssueToBoard(int id)
        {
            var issue = _context.Issues.FirstOrDefault(i => i.ID == id);
            issue.State = _context.KanbanFlag.FirstOrDefault(s => s.Name == "Open");
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
