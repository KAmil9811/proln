using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CGC.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public bool Admin { get; set; }
        public bool Super_Admin { get; set; }
        public bool Manager { get; set; }

        public bool Magazine_management { get; set; }
        public bool Machine_management { get; set; }
        public bool Order_management { get; set; }
        public bool Cut_management { get; set; }

        public string Error_Messege { get; set; }
        public string Reset_pass { get; set; }

        public bool Deleted { get; set; }
        public string Token { get; set; }
        public string Logged { get; set; }
        public bool Session_Start { get; set; }
        public bool Session_End { get; set; }
    }
}
