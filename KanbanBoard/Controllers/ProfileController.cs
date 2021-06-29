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
using System.Security.Claims;
using System.Threading.Tasks;

namespace KanbanBoard.Controllers
{
    [Authorize]
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
        public IActionResult Index(string test)
        {
            return RedirectToAction("MyProfile");
        }
        [HttpPost]
        public async Task<IActionResult> AddSkill(string SkillAdded)
        {
            var user = _userManager.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            var skill = _context.Skills.FirstOrDefault(s => s.SkillName == SkillAdded);

            if(user!=null&&skill!=null)
            {
                var userSkill = new UserSkill
                {
                    user = user,
                    skill = skill
                };

                _context.UserSkills.Add(userSkill);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("MyProfile");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveSkill(string SkillName)
        {
            var user = _userManager.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            var skill = _context.Skills.FirstOrDefault(s => s.SkillName == SkillName);

            if (user != null && skill != null)
            {
                var userSkill = _context.UserSkills.FirstOrDefault(us => us.skill == skill && us.user == user);

                _context.UserSkills.Remove(userSkill);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("MyProfile");
        }

        [Route("Profile")]
        public async Task<IActionResult> MyProfile()
        {
            ProfileModel model = new ProfileModel();

            var userName = User.Identity.Name;
            model.user = _userManager.Users.FirstOrDefault(u => u.UserName == userName);

            model.skills = await _context.UserSkills
                .Include(u => u.user)
                .Include(s => s.skill)
                .Where(s => s.user==model.user).ToListAsync();

            model.allSkils = await _context.Skills.ToListAsync();
            model.allSkils = model.allSkils.Where(s => !model.skills.Any(skill => skill.skill == s)).ToList();

            if (_context.UserDetalis.Any(ud=>ud.user==model.user))
            {
                var userDetails = _context.UserDetalis.Include(u => u.user).Where(ud => ud.user==model.user).First();
                model.details = userDetails;
            }
            else
            {
                model.details = new UserDetails();
            }

            return View(model);
        }

        [Route("Profile/Edit")]
        public async Task<IActionResult> Edit()
        {
            ProfileModel model = new ProfileModel();

            var userName = User.Identity.Name;
            model.user = _userManager.Users.FirstOrDefault(u => u.UserName == userName);

            model.skills = await _context.UserSkills
                .Include(u => u.user)
                .Include(s => s.skill)
                .Where(s => s.user == model.user).ToListAsync();

            model.allSkils = await _context.Skills.ToListAsync();
            model.allSkils = model.allSkils.Where(s => !model.skills.Any(skill => skill.skill == s)).ToList();

            if (_context.UserDetalis.Any(ud => ud.user == model.user))
            {
                var userDetails = _context.UserDetalis.Include(u => u.user).Where(ud => ud.user == model.user).First();
                model.details = userDetails;
            }
            else
            {
                model.details = new UserDetails();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "ManagerAccess")]
        public async Task<IActionResult> DeleteSkill(string SkillName)
        {
            var skill = _context.Skills.FirstOrDefault(s => s.SkillName == SkillName);
            if(skill!=null)
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
                TempData["DeletedWithSuccess"] = "The skill was deleted succesfully";
            }
            return RedirectToAction("ManageSkills");
        }

        [HttpPost]
        [Authorize(Policy = "ManagerAccess")]
        public async Task<IActionResult> AddNewSkill(string SkillName)
        {
            if (SkillName == null || SkillName == "" || _context.Skills.Any(s => s.SkillName == SkillName))
            {
                TempData["ErrorAddSkill"] = "The name is not valid or the skill already exists.";
            }
            else
            {
                Skill newSkill = new Skill()
                {
                    SkillName = SkillName
                };

                _context.Skills.Add(newSkill);
                await _context.SaveChangesAsync();
                TempData["AddedWithSuccess"] = "The new skill was added succesfully";
            }
            return RedirectToAction("ManageSkills");
        }
        [HttpGet]
        [Authorize(Policy = "ManagerAccess")]
        public IActionResult ManageSkills()
        {
            List<Skill> skills = new List<Skill>();
            skills = _context.Skills.ToList();
            return View(skills);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(string Name,string PhoneNumber,string Address,string Description)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            user.PhoneNumber = PhoneNumber;
            UserDetails details = new UserDetails()
            {
                user = user,
                Name = Name,
                Description = Description,
                Address = Address
            };

            if(_context.UserDetalis.Any(ud=>ud.user==user))
            {
                var userDetails = _context.UserDetalis.FirstOrDefault(ud => ud.user == user);
                userDetails.Name = Name;
                userDetails.Description = Description;
                userDetails.Address = Address;
            }
            else
            {
                _context.UserDetalis.Add(details);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("MyProfile");
        }


        public async Task<IActionResult> ChangePassword()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string OldPassword, string NewPassword, string ConfirmNewPassword)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            
            ValidationModel model = new ValidationModel();
            if(await _userManager.CheckPasswordAsync(user, OldPassword))
            {
                model.ChangePasswordValidate(OldPassword, NewPassword, ConfirmNewPassword);
                if(model.errors.Count==0)
                {
                    await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
                    model.success = "The password was changed succesfully";
                }
            }
            else
            {
                model.errors.Add("Password is incorrect");
            }
            return View(model);
        }
        public IActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail(string OldEmail,string NewEmail)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            ValidationModel model = new ValidationModel();
            if (user.Email==OldEmail)
            {
                model.ChangeEmailValidate(OldEmail,NewEmail);
                if (model.errors.Count == 0)
                {
                    await _userManager.SetEmailAsync(user, NewEmail);
                    model.success = "The email was changed succesfully";
                }
            }
            else
            {
                model.errors.Add("Email is incorrect");
            }
            return View(model);
        }

        [HttpGet]
        [Route("Profile/Details/{username?}")]
        public async Task<IActionResult> Details(string username)
        {

            ProfileModel model = new ProfileModel();

            var user = _userManager.Users.FirstOrDefault(u => u.UserName == username);
            var roles = await _userManager.GetClaimsAsync(user);
            model.userRole = roles.First().Value;

            model.user = user;
            model.skills = await _context.UserSkills
                .Include(u => u.user)
                .Include(s => s.skill)
                .Where(s => s.user == model.user).ToListAsync();

            if (_context.UserDetalis.Any(ud => ud.user == model.user))
            {
                var userDetails = _context.UserDetalis.Include(u => u.user).Where(ud => ud.user == model.user).First();
                model.details = userDetails;
            }
            else
            {
                model.details = new UserDetails();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "AdminAccess")]
        public async Task<IActionResult> ChangeRole(string username, string RoleName,string claim)
        {
            var user = _userManager.Users.FirstOrDefault(U => U.UserName == username);

            var oldClaim = new Claim("Role", claim);
            var newClaim = new Claim("Role", RoleName);
            await _userManager.ReplaceClaimAsync(user, oldClaim, newClaim);

            return RedirectToAction("Details", new { username = username });
        }

    }
}
