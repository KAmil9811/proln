using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string Token { get; set; }
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

        public bool Deleted = false;


        public AuthenticateResponse(User user, string token)
        {
            Id = user.Login;
            Password = user.Password;
            Email = user.Email;
            Name = user.Name;
            Surname = user.Surname;
            Admin = user.Admin;
            Super_Admin = user.Super_Admin;
            Manager = user.Manager;
            Magazine_management = user.Magazine_management;
            Machine_management = user.Machine_management;
            Order_management = user.Order_management;
            Cut_management = user.Cut_management;
            Error_Messege = user.Error_Messege;
            Reset_pass = user.Reset_pass;
            Deleted = user.Deleted;
            Token = token;
        }
    }
}
