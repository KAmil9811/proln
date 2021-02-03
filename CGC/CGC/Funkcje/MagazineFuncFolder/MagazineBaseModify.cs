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
            glass.Glass_info = new List<Glass_Id>();

            for (int i = Convert.ToInt32(glass.Count); i > 0; i--)
            {
                code = 0;

                foreach (Glass gl in glasses)
                {
                    foreach (Glass_Id gl2 in gl.Glass_info)
                    {
                        if (Convert.ToInt32(gl2.Id) > code)
                        {
                            code = Convert.ToInt32(gl2.Id);
                        }
                    }
                }

                code++;

                string query = "INSERT INTO dbo.Glass(Hight,Width,Length,Used,Removed,Type,Color,Owner,Desk,Glass_Id) VALUES(@Hight, @Width, @Length, @Used, @Removed, @Type, @Color, @Owner, @Desk, @code)";

                SqlCommand command = new SqlCommand(query, connect.cnn);
                command.Parameters.Add("@Hight", SqlDbType.VarChar, 40).Value = glass.Hight;
                command.Parameters.Add("@Width", SqlDbType.VarChar, 40).Value = glass.Width;
                command.Parameters.Add("@Length", SqlDbType.VarChar, 40).Value = glass.Length;
                command.Parameters.Add("@Used", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@Removed", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = glass.Type;
                command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = glass.Color;
                command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = glass.Owner;
                command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = glass.Desk;
                command.Parameters.Add("@code", SqlDbType.VarChar, 40).Value = code.ToString();
                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "You added glass " + code;
                string magazinehistory = "Glass " + code + " has been added";

                insertHistory.Insert_User_History(userhistory, user.Login);
                insertHistory.Insert_Magazine_History(magazinehistory, user.Login);

                glass.Glass_info.Add(new Glass_Id { Id = (code).ToString() });

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

                command.Parameters.Add("@Hight", SqlDbType.VarChar, 40).Value = glass.Hight;
                command.Parameters.Add("@Width", SqlDbType.VarChar, 40).Value = glass.Width;
                command.Parameters.Add("@Length", SqlDbType.VarChar, 40).Value = glass.Length;
                command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = glass.Type;
                command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = glass.Color;
                command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = glass.Owner;
                command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = glass.Desk;

                command.Parameters.Add("@Glass_Id", SqlDbType.VarChar, 40).Value = glass_Id.ToString();

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "You edited glass " + glass_Id;
                string magazinehistory = "Glass " + glass_Id + " has been edited";

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
                        if (ids.Id == id_glasse.ToString())
                        {
                            if (ids.Used == true)
                            {
                                glass.Error_Messege = "Glass has alreasy been used";
                                temp.Add(glass);
                                return temp;
                            }

                            if (ids.Removed == true)
                            {
                                glass.Error_Messege = "Glass has already been deleted";
                                temp.Add(glass);
                                return temp;
                            }

                            string query = "UPDATE dbo.[Glass] SET Removed = @Removed WHERE Glass_Id = @Glass_Id;";
                            SqlCommand command = new SqlCommand(query, connect.cnn);

                            command.Parameters.Add("@Removed", SqlDbType.Bit).Value = true;
                            command.Parameters.Add("@Glass_Id", SqlDbType.VarChar, 40).Value = id_glasse.ToString();

                            connect.cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            connect.cnn.Close();

                            string userhistory = "You deleted glass " + ids.Id;
                            string magazinehistory = "Glass " + ids.Id + " has been deleted";

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
                        if (ids.Id == id_glasse.ToString())
                        {
                            if (ids.Used == true)
                            {
                                glass.Error_Messege = "Glass has alreasy been used";
                                temp.Add(glass);
                                return temp;
                            }

                            if (ids.Removed == false)
                            {
                                glass.Error_Messege = "Glass has already been restored";
                                temp.Add(glass);
                                return temp;
                            }

                            string query = "UPDATE dbo.[Glass] SET Removed = @Removed WHERE Glass_Id = @Glass_Id;";
                            SqlCommand command = new SqlCommand(query, connect.cnn);

                            command.Parameters.Add("@Removed", SqlDbType.Bit).Value = false;
                            command.Parameters.Add("@Glass_Id", SqlDbType.VarChar, 40).Value = id_glasse.ToString();

                            connect.cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            connect.cnn.Close();

                            string userhistory = "You restored glass " + ids.Id;
                            string magazinehistory = "Glass " + ids.Id + " has been restored";

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

            string userhistory = "You added new glass type: " + type;
            string magazinehistory = "Glass type " + type + " has been added";

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

            string userhistory = "You add new glass color: " + color;
            string magazinehistory = "Glass color " + color + " has been added";

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


            string userhistory = "You changed glass type from " + old_type + " to: " + new_type;
            string magazinehistory = "Glass type " + old_type + " has been changed to: " + new_type;

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

            string userhistory = "You changed glass color from " + old_color + " to: " + new_color;
            string magazinehistory = "Glass color " + old_color + " has been changed to: " + new_color;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Magazine_History(magazinehistory, user.Login);

            temp.Add(old_color);
            return temp;
        }
    }
}
