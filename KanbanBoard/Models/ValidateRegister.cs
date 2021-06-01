using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class ValidateRegister
    {
        private readonly UserManager<IdentityUser> _userManager;

        public List<string> errors;
        public ValidateRegister()
        {
            errors = new List<string>();
        }
        public ValidateRegister(UserManager<IdentityUser> userManager)
        {
            errors = new List<string>();
            _userManager = userManager;
        }
        public void GetListOfError(string UserName,string Password,string ConfirmationPassword,string Email)
        {
            bool atLeastOneIsEmpty = false;
            if (UserName == null)
            {
                this.errors.Add("Username cannot be null");
                atLeastOneIsEmpty = true;
            }

            if (Password == null)
            {
                this.errors.Add("Password cannot be null");
                atLeastOneIsEmpty = true;
            }

            if (ConfirmationPassword == null)
            {
                this.errors.Add("Password confirmation cannot be null");
                atLeastOneIsEmpty = true;
            }

            if (Email == null)
            {
                this.errors.Add("Email cannot be null");
                atLeastOneIsEmpty = true;
            }

            if(atLeastOneIsEmpty==false)
            {
                var userExists = _userManager.Users.Any(u => u.UserName == UserName);
                if(userExists)
                {
                    if (UserName == "admin")
                    {
                        this.errors.Add("You cannot use this username");
                    }
                    else
                    {
                        this.errors.Add("Username already exists");
                    }
                }
                else
                {
                    if(Password!=ConfirmationPassword)
                    {
                        this.errors.Add("The password and confirmation password doesn't match");
                    }
                    if(!Email.Contains("@"))
                    {
                        this.errors.Add("Email doesn't have a valid format");
                    }
                    if(Password.Length<4)
                    {
                        this.errors.Add("Password must contain at least 4 characters");
                    }
                }
            }
        }
    }
}
