using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models.DataBase
{
    public class UserSkill
    {
        public int ID { get; set; }
        public IdentityUser user { get; set; }
        public Skill skill { get; set; }

    }
}
