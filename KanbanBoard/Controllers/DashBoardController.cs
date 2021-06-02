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
        public void Actualize()
        {

        }
        public IActionResult Index()
        {

            DashboardModel model = new DashboardModel();

            model.issues = _context.Issues.Include(u => u.AssignedUser)
                            .Include(p => p.Project)
                            .Include(p => p.Priority)
                            .Include(s => s.State).ToList();

            model.projects = _context.Projects.Include(u => u.Tasks)
                            .Include(p => p.Priority).ToList();

            return View(model);
        }
    }
}
