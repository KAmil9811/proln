using CGC.Funkcje.MagazineFuncFolder.MagazineBase;
using CGC.Funkcje.UserFuncFolder.UserReturn;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.MagazineFuncFolder
{
    public class MagazineFunc
    {
        private static MagazineFunc m_oInstance = null;
        private static readonly object m_oPadLock = new object();
        private MagazineBaseModify magazineBaseModify = new MagazineBaseModify();
        private MagazineBaseReturn magazineBaseReturn = new MagazineBaseReturn();
        private UserBaseReturn userBaseReturn = new UserBaseReturn();

        public static MagazineFunc Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new MagazineFunc();
                    }
                    return m_oInstance;
                }
            }
        }

        public List<string> Return_All_Colors()
        {
            return magazineBaseReturn.GetColor();
        }

        public List<Glass_Receiver> Return_All_Glass()
        {
            return magazineBaseReturn.GetglassForUser();
        }

        public List<string> Return_All_Type()
        {
            return magazineBaseReturn.GetTypes();
        }

        public List<Magazine_History> Return_Magazine_History()
        {
            return magazineBaseReturn.GetMagazineHistories();
        }

        public List<Glass> Add_Glass(Receiver receiver)
        {
            List<Glass> temp = new List<Glass>();

            Glass glass = receiver.glass;
            User user = receiver.user;
            int code = 0;

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    magazineBaseModify.Add_Glass(user, glass, code, magazineBaseReturn.Getglass());
                }
            }
            glass.Error_Messege = "Użytkownik nie istnieje";
            temp.Add(glass);
            return temp;
        }

        public List<Glass> Edit_Glass(Receiver receiver)
        {
            List<Glass> temp = new List<Glass>();
            User user = receiver.user;
            Glass glass = receiver.glass;
            glass.Glass_id = receiver.glass.Glass_id;

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (user.Login == usere.Login)
                {
                    magazineBaseModify.Edit_Glass(user, glass);
                }
            }

            glass.Error_Messege = "Użytkownik nie istnieje";
            temp.Add(glass);

            return temp;
        }

        public List<Glass> Remove_Glass(Receiver receiver)
        {
            List<Glass> temp = new List<Glass>();
            List<int> Id_glasses = receiver.glass_Id;

            Glass glass = new Glass();

            User user = receiver.user;

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    return magazineBaseModify.Remove_Glass(user, Id_glasses, magazineBaseReturn.Getglass());
                }
            }
            glass.Error_Messege = "Użytkownik nie istnieje";
            temp.Add(glass);
            return temp;
        }

        public List<Glass> Restore_Glass(Receiver receiver)
        {
            List<Glass> temp = new List<Glass>();
            List<int> Id_glasses = receiver.glass_Id;

            Glass glass = new Glass();

            User user = receiver.user;

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    return magazineBaseModify.Restore_Glass(user, Id_glasses, magazineBaseReturn.Getglass());
                }
            }
            glass.Error_Messege = "Użytkownik nie istnieje";
            temp.Add(glass);
            return temp;
        }


        public List<string> Add_Type_Admin(Receiver receiver)
        {
            User user = receiver.user;
            string type = receiver.type;
            List<string> temp = new List<string>();

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (user.Login == usere.Login)
                {
                    foreach (string types in magazineBaseReturn.GetTypes())
                    {
                        if (types == type)
                        {
                            temp.Add("Typ już istnieje");
                            return temp;
                        }
                    }
                    return magazineBaseModify.Add_Type_Admin(user, type);
                }
            }
            temp.Add("Użytkownik nie istnieje");
            return temp;
        }

        public List<string> Add_Color_Admin(Receiver receiver)
        {
            User user = receiver.user;
            List<string> temp = new List<string>();
            string color = receiver.color;

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (string colors in magazineBaseReturn.GetColor())
                    {
                        if (colors == color)
                        {
                            temp.Add("Kolor już istnieje");
                            return temp;
                        }
                    }
                    return magazineBaseModify.Add_Color_Admin(user, color);
                }
            }
            temp.Add("Użytkownik nie istnieje");
            return temp;
        }

        public List<string> Change_Type_Admin(Receiver receiver)
        {
            User user = receiver.user;
            string new_type = receiver.new_type;
            string old_type = receiver.old_type;
            List<string> temp = new List<string>();

            foreach (string type in magazineBaseReturn.GetTypes())
            {
                if (type == new_type)
                {
                    temp.Add("Typ już istnieje");
                    return temp;
                }
            }

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (string type in magazineBaseReturn.GetTypes())
                    {
                        if (type == old_type)
                        {
                            magazineBaseModify.Change_Type_Admin(user, new_type, old_type);
                        }
                    }
                    temp.Add("Typ nie istnieje");
                    return temp;
                }
            }
            temp.Add("Użytkownik nie istnieje");
            return temp;
        }

        public List<string> Change_Color_Admin (Receiver receiver)
        {
            User user = receiver.user;
            List<string> temp = new List<string>();
            string new_color = receiver.new_color;
            string old_color = receiver.old_color;

            foreach (string type in magazineBaseReturn.GetColor())
            {
                if (type == new_color)
                {
                    temp.Add("Kolor już istnieje");
                    return temp;
                }
            }

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (string color in magazineBaseReturn.GetColor())
                    {
                        if (color == old_color)
                        {
                            magazineBaseModify.Change_Color_Admin(user, new_color, old_color);
                        }
                    }
                    temp.Add("Kolor nie istnieje");
                    return temp;
                }
            }
            temp.Add("Użytkownik nie istnieje");
            return temp;
        }
    }
}
