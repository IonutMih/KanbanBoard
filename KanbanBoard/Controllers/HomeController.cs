﻿using KanbanBoard.Data;
using KanbanBoard.Models;
using KanbanBoard.Models.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KanbanBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            AppDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            //TODO: LA UI CAUTA DIV CU CLASS ALERT https://www.youtube.com/watch?v=vRqz_zUiJTw 54:05
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Policy = "AdminAccess")]
        public IActionResult Secret()
        {
            List<Priority> priorities = _context.Priorities.Where(p => p.ID > 0).ToList();
            return View(priorities);
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = "",
            };

            var result = await _userManager.CreateAsync(user,password);

            if(result.Succeeded)
            {
                var userClaim = new Claim("Role", "User");
                _userManager.AddClaimAsync(user, userClaim).GetAwaiter().GetResult();

                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}