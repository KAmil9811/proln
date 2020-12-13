using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CGC.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    public sealed class OrderController : Controller
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
        private static OrderController m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static OrderController Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new OrderController();
                    }
                    return m_oInstance;
                }
            }
        }
        public List<Item> items = new List<Item>
        {

        };
        UsersController usersController = new UsersController();
        MagazineController magazineController = new MagazineController();

        public List<Order> GetOrders()
        {
            List<Order> temp = new List<Order>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Order];", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Order order = new Order();
                order.Id_Order = sqlDataReader["Id_Order"].ToString();
                order.Owner = sqlDataReader["Owner"].ToString();
                order.Status = sqlDataReader["Status"].ToString();
                order.Priority = Convert.ToInt32(sqlDataReader["Priority"]);
                order.Deadline = sqlDataReader["Deadline"].ToString();
                order.Stan = sqlDataReader["Stan"].ToString();
                order.Deletead = Convert.ToBoolean(sqlDataReader["Deletead"]);
                order.Frozen = Convert.ToBoolean(sqlDataReader["Frozen"]);
                order.Released = Convert.ToBoolean(sqlDataReader["Released"]);

                temp.Add(order);
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            return temp;
        }

        public List<Item> GetItems(Order order)
        {
            string help;
            List<Item> temp = new List<Item>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Item]", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                help = sqlDataReader["Order_id"].ToString();
                if (help == order.Id_Order)
                {
                    Item item = new Item();
                    item.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    item.Thickness = Convert.ToDouble(sqlDataReader["Height"]);
                    item.Width = Convert.ToDouble(sqlDataReader["Weight"]);
                    item.Length = Convert.ToDouble(sqlDataReader["Lenght"]);
                    item.Type = sqlDataReader["Glass_Type"].ToString();
                    item.Color = sqlDataReader["Color"].ToString();
                    item.Status = sqlDataReader["Status"].ToString();
                    item.Shape = sqlDataReader["Shape"].ToString();
                    item.Order_id = sqlDataReader["Order_id"].ToString();
                    item.Desk = sqlDataReader["Desk"].ToString();

                    temp.Add(item);
                }
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            return temp;
        }

        public void Insert_Order_History(string Description, string Login)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.[User_History](Date, Login, Description) VALUES(@data, @Login, @Description)";
            SqlCommand command = new SqlCommand(query, cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();
        }
        //kod do czasu bazy danych
        public bool Check_Code(string code)
        {
            foreach (Order order in GetOrders())
            {
                if (code == order.Id_Order)
                {
                    return false;
                }
            }
            return true;
        }

        public void Repeat(string code)
        {
            Random rand = new Random();
            var temp = rand.Next(1, 9000);
            bool check;

            check = Check_Code(code);
            if (check == false)
            {
                temp = rand.Next(1, 9000);
                code = temp.ToString();
                Repeat(code);
            }
        }

        public void Repeat_item(int code, Order order)
        {
            Random rand = new Random();
            code = rand.Next(1, 9000);
            bool check;

            check = Check_Code_item(code, order);
            if (check == false)
            {
                code = rand.Next(1, 9000);
                Repeat_item(code, order);
            }
        }

        public bool Check_Code_item(int code, Order order)
        {
            foreach (Item item in order.items)
            {
                if (code == item.Id)
                {
                    return false;
                }
            }
            return true;
        }

        public int Avaible_Cut(Order order)
        {
            int count = 0;
            foreach (Item item in GetItems(order))
            {
                if  (item.Status == "awaiting")
                {
                    foreach (Glass glass in magazineController.Getglass())
                    {
                        if  (item.Width <= glass.Width && item.Thickness == glass.Hight && item.Length <= glass.Length && glass.Color == item.Color && glass.Type == item.Type)
                        {
                            foreach  (Glass_Id glass_Id in glass.Glass_info)
                            {
                                if  (glass_Id.Used == false && glass_Id.Removed == false && glass_Id.Destroyed == false)
                                {
                                    count++;
                                }
                            }
                        }
                    }
                }
            }
            return count;
        }

        public int Item_To_Cut(Order order)
        {
            int count = 0;
            foreach (Item item in order.items)
            {
                if(item.Status == "awaiting")
                {
                    count++;
                }
            }
            return count;
        }

        public int Item_To_In_Cut(Order order)
        {
            int count = 0;
            foreach (Item item in order.items)
            {
                if (item.Status == "InUse")
                {
                    count++;
                }
            }
            return count;
        }

        [HttpGet("Return_All_Orders")]
        public async Task<List<Order>> Return_All_Orders()
        {
            List<Order> temp = new List<Order>();

            foreach (Order order in GetOrders())
            {
                if (order.Deletead == false && order.Released == false)
                {
                    temp.Add(order);
                }
            }
            return temp;
        }

        [HttpPost("Return_All_Items")]
        public async Task<List<Item>> Return_All_Items([FromBody] Order order)
        {
            List<Item> temp = new List<Item>();
            foreach (Order orderer in GetOrders())
            {
                if (orderer.Id_Order == order.Id_Order)
                {
                    foreach (Item item in GetItems(orderer))
                    {
                        temp.Add(item);
                    }
                }
            }
            return temp;
        }

        [HttpPost("Add_Order")]
        public async Task<List<Order>> Add_Order([FromBody] Receiver receiver)
        {
            List<Order> temp = new List<Order>();

            Order order = receiver.order;
            User user = receiver.user;

            foreach(Item item in receiver.items)
            {
                order.items.Add(item);
            }
            

            Random rand = new Random();
            var temper = rand.Next(1, 9000);
            string code = temper.ToString();
            
            Repeat(code);

            order.Id_Order = code;
            List<Item> itemstemp = new List<Item>();

            foreach (Item item in order.items)
            {             
                if (item.Amount == 1)
                {
                    item.Status = "awaiting";
                    item.Can_Be_Createad = false;

                    int code2 = rand.Next(1, 9000);

                    Repeat_item(code2, order);
                    item.Id = code2;
                    item.Amount = 0;
                }
                else if(item.Amount > 1)
                {
                    item.Status = "awaiting";
                    item.Can_Be_Createad = false;

                    int code2 = rand.Next(1, 9000);

                    Repeat_item(code2, order);
                    item.Id = code2;

                    while (item.Amount > 0)
                    {
                        Item new_item = new Item();
                        new_item.Shape = item.Shape;
                        new_item.Width = item.Width;
                        new_item.Length = item.Length;
                        new_item.Can_Be_Createad = false;
                        new_item.Color = item.Color;
                        new_item.Desk = item.Desk;
                        new_item.Order_id = item.Order_id;
                        new_item.Thickness = item.Thickness;
                        new_item.Type = item.Type;

                        int code3 = rand.Next(1, 9000);

                        Repeat_item(code3, order);
                        new_item.Id = code3;
                        new_item.Amount = 0;
                        itemstemp.Add(new_item);
                        item.Amount--;
                    }
                }
            }

            foreach(Item item1 in itemstemp)
            {
                order.items.Add(item1);
            }

            if (order.Priority.ToString() == null)
            {
                order.Priority = 5;
            }

            
            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    try
                    {
                        string query;
                        SqlCommand command;
                        foreach (Item item in order.items)
                        {
                            query = "INSERT INTO dbo.[Item](Id, Weight, Height, Lenght, Glass_Type, Color, Status,Desk, Order_id) VALUES(@Id, @Weight,@Height, @Lenght, @Glass_Type, @Color, @Status, @Desk, @Order_id)";
                            command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = item.Id;
                            command.Parameters.Add("@Weight", SqlDbType.Float).Value = item.Width;
                            command.Parameters.Add("@Height", SqlDbType.Float).Value = item.Thickness;
                            command.Parameters.Add("@Lenght", SqlDbType.Float).Value = item.Length;
                            command.Parameters.Add("@Glass_Type", SqlDbType.VarChar, 40).Value = item.Type;
                            command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = item.Color;
                            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "awaiting";
                            command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = "";
                            command.Parameters.Add("@Order_id", SqlDbType.VarChar, 40).Value = order.Id_Order;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();
                        }

                        cnn.Open();
                        query = "INSERT INTO dbo.[Order](Id_Order,Owner,Status,Priority,Deadline,Stan,Deletead,Frozen,Released ) VALUES(@Id_Order, @Owner, @Status, @Priority, @Deadline, @Stan, @Deletead, @Frozen, @Released)";
                        command = new SqlCommand(query, cnn);

                        command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;
                        command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = order.Owner;
                        command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "awaiting";
                        command.Parameters.Add("@Priority", SqlDbType.Int).Value = order.Priority;
                        command.Parameters.Add("@Deadline", SqlDbType.VarChar, 40).Value = order.Deadline;
                        command.Parameters.Add("@Stan", SqlDbType.VarChar, 40).Value = Avaible_Cut(order).ToString() + "/" + order.items.Count + "/" + "0";
                        command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = false;
                        command.Parameters.Add("@Frozen", SqlDbType.Bit).Value = false;
                        command.Parameters.Add("@Released", SqlDbType.Bit).Value = false;

                        command.ExecuteNonQuery();
                        command.Dispose();
                        cnn.Close();
                        string userhistory = "You added new order " + order.Id_Order;

                        usersController.Insert_User_History(userhistory, user.Login);
                    }
                    catch(Exception ex)
                    {
                        user.Error_Messege = ex.ToString();
                    }
                }
            }

            string Orderhistory = "Order has been created";
            //Insert_Order_History(Orderhistory,user.Login);

            temp.Add(order);
   
            return temp;
        }

        [HttpPost("Edit_Order")]
        public async Task<List<Order>> Edit_Order([FromBody] Receiver receiver)
        {
            List<Order> temp = new List<Order>(); //breakpoint

            Order order = receiver.order;
            User user = receiver.user;

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (Order ord in GetOrders())
                    {
                        if (ord.Id_Order == order.Id_Order)
                        {
                            try
                            {
                                string query = "UPDATE dbo.[Order] SET Deadline = @Deadline, Owner = @Owner, Priority = @Priority WHERE Id_Order = @Id_Order;";
                                SqlCommand command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Deadline", SqlDbType.VarChar, 40).Value = order.Deadline;
                                command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = order.Owner;
                                command.Parameters.Add("@Priority", SqlDbType.Decimal).Value = order.Priority;

                                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();
                            }
                            catch(SqlException e)
                            {
                                Console.WriteLine(e);
                            }

                            //ord.order_Histories.Add(new Order_History { Data = DateTime.Today.ToString("d"), Login = usere.Login, Description = "order has been edited" });
                            //usere.user_history.Add(new User_History { Data = DateTime.Today.ToString("d"), Description = "User edit " + ord.Id_Order });
                        }
                    }
                }
            }
            order.Error_Messege = "User_not_found";
            temp.Add(order);
            return temp;
        }

        [HttpPost("Edit_Order_Items")]
        public async Task<List<Order>> Edit_Order_Items([FromBody] Receiver receiver)
        {
            List<Order> temp = new List<Order>(); //breakpoint
     
            Order order = receiver.order;
            User user = receiver.user;
            Item items = receiver.item;

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (Item item in GetItems(order))
                    {
                        if(item.Id == items.Id)
                        {
                            try
                            {
                                string query = "UPDATE dbo.[Item] SET Height = @Height, Lenght = @Lenght, Weight = @Weight, Glass_Type = @Glass_Type, Color = @Color, Status = @Status, Desk = @Desk WHERE Id = @Id;";
                                SqlCommand command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Height", SqlDbType.Decimal).Value = items.Thickness;
                                command.Parameters.Add("@Lenght", SqlDbType.Decimal).Value = items.Length;
                                command.Parameters.Add("@Weight", SqlDbType.Decimal).Value = items.Width;
                                command.Parameters.Add("@Glass_Type", SqlDbType.VarChar, 40).Value = items.Type;
                                command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = items.Color;
                                command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = items.Status;
                                command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = items.Desk;

                                command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = items.Id;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();

                                //ord.order_Histories.Add(new Order_History { Data = DateTime.Today.ToString("d"), Login = usere.Login, Description = "order has been edited" });
                                //usere.user_history.Add(new User_History { Data = DateTime.Today.ToString("d"), Description = "User edit " + ord.Id_Order });
                            }
                            catch (SqlException e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                    }
                }
            }
            order.Error_Messege = "User_not_found";
            temp.Add(order);
            return temp;
        }

        [HttpPost("Set_Stan")]
        public async Task<List<Order>> Set_Stan([FromBody] Receiver receiver, string name)
        {
            List<Order> temp = new List<Order>();

            Order order = receiver.order;
            User user = receiver.user;

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (Order ord in GetOrders())
                    {
                        if (ord.Id_Order == ord.Id_Order)
                        {
                            if (name == "deletead" && ord.Deletead == false && ord.Released == false)
                            {
                                ord.Deletead = true;
                                ord.Frozen = false;

                                ord.order_Histories.Add(new Order_History { Data = DateTime.Today.ToString("d"), Login = usere.Login, Description = "Order has been removed" });
                                usere.user_history.Add(new User_History { Data = DateTime.Today.ToString("d"), Description = "You deleted order " + ord.Id_Order });

                                temp.Add(order);
                                return temp;
                            }
                            else if (name == "deletead" && ord.Deletead == true && ord.Released == false && ord.Frozen == false)
                            {
                                ord.Deletead = false;

                                ord.order_Histories.Add(new Order_History { Data = DateTime.Today.ToString("d"), Login = usere.Login, Description = "Order has been restored" });
                                usere.user_history.Add(new User_History { Data = DateTime.Today.ToString("d"), Description = "You restored order " + ord.Id_Order });

                                temp.Add(order);
                                return temp;
                            }
                            if (name == "frozen" && ord.Deletead == false && ord.Released == false && ord.Frozen == false)
                            {
                                ord.Frozen = true;

                                ord.order_Histories.Add(new Order_History { Data = DateTime.Today.ToString("d"), Login = usere.Login, Description = "Order had been frozened" });
                                usere.user_history.Add(new User_History { Data = DateTime.Today.ToString("d"), Description = "You frozened order " + ord.Id_Order });

                                temp.Add(order);
                                return temp;
                            }
                            else if (name == "frozen" && ord.Deletead == false && ord.Released == false && ord.Frozen == true)
                            {
                                ord.Frozen = false;

                                ord.order_Histories.Add(new Order_History { Data = DateTime.Today.ToString("d"), Login = usere.Login, Description = "Order has been unfreeze" });
                                usere.user_history.Add(new User_History { Data = DateTime.Today.ToString("d"), Description = "You unfreeze order " + ord.Id_Order });

                                temp.Add(order);
                                return temp;
                            }
                            if (name == "released" && ord.Status == "done" && ord.Deletead == false && ord.Released == false)
                            {
                                ord.Released = true;

                                ord.order_Histories.Add(new Order_History { Data = DateTime.Today.ToString("d"), Login = usere.Login, Description = "Order has been released" });
                                usere.user_history.Add(new User_History { Data = DateTime.Today.ToString("d"), Description = "You released order " + ord.Id_Order });

                                temp.Add(order);
                                return temp;
                            }
                            else if (name == "released" && ord.Deletead == false && ord.Released == true)
                            {
                                ord.Released = false;

                                ord.order_Histories.Add(new Order_History { Data = DateTime.Today.ToString("d"), Login = usere.Login, Description = "Order has been unreleased" });
                                usere.user_history.Add(new User_History { Data = DateTime.Today.ToString("d"), Description = "You unreleased order " + ord.Id_Order });

                                temp.Add(order);
                                return temp;
                            }
                        }
                    }
                }
            }

            order.Error_Messege = "User_not_found";
            temp.Add(order);
            return temp;
        }
    }
}