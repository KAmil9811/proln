using CGC.Funkcje.History;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.MagazineFuncFolder.MagazineBase
{
    public class MagazineBaseModify
    {
        private Connect connect = new Connect();
        private InsertHistory insertHistory = new InsertHistory();
        private MagazineBaseReturn magazineBaseReturn = new MagazineBaseReturn();

        private static MagazineBaseModify m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static MagazineBaseModify Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new MagazineBaseModify();
                    }
                    return m_oInstance;
                }
            }
        }

        public List<Glass> Add_Glass(User user, Glass glass, int code, List<Glass> glasses)
        {
            List<Glass> temp = new List<Glass>();

            for (int i = glass.Count; i > 0; i--)
            {
                code = 0;

                foreach (Glass gl in glasses)
                {
                    foreach (Glass_Id gl2 in gl.Glass_info)
                    {
                        if (gl2.Id > code)
                        {
                            code = gl2.Id;
                        }
                    }
                }

                code++;

                string query = "INSERT INTO dbo.Glass(Hight,Width,Length,Used,Removed,Type,Color,Owner,Desk,Glass_Id) VALUES(@Hight, @Width, @Length, @Used, @Removed, @Type, @Color, @Owner, @Desk, @code)";

                SqlCommand command = new SqlCommand(query, connect.cnn);
                command.Parameters.Add("@Hight", SqlDbType.Decimal).Value = glass.Hight;
                command.Parameters.Add("@Width", SqlDbType.Decimal).Value = glass.Width;
                command.Parameters.Add("@Length", SqlDbType.Decimal).Value = glass.Length;
                command.Parameters.Add("@Used", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@Removed", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = glass.Type;
                command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = glass.Color;
                command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = glass.Owner;
                command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = glass.Desk;
                command.Parameters.Add("@code", SqlDbType.Int).Value = code;
                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "Dodales szklo " + code;
                string magazinehistory = "Szklo " + code + " zostalo dodane";

                insertHistory.Insert_User_History(userhistory, user.Login);
                insertHistory.Insert_Magazine_History(magazinehistory, user.Login);

                glass.Glass_info.Add(new Glass_Id { Id = code });

                //SetOrderStan();
                glasses.Add(glass);
                temp.Add(glass);
            }
            return temp;
        }

        public List<Glass> Edit_Glass(User user, Glass glass)
        {
            List<Glass> temp = new List<Glass>();

            foreach (int glass_Id in glass.Glass_id)
            {
                string query = "UPDATE dbo.[Glass] SET Hight = @Hight, Width = @Width, Length = @Length, Type = @Type, Color = @Color, Owner = @Owner, Desk = @Desk WHERE Glass_Id = @Glass_Id;";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Hight", SqlDbType.Decimal).Value = glass.Hight;
                command.Parameters.Add("@Width", SqlDbType.Decimal).Value = glass.Width;
                command.Parameters.Add("@Length", SqlDbType.Decimal).Value = glass.Length;
                command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = glass.Type;
                command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = glass.Color;
                command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = glass.Owner;
                command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = glass.Desk;

                command.Parameters.Add("@Glass_Id", SqlDbType.Int).Value = glass_Id;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "Zedytowales szklo " + glass_Id;
                string magazinehistory = "Szklo " + glass_Id + " zostalo zedytowane";

                insertHistory.Insert_User_History(userhistory, user.Login);
                insertHistory.Insert_Magazine_History(magazinehistory, user.Login);                
            }
            temp.Add(glass);
            return temp;
        }

        public List<Glass> Remove_Glass(User user, List<int> Id_glasses, List<Glass> glasses)
        {
            List<Glass> temp = new List<Glass>();
            Glass glass = new Glass();

            foreach (Glass glasse in glasses)
            {
                foreach (Glass_Id ids in glasse.Glass_info)
                {
                    foreach (int id_glasse in Id_glasses)
                    {
                        if (ids.Id == id_glasse)
                        {
                            if (ids.Used == true)
                            {
                                glass.Error_Messege = "Szklo zostalo juz zuzyte";
                                temp.Add(glass);
                                return temp;
                            }

                            if (ids.Removed == true)
                            {
                                glass.Error_Messege = "Szklo zostalo juz usuniete";
                                temp.Add(glass);
                                return temp;
                            }

                            string query = "UPDATE dbo.[Glass] SET Removed = @Removed WHERE Glass_Id = @Glass_Id;";
                            SqlCommand command = new SqlCommand(query, connect.cnn);

                            command.Parameters.Add("@Removed", SqlDbType.Bit).Value = true;
                            command.Parameters.Add("@Glass_Id", SqlDbType.Int).Value = id_glasse;

                            connect.cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            connect.cnn.Close();

                            string userhistory = "Usunales szklo " + ids.Id;
                            string magazinehistory = "Szklo " + ids.Id + " zostalo usuniete";

                            insertHistory.Insert_User_History(userhistory, user.Login);
                            insertHistory.Insert_Magazine_History(magazinehistory, user.Login);

                            //SetOrderStan();

                            temp.Add(glass);
                        }
                    }
                    return temp;
                }
            }
            return temp;
        }

        public List<Glass> Restore_Glass(User user, List<int> Id_glasses, List<Glass> glasses)
        {
            List<Glass> temp = new List<Glass>();
            Glass glass = new Glass();

            foreach (Glass glasse in glasses)
            {
                foreach (Glass_Id ids in glasse.Glass_info)
                {
                    foreach (int id_glasse in Id_glasses)
                    {
                        if (ids.Id == id_glasse)
                        {
                            if (ids.Used == true)
                            {
                                glass.Error_Messege = "Szklo zostalo juz zuzyte";
                                temp.Add(glass);
                                return temp;
                            }

                            if (ids.Removed == false)
                            {
                                glass.Error_Messege = "Szklo zostalo juz przywrocone";
                                temp.Add(glass);
                                return temp;
                            }

                            string query = "UPDATE dbo.[Glass] SET Removed = @Removed WHERE Glass_Id = @Glass_Id;";
                            SqlCommand command = new SqlCommand(query, connect.cnn);

                            command.Parameters.Add("@Removed", SqlDbType.Bit).Value = false;
                            command.Parameters.Add("@Glass_Id", SqlDbType.Int).Value = id_glasse;

                            connect.cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            connect.cnn.Close();

                            string userhistory = "Przywrociles szklo " + ids.Id;
                            string magazinehistory = "Szklo " + ids.Id + " zostalo przywrocone";

                            insertHistory.Insert_User_History(userhistory, user.Login);
                            insertHistory.Insert_Magazine_History(magazinehistory, user.Login);

                            //SetOrderStan();

                            temp.Add(glass);
                        }
                    }
                    return temp;
                }
            }
            return temp;
        }

        public List<string> Add_Type_Admin(User user, string type)
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("INSERT INTO dbo.Glass_type(Type) VALUES(@type)", connect.cnn);
            command.Parameters.Add("@type", SqlDbType.VarChar, 40).Value = type;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = "Dodales nowy typ szkla: " + type;
            string magazinehistory = "Typ szkla " + type + " zostal dodany";

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Magazine_History(magazinehistory, user.Login);

            temp.Add(type);
            return temp;
        }

        public List<string> Add_Color_Admin(User user, string color)
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("INSERT INTO dbo.Color(Color) VALUES(@Color)", connect.cnn);

            command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = color;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = "Dodales nowy kolor szkla: " + color;
            string magazinehistory = "Kolor szkla " + color + " zostal dodany";

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Magazine_History(magazinehistory, user.Login);

            temp.Add(color);
            return temp;
        }

        public List<string> Change_Type_Admin(User user, string new_type, string old_type)
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("UPDATE dbo.Glass_type SET Type = @new_type WHERE Type = @old_type", connect.cnn);

            command.Parameters.Add("@new_type", SqlDbType.VarChar, 40).Value = new_type;
            command.Parameters.Add("@old_type", SqlDbType.VarChar, 40).Value = old_type;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();


            command = new SqlCommand("UPDATE dbo.Glass SET Type = @new_type WHERE Type = @old_type", connect.cnn);

            command.Parameters.Add("@new_type", SqlDbType.VarChar, 40).Value = new_type;
            command.Parameters.Add("@old_type", SqlDbType.VarChar, 40).Value = old_type;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();


            string userhistory = "Zmieniles typ z " + old_type + " na: " + new_type;
            string magazinehistory = "Typ " + old_type + " zostal zmieniony na: " + new_type;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Magazine_History(magazinehistory, user.Login);

            temp.Add(new_type);
            return temp;
        }

        public List<string> Change_Color_Admin(User user, string new_color, string old_color)
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("UPDATE dbo.Color SET Color = @new_color WHERE Color = @old_color", connect.cnn);

            command.Parameters.Add("@new_color", SqlDbType.VarChar, 40).Value = new_color;
            command.Parameters.Add("@old_color", SqlDbType.VarChar, 40).Value = old_color;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();


            command = new SqlCommand("UPDATE dbo.Glass SET Color = @new_color WHERE Color = @old_color", connect.cnn);

            command.Parameters.Add("@new_color", SqlDbType.VarChar, 40).Value = new_color;
            command.Parameters.Add("@old_color", SqlDbType.VarChar, 40).Value = old_color;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = "Zmieniles kolor z " + old_color + " na: " + new_color;
            string magazinehistory = "Kolor " + old_color + " zostal zmieniony na: " + new_color;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Magazine_History(magazinehistory, user.Login);

            temp.Add(old_color);
            return temp;
        }
    }
}
