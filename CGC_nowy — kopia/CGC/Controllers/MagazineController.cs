using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CGC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using MySql.Data.MySqlClient;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    public sealed class MagazineController : Controller
    {
        static MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        {
            Server = "projekt-inz.database.windows.net",
            Database = "projekt-inz",
            UserID = "Michal",
            Password = "lemES98naw141",
            //SslMode = MySqlSslMode.Required,
        };

        SqlConnection cnn = new SqlConnection(builder.ConnectionString);
        private static MagazineController m_oInstance = null;
        private static readonly object m_oPadLock = new object();
        public static MagazineController Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new MagazineController();
                    }
                    return m_oInstance;
                }
            }
        }

        //OrderController orderController = new OrderController();

        public List<Glass> Getglass()
        {
            List<Glass_Receiver> temp2 = new List<Glass_Receiver>();
            List<Glass> temp = new List<Glass>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Glass];", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Glass_Receiver glass_Receiver = new Glass_Receiver();
                glass_Receiver.Hight = Convert.ToDouble(sqlDataReader["Hight"]);
                glass_Receiver.Width = Convert.ToDouble(sqlDataReader["Width"]);
                glass_Receiver.Length = Convert.ToDouble(sqlDataReader["Length"]);
                glass_Receiver.Used = Convert.ToBoolean(sqlDataReader["Used"]);
                glass_Receiver.Destroyed = Convert.ToBoolean(sqlDataReader["Destroyed"]);
                glass_Receiver.Removed = Convert.ToBoolean(sqlDataReader["Removed"]);
                glass_Receiver.Type = sqlDataReader["Type"].ToString();
                glass_Receiver.Color = sqlDataReader["Color"].ToString();
                glass_Receiver.Owner = sqlDataReader["Owner"].ToString();
                glass_Receiver.Desk = sqlDataReader["Desk"].ToString();
                glass_Receiver.Glass_Id = Convert.ToInt32(sqlDataReader["Glass_Id"]);

                temp2.Add(glass_Receiver);
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            foreach (Glass_Receiver glass_Receiver in temp2)
            {
                bool check = false;
                foreach (Glass glass in temp)
                {
                    if (glass.Width == glass_Receiver.Width && glass.Hight == glass_Receiver.Hight && glass.Length == glass_Receiver.Length && glass.Type == glass_Receiver.Type && glass.Color == glass_Receiver.Color && glass.Owner == glass_Receiver.Owner)
                    {
                        glass.Count = glass.Count + 1;
                        glass.Glass_info.Add(new Glass_Id { Id = glass_Receiver.Glass_Id, Destroyed = glass_Receiver.Destroyed, Removed = glass_Receiver.Removed, Used = glass_Receiver.Used });
                        check = true;
                    }
                }

                if (check == false)
                {
                    Glass newGlass = new Glass();

                    newGlass.Width = glass_Receiver.Width;
                    newGlass.Hight = glass_Receiver.Hight;
                    newGlass.Length = glass_Receiver.Length;
                    newGlass.Type = glass_Receiver.Type;
                    newGlass.Color = glass_Receiver.Color;
                    newGlass.Owner = glass_Receiver.Owner;
                    newGlass.Desk = glass_Receiver.Desk;
                    newGlass.Glass_info.Add(new Glass_Id { Id = glass_Receiver.Glass_Id, Destroyed = glass_Receiver.Destroyed, Removed = glass_Receiver.Removed, Used = glass_Receiver.Used });
                    newGlass.Count = 1;
                    temp.Add(newGlass);
                }
            }

            return temp;
        }
        
        public List<string> GetColor()
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Color];", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string color = sqlDataReader["Color"].ToString();
                temp.Add(color);
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();
            return temp;
        }

        public List<string> GetTypes()
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Glass_type];", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string type = sqlDataReader["Type"].ToString();
                temp.Add(type);
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();
            return temp;
        }

        //public void SetOrderStan()
        //{
        //    int count_x;
        //    int count_y;
        //    int count_z;

        //    foreach (Order order in orderController.GetOrders())
        //    {
        //        string stan = "";
        //        count_y = orderController.Item_To_Cut(order);
        //        count_x = orderController.Avaible_Cut(order);
        //        count_x = count_y - count_x;
        //        count_z = orderController.Item_To_In_Cut(order);

        //        stan = count_x + "/" + count_y + "/" + count_z;

        //        string query = "UPDATE dbo.[Order] SET Stan = @stan WHERE Id_Order = @Id_Order";
        //        SqlCommand command = new SqlCommand(query, cnn);
        //        command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;
        //        command.Parameters.Add("@Stan", SqlDbType.VarChar, 40).Value = stan;

        //    }
        //}

        public void Insert_Magazine_History(string Description, string Login)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.Magazine_History(Data, Login, Description) VALUES(@data, @Login, @Description)";

            SqlCommand command = new SqlCommand(query, cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();
        }

        public List<Glass> Set_Filter(List<Glass> temporary)
        {
            List<Glass> temp = new List<Glass>();

            foreach (Glass glass in temporary)
            {
                List<Glass_Id> temp2 = new List<Glass_Id>();
                foreach (Glass_Id glass_Id in glass.Glass_info)
                {
                    if (glass_Id.Destroyed == false && glass_Id.Used == false && glass_Id.Removed == false)
                    {
                        temp2.Add(glass_Id);
                    }
                }
                if (temp2.Count != 0)
                {
                    glass.Glass_info = temp2;

                    temp.Add(glass);
                }
            }

            foreach (Glass tmp in temp)
            {
                foreach (Glass_Id glass_Id in tmp.Glass_info)
                {
                    tmp.Glass_id.Add(glass_Id.Id);
                }
            }

            return temp;
        }

        public List<Magazine_History> GetMagazineHistories()
        {
            List<Magazine_History> magazine_Histories = new List<Magazine_History>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Magazine_History];", cnn);
            cnn.Open();

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
            cnn.Close();

            return magazine_Histories;
        }

        public UsersController usersController = new UsersController();
        
        [HttpGet("Return_All_Colors")]
        public async Task<List<string>> Return_All_Colors()
        {
            return GetColor();
        }

        [HttpGet("Return_All_Glass")]
        public async Task<List<Glass>> Return_All_Glass()
        {
            List<Glass> temp = Set_Filter(Getglass());

            return temp;
        }

        [HttpGet("Return_All_Type")]
        public async Task<List<string>> Return_All_Type()
        {
            return GetTypes();
        }

        [HttpGet("Return_Magazine_History")]
        public async Task<List<Magazine_History>> Return_Magazine_History()
        {
            return GetMagazineHistories();
        }

        [HttpPost("Add_Glass")]
        public async Task<List<Glass>> Add_Glass([FromBody] Receiver receiver)
        {
            List<Glass> temp = new List<Glass>();

            Glass glass = receiver.glass;
            User user = receiver.user;
            int code;

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    for (int i = glass.Count; i > 0; i--)
                    {
                        if(Getglass().Last() != null)
                        {
                            code = Getglass().Last().Glass_info.Last().Id + 1;
                        }
                        else
                        {
                            code = 1;
                        }

                        string query = "INSERT INTO dbo.Glass(Hight,Width,Length,Used,Destroyed,Removed,Type,Color,Owner,Desk,Glass_Id) VALUES(@Hight, @Width, @Length, @Used, @Destroyed, @Removed, @Type, @Color, @Owner, @Desk, @code)";

                        SqlCommand command = new SqlCommand(query, cnn);
                        command.Parameters.Add("@Hight", SqlDbType.Decimal).Value = glass.Hight;
                        command.Parameters.Add("@Width", SqlDbType.Decimal).Value = glass.Width;
                        command.Parameters.Add("@Length", SqlDbType.Decimal).Value = glass.Length;
                        command.Parameters.Add("@Used", SqlDbType.Bit).Value = 0;
                        command.Parameters.Add("@Destroyed", SqlDbType.Bit).Value = 0;
                        command.Parameters.Add("@Removed", SqlDbType.Bit).Value = 0;
                        command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = glass.Type;
                        command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = glass.Color;
                        command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = glass.Owner;
                        command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = glass.Desk;
                        command.Parameters.Add("@code", SqlDbType.Int).Value = code;
                        cnn.Open();
                        command.ExecuteNonQuery();
                        command.Dispose();
                        cnn.Close();

                        string userhistory = "You added glass " + code;
                        string magazinehistory = code + " has been added";

                        usersController.Insert_User_History(userhistory, user.Login);
                        Insert_Magazine_History(magazinehistory, user.Login);

                        //SetOrderStan();

                        temp.Add(glass);
                    }
                    return temp;
                }
            }
            glass.Error_Messege = "User_no_exist";
            temp.Add(glass);
            return temp;
        }

        [HttpPost("Edit_Glass")]
        public async Task<List<Glass>> Edit_Glass([FromBody] Receiver receiver)
        {
            List<Glass> temp = new List<Glass>();
            User user = receiver.user;
            Glass glass = receiver.glass;
            glass.Glass_id = receiver.glass.Glass_id;

            foreach(User usere in usersController.GetUsers())
            {
                if(user.Login == usere.Login)
                {
                    foreach (int glass_Id in glass.Glass_id)
                    {
                        string query = "UPDATE dbo.[Glass] SET Hight = @Hight, Width = @Width, Length = @Length, Type = @Type, Color = @Color, Owner = @Owner, Desk = @Desk WHERE Glass_Id = @Glass_Id;";
                        SqlCommand command = new SqlCommand(query, cnn);

                        command.Parameters.Add("@Hight", SqlDbType.Decimal).Value = glass.Hight;
                        command.Parameters.Add("@Width", SqlDbType.Decimal).Value = glass.Width;
                        command.Parameters.Add("@Length", SqlDbType.Decimal).Value = glass.Length;
                        command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = glass.Type;
                        command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = glass.Color;
                        command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = glass.Owner;
                        command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = glass.Desk;

                        command.Parameters.Add("@Glass_Id", SqlDbType.Int).Value = glass_Id;

                        cnn.Open();
                        command.ExecuteNonQuery();
                        command.Dispose();
                        cnn.Close();

                        string userhistory = "You edited glass " + glass_Id;
                        string magazinehistory = glass_Id + " has been edited";

                        usersController.Insert_User_History(userhistory, user.Login);
                        Insert_Magazine_History(magazinehistory, user.Login);
                    }
                    return temp;
                }
            }
            glass.Error_Messege = "Zły login";
            temp.Add(glass);

            return temp;
        }

        [HttpPost("Remove_Glass")]
        public async Task<List<Glass>> Remove_Glass([FromBody] Receiver receiver)
        {
            List<Glass> temp = new List<Glass>();

            Glass glass = new Glass();            
            
            User user = receiver.user;
            string help = receiver.id_glass;
            int id_glass = Convert.ToInt32(help);

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (Glass glasse in Getglass())
                    {
                        foreach (Glass_Id ids in glasse.Glass_info)
                        {
                            if (ids.Id == id_glass)
                            {
                                if (ids.Used == true)
                                {
                                    glass.Error_Messege = "Glass_already_Used";
                                    temp.Add(glass);
                                    return temp;
                                }

                                if (ids.Removed == true)
                                {
                                    glass.Error_Messege = "Glass_already_deleated";
                                    temp.Add(glass);
                                    return temp;
                                }

                                string query = "UPDATE dbo.[Glass] SET Removed = @Removed WHERE Glass_Id = @Glass_Id;";
                                SqlCommand command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Removed", SqlDbType.Bit).Value = true;
                                command.Parameters.Add("@Glass_Id", SqlDbType.Int).Value = id_glass;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();

                                string userhistory = "You deleated glass " + ids.Id;
                                string magazinehistory = ids.Id + " has been deleated";

                                usersController.Insert_User_History(userhistory, user.Login);
                                Insert_Magazine_History(magazinehistory, user.Login);

                                //SetOrderStan();

                                temp.Add(glass);
                                return temp;
                            }
                        }
                    }
                    glass.Error_Messege = "Glass_Id_no_exist";
                    temp.Add(glass);
                    return temp;
                }
            }
            glass.Error_Messege = "User_no_exist";
            temp.Add(glass);
            return temp;
        }

        [HttpPost("Restore_Glass")]
        public async Task<List<Glass>> Restore_Glass([FromBody] Receiver receiver)
        {
            List<Glass> temp = new List<Glass>();

            Glass glass = receiver.glass;
            User user = receiver.user;
            string help = receiver.id_glass;
            int id_glass = Convert.ToInt32(help);

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (Glass glasse in Getglass())
                    {
                        foreach (Glass_Id ids in glasse.Glass_info)
                        {
                            if (ids.Id == id_glass)
                            {
                                if (ids.Used == true)
                                {
                                    glass.Error_Messege = "Glass_already_Used";
                                    temp.Add(glass);
                                    return temp;
                                }

                                if (ids.Removed == false)
                                {
                                    glass.Error_Messege = "Glass_already_Restored";
                                    temp.Add(glass);
                                    return temp;
                                }

                                string query = "UPDATE dbo.[Glass] SET Removed = @Removed WHERE Glass_Id = @Glass_Id;";
                                SqlCommand command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Removed", SqlDbType.Bit).Value = false;
                                command.Parameters.Add("@Glass_Id", SqlDbType.Int).Value = id_glass;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();

                                string userhistory = "You restored glass " + ids.Id;
                                string magazinehistory = ids.Id + " has been restored";

                                usersController.Insert_User_History(userhistory, user.Login);
                                Insert_Magazine_History(magazinehistory, user.Login);

                                //SetOrderStan();

                                temp.Add(glass);
                                return temp;
                            }
                        }
                    }
                    glass.Error_Messege = "Glass_Id_no_exist";
                    temp.Add(glass);
                    return temp;
                }
            }
            glass.Error_Messege = "User_no_exist";
            temp.Add(glass);
            return temp;
        }

        [HttpPost("Add_type_Admin")]
        public async Task<List<string>> Add_Type_Admin([FromBody] Receiver receiver)
        {
            User user = receiver.user;
            string type = receiver.type;
            List<string> temp = new List<string>();

            foreach (User usere in usersController.GetUsers())
            {
                if (user.Login == usere.Login)
                {
                    foreach (string types in GetTypes())
                    {
                        if (types == type)
                        {
                            temp.Add("Type_alredy_exist");
                            return temp;
                        }
                    }

                    SqlCommand command = new SqlCommand("INSERT INTO dbo.Glass_type(Type) VALUES(@type)", cnn);
                    command.Parameters.Add("@type", SqlDbType.VarChar, 40).Value = type;

                    cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    cnn.Close();

                    string userhistory = "You added new type " + type;
                    string magazinehistory = type + " has been added";

                    usersController.Insert_User_History(userhistory, user.Login);
                    Insert_Magazine_History(magazinehistory, user.Login);

                    temp.Add(type);
                    return temp;
                }

            }
            temp.Add("Admin_no_exist");
            return temp;
        }

        [HttpPost("Add_Color_Admin")]
        public async Task<List<string>> Add_Color_Admin([FromBody] Receiver receiver)
        {
            User user = receiver.user;
            List<string> temp = new List<string>();
            string color = receiver.color;

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (string colors in GetColor())
                    {
                        if (colors == color)
                        {
                            temp.Add("Color_alredy_exist");
                            return temp;
                        }
                    }

                    SqlCommand command = new SqlCommand("INSERT INTO dbo.Color(Color) VALUES(@Color)", cnn);

                    command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = color;

                    cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    cnn.Close();

                    string userhistory = "You added new color " + color;
                    string magazinehistory = color + " has been added";

                    usersController.Insert_User_History(userhistory, user.Login);
                    Insert_Magazine_History(magazinehistory, user.Login);

                    temp.Add("color");
                    return temp;
                }
            }
            temp.Add("Admin_no_exist");
            return temp;
        }

        [HttpPost("Change_type_Admin")]
        public async Task<List<string>> Change_type_Admin([FromBody] Receiver receiver)
        {
            User user = receiver.user;
            string new_type = receiver.new_type;
            string old_type = receiver.old_type;
            List<string> temp = new List<string>();

            foreach (string type in GetTypes())
            {
                if (type == new_type)
                {
                    temp.Add("New_type_already_exist");
                    return temp;
                }
            }

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (string type in GetTypes())
                    {
                        if (type == old_type)
                        {
                            SqlCommand command = new SqlCommand("UPDATE dbo.Glass_type SET Type = @new_type WHERE Type = @old_type", cnn);

                            command.Parameters.Add("@new_type", SqlDbType.VarChar, 40).Value = new_type;
                            command.Parameters.Add("@old_type", SqlDbType.VarChar, 40).Value = old_type;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();

                            foreach (Glass glass in Getglass())
                            {
                                if (glass.Type == old_type)
                                {
                                    command = new SqlCommand("UPDATE dbo.Glass SET Type = @new_type WHERE Type = @old_type", cnn);

                                    command.Parameters.Add("@new_type", SqlDbType.VarChar, 40).Value = new_type;
                                    command.Parameters.Add("@old_type", SqlDbType.VarChar, 40).Value = old_type;

                                    cnn.Open();
                                    command.ExecuteNonQuery();
                                    command.Dispose();
                                    cnn.Close();
                                }
                            }

                            string userhistory = "You changed type " + old_type + " to " + new_type;
                            string magazinehistory = old_type + " has been changed to " + new_type;

                            usersController.Insert_User_History(userhistory, user.Login);
                            Insert_Magazine_History(magazinehistory, user.Login);

                            temp.Add(new_type);
                            return temp;
                        }
                    }
                    temp.Add("Type_dont_exist");
                    return temp;
                }
            }
            temp.Add("Admin_no_exist");
            return temp;
        }

        [HttpPost("Change_Color_Admin")]
        public async Task<List<string>> Change_Color_Admin([FromBody] Receiver receiver)
        {
            User user = receiver.user;
            List<string> temp = new List<string>();
            string new_color = receiver.new_color;
            string old_color = receiver.old_color;

            foreach (string type in GetColor())
            {
                if (type == new_color)
                {
                    temp.Add("New_Color_already_exist");
                    return temp;
                }
            }

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (string color in GetColor())
                    {
                        if (color == old_color)
                        {
                            SqlCommand command = new SqlCommand("UPDATE dbo.Color SET Color = @new_color WHERE Color = @old_color", cnn);

                            command.Parameters.Add("@new_color", SqlDbType.VarChar, 40).Value = new_color;
                            command.Parameters.Add("@old_color", SqlDbType.VarChar, 40).Value = old_color;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();

                            foreach (Glass glass in Getglass())
                            {
                                if (glass.Color == old_color)
                                {
                                    command = new SqlCommand("UPDATE dbo.Glass SET Color = @new_color WHERE Color = @old_color", cnn);

                                    command.Parameters.Add("@new_color", SqlDbType.VarChar, 40).Value = new_color;
                                    command.Parameters.Add("@old_color", SqlDbType.VarChar, 40).Value = old_color;

                                    cnn.Open();
                                    command.ExecuteNonQuery();
                                    command.Dispose();
                                    cnn.Close();
                                }
                            }
                            string userhistory = "You changed " + old_color + " to " + new_color;
                            string magazinehistory = old_color + " has been change on " + new_color;

                            usersController.Insert_User_History(userhistory, user.Login);
                            Insert_Magazine_History(magazinehistory, user.Login);

                            temp.Add(old_color);
                            return temp;
                        }
                    }
                    temp.Add("Color_dont_exist");
                    return temp;
                }
            }
            temp.Add("Admin_no_exist");
            return temp;
        }
    }
}