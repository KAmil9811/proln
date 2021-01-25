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
        public List<User> Return_All_Users()
        {
            return userBaseReturn.GetUsers_Manager();
        }

        public List<User> Return_All_SuperAdmin()
        {
            return userBaseReturn.GetUsers_Admin();
        }

        public List<User> Return_All_Admin()
        {
           return userBaseReturn.GetUsers_Admin();
        }
        
        public List<UserHistory> Return_Users_History()
        {
            return userBaseReturn.GetAllUserHistory();
        }
        
        public List<UserHistory> Return_User_History(Receiver receiver)
        {
            return userBaseReturn.GetAllUserHistory(receiver.user.Login);
        }

        //<-- Zwrawcanie rekordow z bazy

        //Akcje admina -->
        public List<User> Add_User_Admin(Receiver receiver)
        {
            User user = receiver.user;
            User admin = receiver.admin;

            List<User> temp = new List<User>();
            bool check;

            check = userCheck.Is_Email_Correct(user.Email);
            if (check == false)
            {
                admin.Error_Messege = "Niepoprawny email";
                temp.Add(admin);
                return temp;
            }

            check = userCheck.Is_Email_Exist(user.Email);
            if (check == true)
            {
                admin.Error_Messege = "Email jest juz zajety";
                temp.Add(admin);
                return temp;
            }

            check = userCheck.Is_Login_Correct(user.Login);
            if (check == false)
            {
                admin.Error_Messege = "Niepoprawny login";
                temp.Add(admin);
                return temp;
            }

            check = userCheck.Is_Login_Exist(user.Login);
            if (check == true)
            {
                admin.Error_Messege = "Login jest juz zajety";
                temp.Add(admin);
                return temp;
            }

            check = userCheck.Is_Password_Correct(user.Password);
            if (check == false)
            {
                admin.Error_Messege = "Niepoprawne haslo";
                temp.Add(admin);
                return temp;
            }

            foreach (User use in userBaseReturn.GetUsers())
            {
                if (use.Login == admin.Login && (use.Manager == true || use.Super_Admin == true || use.Admin == true))
                {
                    return userBaseModify.Add_User(user,use);
                }
            }
            admin.Error_Messege = "Nie odnaleziono admina";
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

            foreach (User use in userBaseReturn.GetUsers())
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
                admin.Error_Messege = "Niepoprawny email";
                return admin;
            }

            check = userCheck.Is_Email_Exist(user.Email);
            if (check == true)
            {
                admin.Error_Messege = "Email jest juz zajety";
                return admin;
            }

            foreach (User use in userBaseReturn.GetUsers())
            {
                if (use.Login == admin.Login)
                {
                    foreach (User usere in userBaseReturn.GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            return userBaseModify.Change_Email_Admin(user, admin);
                        }
                    }
                    admin.Error_Messege = "Nie odnaleziono uzytkownika";
                    return admin;
                }
            }
            admin.Error_Messege = "Nie odnaleziono admina";
            return admin;
        }

        public List<User> Remove_User_Admin(Receiver receiver)
        {
            List<User> temp = new List<User>();

            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User adm in userBaseReturn.GetUsers())
            {
                if (adm.Login == admin.Login)
                {
                    foreach (User usere in userBaseReturn.GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            if (usere.Deleted == true)
                            {
                                user.Error_Messege = "Uzytkownik zostal juz usuniety";
                                temp.Add(user);
                                return temp;
                            }
                            return userBaseModify.Delete_User(user,admin);
                        }
                    }
                    user.Error_Messege = "Nie odnaleziono uzytkownika";
                    temp.Add(user);
                    return temp;
                }
            }
            user.Error_Messege = "Nie odnaleziono admina";
            temp.Add(user);
            return temp;

        }

        public List<User> Restore_User_Admin(Receiver receiver)
        {
            List<User> temp = new List<User>();

            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User adm in userBaseReturn.GetUsers())
            {
                if (adm.Login == admin.Login)
                {
                    foreach (User usere in userBaseReturn.GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            if (usere.Deleted == true)
                            {
                                return userBaseModify.Restore_User(user, admin);
                            }
                            user.Error_Messege = "Uzytkownik jest aktywny";
                            temp.Add(user);
                            return temp;
                        }
                    }
                    user.Error_Messege = "Nie odnaleziono uzytkownika";
                    temp.Add(user);
                    return temp;
                }
            }
            user.Error_Messege = "Nie odnaleziono admina";
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
                admin.Error_Messege = "Niepoprawne haslo";
                return admin;
            }

            foreach (User use in userBaseReturn.GetUsers())
            {
                if (use.Login == admin.Login)
                {
                    foreach (User usere in userBaseReturn.GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            return userBaseModify.Change_Password_Admin(user, admin);
                        }
                    }
                    admin.Error_Messege = "Nie odnaleziono uzytkownika";
                    return admin;
                }
            }
            admin.Error_Messege = "Nie odnaleziono admina";
            return admin;
        }
        //do zmiany
        public User Set_Permissions_Admin(Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User use in userBaseReturn.GetUsers())
            {
                if (use.Login == admin.Login)
                {
                    foreach (User usere in userBaseReturn.GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            return userBaseModify.Change_Permision(user, admin);
                        }
                    }
                    admin.Error_Messege = "Nie odnaleziono uzytkownika";
                    return admin;
                }
            }
            admin.Error_Messege = "Nie odnaleziono admina";
            return admin;
        }

        public User Change_Name_Admin(Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User use in userBaseReturn.GetUsers())
            {
                if (use.Login == admin.Login)
                {
                    foreach (User usere in userBaseReturn.GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            return userBaseModify.Change_Name_Admin(user, admin);
                        }
                    }
                    admin.Error_Messege = "Nie odnaleziono uzytkownika";
                    return admin;
                }
            }
            admin.Error_Messege = "Nie odnaleziono admina";
            return admin;
        }

        public User Change_Surname_Admin(Receiver receiver)
        {
            User admin = receiver.admin;
            User user = receiver.user;

            foreach (User use in userBaseReturn.GetUsers())
            {
                if (use.Login == admin.Login)
                {
                    foreach (User usere in userBaseReturn.GetUsers())
                    {
                        if (usere.Login == user.Login)
                        {
                            return userBaseModify.Change_Surname_Admin(user, admin);
                        }
                    }
                    admin.Error_Messege = "Nie odnaleziono uzytkownika";
                    return admin;
                }
            }
            admin.Error_Messege = "Nie odnaleziono admina";
            return admin;
        }
        
        //<-- Akcje admina
                
        //Zarzadzanie kontem -->
        public List<User> Log_in(Receiver receiver)
        {
            User user = receiver.user;
            List<User> temp = new List<User>();

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && usere.Password == user.Password)
                {
                    if(usere.Deleted == false)
                    {
                        temp.Add(usere);
                        return temp;
                    }
                    else
                    {
                        user.Error_Messege = "Uzytkownik zostal usuniety";
                        temp.Add(user);
                        return temp;
                    }
                }
            }
            user.Error_Messege = "Zly login lub haslo";
            temp.Add(user);
            return temp;
        }

        public List<User> Change_Email(Receiver receiver)
        {
            User user = receiver.user;
            List<User> temp = new List<User>();

            bool check;

            check = userCheck.Is_Email_Exist(user.Email);
            if (check == true)
            {
                user.Error_Messege = "Email jest juz zajety";
                temp.Add(user);
                return temp;
            }

            check = userCheck.Is_Email_Correct(user.Email);
            if (check == false)
            {
                user.Error_Messege = "Niepoprawny email";
                temp.Add(user);
                return temp;
            }


            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && usere.Password == user.Password)
                {
                    return userBaseModify.Change_Email(user);
                }
            }
            user.Error_Messege = "Nie odnaleziono uzytkownika";
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
                user.Error_Messege = "Niepoprawne haslo";
                temp.Add(user);
                return temp;
            }

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && usere.Password == user.Password)
                {
                    return userBaseModify.Change_Password(user, password);
                }
            }
            user.Error_Messege = "Zly login, lub email";
            temp.Add(user);
            return temp;
        }

        public List<User> Reset_Password_Code(Receiver receiver)
        {
            User user = receiver.user;
            List<User> temp = new List<User>();
            Helper helper;

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Email == user.Email)
                {
                    Send_Mail send_Mail = new Send_Mail();

                    helper = send_Mail.Reset_Pass_Code(usere.Email);

                    if (helper.check == true)
                    {
                        return userBaseModify.Save_Reset_Code(user, helper.word);
                    }
                    else
                    {
                        user.Error_Messege = "Email nie zostal wyslany";
                        temp.Add(user);
                        return temp;
                    }
                }
            }

            user.Error_Messege = "Zly login, lub email";
            temp.Add(user);
            return temp;
        }

        public List<User> Reset_Password_Pass(Receiver receiver)
        {
            User user = receiver.user;
            List<User> temp = new List<User>();
            Helper helper;

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Reset_pass == user.Reset_pass)
                {
                    Send_Mail send_Mail = new Send_Mail();

                    helper = send_Mail.Reset_Pass_Pass(usere.Email);

                    if (helper.check == true)
                    {
                        return userBaseModify.Save_Reset_Password(usere, helper.word);
                    }
                    else
                    {
                        user.Error_Messege = "Email nie zostal wyslany";
                        temp.Add(user);
                        return temp;
                    }
                }
            }
            user.Error_Messege = "Zly kod";
            temp.Add(user);
            return temp;
        }

        //<-- Zarzadzanie kontem
    }
}
