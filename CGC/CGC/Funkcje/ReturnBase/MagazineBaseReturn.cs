using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.MagazineFuncFolder.MagazineBase
{
    public class MagazineBaseReturn
    {
        private Connect connect = new Connect();

        private static MagazineBaseReturn m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static MagazineBaseReturn Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new MagazineBaseReturn();
                    }
                    return m_oInstance;
                }
            }
        }

        public List<Glass> Getglass()
        {
            List<Glass_Receiver> temp2 = new List<Glass_Receiver>();
            List<Glass> temp = new List<Glass>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Glass];", connect.cnn);
            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Glass_Receiver glass_Receiver = new Glass_Receiver();
                glass_Receiver.Hight = Convert.ToDouble(sqlDataReader["Hight"]);
                glass_Receiver.Width = Convert.ToDouble(sqlDataReader["Width"]);
                glass_Receiver.Length = Convert.ToDouble(sqlDataReader["Length"]);
                glass_Receiver.Used = Convert.ToBoolean(sqlDataReader["Used"]);
                glass_Receiver.Removed = Convert.ToBoolean(sqlDataReader["Removed"]);
                glass_Receiver.Type = sqlDataReader["Type"].ToString();
                glass_Receiver.Color = sqlDataReader["Color"].ToString();
                glass_Receiver.Owner = sqlDataReader["Owner"].ToString();
                glass_Receiver.Desk = sqlDataReader["Desk"].ToString();
                glass_Receiver.Glass_Id = sqlDataReader["Glass_Id"].ToString();

                if (sqlDataReader["Cut_id"].ToString() == "")
                {
                    glass_Receiver.Cut_id = "0";
                }
                else
                {
                    glass_Receiver.Cut_id = sqlDataReader["Cut_id"].ToString();
                }

                temp2.Add(glass_Receiver);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            foreach (Glass_Receiver glass_Receiver in temp2)
            {
                bool check = false;
                foreach (Glass glass in temp)
                {
                    if (Convert.ToDouble(glass.Width) == glass_Receiver.Width && Convert.ToDouble(glass.Hight) == glass_Receiver.Hight && Convert.ToDouble(glass.Length) == glass_Receiver.Length && glass.Type == glass_Receiver.Type && glass.Color == glass_Receiver.Color && glass.Owner == glass_Receiver.Owner)
                    {
                        glass.Count = (Convert.ToInt32(glass.Count) + 1).ToString();
                        glass.Glass_info.Add(new Glass_Id { Id = glass_Receiver.Glass_Id, Removed = glass_Receiver.Removed, Used = glass_Receiver.Used, Cut_id = glass_Receiver.Cut_id });
                        check = true;
                    }
                }

                if (check == false)
                {
                    Glass newGlass = new Glass { Glass_info = new List<Glass_Id>()};

                    newGlass.Width = glass_Receiver.Width.ToString();
                    newGlass.Hight = glass_Receiver.Hight.ToString();
                    newGlass.Length = glass_Receiver.Length.ToString();
                    newGlass.Type = glass_Receiver.Type;
                    newGlass.Color = glass_Receiver.Color;
                    newGlass.Owner = glass_Receiver.Owner;
                    newGlass.Desk = glass_Receiver.Desk;
                    newGlass.Glass_info.Add(new Glass_Id { Id = glass_Receiver.Glass_Id, Removed = glass_Receiver.Removed, Used = glass_Receiver.Used, Cut_id = glass_Receiver.Cut_id });
                    newGlass.Count = "1";
                    temp.Add(newGlass);
                }
            }
            return temp;
        }

        public List<Glass> Getglass(Glass glasss)
        {
            List<Glass_Receiver> temp2 = new List<Glass_Receiver>();
            List<Glass> temp = new List<Glass>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Glass] WHERE Width >= @Width AND Length >= @Length AND Used = @Used AND Removed = @Removed AND Cut_id = @Cut_id AND Hight = @Hight AND Type = @Type AND Color = @Color AND (Owner = @Owner OR Owner = @Owner2);", connect.cnn);

            command.Parameters.Add("@Hight", SqlDbType.Float).Value = Convert.ToDouble(glasss.Hight);
            command.Parameters.Add("@Width", SqlDbType.Float).Value = Convert.ToDouble(glasss.Width);
            command.Parameters.Add("@Length", SqlDbType.Float).Value = Convert.ToDouble(glasss.Length);
            command.Parameters.Add("@Used", SqlDbType.Bit).Value = false;
            command.Parameters.Add("@Removed", SqlDbType.Bit).Value = false;
            command.Parameters.Add("@Cut_id", SqlDbType.VarChar, 40).Value = "0";
            command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = glasss.Type;
            command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = glasss.Color;
            command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = glasss.Owner;
            command.Parameters.Add("@Owner2", SqlDbType.VarChar, 40).Value = "";

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Glass_Receiver glass_Receiver = new Glass_Receiver();
                glass_Receiver.Hight = Convert.ToDouble(sqlDataReader["Hight"]);
                glass_Receiver.Width = Convert.ToDouble(sqlDataReader["Width"]);
                glass_Receiver.Length = Convert.ToDouble(sqlDataReader["Length"]);
                glass_Receiver.Used = Convert.ToBoolean(sqlDataReader["Used"]);
                glass_Receiver.Removed = Convert.ToBoolean(sqlDataReader["Removed"]);
                glass_Receiver.Type = sqlDataReader["Type"].ToString();
                glass_Receiver.Color = sqlDataReader["Color"].ToString();
                glass_Receiver.Owner = sqlDataReader["Owner"].ToString();
                glass_Receiver.Desk = sqlDataReader["Desk"].ToString();
                glass_Receiver.Glass_Id = sqlDataReader["Glass_Id"].ToString();

                if (sqlDataReader["Cut_id"].ToString() == "")
                {
                    glass_Receiver.Cut_id = "0";
                }
                else
                {
                    glass_Receiver.Cut_id = sqlDataReader["Cut_id"].ToString();
                }

                temp2.Add(glass_Receiver);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            foreach (Glass_Receiver glass_Receiver in temp2)
            {
                bool check = false;
                foreach (Glass glass in temp)
                {
                    if (Convert.ToDouble(glass.Width) == glass_Receiver.Width && Convert.ToDouble(glass.Hight) == glass_Receiver.Hight && Convert.ToDouble(glass.Length) == glass_Receiver.Length && glass.Type == glass_Receiver.Type && glass.Color == glass_Receiver.Color && glass.Owner == glass_Receiver.Owner)
                    {
                        glass.Count = (Convert.ToInt32(glass.Count) + 1).ToString();
                        glass.Glass_info.Add(new Glass_Id { Id = glass_Receiver.Glass_Id, Removed = glass_Receiver.Removed, Used = glass_Receiver.Used, Cut_id = glass_Receiver.Cut_id });
                        check = true;
                    }
                }

                if (check == false)
                {
                    Glass newGlass = new Glass { Glass_info = new List<Glass_Id>() };

                    newGlass.Width = glass_Receiver.Width.ToString();
                    newGlass.Hight = glass_Receiver.Hight.ToString();
                    newGlass.Length = glass_Receiver.Length.ToString();
                    newGlass.WidthSort = glass_Receiver.Width;
                    newGlass.LengthSort = glass_Receiver.Length;
                    newGlass.Type = glass_Receiver.Type;
                    newGlass.Color = glass_Receiver.Color;
                    newGlass.Owner = glass_Receiver.Owner;
                    newGlass.Desk = glass_Receiver.Desk;
                    newGlass.Glass_info.Add(new Glass_Id { Id = glass_Receiver.Glass_Id, Removed = glass_Receiver.Removed, Used = glass_Receiver.Used, Cut_id = glass_Receiver.Cut_id });
                    newGlass.Count = "1";
                    temp.Add(newGlass);
                }
            }
            return temp;
        }

        public List<Glass_Receiver> Getglass(bool used, bool removed)
        {
            List<Glass_Receiver> temp = new List<Glass_Receiver>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Glass] WHERE Used = @Used and Removed = @Removed;", connect.cnn);

            command.Parameters.Add("@Used", SqlDbType.Bit).Value = used;
            command.Parameters.Add("@Removed", SqlDbType.Bit).Value = removed;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Glass_Receiver glass_Receiver = new Glass_Receiver();
                glass_Receiver.Hight = Convert.ToDouble(sqlDataReader["Hight"]);
                glass_Receiver.Width = Convert.ToDouble(sqlDataReader["Width"]);
                glass_Receiver.Length = Convert.ToDouble(sqlDataReader["Length"]);
                glass_Receiver.Used = Convert.ToBoolean(sqlDataReader["Used"]);
                glass_Receiver.Removed = Convert.ToBoolean(sqlDataReader["Removed"]);
                glass_Receiver.Type = sqlDataReader["Type"].ToString();
                glass_Receiver.Color = sqlDataReader["Color"].ToString();
                glass_Receiver.Owner = sqlDataReader["Owner"].ToString();
                glass_Receiver.Desk = sqlDataReader["Desk"].ToString();
                glass_Receiver.Glass_Id = sqlDataReader["Glass_Id"].ToString();

                if (sqlDataReader["Cut_id"].ToString() == "")
                {
                    glass_Receiver.Cut_id = "0";
                }
                else
                {
                    glass_Receiver.Cut_id = sqlDataReader["Cut_id"].ToString();
                }

                temp.Add(glass_Receiver);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<string> GetColor()
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Color];", connect.cnn);
            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string color = sqlDataReader["Color"].ToString();
                temp.Add(color);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<string> GetTypes()
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Glass_type];", connect.cnn);
            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string type = sqlDataReader["Type"].ToString();
                temp.Add(type);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<Magazine_History> GetMagazineHistories()
        {
            List<Magazine_History> magazine_Histories = new List<Magazine_History>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Magazine_History];", connect.cnn);
            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Magazine_History magazine_History = new Magazine_History();
                magazine_History.Login = sqlDataReader["Login"].ToString();
                magazine_History.Date = sqlDataReader["Data"].ToString();
                magazine_History.Description = sqlDataReader["Description"].ToString();

                magazine_Histories.Add(magazine_History);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return magazine_Histories;
        }

    }
}
