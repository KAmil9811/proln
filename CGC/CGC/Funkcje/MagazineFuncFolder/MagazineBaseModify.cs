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

        public List<Glass> Add_Glass(User user, Glass glass, int code, List<Glass> glasses, string LastGlobalIdGlass)
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

                if (Convert.ToDouble(glass.Width) <= 5000 && Convert.ToDouble(glass.Length) <= 5000 && Convert.ToDouble(glass.Width) >= 200 && Convert.ToDouble(glass.Length) >=200)
                {
                    string query = "INSERT INTO dbo.Glass(Global_id, Hight,Width,Length,Used,Removed,Type,Color,Owner,Desk,Cut_id,Glass_Id, Company) VALUES(@Global_id, @Hight, @Width, @Length, @Used, @Removed, @Type, @Color, @Owner, @Desk, @Cut_id, @code, @Company)";

                    SqlCommand command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalIdGlass;
                    command.Parameters.Add("@Hight", SqlDbType.Float).Value = Convert.ToDouble(glass.Hight);
                    command.Parameters.Add("@Width", SqlDbType.Float).Value = Convert.ToDouble(glass.Width);
                    command.Parameters.Add("@Length", SqlDbType.Float).Value = Convert.ToDouble(glass.Length);
                    command.Parameters.Add("@Used", SqlDbType.Bit).Value = 0;
                    command.Parameters.Add("@Removed", SqlDbType.Bit).Value = 0;
                    command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = glass.Type;
                    command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = glass.Color;
                    command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = glass.Owner;
                    command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = glass.Desk;
                    command.Parameters.Add("@Cut_id", SqlDbType.VarChar, 40).Value = "0";
                    command.Parameters.Add("@code", SqlDbType.VarChar, 40).Value = code.ToString();
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;
                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    string userhistory = "You added glass " + code;
                    string magazinehistory = "Glass " + code + " has been added";

                    insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                    insertHistory.Insert_Magazine_History(magazinehistory, user.Login, user.Company);

                    glass.Glass_info.Add(new Glass_Id { Id = (code).ToString() });

                    //SetOrderStan();
                    glasses.Add(glass);
                    temp.Add(glass);

                    LastGlobalIdGlass = (Convert.ToInt32(LastGlobalIdGlass) + 1).ToString();
                }
                else
                {
                    glass.Error_Messege = "Wrong size";
                    glasses.Add(glass);
                    temp.Add(glass);
                    return temp;
                }
            }
            return temp;
        }

        public List<Glass> Edit_Glass(User user, Glass glass)
        {
            List<Glass> temp = new List<Glass>();

            string query = "UPDATE dbo.[Glass] SET Hight = @Hight, Width = @Width, Length = @Length, Type = @Type, Color = @Color, Owner = @Owner, Desk = @Desk WHERE Glass_Id = @Glass_Id AND Company = @Company;";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Hight", SqlDbType.Float).Value = Convert.ToDouble(glass.Hight);
            command.Parameters.Add("@Width", SqlDbType.Float).Value = Convert.ToDouble(glass.Width);
            command.Parameters.Add("@Length", SqlDbType.Float).Value = Convert.ToDouble(glass.Length);
            command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = glass.Type;
            command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = glass.Color;
            command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = glass.Owner;
            command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = glass.Desk;

            command.Parameters.Add("@Glass_Id", SqlDbType.VarChar, 40).Value = glass.Ids;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You edited glass " + glass.Ids;
            string magazinehistory = "Glass " + glass.Ids + " has been edited";

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Magazine_History(magazinehistory, user.Login, user.Company);                
            
            temp.Add(glass);
            return temp;
        }

        public List<Glass> Remove_Glass(User user, List<int> Id_glasses)
        {
            List<Glass> temp = new List<Glass>();
            Glass glass = new Glass();

            foreach (int id_glasse in Id_glasses)
            {
                try
                {
                    string query = "UPDATE dbo.[Glass] SET Removed = @Removed WHERE Glass_Id = @Glass_Id AND Company = @Company;";
                    SqlCommand command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Removed", SqlDbType.Bit).Value = true;
                    command.Parameters.Add("@Glass_Id", SqlDbType.VarChar, 40).Value = id_glasse.ToString();
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    string userhistory = "You deleted glass " + id_glasse;
                    string magazinehistory = "Glass " + id_glasse + " has been deleted";

                    insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                    insertHistory.Insert_Magazine_History(magazinehistory, user.Login, user.Company);

                    //SetOrderStan();

                    temp.Add(glass);
                }
                catch
                {
                    Glass error = new Glass { Error_Messege = "Id don't exist" };
                    temp.Add(error);
                    return temp;
                }
            }              
            return temp;
        }

        public List<Glass> Restore_Glass(User user, List<int> Id_glasses)
        {
            List<Glass> temp = new List<Glass>();
            Glass glass = new Glass();

            foreach (int id_glasse in Id_glasses)
            {
                try
                {
                    string query = "UPDATE dbo.[Glass] SET Removed = @Removed WHERE Glass_Id = @Glass_Id AND Company = @Company;";
                    SqlCommand command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Removed", SqlDbType.Bit).Value = false;
                    command.Parameters.Add("@Glass_Id", SqlDbType.VarChar, 40).Value = id_glasse.ToString();
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    string userhistory = "You restored glass " + id_glasse;
                    string magazinehistory = "Glass " + id_glasse + " has been restored";

                    insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                    insertHistory.Insert_Magazine_History(magazinehistory, user.Login, user.Company);

                    //SetOrderStan();

                    temp.Add(glass);
                }
                catch
                {
                    Glass error = new Glass {Error_Messege = "Id don't exist" };
                    temp.Add(error);
                    return temp;
                }
            }
            return temp;
        }

        public List<string> Add_Type_Admin(User user, string type, string LastGlobalIdType)
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("INSERT INTO dbo.Glass_type(Global_id, Type, Company) VALUES(@Global_id, @type, @Company)", connect.cnn);

            command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalIdType;
            command.Parameters.Add("@type", SqlDbType.VarChar, 40).Value = type;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You added new glass type: " + type;
            string magazinehistory = "Glass type " + type + " has been added";

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Magazine_History(magazinehistory, user.Login, user.Company);

            temp.Add(type);
            return temp;
        }

        public List<string> Add_Color_Admin(User user, string color, string LastGlobalIdColor)
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("INSERT INTO dbo.Color(Global_id, Color, Company) VALUES(@Global_id, @Color, @Company)", connect.cnn);

            command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalIdColor;
            command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = color;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You add new glass color: " + color;
            string magazinehistory = "Glass color " + color + " has been added";

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Magazine_History(magazinehistory, user.Login, user.Company);

            temp.Add(color);
            return temp;
        }

        public List<string> Change_Type_Admin(User user, string new_type, string old_type)
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("UPDATE dbo.Glass_type SET Type = @new_type WHERE Type = @old_type AND Company = @Company", connect.cnn);

            command.Parameters.Add("@new_type", SqlDbType.VarChar, 40).Value = new_type;
            command.Parameters.Add("@old_type", SqlDbType.VarChar, 40).Value = old_type;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            try
            {
                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();
            }
            catch
            {
                temp.Add("Type don't exist");
                return temp;
            }


            command = new SqlCommand("UPDATE dbo.Glass SET Type = @new_type WHERE Type = @old_type AND Company = @Company", connect.cnn);

            command.Parameters.Add("@new_type", SqlDbType.VarChar, 40).Value = new_type;
            command.Parameters.Add("@old_type", SqlDbType.VarChar, 40).Value = old_type;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();


            string userhistory = "You changed glass type from " + old_type + " to: " + new_type;
            string magazinehistory = "Glass type " + old_type + " has been changed to: " + new_type;

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Magazine_History(magazinehistory, user.Login, user.Company);

            temp.Add(new_type);
            return temp;
        }

        public List<string> Change_Color_Admin(User user, string new_color, string old_color)
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("UPDATE dbo.Color SET Color = @new_color WHERE Color = @old_color AND Company = @Company", connect.cnn);

            command.Parameters.Add("@new_color", SqlDbType.VarChar, 40).Value = new_color;
            command.Parameters.Add("@old_color", SqlDbType.VarChar, 40).Value = old_color;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            try
            {
                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();
            }
            catch
            {
                temp.Add("Color don't exist");
                return temp;
            }

            command = new SqlCommand("UPDATE dbo.Glass SET Color = @new_color WHERE Color = @old_color AND Company = @Company", connect.cnn);

            command.Parameters.Add("@new_color", SqlDbType.VarChar, 40).Value = new_color;
            command.Parameters.Add("@old_color", SqlDbType.VarChar, 40).Value = old_color;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You changed glass color from " + old_color + " to: " + new_color;
            string magazinehistory = "Glass color " + old_color + " has been changed to: " + new_color;

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Magazine_History(magazinehistory, user.Login, user.Company);

            temp.Add(old_color);
            return temp;
        }
    }
}
