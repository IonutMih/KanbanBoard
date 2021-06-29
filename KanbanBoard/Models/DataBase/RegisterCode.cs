using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoard.Models.DataBase
{
    public class RegisterCode
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public bool isUsed { get; set; }
        public RegisterCode(string email)
        {
            this.Email = email;
            CreationDate = DateTime.Now;
            isUsed = false;
        }

    }
}
