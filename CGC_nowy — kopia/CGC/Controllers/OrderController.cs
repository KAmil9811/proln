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

                    if (sqlDataReader["Cut_id"].ToString() == "")
                    {
                        item.Cut_id = 0;
                    }
                    else
                    {
                        item.Cut_id = Convert.ToInt32(sqlDataReader["Cut_id"]);
                    }
                    if (sqlDataReader["Product_Id"].ToString() == "")
                    {
                        item.Product_Id = 0;
                    }
                    else
                    {
                        item.Product_Id = Convert.ToInt32(sqlDataReader["Product_Id"]);
                    }

                    temp.Add(item);
                }
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            return temp;
        }

        public List<Item> GetAllItems()
        {
            List<Item> temp = new List<Item>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Item]", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
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

                if (sqlDataReader["Cut_id"].ToString() == "")
                {
                    item.Cut_id = 0;
                }
                else
                {
                    item.Cut_id = Convert.ToInt32(sqlDataReader["Cut_id"]);
                }

                if (sqlDataReader["Product_Id"].ToString() == "")
                {
                    item.Product_Id = 0;
                }
                else
                {
                    item.Product_Id = Convert.ToInt32(sqlDataReader["Product_Id"]);
                }

                temp.Add(item);              
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            return temp;
        }

        public void Insert_Order_History(string Description, string Login, string Id_Order)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.[Order_History](Date, Login, Description, Id_Order) VALUES(@data, @Login, @Description, @Id_Order)";
            SqlCommand command = new SqlCommand(query, cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = Id_Order;

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();
        }

        public List<Order_History> Return_Order_History(string order_id)
        {
            List<Order_History> order_Histories = new List<Order_History>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Order_History] WHERE Id_Order = @Id_Order;", cnn);

            command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order_id;

            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Order_History order_History = new Order_History();
                order_History.Id_Order = sqlDataReader["Id_Order"].ToString();
                order_History.Login = sqlDataReader["Login"].ToString();
                order_History.Date = sqlDataReader["Date"].ToString();
                order_History.Description = sqlDataReader["Description"].ToString();

                order_Histories.Add(order_History);               
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            return order_Histories;
        }

        public int Avaible_Cut(Order order)
        {
            int count = 0;
            foreach (Item item in GetItems(order))
            {
                if  (item.Status == "awaiting" && item.Cut_id == 0)
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
        
        public List<Order> ReleasedOrder(Order order, User user)
        {
            List<Order> temp = new List<Order>();

            foreach (User use in usersController.GetUsers())
            {
                if (use.Login == user.Login)
                {
                    if (order.Status == "Ready")
                    {
                        string query;
                        SqlCommand command;

                        foreach (Item item in GetItems(order))
                        {
                            if (item.Status == "Ready")
                            {
                                query = "UPDATE dbo.[Item] SET Status = @Status WHERE Id = @Id;";
                                command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Status", SqlDbType.Bit).Value = "Released";
                                command.Parameters.Add("@Id", SqlDbType.Int).Value = item.Id;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();

                                query = "UPDATE dbo.[Product] SET Status = @Status WHERE Id = @Product_Id;";
                                command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Status", SqlDbType.Bit).Value = "Released";
                                command.Parameters.Add("@Product_Id", SqlDbType.Int).Value = item.Product_Id;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();

                                string producthistory = "Product has been released";
                                //productController.InsertProductHistory(item.Product_Id, user.Login, producthistory);
                            }
                        }

                        query = "UPDATE dbo.[Order] SET Released = @Released WHERE Order_Id = @Order_Id;";
                        command = new SqlCommand(query, cnn);

                        command.Parameters.Add("@Released", SqlDbType.Bit).Value = true;
                        command.Parameters.Add("@Order_Id", SqlDbType.Int).Value = order.Id_Order;

                        cnn.Open();
                        command.ExecuteNonQuery();
                        command.Dispose();
                        cnn.Close();

                        temp.Add(order);

                        string userhistory = "You released order " + order.Id_Order;
                        string orderhistory = "Order has been released";

                        usersController.Insert_User_History(userhistory, user.Login);
                        Insert_Order_History(orderhistory, user.Login, order.Id_Order);

                        return temp;
                    }
                    order.Error_Messege = "Order status is not ready";
                    temp.Add(order);
                    return temp;
                }
            }
            order.Error_Messege = "User not found";
            temp.Add(order);
            return temp;

        }

        public List<Item> ReleasedItem(User user, List<Item> items)
        {
            List<Item> temp = new List<Item>();
            Item temp_item = new Item();
            Order order = new Order { Id_Order = items.First().Order_id  };
            List<Item> temp2 = GetItems(order);

            foreach (Item item in temp2)
            {
                if(item.Status != "ready" && item.Status != "Released")
                {
                    temp_item.Error_Messege = "Items are not ready";
                    temp.Add(temp_item);
                    return temp;
                }
            }                    

            foreach (User use in usersController.GetUsers())
            {
                if (use.Login == user.Login)
                {
                    foreach (Item item in temp2)
                    {
                        if (item.Status != "Released")
                        {
                            string query = "UPDATE dbo.[Item] SET Status = @Status WHERE Id = @Id;";
                            SqlCommand command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@Status", SqlDbType.Bit).Value = "Released";
                            command.Parameters.Add("@Id", SqlDbType.Int).Value = item.Id;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();
                        }

                        temp.Add(temp_item);

                        string userhistory = "You released item " + item.Id;
                        string orderhistory = "Item " + item.Id + " has been released";

                        usersController.Insert_User_History(userhistory, user.Login);
                        Insert_Order_History(orderhistory, user.Login, item.Order_id);

                        return temp;
                    }

                }
            }
            temp_item.Error_Messege = "User not found";
            temp.Add(temp_item);
            return temp;
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
        public async Task<List<Item>> Return_All_Items([FromBody] Receiver receiver)
        {
            Order order = receiver.order;

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
        
        [HttpPost("Return_Order_History")]
        public async Task<List<Order_History>> Return_Order_History([FromBody] Receiver receiver)
        {
            Order order = receiver.order;

            return Return_Order_History(order.Id_Order);
        }

        [HttpPost("Add_Order")]
        public async Task<List<Order>> Add_Order([FromBody] Receiver receiver)
        {
            List<Order> temp = new List<Order>();

            Order order = receiver.order;
            User user = receiver.user;
            int code;

            foreach (Item item in receiver.items)
            {
                order.items.Add(item);
            }

            if(GetOrders().Last() != null)
            {
                code = Int32.Parse(GetOrders().Last().Id_Order) + 1;
            }
            else
            {
                code = 1;
            }


            order.Id_Order = code.ToString();

            List<Item> itemstemp = new List<Item>();

            foreach (Item item in order.items)
            {             
                if (item.Amount == 1)
                {
                    item.Status = "awaiting";
                    item.Can_Be_Createad = false;

                    if(GetAllItems().Last() != null)
                    {
                        item.Id = GetAllItems().Last().Id + 1;
                    }
                    else
                    {
                        item.Id = 1;
                    }
                    item.Amount = 0;
                }
                else if(item.Amount > 1)
                {
                    item.Status = "awaiting";
                    item.Can_Be_Createad = false;

                    if (GetAllItems().Last() != null)
                    {
                        item.Id = GetAllItems().Last().Id + 1;
                    }
                    else
                    {
                        item.Id = 1;
                    }

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

                        new_item.Id = GetAllItems().Last().Id + 1;
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
                    string orderhistory = "Order has been created";

                    usersController.Insert_User_History(userhistory, user.Login);
                    Insert_Order_History(orderhistory,  user.Login,  order.Id_Order);
                }
            }

            order.Error_Messege = "Bad login";

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

                            string userhistory = "You edited order " + order.Id_Order;
                            string orderhistory = "Order has been edited";

                            usersController.Insert_User_History(userhistory, user.Login);
                            Insert_Order_History(orderhistory, user.Login, order.Id_Order);
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

                            string userhistory = "You edited order items " + order.Id_Order;
                            string orderhistory = "Order items have been edited";

                            usersController.Insert_User_History(userhistory, user.Login);
                            Insert_Order_History(orderhistory, user.Login, order.Id_Order);
                        }
                    }
                }
            }
            order.Error_Messege = "User_not_found";
            temp.Add(order);
            return temp;
        }

        [HttpPost("Set_Stan")]
        public async Task<List<Order>> Set_Stan([FromBody] Receiver receiver)
        {
            List<Order> temp = new List<Order>();

            Order order = receiver.order;
            User user = receiver.user;
            string name = receiver.new_name;

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (Order ord in GetOrders())
                    {
                        if (ord.Id_Order == order.Id_Order)
                        {
                            if (name == "deletead" && ord.Deletead == false && ord.Released == false)
                            {
                                string query = "UPDATE dbo.[Order] SET Deletead = @Deletead WHERE Id_Order = @Id_Order;";
                                SqlCommand command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = true;

                                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();

                                string userhistory = "You change order status " + order.Id_Order + " on deletead";
                                string orderhistory = "Order has been deleted";

                                usersController.Insert_User_History(userhistory, user.Login);
                                Insert_Order_History(orderhistory, user.Login, order.Id_Order);

                                temp.Add(order);
                                return temp;
                            }
                            else if (name == "deletead" && ord.Deletead == true && ord.Released == false && ord.Frozen == false)
                            {
                                string query = "UPDATE dbo.[Order] SET Deletead = @Deletead WHERE Id_Order = @Id_Order;";
                                SqlCommand command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = false;

                                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();

                                string userhistory = "You changed order ststus " + order.Id_Order + " on restored";
                                string orderhistory = "Order has been restored";

                                usersController.Insert_User_History(userhistory, user.Login);
                                Insert_Order_History(orderhistory, user.Login, order.Id_Order);

                                temp.Add(order);
                                return temp;
                            }
                            if (name == "frozen" && ord.Deletead == false && ord.Released == false && ord.Frozen == false)
                            {
                                string query = "UPDATE dbo.[Order] SET Frozen = @Frozen WHERE Id_Order = @Id_Order;";
                                SqlCommand command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Frozen", SqlDbType.Bit).Value = true;

                                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();

                                string userhistory = "You change order status " + order.Id_Order + " on frozen";
                                string orderhistory = "Order has been frozen";

                                usersController.Insert_User_History(userhistory, user.Login);
                                Insert_Order_History(orderhistory, user.Login, order.Id_Order);

                                temp.Add(order);
                                return temp;
                            }
                            else if (name == "frozen" && ord.Deletead == false && ord.Released == false && ord.Frozen == true)
                            {
                                string query = "UPDATE dbo.[Order] SET Frozen = @Frozen WHERE Id_Order = @Id_Order;";
                                SqlCommand command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Frozen", SqlDbType.Bit).Value = false;

                                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();

                                string userhistory = "You changed order status " + order.Id_Order + " on unfrozen";
                                string orderhistory = "Order has been unfrozen";

                                usersController.Insert_User_History(userhistory, user.Login);
                                Insert_Order_History(orderhistory, user.Login, order.Id_Order);

                                temp.Add(order);
                                return temp;
                            }
                            if (name == "released" && ord.Status == "Done" && ord.Deletead == false && ord.Released == false)
                            {
                                string query = "UPDATE dbo.[Order] SET Released = @Released WHERE Id_Order = @Id_Order;";
                                SqlCommand command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Released", SqlDbType.Bit).Value = true;

                                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();

                                string userhistory = "You change order status " + order.Id_Order + "on released";
                                string orderhistory = "Order has been released";

                                usersController.Insert_User_History(userhistory, user.Login);
                                Insert_Order_History(orderhistory, user.Login, order.Id_Order);

                                temp.Add(order);
                                return temp;
                            }
                            else if (name == "released" && ord.Deletead == false && ord.Released == true)
                            {
                                string query = "UPDATE dbo.[Order] SET Released = @Released WHERE Id_Order = @Id_Order;";
                                SqlCommand command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Released", SqlDbType.Bit).Value = false;

                                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();

                                string userhistory = "You change order status " + order.Id_Order + " to ready";
                                string orderhistory = "Order ststus has been changed to ready";

                                usersController.Insert_User_History(userhistory, user.Login);
                                Insert_Order_History(orderhistory, user.Login, order.Id_Order);

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
        
        [HttpPost("Released_Order")]
        public async Task<List<Order>> Released_Order([FromBody] Receiver receiver)
        {
            return ReleasedOrder(receiver.order, receiver.user);
        }

        [HttpPost("Released_Item")]
        public async Task<List<Item>> Released_Item([FromBody] Receiver receiver)
        {
            return ReleasedItem(receiver.user, receiver.items);
        }
        
    }
}