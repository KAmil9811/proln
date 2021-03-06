using CGC.Funkcje.UserFuncFolder.UserReturn;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CGC.Funkcje.UserFuncFolder
{
    public class UserCheck
    {
        private static UserCheck m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static UserCheck Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new UserCheck();
                    }
                    return m_oInstance;
                }
            }
        }

        UserBaseReturn userBaseReturn = new UserBaseReturn();

        //Funckje pomocnicze

        public bool Is_Email_Exist(string email, List<User> users)
        {
            foreach (User user in users)
            {
                if (user.Email == email)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Is_Login_Exist(string login, List<User> users)
        {
            foreach (User user in users)
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

    }
}
