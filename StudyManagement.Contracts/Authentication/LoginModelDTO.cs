using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManagement.Contracts.Authentication
{
    public class LoginModelDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
