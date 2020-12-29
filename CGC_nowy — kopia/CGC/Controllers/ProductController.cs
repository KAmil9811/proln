using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CGC.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
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
        private static ProductController m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static ProductController Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new ProductController();
                    }
                    return m_oInstance;
                }
            }
        }

        UsersController usersController = new UsersController();
        MagazineController magazineController = new MagazineController();
        OrderController orderController = new OrderController();

        public List<Order> GetOrders()
        {
            List<Order> temp = new List<Order>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Order];", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                if ((sqlDataReader["Status"].ToString() == "ready" || sqlDataReader["Status"].ToString() == "stopped") && Convert.ToBoolean(sqlDataReader["Released"]) == false && Convert.ToBoolean(sqlDataReader["Deletead"]) == false)
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

        public List<Product_History> GetProductHistory (int Id)
        {
            List<Product_History> product_Histories = new List<Product_History>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Product_History] WHERE Id = @Id;", cnn);
            cnn.Open();

            command.Parameters.Add("@Id", SqlDbType.Bit).Value = Id;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Product_History product_History = new Product_History();
                product_History.Id = Convert.ToInt32(sqlDataReader["Id"]);
                product_History.Date = sqlDataReader["Data"].ToString();
                product_History.Description = sqlDataReader["Description"].ToString();

                product_Histories.Add(product_History);                
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            return product_Histories;
        }

        public void InsertProductHistory(int Id, string Login, string Description)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.[Product_History](Data, Login, Description, Id) VALUES(@data, @Login, @Description, @Id)";
            SqlCommand command = new SqlCommand(query, cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = Id;

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();
        }

        public List<Product> GetProducts()
        {
            List<Product> temp = new List<Product>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Product];", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                if (sqlDataReader["Status"].ToString() == "Send" || sqlDataReader["Status"].ToString() == "In magazine" || sqlDataReader["Status"].ToString() == "Send to magazine")
                {
                    Product product = new Product();
                    product.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    product.Owner = sqlDataReader["Owner"].ToString();
                    product.Status = sqlDataReader["Status"].ToString();
                    product.Desk = sqlDataReader["Desk"].ToString();
                    product.Id_item = Convert.ToInt32(sqlDataReader["Id_item"]);

                    temp.Add(product);
                }
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            return temp;
        }

        public List<Product> ReleasedProduct(User user, List<Product> products)
        {
            List<Product> temp = new List<Product>();
            List<Product> wynik = new List<Product>();
            Product product = new Product();
            List<int> Id_items = new List<int>();
            List<string> Ordr_ids = new List<string>();

            foreach (Product pro in products)
            {
                if (pro.Status != "ready")
                {
                    product.Error_Messege = "Products not ready";
                    temp.Add(product);
                    return temp;
                }

                Id_items.Add(pro.Id_item);
            }

            foreach (User use in usersController.GetUsers())
            {
                if (use.Login == user.Login)
                {
                    foreach(Product pro in products)
                    {
                        if(pro.Status == "ready")
                        {
                            string query = "UPDATE dbo.[Product] SET Status = @Status WHERE Id = @Id;";
                            SqlCommand command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@Status", SqlDbType.Bit).Value = "Released";
                            command.Parameters.Add("@Id", SqlDbType.Int).Value = pro.Id;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();

                            string userhistory = "You released product " + pro.Id;
                            string producthistory = "Product has been released";

                            usersController.Insert_User_History(userhistory, user.Login);
                            InsertProductHistory(pro.Id, producthistory, user.Login);

                            wynik.Add(pro);
                        }                        
                    }

                    foreach(int Id_item in Id_items.Distinct())
                    {
                        string ord_id = "";
                        SqlCommand command = new SqlCommand("SELECT Order_id FROM [Item] WHERE Id = @Id_item", cnn);
                        cnn.Open();

                        command.Parameters.Add("@Id_item", SqlDbType.Int).Value = Id_item;

                        SqlDataReader sqlDataReader = command.ExecuteReader();

                        while (sqlDataReader.Read())
                        {    
                            Ordr_ids.Add(sqlDataReader["Order_id"].ToString());
                            ord_id = sqlDataReader["Order_id"].ToString();
                        }

                        sqlDataReader.Close();
                        command.Dispose();
                        cnn.Close();

                        string query = "UPDATE dbo.[Item] SET Status = @Status WHERE Id = @Id_item;";
                        command = new SqlCommand(query, cnn);

                        command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Released";
                        
                        command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = Id_item;

                        cnn.Open();
                        command.ExecuteNonQuery();
                        command.Dispose();
                        cnn.Close();

                        string orderhistory = "Order item has been released " + Id_item;

                        orderController.Insert_Order_History(orderhistory, user.Login, ord_id);
                    }

                    foreach (string Ord_Id in Ordr_ids.Distinct())
                    {
                        bool check = true;
                        Order order = new Order { Id_Order = Ord_Id  };
                        foreach(Item item in orderController.GetItems(order))
                        {
                            if(item.Status != "Released" && item.Status != "Deleted")
                            {
                                check = false;
                                break;
                            }
                        }

                        if(check == true)
                        {
                            string query = "UPDATE dbo.[Order] SET Released = @Released WHERE Order_Id = @Order_Id;";
                            SqlCommand command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@Released", SqlDbType.Bit).Value = true;
                            command.Parameters.Add("@Order_Id", SqlDbType.Int).Value = order.Id_Order;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();

                            string userhistory = "You released order " + order.Id_Order;
                            string orderhistory = "Order has been released";

                            usersController.Insert_User_History(userhistory, user.Login);
                            orderController.Insert_Order_History(orderhistory, user.Login, order.Id_Order);
                        }
                    }
                    return wynik;
                }       
            }

            product.Error_Messege = "User not found";
            temp.Add(product);
            return temp;
        }

        [HttpGet("Get_Products")]
        public async Task<List<Product>> Get_Products()
        {
            return GetProducts();
        }

        [HttpPost("Get_Product_History")]
        public async Task<List<Product_History>> Get_Product_History([FromBody] Receiver receiver)
        {
            return GetProductHistory(receiver.id);
        }

        [HttpPost("Released_Product")]
        public async Task<List<Product>> Released_Product([FromBody] Receiver receiver)
        {
            return ReleasedProduct(receiver.user, receiver.products);
        }

        [HttpPost("Delete_Product")]
        public async Task<List<Product>> Delete_Product([FromBody] Receiver receiver)
        {
            List<Product> temp = new List<Product>();
            Product product = receiver.product;
            User user = receiver.user;

            foreach (User use in usersController.GetUsers())
            {
                if (use.Login == user.Login)
                {           
                    string query = "UPDATE dbo.[Product] SET Status = @Status WHERE Id = @Id;";
                    SqlCommand command = new SqlCommand(query, cnn);

                    command.Parameters.Add("@Status", SqlDbType.VarChar).Value = "Deleted";
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = product.Id;

                    cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    cnn.Close();

                    string userhistory = "You deleted product " + product.Id.ToString();
                    string Producthistory = "Product has been deleted";

                    InsertProductHistory(product.Id, user.Login, Producthistory);
                    usersController.Insert_User_History(userhistory, user.Login);

                    return temp;
                }
            }
            product.Error_Messege = "User not found";
            temp.Add(product);
            return temp;
        }
    }
}