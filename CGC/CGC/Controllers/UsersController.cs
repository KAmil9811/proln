﻿using CGC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CGC.Funkcje.UserFuncFolder;
using CGC.Helpers;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    public sealed class UsersController : Controller
    {
        private static UsersController m_oInstance = null;
        private static readonly object m_oPadLock = new object();
        UserFunc userFunc = new UserFunc();

        public static UsersController Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new UsersController();
                    }
                    return m_oInstance;
                }
            }
        }

        [HttpPost("Return_All_Users")]
        public List<User> Return_All_Users([FromBody] Receiver receiver)
        {       
            return userFunc.Return_All_Users(receiver);
        }

        [HttpPost("Return_All_SuperAdmin")]
        public async Task<List<User>> Return_All_SuperAdmin([FromBody] Receiver receiver)
        {
            return userFunc.Return_All_SuperAdmin(receiver);
        }

        [HttpPost("Return_All_Admin")]
        public async Task<List<User>> Return_All_Admin([FromBody] Receiver receiver)
        {
            return userFunc.Return_All_Admin(receiver);
        }

        [HttpPost("Return_Users_History")]
        public async Task<List<UserHistory>> Return_Users_History([FromBody] Receiver receiver)
        {
            return userFunc.Return_Users_History(receiver);
        }

        [HttpPost("Return_User_History")]
        public async Task<List<UserHistory>> Return_User_History([FromBody] Receiver receiver)
        {
            return userFunc.Return_User_History(receiver);
        }

        //Admin
        [Authorize]
        [HttpPost("Add_User_Admin")]
        public async Task<List<User>> Add_User_Admin([FromBody] Receiver receiver)
        {
            return userFunc.Add_User_Admin(receiver);
        }

        [Authorize]
        [HttpPost("Edit_User_Admin")]
        public async Task<List<User>> Edit_User_Admin([FromBody] Receiver receiver)
        {
            return userFunc.Edit_User_Admin(receiver);
        }

        [Authorize]
        [HttpPost("Remove_User_Admin")]
        public async Task<List<User>> Remove_User_Admin([FromBody] Receiver receiver)
        {
            return userFunc.Remove_User_Admin(receiver);
        }

        [Authorize]
        [HttpPost("Restore_User_Admin")]
        public async Task<List<User>> Restore_User_Admin([FromBody] Receiver receiver)
        {
            return userFunc.Restore_User_Admin(receiver);
        }

        //User
        [HttpPost("Log_in")]
        public async Task<List<User>> Log_in([FromBody] Receiver receiver)
        {
            List<User> temp = userFunc.Log_in(receiver);
            return temp;
        }

        [Authorize]
        [HttpPost("Change_Email")]
        public async Task<List<User>> Change_Email([FromBody] Receiver receiver)
        {
            return userFunc.Change_Email(receiver);
        }

        [Authorize]
        [HttpPost("Change_Password")]
        public async Task<List<User>> Change_Password([FromBody] Receiver receiver)
        {
            return userFunc.Change_Password(receiver);
        }

        [HttpPost("Reset_Password_Code")]
        public async Task<List<User>> Reset_Password_Code([FromBody] Receiver receiver)
        {
            return userFunc.Reset_Password_Code(receiver);
        }
        
        [HttpPost("Reset_Password_Pass")]
        public async Task<List<User>> Reset_Password_Pass([FromBody] Receiver receiver)
        {
            return userFunc.Reset_Password_Pass(receiver);
        }

    }
}
