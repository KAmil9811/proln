using CGC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    public sealed class UsersController : Controller
    {
        static MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        {
            Server = "projekt-inz.database.windows.net",
            Database = "projekt-inz",
            UserID = "Michal",
            Password = "lemES98naw141",
            //SslMode = MySqlSslMode.Required,
        };

       
        SqlConnection cnn = new SqlConnection(builder.ConnectionString);
        private static UsersController m_oInstance = null;
        private static readonly object m_oPadLock = new object();

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

        public void Change_Permision(User user)
        {
            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Admin=@Admin, Super_Admin=@Super_Admin, Magazine_management= @Magazine_management,Machine_management= @Machine_management, Order_management= @Order_management , Cut_management= @Cut_management WHERE Login = @Login", cnn);
            cnn.Open();

            command.Parameters.Add("@Admin", SqlDbType.Bit).Value = user.Admin;
            command.Parameters.Add("@Super_Admin", SqlDbType.Bit).Value = user.Super_Admin;
            command.Parameters.Add("@Manager", SqlDbType.Bit).Value = user.Manager;
            command.Parameters.Add("@Magazine_management", SqlDbType.Bit).Value = user.Magazine_management;
            command.Parameters.Add("@Machine_management", SqlDbType.Bit).Value = user.Machine_management;
            command.Parameters.Add("@Order_management", SqlDbType.Bit).Value = user.Order_management;
            command.Parameters.Add("@Cut_management", SqlDbType.Bit).Value = user.Cut_management;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;

            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();
        }

        public bool Is_Email_Exist(string email)
        {
            List<User> temp = new List<User>();
           
            SqlCommand command = new SqlCommand("SELECT Email FROM [User] ", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                User user = new User();
                user.Email = sqlDataReader.GetString(0);
                temp.Add(user);
            }

            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            foreach (User user in temp)
            {
                if (user.Email == email)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Is_Login_Exist(string login)
        {
            List<User> temp = new List<User>();
           
            SqlCommand command = new SqlCommand("SELECT Login FROM [User] ", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                User user = new User();
                user.Login = sqlDataReader.GetString(0);
                temp.Add(user);
            }

            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            foreach (User user in GetUsers())
            {
                if (user.Login == login)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Is_Login_Correct(string login)
        {
            foreach (var substring in login)
            {
                if (substring < 'A' || substring > 'z' || (substring > 'Z' && substring < 'a'))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Is_Password_Correct(string password)
        {
            bool check_big = false;
            bool check_small = false;
            bool check_number = false;

            if (password.Length < 8 && password.Length > 24)
            {
                return false;
            }

            foreach (var substring in password)
            {
                if (substring >= 'A' && substring <= 'Z')
                {
                    check_big = true;
                }
                if (substring >= 'a' && substring <= 'z')
                {
                    check_small = true;
                }
                if (substring >= '0' && substring <= '9')
                {
                    check_number = true;
                }
            }
            if (check_big == true && check_small == true && check_number == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool Is_Email_Correct(string email)
        {
            string pattent = @"\.*@{1}\.*";

            return Regex.IsMatch(email, pattent, RegexOptions.IgnoreCase);
        }

        //nie uzyta
        public bool Is_Password_Correct(string password, string login)
        {
            foreach (User user in GetUsers())
            {
                if (user.Password == password && user.Login == login)
                {
                    return true;
                }
            }
            return false;
        }

        public List<User> GetUsers()
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User];", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                User user = new User();
                user.Login = sqlDataReader["Login"].ToString();
                user.Password = sqlDataReader["Password"].ToString();
                user.Email = sqlDataReader["Email"].ToString();
                user.Name = sqlDataReader["Name"].ToString();
                user.Surname = sqlDataReader["Surname"].ToString();
                user.Admin = Convert.ToBoolean(sqlDataReader["Admin"]);
                user.Super_Admin = Convert.ToBoolean(sqlDataReader["Super_Admin"]);
                user.Manager = Convert.ToBoolean(sqlDataReader["Manager"]);
                user.Magazine_management = Convert.ToBoolean(sqlDataReader["Magazine_management"]);
                user.Machine_management = Convert.ToBoolean(sqlDataReader["Machine_management"]);
                user.Order_management = Convert.ToBoolean(sqlDataReader["Order_management"]);
                user.Cut_management = Convert.ToBoolean(sqlDataReader["Cut_management"]);
                user.Reset_pass = sqlDataReader["Reset_pass"].ToString();
                user.Deleted = Convert.ToBoolean(sqlDataReader["Deleted"]);
                temp.Add(user);
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();
            return temp;
        }

        public List<UserHistory> GetAllUserHistory()
        {
            List<UserHistory> userHistories = new List<UserHistory>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User_History];", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                UserHistory userHistory = new UserHistory();
                userHistory.Login = sqlDataReader["Login"].ToString();
                userHistory.Data = sqlDataReader["Data"].ToString();
                userHistory.Description = sqlDataReader["Description"].ToString();
                userHistories.Add(userHistory);
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            return userHistories;
        }

        public void Insert_User_History(string Description, string Login)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.User_History(Data, Description, Login) VALUES(@data, @Description, @Login)";
            SqlCommand command = new SqlCommand(query, cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();
        }

        [HttpGet("Return_All_Users")]
        public async Task<List<User>> Return_All_Users()
        {
            List<User> temp = new List<User>();

            foreach (User user in GetUsers())
            {
                if (user.Manager == false)
                {
                    temp.Add(user);
                }
            }
            return temp;
        }

        [HttpGet("Return_All_SuperAdmin")]
        public async Task<List<User>> Return_All_SuperAdmin()
        {
            List<User> temp = new List<User>();

            foreach (User user in GetUsers())
            {
                if (user.Manager == false && user.Super_Admin == false)
                {
                    temp.Add(user);
                }
            }
            return temp;
        }

        [HttpGet("Return_All_Admin")]
        public async Task<List<User>> Return_All_Admin()
        {
            List<User> temp = new List<User>();

            foreach (User user in GetUsers())
            {
                if (user.Manager == false && user.Super_Admin == false && user.Admin == false)
                {
                    temp.Add(user);
                }
            }
            return temp;
        }
        
        [HttpGet("Return_Users_History")]
        public async Task<List<UserHistory>> Return_Users_History()
        {
            return GetAllUserHistory();
        }

        //Admin
        [HttpPost("Add_User_Admin")]
        public async Task<List<User>> Add_User_Admin([FromBody] Receiver receiver)
        {
            User user = receiver.user;
            User admin = receiver.admin;
            string perm = receiver.perm;

            if(perm == "admin")
            {
                user.Admin = true;
            }
            else if (perm == "superAdmin")
            {
                user.Super_Admin = true;
            }

            List<User> temp = new List<User>();
            bool check;

            check = Is_Email_Correct(user.Email);
            if (check == false)
            {
                admin.Error_Messege = "Wrong_email";
                temp.Add(admin);
                return temp;
            }

            check = Is_Email_Exist(user.Email);
            if (check == true)
            {
                admin.Error_Messege = "Email_taken";
                temp.Add(admin);
                return temp;
            }

            check = Is_Login_Correct(user.Login);
            if (check == false)
            {
                admin.Error_Messege = "Wrong_login";
                temp.Add(admin);
                return temp;
            }

            check = Is_Login_Exist(user.Login);
            if (check == true)
            {
                admin.Error_Messege = "Login_taken";
                temp.Add(admin);
                return temp;
            }

            check = Is_Password_Correct(user.Password);
            if (check == false)
            {
                admin.Error_Messege = "Wrong_passowrd";
                temp.Add(admin);
                return temp;
            }

            foreach (User use in GetUsers())
            {
                if (use.Login == admin.Login)
                {

                    string query = "INSERT INTO dbo.[User](Login,Password,Email,Name,Surname,Admin,Super_Admin,Manager,Magazine_management,Machine_management,Order_management,Cut_management,Reset_pass,Deleted) VALUES(@Login, @Password, @Email, @Name, @Surname, @Admin, @Super_Admin, @Manager, @Magazine_management, @Machine_management, @Order_management, @Cut_management, @Reset_pass, @Deleted)";
                    SqlCommand command = new SqlCommand(query, cnn);

                    command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;
                    command.Parameters.Add("@Password", SqlDbType.VarChar, 40).Value = user.Password;
                    command.Parameters.Add("@Email", SqlDbType.VarChar, 40).Value = user.Email;
                    command.Parameters.Add("@Name", SqlDbType.VarChar, 40).Value = user.Name;
                    command.Parameters.Add("@Surname", SqlDbType.VarChar, 40).Value = user.Surname;
                    command.Parameters.Add("@Admin", SqlDbType.Bit).Value = user.Admin;
                    command.Parameters.Add("@Super_Admin", SqlDbType.Bit).Value = user.Super_Admin;
                    command.Parameters.Add("@Manager", SqlDbType.Bit).Value = user.Manager;
                    command.Parameters.Add("@Magazine_management", SqlDbType.Bit).Value = user.Magazine_management;
                    command.Parameters.Add("@Machine_management", SqlDbType.Bit).Value = user.Machine_management;
                    command.Parameters.Add("@Order_management", SqlDbType.Bit).Value = user.Order_management;
                    command.Parameters.Add("@Cut_management", SqlDbType.Bit).Value = user.Cut_management;
                    command.Parameters.Add("@Reset_pass", SqlDbType.VarChar, 40).Value = "";
                    command.Parameters.Add("@Deleted", SqlDbType.Bit).Value = user.Deleted;

                    cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    cnn.Close();

                    string userhistory = admin.Login + " created this account";
                    string Adminhistory = admin.Login + "created " + user.Login + " account";

                    Insert_User_History(userhistory, user.Login);
                    Insert_User_History(Adminhistory, admin.Login);

                    temp.Add(admin);
                    return temp;
                }
            }
            admin.Error_Messege = "Admin_not_exist";
            temp.Add(admin);
            return temp;
        }

        [HttpPost("Edit_User_Admin")]
        public async Task<List<User>> Edit_User_Admin([FromBody] Receiver receiver)
        {
            List<User> temporary = new List<User>();
            User temp = new User();
            User temp2 = new User();
            User temp3 = new User();
            User temp4 = new User();
            User temp5 = new User();
            User user = new User();
            string check = receiver.perm;
            bool check2 = false;

            if(check != "")
            {
                if(check == "user")
                {
                    receiver.user.Admin = false;
                    receiver.user.Super_Admin = false;
                    check2 = true;
                }
                else if(check == "admin")
                {
                    receiver.user.Admin = true;
                    receiver.user.Super_Admin = false;
                    check2 = true;
                }
                else if (check == "superAdmin")
                {
                    receiver.user.Admin = false;
                    receiver.user.Super_Admin = true;
                    check2 = true;
                }
            }

            foreach (User use in GetUsers())
            {
                if (use.Login == receiver.user.Login)
                {

                    if (receiver.user.Email != use.Email)
                    {
                        temp = Edit_Email_Admin(receiver);
                    }

                    if (receiver.user.Password != use.Password)
                    {
                        temp2 = Change_Password_Admin(receiver);
                    }

                    if (receiver.user.Name != use.Name)
                    {
                        temp3 = Change_Name_Admin(receiver);
                    }

                    if (receiver.user.Surname != use.Surname)
                    {
                        temp4 = Change_Surname_Admin(receiver);
                    }


                    if (use.Cut_management != receiver.user.Cut_management || use.Order_management != receiver.user.Order_management || use.Magazine_management != receiver.user.Magazine_management || use.Machine_management != receiver.user.Machine_management || check2 == true)
                    {
                        temp5 = Set_Permissions_Admin(receiver);
                    }
                }
            }

            if (temp.Error_Messege != null )
            {
                temporary.Add(temp);
                return temporary;
            }
            if (temp2.Error_Messege != null)
            {
                temporary.Add(temp2);
                return temporary;
            }
            if (temp3.Error_Messege != null)
            {
                temporary.Add(temp3);
                return temporary;
            }
            if (temp4.Error_Messege != null)
            {
                temporary.Add(temp4);
                return temporary;
            }
            if (temp5.Error_Messege != null)
            {
                temporary.Add(temp5);
                return temporary;
            }

            user.Error_Messege = "";
            temporary.Add(user);
            return temporary;

        }

        public  User Edit_Email_Admin(Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            bool check;

            check = Is_Email_Correct(user.Email);
            if (check == false)
            {
                admin.Error_Messege = "Wrong_Email";
                return admin;
            }

            check = Is_Email_Exist(user.Email);
            if (check == true)
            {
                admin.Error_Messege = "Email_taken";
                return admin;
            }

            foreach (User use in GetUsers())
            {
                if (use.Login == admin.Login)
                {
                    foreach (User usere in GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            string query = "UPDATE dbo.[User] SET Email = @new_email WHERE Login = @Login";
                            SqlCommand command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;
                            command.Parameters.Add("@new_email", SqlDbType.VarChar, 40).Value = user.Email;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();

                            string userhistory = admin.Login + " changed e-mail for this account";
                            string Adminhistory = admin.Login + "changed " + user.Login + " e-mail";

                            Insert_User_History(userhistory, user.Login);
                            Insert_User_History(Adminhistory, admin.Login);

                            return admin;
                        }
                    }
                    admin.Error_Messege = "User_Login_not_found";
                    return admin;
                }
            }
            admin.Error_Messege = "Wrong_admin_login_or_password";
            return admin;
        }

        [HttpPost("Remove_User_Admin")]
        public async Task<List<User>> Remove_User_Admin([FromBody] Receiver receiver)
        {
            List<User> temp = new List<User>();

            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User adm in GetUsers())
            {
                if (adm.Login == admin.Login)
                {
                    foreach(User usere in GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            if (usere.Deleted == true)
                            {
                                user.Error_Messege = "User_already_deleted";
                                temp.Add(user);
                                cnn.Close();
                                return temp;
                            }

                            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Deleted = @Deleted WHERE Login = @Login", cnn);

                            command.Parameters.Add("@Deleted", SqlDbType.Bit).Value = true;
                            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();


                            string userhistory = admin.Login + " remove " + user.Login;
                            string Adminhistory = "You removed " + user.Login;

                            Insert_User_History(userhistory, user.Login);
                            Insert_User_History(Adminhistory, admin.Login);

                            temp.Add(user);
                            return temp;
                        }
                    }
                    user.Error_Messege = "User_dont_exist";
                    temp.Add(user);
                    return temp;
                }
            }
            user.Error_Messege = "Admin_dont_exist";
            temp.Add(user);
            return temp;

        }

        [HttpPost("Restore_User_Admin")]
        public async Task<List<User>> Restore_User_Admin([FromBody] Receiver receiver)
        {
            List<User> temp = new List<User>();

            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User adm in GetUsers())
            {
                if (adm.Login == admin.Login)
                {
                    foreach (User usere in GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            if (usere.Deleted == true)
                            {
                                SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Deleted = @Deleted WHERE Login = @Login", cnn);

                                command.Parameters.Add("@Deleted", SqlDbType.Bit).Value = false;
                                command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();


                                string userhistory = admin.Login + " restore " + user.Login;
                                string Adminhistory = "You restore " + user.Login;

                                Insert_User_History(userhistory, user.Login);
                                Insert_User_History(Adminhistory, admin.Login);

                                temp.Add(user);
                                return temp;
                            }
                            user.Error_Messege = "User_already_restored";
                            temp.Add(user);
                            return temp;
                        }
                    }
                    user.Error_Messege = "User_dont_exist";
                    temp.Add(user);
                    return temp;
                }
            }
            user.Error_Messege = "Admin_dont_exist";
            temp.Add(user);
            return temp;
        }

        public User Change_Password_Admin(Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            bool check;

            check = Is_Password_Correct(user.Password);
            if (check == false)
            {
                admin.Error_Messege = "Password_incorect";
                return admin;
            }

            foreach (User use in GetUsers())
            {
                if (use.Login == admin.Login)
                {
                    foreach (User usere in GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Password = @new_password WHERE Login = @Login", cnn);

                            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;
                            command.Parameters.Add("@new_password", SqlDbType.VarChar, 40).Value = user.Password;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();


                            string userhistory = admin.Login + " changed password for this account";
                            string Adminhistory = admin.Login + "changed " + user.Login + " password";

                            Insert_User_History(userhistory, user.Login);
                            Insert_User_History(Adminhistory, admin.Login);

                            cnn.Close();
                            return admin;
                        }
                    }
                    admin.Error_Messege = "User_Login_not_found";
                    return admin;
                }
            }
            admin.Error_Messege = "Wrong_admin_login_or_password";
            return admin;
        }

        public User Set_Permissions_Admin( Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User use in GetUsers())
            {
                if (use.Login == admin.Login)
                {
                    foreach (User usere in GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            Change_Permision(user);

                            
                            string userhistory = admin.Login + "change permission for this account";
                            string Adminhistory = admin.Login + "change" + user.Login + "permission";

                            Insert_User_History(userhistory, user.Login);
                            Insert_User_History(Adminhistory, admin.Login);

                            return admin;
                        }
                    }
                    admin.Error_Messege = "User_Login_not_found";
                    return admin;
                }
            }
            admin.Error_Messege = "Wrong_admin_login_or_password";
            return admin;
        }

        public User Change_Name_Admin( Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User use in GetUsers())
            {
                if (use.Login == admin.Login)
                {
                    foreach (User usere in GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Name = @new_name WHERE Login = @Login", cnn);

                            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;
                            command.Parameters.Add("@new_name", SqlDbType.VarChar, 40).Value = user.Name;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();


                            string userhistory = admin.Login + " changed name for this account";
                            string Adminhistory = admin.Login + "changed " + user.Login + " name";

                            Insert_User_History(userhistory, user.Login);
                            Insert_User_History(Adminhistory, admin.Login);

                            return admin;
                        }
                    }
                    admin.Error_Messege = "User_Login_not_found";
                    return admin;
                }
            }
            admin.Error_Messege = "Wrong_admin_login_or_password";
            return admin;
        }

        public User Change_Surname_Admin(Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User use in GetUsers())
            {
                if (use.Login == admin.Login)
                {
                    foreach (User usere in GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Surname = @new_surname WHERE Login = @Login", cnn);

                            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;
                            command.Parameters.Add("@new_surname", SqlDbType.VarChar, 40).Value = user.Surname;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();


                            string userhistory = admin.Login + " changed Surname for this account";
                            string Adminhistory = admin.Login + "changed " + user.Login + " Surname";

                            Insert_User_History(userhistory, user.Login);
                            Insert_User_History(Adminhistory, admin.Login);

                            cnn.Close();
                            return admin;

                        }
                    }
                    admin.Error_Messege = "User_Login_not_found";
                    return admin;
                }
            }
            admin.Error_Messege = "Wrong_admin_login_or_password";
            return admin;
        }

        //User
        [HttpPost("Log_in")]
        public async Task<List<User>> Log_in([FromBody] User user)
        {
            List<User> temp = new List<User>();

            foreach (User usere in GetUsers())
            {
                if (usere.Login == user.Login && usere.Password == user.Password && usere.Deleted == false)
                {
                    temp.Add(usere);
                    return temp;
                }
            }
            user.Error_Messege = "Wrong_login_or_password";
            temp.Add(user);
            return temp;
        }

        [HttpPost("Change_Email")]
        public async Task<List<User>> Change_Email([FromBody] User user)
        {
            List<User> temp = new List<User>();

            bool check;

            check = Is_Email_Exist(user.Email);
            if (check == true)
            {
                user.Error_Messege = "Email_taken";
                temp.Add(user);
                return temp;
            }

            check = Is_Email_Correct(user.Email);
            if(check == false)
            {
                user.Error_Messege = "Wrong_email";
                temp.Add(user);
                return temp;
            }

            
            foreach (User usere in GetUsers())
            {
                if (usere.Login == user.Login && usere.Password == user.Password)
                {
                    SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Email = @email_new WHERE Login = @Login", cnn);
                    
                    command.Parameters.Add("@email_new", SqlDbType.VarChar, 40).Value = user.Email;
                    command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;

                    cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    cnn.Close();


                    string userhistory = "You changed your e-mail";

                    Insert_User_History(userhistory, user.Login);

                    temp.Add(user);
                    cnn.Close();
                    return temp;
                }
            }
            user.Error_Messege = "Wrong_login_or_password";
            temp.Add(user);
            return temp;
        }

        [HttpPost("Change_Password")]
        public async Task<List<User>> Change_Password([FromBody] Receiver receiver)
        {
            User user = receiver.user;
            string password = receiver.new_password;
            List<User> temp = new List<User>();

            bool check;

            check = Is_Password_Correct(password);
            if (check == false)
            {
                user.Error_Messege = "Wrong_new_passowrd";
                temp.Add(user);
                return temp;
            }

            foreach (User usere in GetUsers())
            {
                if (usere.Login == user.Login && usere.Password == user.Password)
                {
                    SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Password = @password WHERE Login = @Login", cnn);

                    command.Parameters.Add("@password", SqlDbType.VarChar, 40).Value = password;
                    command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;

                    cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    cnn.Close();

                    string userhistory = "You changed your password";

                    Insert_User_History(userhistory, user.Login);

                    temp.Add(user);
                    return temp;
                }
            }
            user.Error_Messege = "Wrong_old_password_or_login";
            temp.Add(user);
            cnn.Close();
            return temp;
        }


        [HttpPost("Reset_Password_Code")]
        public async Task<List<User>> Reset_Password_Code([FromBody] User user)
        {
            List<User> temp = new List<User>();
            Helper helper;

            foreach (User usere in GetUsers())
            {
                if (usere.Email == user.Email)
                {
                    Send_Mail send_Mail = new Send_Mail();

                    helper = send_Mail.Reset_Pass_Code(usere.Email);

                    if (helper.check == true)
                    {
                        SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Reset_pass = @Reset_pass WHERE Email = @Email", cnn);
                        
                        command.Parameters.Add("@Reset_pass", SqlDbType.VarChar, 40).Value = helper.word;
                        command.Parameters.Add("@Email", SqlDbType.VarChar, 40).Value = user.Email;

                        cnn.Open();
                        command.ExecuteNonQuery();
                        command.Dispose();
                        cnn.Close();

                        temp.Add(user);
                        return temp;
                    }
                    else
                    {
                        user.Error_Messege = "Email_wasn't_send";
                        temp.Add(user);
                        return temp;
                    }
                }
            }

            user.Error_Messege = "Wrong_login_or_email";
            temp.Add(user);
            return temp;
        }

        [HttpPost("Reset_Password_Pass")]
        public async Task<List<User>> Reset_Password_Pass([FromBody] User user)
        {
            List<User> temp = new List<User>();
            Helper helper;

            foreach (User usere in GetUsers())
            {
                if (usere.Reset_pass == user.Reset_pass)
                {
                    Send_Mail send_Mail = new Send_Mail();

                    helper = send_Mail.Reset_Pass_Pass(usere.Email);

                    if (helper.check == true)
                    {
                        SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Password = @Password, Reset_pass = @Reset_pass WHERE Email = @Email", cnn);

                        command.Parameters.Add("@Password", SqlDbType.VarChar, 40).Value = helper.word;
                        command.Parameters.Add("@Reset_pass", SqlDbType.VarChar, 40).Value = "";
                        command.Parameters.Add("@Email", SqlDbType.VarChar, 40).Value = usere.Email;

                        cnn.Open();
                        command.ExecuteNonQuery();
                        command.Dispose();
                        cnn.Close();

                        string userhistory = "You reset your password";

                        Insert_User_History(userhistory, user.Login);

                        temp.Add(user);
                        return temp;
                    }
                    else
                    {
                        user.Error_Messege = "Email_wasn't_send";
                        temp.Add(user);
                        return temp;
                    }
                }
            }
            user.Error_Messege = "Wrong_code";
            temp.Add(user);
            return temp;
        }

    }
}
