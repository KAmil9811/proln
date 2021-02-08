using CGC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CGC.Funkcje.UserFuncFolder;
using Microsoft.AspNetCore.Authorization;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
     
        [HttpGet("Return_All_Users")]
        public List<User> Return_All_Users()
        {       
            return userFunc.Return_All_Users();
        }

        [HttpGet("Return_All_SuperAdmin")]
        public async Task<List<User>> Return_All_SuperAdmin()
        {
            return userFunc.Return_All_SuperAdmin();
        }

        [HttpGet("Return_All_Admin")]
        public async Task<List<User>> Return_All_Admin()
        {
            return userFunc.Return_All_Admin();
        }
        
        [HttpGet("Return_Users_History")]
        public async Task<List<UserHistory>> Return_Users_History()
        {
            return userFunc.Return_Users_History();
        }

        [HttpPost("Return_User_History")]
        public async Task<List<UserHistory>> Return_User_History([FromBody] Receiver receiver)
        {
            return userFunc.Return_User_History(receiver);
        }

        //Admin
        [HttpPost("Add_User_Admin")]
        public async Task<List<User>> Add_User_Admin([FromBody] Receiver receiver)
        {
            return userFunc.Add_User_Admin(receiver);
        }

        [HttpPost("Edit_User_Admin")]
        public async Task<List<User>> Edit_User_Admin([FromBody] Receiver receiver)
        {
            return userFunc.Edit_User_Admin(receiver);
        }

        [HttpPost("Remove_User_Admin")]
        public async Task<List<User>> Remove_User_Admin([FromBody] Receiver receiver)
        {
            return userFunc.Remove_User_Admin(receiver);
        }

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

        [HttpPost("Change_Email")]
        public async Task<List<User>> Change_Email([FromBody] Receiver receiver)
        {
            return userFunc.Change_Email(receiver);
        }

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
