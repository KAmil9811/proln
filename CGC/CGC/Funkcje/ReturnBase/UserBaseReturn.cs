using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.UserFuncFolder.UserReturn
{
    public class UserBaseReturn
    {
        private Connect connect = new Connect();

        private static UserBaseReturn m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static UserBaseReturn Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new UserBaseReturn();
                    }
                    return m_oInstance;
                }
            }
        }

        public List<User> GetUser(string login)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Login = @Login;", connect.cnn);

            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = login;

            connect.cnn.Open();

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
                try
                {
                    user.Token = sqlDataReader["Token"].ToString();
                }
                catch
                {
                    user.Token = "";
                }
                temp.Add(user);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUser(string login, bool deleted)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Login = @Login AND Deleted = @Deleted;", connect.cnn);

            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = login;
            command.Parameters.Add("@Deleted", SqlDbType.VarChar, 40).Value = deleted;

            connect.cnn.Open();

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
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUserByEmail(string email)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Email = @Email;", connect.cnn);

            command.Parameters.Add("@Email", SqlDbType.VarChar, 40).Value = email;

            connect.cnn.Open();

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
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUserByCode(string code)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Reset_pass = @Reset_pass;", connect.cnn);

            command.Parameters.Add("@Reset_pass", SqlDbType.VarChar, 40).Value = code;

            connect.cnn.Open();

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
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUsers()
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User];", connect.cnn);
            connect.cnn.Open();

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
                try
                {
                    user.Token = sqlDataReader["Token"].ToString();
                }
                catch
                {
                    user.Token = "";
                }
                temp.Add(user);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUsers_Manager()
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Manager = @Manager;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Manager", SqlDbType.Bit).Value = 0;

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
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUsers_Super_Admin()
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Manager = @Manager and Super_Admin = @Super_Admin;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Manager", SqlDbType.VarBinary).Value = false;
            command.Parameters.Add("@Super_Admin", SqlDbType.VarBinary).Value = false;

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
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUsers_Admin()
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Manager = @Manager and Super_Admin = @Super_Admin and Admin = Admin;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Manager", SqlDbType.VarBinary).Value = false;
            command.Parameters.Add("@Super_Admin", SqlDbType.VarBinary).Value = false;
            command.Parameters.Add("@Admin", SqlDbType.VarBinary).Value = false;

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
            connect.cnn.Close();
            return temp;
        }

        public List<UserHistory> GetAllUserHistory()
        {
            List<UserHistory> userHistories = new List<UserHistory>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User_History];", connect.cnn);
            connect.cnn.Open();

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
            connect.cnn.Close();

            return userHistories;
        }

        public List<UserHistory> GetAllUserHistory(string Login)
        {
            List<UserHistory> userHistories = new List<UserHistory>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User_History] Where Login = @Login;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;

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
            connect.cnn.Close();

            return userHistories;
        }

    }
}
