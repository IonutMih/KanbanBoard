﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KanbanBoard.Data;
using KanbanBoard.Models.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Controllers
{
    [Authorize(Policy = "ManagerAccess")]
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var projects = _context.Projects.ToList();
            return View(projects);
            
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string ProjectName)
        {
            Project project = new Project();
            project.ProjectName = ProjectName;

            var checkForDuplicates = _context.Projects.Any(p => p.ProjectName == ProjectName);

            if(checkForDuplicates==false)
            {
                await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();
            }
            else
            {
                //TODO:ERROR MESSAGE
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string ProjectName)
        {
            var Project = _context.Projects.FirstOrDefault(p => p.ProjectName == ProjectName);

            if (Project != null)
            {
                var issueList = await _context.Issues.Where(i => i.Project == Project).ToListAsync();

                if (issueList.Count == 0)
                {
                    _context.Projects.Remove(Project);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    //TODO:Implement case where the project has open issues
                }
            }

            return RedirectToAction("Index");
        }
    }
}