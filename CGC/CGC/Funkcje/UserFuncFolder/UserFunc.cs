using CGC.Funkcje.UserFuncFolder.UserReturn;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CGC.Funkcje.UserFuncFolder
{
    public class UserFunc
    {
        private static UserFunc m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static UserFunc Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new UserFunc();
                    }
                    return m_oInstance;
                }
            }
        }

        private UserBaseReturn userBaseReturn = new UserBaseReturn();
        private UserCheck userCheck = new UserCheck();
        private UserBaseModify userBaseModify = new UserBaseModify();
        
        //Zwrawcanie rekordow z bazy -->
        public List<User> Return_All_Users(Receiver receiver)
        {
            return userBaseReturn.GetUsers_Manager(receiver.user.Company);
        }

        public List<User> Return_All_SuperAdmin(Receiver receiver)
        {
            return userBaseReturn.GetUsers_Admin(receiver.user.Company);
        }

        public List<User> Return_All_Admin(Receiver receiver)
        {
           return userBaseReturn.GetUsers_Admin(receiver.user.Company);
        }
        
        public List<UserHistory> Return_Users_History(Receiver receiver)
        {
            return userBaseReturn.GetAllUserHistory(receiver.user.Company);
        }
        
        public List<UserHistory> Return_User_History(Receiver receiver)
        {
            return userBaseReturn.GetAllUserHistory(receiver.user.Login, receiver.user.Company);
        }

        //<-- Zwrawcanie rekordow z bazy

        //Akcje admina -->
        public List<User> Add_User_Admin(Receiver receiver)
        {
            User user = receiver.user;
            User admin = receiver.admin;
            List<User> users = userBaseReturn.GetUsers(admin.Company);

            List<User> temp = new List<User>();
            bool check;

            check = userCheck.Is_Email_Correct(user.Email);
            if (check == false)
            {
                admin.Error_Messege = "Incorrect e-mail";
                temp.Add(admin);
                return temp;
            }

            check = userCheck.Is_Email_Exist(user.Email, users);
            if (check == true)
            {
                admin.Error_Messege = "Email has been already taken";
                temp.Add(admin);
                return temp;
            }

            check = userCheck.Is_Login_Correct(user.Login);
            if (check == false)
            {
                admin.Error_Messege = "Incorrect login";
                temp.Add(admin);
                return temp;
            }

            check = userCheck.Is_Login_Exist(user.Login, users);
            if (check == true)
            {
                admin.Error_Messege = "Login has been already taken";
                temp.Add(admin);
                return temp;
            }

            check = userCheck.Is_Password_Correct(user.Password);
            if (check == false)
            {
                admin.Error_Messege = "Incorrect password";
                temp.Add(admin);
                return temp;
            }

            if(receiver.perm == "superAdmin")
            {
                user.Super_Admin = true;
            }else if(receiver.perm == "admin")
            {
                user.Admin = true;
            }

            user.Id = userBaseReturn.GetUsers(admin.Company).OrderByDescending(x=> x.Id).First().Id;

            foreach (User use in userBaseReturn.GetUser(admin.Login, false, admin.Company))
            {
                if (use.Manager == true || use.Super_Admin == true || use.Admin == true)
                {
                    string LastGlobalId = userBaseReturn.GetLastGlobalIdUser(admin.Company).Last().Global_Id.ToString();
                    return userBaseModify.Add_User(user,use,LastGlobalId);
                }
            }
            admin.Error_Messege = "Admin not found";
            temp.Add(admin);
            return temp;
        }
        //do zmiany
        public List<User> Edit_User_Admin(Receiver receiver)
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

            if (check != "")
            {
                if (check == "user")
                {
                    receiver.user.Admin = false;
                    receiver.user.Super_Admin = false;
                    check2 = true;
                }
                else if (check == "admin")
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

            foreach (User use in userBaseReturn.GetUser(receiver.user.Login, receiver.admin.Company))
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

            if (temp.Error_Messege != null)
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

        public User Edit_Email_Admin(Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            bool check;

            check = userCheck.Is_Email_Correct(user.Email);
            if (check == false)
            {
                admin.Error_Messege = "Incorrect e-mail";
                return admin;
            }

            check = userCheck.Is_Email_Exist(user.Email, userBaseReturn.GetUsers(admin.Company));
            if (check == true)
            {
                admin.Error_Messege = "E-mail has been already taken";
                return admin;
            }

            foreach (User use in userBaseReturn.GetUser(admin.Login, false, admin.Company))
            {
                foreach (User usere in userBaseReturn.GetUser(user.Login, admin.Company))
                {
                    return userBaseModify.Change_Email_Admin(user, admin);
                }
                admin.Error_Messege = "User not found";
                return admin;            
            }
            admin.Error_Messege = "Admin not found";
            return admin;
        }

        public List<User> Remove_User_Admin(Receiver receiver)
        {
            List<User> temp = new List<User>();

            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User adm in userBaseReturn.GetUser(admin.Login,false, admin.Company))
            {
                foreach (User usere in userBaseReturn.GetUser(user.Login, admin.Company))
                {
                    if (usere.Deleted == true)
                    {
                        user.Error_Messege = "User has been already deleted";
                        temp.Add(user);
                        return temp;
                    }
                    return userBaseModify.Delete_User(user,admin);                  
                }
                user.Error_Messege = "User not found";
                temp.Add(user);
                return temp;               
            }
            user.Error_Messege = "Admin not found";
            temp.Add(user);
            return temp;

        }

        public List<User> Restore_User_Admin(Receiver receiver)
        {
            List<User> temp = new List<User>();

            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User adm in userBaseReturn.GetUser(admin.Login,false, admin.Company))
            {
                foreach (User usere in userBaseReturn.GetUser(user.Login, admin.Company))
                {
                    if (usere.Deleted == true)
                    {
                        return userBaseModify.Restore_User(user, admin);
                    }
                    user.Error_Messege = "User is active";
                    temp.Add(user);
                    return temp;                  
                }
                user.Error_Messege = "User not found";
                temp.Add(user);
                return temp;
            }           
            user.Error_Messege = "Admin not found";
            temp.Add(user);
            return temp;
        }

        public User Change_Password_Admin(Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            bool check;

            check = userCheck.Is_Password_Correct(user.Password);
            if (check == false)
            {
                admin.Error_Messege = "Incorrect password";
                return admin;
            }

            foreach (User use in userBaseReturn.GetUser(admin.Login,false, admin.Company))
            {
                foreach (User usere in userBaseReturn.GetUser(user.Login, admin.Company))
                {
                    return userBaseModify.Change_Password_Admin(user, admin);
                }
                admin.Error_Messege = "User not found";
                return admin;         
            }
            admin.Error_Messege = "Admin not found";
            return admin;
        }

        public User Set_Permissions_Admin(Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User use in userBaseReturn.GetUser(admin.Login,false, admin.Company))
            {
                foreach (User usere in userBaseReturn.GetUser(user.Login, admin.Company))
                {
                    return userBaseModify.Change_Permision(user, admin);
                }
                admin.Error_Messege = "User not found";
                return admin;                
            }
            admin.Error_Messege = "Admin not found";
            return admin;
        }

        public User Change_Name_Admin(Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User use in userBaseReturn.GetUser(admin.Login,false, admin.Company))
            {
                foreach (User usere in userBaseReturn.GetUser(user.Login, admin.Company))
                {
                     return userBaseModify.Change_Name_Admin(user, admin);               
                }
                admin.Error_Messege = "User not found";
                return admin;  
            }
            admin.Error_Messege = "Admin not found";
            return admin;
        }

        public User Change_Surname_Admin(Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User use in userBaseReturn.GetUser(admin.Login, false, admin.Company))
            {
                foreach (User usere in userBaseReturn.GetUser(user.Login, admin.Company))
                {
                    return userBaseModify.Change_Surname_Admin(user, admin);
                }
                admin.Error_Messege = "User not found";
                return admin;
            }
            admin.Error_Messege = "Admin not found";
            return admin;
        }
        
        //<-- Akcje admina
                
        //Zarzadzanie kontem -->
        public List<User> Log_in(Receiver receiver)
        {
            User user = receiver.user;
            List<User> temp = new List<User>();

            foreach (User usere in userBaseReturn.GetUser(user.Login,false, user.Company))
            {
                if (usere.Password == user.Password)
                {
                    temp.Add(usere);
                    return temp;
                }
            }
            user.Error_Messege = "Incorrect login or password";
            temp.Add(user);
            return temp;
        }

        public List<User> Change_Email(Receiver receiver)
        {
            User user = receiver.user;
            List<User> temp = new List<User>();

            bool check;

            check = userCheck.Is_Email_Exist(user.Email, userBaseReturn.GetUsers(user.Company));
            if (check == true)
            {
                user.Error_Messege = "E-mail has been already taken";
                temp.Add(user);
                return temp;
            }

            check = userCheck.Is_Email_Correct(user.Email);
            if (check == false)
            {
                user.Error_Messege = "Incorrect e-mail";
                temp.Add(user);
                return temp;
            }


            foreach (User usere in userBaseReturn.GetUser(user.Login,false, user.Company))
            {
                if (usere.Password == user.Password)
                {
                    return userBaseModify.Change_Email(user);
                }
            }
            user.Error_Messege = "User not found";
            temp.Add(user);
            return temp;
        }

        public  List<User> Change_Password(Receiver receiver)
        {
            User user = receiver.user;
            string password = receiver.new_password;
            List<User> temp = new List<User>();

            bool check;

            check = userCheck.Is_Password_Correct(password);
            if (check == false)
            {
                user.Error_Messege = "Incorrect password";
                temp.Add(user);
                return temp;
            }

            foreach (User usere in userBaseReturn.GetUser(user.Login,false, user.Company))
            {
                if (usere.Password == user.Password)
                {
                    return userBaseModify.Change_Password(user, password);
                }
            }
            user.Error_Messege = "Incorrect login or password";
            temp.Add(user);
            return temp;
        }

        public List<User> Reset_Password_Code(Receiver receiver)
        {
            User user = receiver.user;
            List<User> temp = new List<User>();
            Helper helper;

            foreach (User usere in userBaseReturn.GetUserByEmail(user.Email, user.Company))
            {
                Send_Mail send_Mail = new Send_Mail();

                helper = send_Mail.Reset_Pass_Code(usere.Email, user.Company);

                if (helper.check == true)
                {
                    return userBaseModify.Save_Reset_Code(user, helper.word);
                }
                else
                {
                    user.Error_Messege = "E-mail has nor been sent";
                    temp.Add(user);
                    return temp;
                }            
            }

            user.Error_Messege = "Incorrect login or e-mail";
            temp.Add(user);
            return temp;
        }

        public List<User> Reset_Password_Pass(Receiver receiver)
        {
            User user = receiver.user;
            List<User> temp = new List<User>();
            Helper helper;

            foreach (User usere in userBaseReturn.GetUserByCode(user.Reset_pass, user.Company))
            {
                Send_Mail send_Mail = new Send_Mail();

                helper = send_Mail.Reset_Pass_Pass(usere.Email);

                if (helper.check == true)
                {
                    return userBaseModify.Save_Reset_Password(usere, helper.word);
                }
                else
                {
                    user.Error_Messege = "E-mail has not been sent";
                    temp.Add(user);
                    return temp;
                }              
            }
            user.Error_Messege = "Incorrect code";
            temp.Add(user);
            return temp;
        }

        //<-- Zarzadzanie kontem
    }
}
