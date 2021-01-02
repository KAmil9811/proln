using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGC.Models
{
    public class User
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public bool Admin = false;
        public bool Super_Admin = false;
        public bool Manager = false;

        public bool Magazine_management = false;
        public bool Machine_management = false;
        public bool Order_management = false;
        public bool Cut_management = false;

        public string Error_Messege { get; set; }
        public string Reset_pass { get; set; }

        public bool Deleted = false;


    }
}