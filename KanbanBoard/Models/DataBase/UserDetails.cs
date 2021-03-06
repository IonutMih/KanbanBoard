using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KanbanBoard.Models.DataBase
{
    public class UserDetails
    {
        public int  ID { get; set; }
        public IdentityUser user { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Description { get; set; }

        public UserDetails()
        {
            Name = "Not set";
            Address = "Not set";
            Description = "Not set";
        }
    }
}
