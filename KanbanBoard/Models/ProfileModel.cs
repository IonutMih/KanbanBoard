using KanbanBoard.Models.DataBase;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models
{
    public class ProfileModel
    {
        public IdentityUser user { get; set; }
        public List<UserSkill> skills { get; set; }
        public List<Skill> allSkils { get; set; }
        public UserDetails details { get; set; }
        public ProfileModel()
        {
            skills = new List<UserSkill>();
            allSkils = new List<Skill>();
            details = new UserDetails();
        }
    }
}
