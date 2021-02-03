using CGC.Models;
using CGC.Funkcje.History;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.UserFuncFolder.UserReturn
{
    public class UserBaseModify
    {
        private static UserBaseModify m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static UserBaseModify Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new UserBaseModify();
                    }
                    return m_oInstance;
                }
            }
        }

        private Connect connect = new Connect();
        private InsertHistory insertHistory = new InsertHistory();

        public List<User> Add_User(User user, User admin)
        {
            List<User> temp = new List<User>();

            string query = "INSERT INTO dbo.[User](Login,Password,Email,Name,Surname,Admin,Super_Admin,Manager,Magazine_management,Machine_management,Order_management,Cut_management,Reset_pass,Deleted) VALUES(@Login, @Password, @Email, @Name, @Surname, @Admin, @Super_Admin, @Manager, @Magazine_management, @Machine_management, @Order_management, @Cut_management, @Reset_pass, @Deleted)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

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

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = admin.Login + " created this account";
            string Adminhistory = "You created account: " + user.Login;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_User_History(Adminhistory, admin.Login);

            temp.Add(admin);
            return temp;
        }

        public User Change_Email_Admin(User user, User admin)
        {
            User temp = new User();

            string query = "UPDATE dbo.[User] SET Email = @new_email WHERE Login = @Login";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;
            command.Parameters.Add("@new_email", SqlDbType.VarChar, 40).Value = user.Email;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = admin.Login + " changed e-mail to: " + user.Email;
            string Adminhistory = "You changed e-mail for " + user.Login + " to " + user.Email;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_User_History(Adminhistory, admin.Login);

            return temp;
        }

        public User Change_Password_Admin(User user, User admin)
        {
            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Password = @new_password WHERE Login = @Login", connect.cnn);

            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;
            command.Parameters.Add("@new_password", SqlDbType.VarChar, 40).Value = user.Password;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();


            string userhistory = admin.Login + " changed password to this account";
            string Adminhistory = "You changed password for " + user.Login;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_User_History(Adminhistory, admin.Login);

            connect.cnn.Close();
            return admin;
        }

        public List<User> Delete_User(User user, User admin)
        {
            List<User> temp = new List<User>();
            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Deleted = @Deleted WHERE Login = @Login", connect.cnn);

            command.Parameters.Add("@Deleted", SqlDbType.Bit).Value = true;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();


            string userhistory = admin.Login + " deleted this account";
            string Adminhistory = "You deleted account " + user.Login;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_User_History(Adminhistory, admin.Login);

            temp.Add(user);
            return temp;
        }

        public List<User> Restore_User(User user, User admin)
        {
            List<User> temp = new List<User>();
            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Deleted = @Deleted WHERE Login = @Login", connect.cnn);

            command.Parameters.Add("@Deleted", SqlDbType.Bit).Value = false;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();


            string userhistory = admin.Login + " przywrocil to konto";
            string Adminhistory = "Przywrociles konto " + user.Login;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_User_History(Adminhistory, admin.Login);

            temp.Add(user);
            return temp;
        }

        public User Change_Permision(User user, User admin)
        {
            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Admin=@Admin, Super_Admin=@Super_Admin, Magazine_management= @Magazine_management,Machine_management= @Machine_management, Order_management= @Order_management , Cut_management= @Cut_management WHERE Login = @Login", connect.cnn);
            connect.cnn.Open();

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
            connect.cnn.Close();

            string userhistory = admin.Login + " changed permission for thes account";
            string Adminhistory = "You changed permission for " + user.Login;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_User_History(Adminhistory, admin.Login);

            return admin;
        }
        public User Change_Name_Admin(User user, User admin)
        {
            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Name = @new_name WHERE Login = @Login", connect.cnn);

            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;
            command.Parameters.Add("@new_name", SqlDbType.VarChar, 40).Value = user.Name;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();


            string userhistory = admin.Login + " changed your name";
            string Adminhistory = "You changed user name " + user.Login + " to " + user.Name;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_User_History(Adminhistory, admin.Login);

            return admin;
        }

        public User Change_Surname_Admin(User user, User admin)
        {
            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Surname = @new_surname WHERE Login = @Login", connect.cnn);

            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;
            command.Parameters.Add("@new_surname", SqlDbType.VarChar, 40).Value = user.Surname;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();


            string userhistory = admin.Login + " changed your surname";
            string Adminhistory = "You changed user surname " + user.Login + " to " + user.Surname;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_User_History(Adminhistory, admin.Login);

            return admin;
        }

        public List<User> Change_Email(User user)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Email = @email_new WHERE Login = @Login", connect.cnn);

            command.Parameters.Add("@email_new", SqlDbType.VarChar, 40).Value = user.Email;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();


            string userhistory = "You changed your e-mail";

            insertHistory.Insert_User_History(userhistory, user.Login);

            temp.Add(user);
            connect.cnn.Close();
            return temp;
        }
    
        public List<User> Change_Password(User user, string password)
        {
            List<User> temp = new List<User>();
            
            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Password = @password WHERE Login = @Login", connect.cnn);

            command.Parameters.Add("@password", SqlDbType.VarChar, 40).Value = password;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = user.Login;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You changed your password";

            insertHistory.Insert_User_History(userhistory, user.Login);

            temp.Add(user);
            return temp;
        }

        public List<User> Save_Reset_Code(User user, string word)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Reset_pass = @Reset_pass WHERE Email = @Email", connect.cnn);

            command.Parameters.Add("@Reset_pass", SqlDbType.VarChar, 40).Value = word;
            command.Parameters.Add("@Email", SqlDbType.VarChar, 40).Value = user.Email;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            temp.Add(user);
            return temp;
        }

        public List<User> Save_Reset_Password(User user, string word)
        {
            List<User> temp = new List<User>();

            SqlCommand command = new SqlCommand("UPDATE dbo.[User] SET Password = @Password, Reset_pass = @Reset_pass WHERE Email = @Email", connect.cnn);

            try
            {
                command.Parameters.Add("@Password", SqlDbType.VarChar, 40).Value = word;
                command.Parameters.Add("@Reset_pass", SqlDbType.VarChar, 40).Value = "";
                command.Parameters.Add("@Email", SqlDbType.VarChar, 40).Value = user.Email;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

            }
            catch(Exception e)
            {
                e.ToString();
            }
            string userhistory = "You reseted your password";

            insertHistory.Insert_User_History(userhistory, user.Login);

            temp.Add(user);
            return temp;
        }
    }
}
