using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class ValidationModel
    {
        public List<string> errors { get; set; }
        public string success { get; set; }
        public ValidationModel()
        {
            errors = new List<string>();
        }

        public void ChangePasswordValidate(string old, string newPass, string newPassConfirmation)
        {
            if (old == null || newPass == null || newPassConfirmation == null || old == "" || newPass == "" || newPassConfirmation == "")
            {
                errors.Add("All fields are required");
            }
            else
            {
                if (newPass == old)
                {
                    errors.Add("The new password cannot be the same as the last one");
                }
                if (newPass != newPassConfirmation)
                {
                    errors.Add("The new password and confirmation doesn't match");
                }
                if (newPass.Length < 4)
                {
                    errors.Add("The new password must have at least 4 characters");
                }
            }
        }

        public void ChangeEmailValidate(string old,string newEmail)
        {
            if(old==null||old==""||newEmail==null||newEmail=="")
            {
                errors.Add("All fields are required");
            }
            else
            {
                if(!newEmail.Contains("@"))
                {
                    errors.Add("New email is incorrect");
                }
                if (old==newEmail)
                {
                    errors.Add("The old email is the same as the new one");
                }
            }
        }
    }
}
