using KanbanBoard.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(AppDbContext context,
                                    UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return RedirectToAction("MyProfile");
        }

        [Route("Profile")]
        public async Task<IActionResult> MyProfile()
        {
            return View();
        }

        [Route("Profile/MyProfile/ChangePassword")]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
    }
}
