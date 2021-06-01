using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class ValidateLogin
    {
        private readonly UserManager<IdentityUser> _userManager;

        public List<string> errors;

        public ValidateLogin()
        {
            errors = new List<string>();
        }
        public ValidateLogin(UserManager<IdentityUser> userManager)
        {
            errors = new List<string>();
            _userManager = userManager;
        }

        public void GetListOfError(string UserName, string Password)
        {
            List<string> errors = new List<string>();

            bool userIsNull = false;
            bool passwordIsNull = false;
            if (UserName == null)
            {
                this.errors.Add("Username cannot be null");
                userIsNull = true;
            }

            if (Password == null)
            {
                this.errors.Add("Password cannot be null");
                passwordIsNull = true;
            }

            if (userIsNull == false&&passwordIsNull==false)
            {
                var userExists = _userManager.Users.FirstOrDefault(u => u.UserName == UserName);
                if (userExists == null)
                {
                    this.errors.Add("Username doesn't exist");
                }
                else
                {
                    var checkPassword = _userManager.PasswordHasher.VerifyHashedPassword(userExists, userExists.PasswordHash, Password);
                    if (checkPassword != PasswordVerificationResult.Success)
                    {
                        this.errors.Add("Password is incorrect");
                    }
                }
            }
        }
    }
}
