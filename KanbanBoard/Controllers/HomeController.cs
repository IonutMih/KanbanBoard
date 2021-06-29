using KanbanBoard.Data;
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

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "ManagerAccess")]
        public IActionResult RegisterLink()
        {
            return View();
        }

        [Authorize(Policy = "ManagerAccess")]
        [HttpPost]
        public async Task<IActionResult> AddNewUser(string email)
        {
            if (email != null && email != "" && !_userManager.Users.Any(u=>u.Email==email))
            {
                RegisterCode model = new RegisterCode(email);
                _context.RegisterEmails.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The user can register using this email";
            }
            else
            {
                TempData["Error"] = "An account with the same email is already existing";
            }
            return View("RegisterLink");
        }


        public IActionResult Login()
        {
            return View(new ValidateLogin());
        }

        [HttpPost]
        public async Task<IActionResult> Login(string UserName, string Password)
        {
            ValidateLogin validateLogin = new ValidateLogin(_userManager);
            validateLogin.GetListOfError(UserName, Password);

            if (validateLogin.errors.Count == 0)
            {
                var user = await _userManager.FindByNameAsync(UserName);
                var signInResult = await _signInManager.PasswordSignInAsync(user, Password, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                 return View(validateLogin);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new ValidateRegister());
        }


        [HttpPost]
        public async Task<IActionResult> Register(string UserName, string Password,
                                                  string ConfirmPassword,string Email)
        {
            if (_context.RegisterEmails.Any(r => r.Email == Email))
            {

                ValidateRegister validateRegister = new ValidateRegister(_userManager);
                validateRegister.GetListOfError(UserName, Password, ConfirmPassword, Email);


                if (validateRegister.errors.Count == 0)
                {
                    var user = new IdentityUser
                    {
                        UserName = UserName,
                        Email = Email,
                    };

                    var result = await _userManager.CreateAsync(user, Password);

                    if (result.Succeeded)
                    {
                        var userClaim = new Claim("Role", "User");
                        _userManager.AddClaimAsync(user, userClaim).GetAwaiter().GetResult();

                        var signInResult = await _signInManager.PasswordSignInAsync(user, Password, false, false);
                        if (signInResult.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    return View(validateRegister);
                }
            }
            else
            {
                TempData["ErrorWhileRegister"] = "This email is not autorize to be used";
                return View();
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

    }
}
