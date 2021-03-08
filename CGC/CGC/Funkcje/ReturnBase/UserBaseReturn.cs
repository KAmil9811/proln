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

        public List<User> GetUser(string login, string company)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Login = @Login AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = login;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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
                user.Company = sqlDataReader["Company"].ToString();
                temp.Add(user);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUser(string login, bool deleted, string company)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Login = @Login AND Deleted = @Deleted AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = login;
            command.Parameters.Add("@Deleted", SqlDbType.VarChar, 40).Value = deleted;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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
                user.Company = sqlDataReader["Company"].ToString();
                temp.Add(user);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUserByEmail(string email, string company)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Email = @Email AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Email", SqlDbType.VarChar, 40).Value = email;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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
                user.Company = sqlDataReader["Company"].ToString();
                temp.Add(user);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUserByCode(string code, string company)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Reset_pass = @Reset_pass AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Reset_pass", SqlDbType.VarChar, 40).Value = code;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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
                user.Company = sqlDataReader["Company"].ToString();
                temp.Add(user);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUsers(string company)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Company = @Company;", connect.cnn);

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                User user = new User();
                user.Id = Convert.ToInt32(sqlDataReader["Id"]);
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
                user.Company = sqlDataReader["Company"].ToString();
                temp.Add(user);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUsers_Manager(string company)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Manager = @Manager AND Company = @Company;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            command.Parameters.Add("@Manager", SqlDbType.Bit).Value = false;

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
                user.Company = sqlDataReader["Company"].ToString();
                temp.Add(user);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUsers_Super_Admin(string company)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Manager = @Manager and Super_Admin = @Super_Admin AND Company = @Company;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Manager", SqlDbType.Bit).Value = false;
            command.Parameters.Add("@Super_Admin", SqlDbType.Bit).Value = false;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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
                user.Company = sqlDataReader["Company"].ToString();
                temp.Add(user);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<User> GetUsers_Admin(string company)
        {
            List<User> temp = new List<User>();
            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE Manager = @Manager and Super_Admin = @Super_Admin and Admin = @Admin AND Company = @Company;", connect.cnn);
                connect.cnn.Open();

                command.Parameters.Add("@Manager", SqlDbType.Bit).Value = false;
                command.Parameters.Add("@Super_Admin", SqlDbType.Bit).Value = false;
                command.Parameters.Add("@Admin", SqlDbType.Bit).Value = false;
                command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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
                    user.Company = sqlDataReader["Company"].ToString();
                    temp.Add(user);
                }
                sqlDataReader.Close();
                command.Dispose();
                connect.cnn.Close();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return temp;
        }

        public List<UserHistory> GetAllUserHistory(string company)
        {
            List<UserHistory> userHistories = new List<UserHistory>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User_History] WHERE Company = @Company;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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

        public List<UserHistory> GetAllUserHistory(string Login, string company)
        {
            List<UserHistory> userHistories = new List<UserHistory>();

            SqlCommand command = new SqlCommand("SELECT * FROM [User_History] Where Login = @Login AND Company = @Company;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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

        public List<User> GetLastGlobalIdUser(string company)
        {
            List<User> temp = new List<User>();
            SqlCommand command = new SqlCommand("Select TOP(1) Global_id From [User] ORDER BY convert(int, Global_id) DESC", connect.cnn);
            connect.cnn.Open();

            //command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                User user = new User();
                user.Global_Id = Convert.ToInt32(sqlDataReader["Global_id"]) + 1;

                temp.Add(user);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            if (temp.Count == 0)
            {
                temp.Add(new User { Global_Id = 1 });
            }

            return temp;
        }

        public List<UserHistory> GetLastGlobalIdUserHistory(string company)
        {
            List<UserHistory> temp = new List<UserHistory>();
            SqlCommand command = new SqlCommand("Select TOP(1) Global_id From [User_History] ORDER BY convert(int, Global_id) DESC", connect.cnn);
            connect.cnn.Open();

            //command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                UserHistory userHistory = new UserHistory();
                userHistory.Global_Id = Convert.ToInt32(sqlDataReader["Global_id"]) + 1;

                temp.Add(userHistory);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            if (temp.Count == 0)
            {
                temp.Add(new UserHistory { Global_Id = 1 });
            }

            return temp;
        }

        public List<Entities.User> GetUsersToLogin()
        {
            List<Entities.User> temp = new List<Entities.User>();
            SqlCommand command = new SqlCommand("SELECT * FROM [User];", connect.cnn);

            try
            {
                connect.cnn.Open();
            }
            catch(Exception e)
            {
                e.ToString();
            }
            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Entities.User user = new Entities.User();
                user.Id = Convert.ToInt32(sqlDataReader["Id"]);
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
                user.Company = sqlDataReader["Company"].ToString();
                try
                {
                    user.Session_Start = Convert.ToBoolean(sqlDataReader["Session_Start"]);
                    user.Session_End = Convert.ToBoolean(sqlDataReader["Session_End"]);
                    user.Token = sqlDataReader["Token"].ToString();
                }
                catch
                {
                    user.Session_Start = false;
                    user.Session_End = false;
                    user.Token = "";
                }
                temp.Add(user);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }
    }
}
