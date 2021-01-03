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
    public sealed class ProductController : Controller
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
        OrderController orderController = new OrderController();
        
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
                if (sqlDataReader["Status"].ToString() == "Ready" || sqlDataReader["Status"].ToString() == "In magazine" || sqlDataReader["Status"].ToString() == "Send to magazine")
                {
                    Product product = new Product();
                    product.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    product.Owner = sqlDataReader["Owner"].ToString();
                    product.Status = sqlDataReader["Status"].ToString();
                    product.Desk = sqlDataReader["Desk"].ToString();
                    product.Id_item = Convert.ToInt32(sqlDataReader["Id_item"]);
                    product.Id_order = sqlDataReader["Id_order"].ToString();

                    temp.Add(product);
                }

            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            return temp;
        }
        
        public List<Product> ReleasedProduct(User user, List<int> product_id)
        {
            List<Product> temp = new List<Product>();
            List<Product> products_to_change = new List<Product>();
            List<Product> wynik = new List<Product>();
            Product product = new Product();
            List<string> Ordr_ids = new List<string>();

            foreach (Product pro in GetProducts())
            {
                foreach (int pro2 in product_id)
                {
                    if(pro2 == pro.Id)
                    {
                        products_to_change.Add(pro);
                    }
                }
            }

            foreach (Product pro in products_to_change)
            {
                if (pro.Status != "Ready")
                {
                    product.Error_Messege = "Products not ready";
                    temp.Add(product);
                    return temp;
                }
            }

            foreach (User use in usersController.GetUsers())
            {
                if (use.Login == user.Login)
                {
                    foreach(Product pro in products_to_change)
                    {
                        if(pro.Status == "Ready")
                        {
                            string query = "UPDATE dbo.[Product] SET Status = @Status WHERE Id = @Id;";
                            SqlCommand command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Released";
                            command.Parameters.Add("@Id", SqlDbType.Int).Value = pro.Id;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();

                            string userhistory = "You released product " + pro.Id;
                            string producthistory = "Product has been released";

                            usersController.Insert_User_History(userhistory, user.Login);
                            InsertProductHistory(pro.Id, producthistory, user.Login);

                            query = "UPDATE dbo.[Item] SET Status = @Status WHERE Id = @Id;";
                            command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Released";
                            command.Parameters.Add("@Id", SqlDbType.Int).Value = pro.Id_item;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();

                            string orderhistory = "Order item has been released " + pro.Id_item;
                            orderController.Insert_Order_History(orderhistory, user.Login, pro.Id_order);

                            wynik.Add(pro);
                            Ordr_ids.Add(pro.Id_order);
                        }                        
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
                            command.Parameters.Add("@Order_Id", SqlDbType.VarChar, 40).Value = order.Id_Order;

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
            return ReleasedProduct(receiver.user, receiver.product_Id);
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

                    query = "UPDATE dbo.[Item] SET Status = @Status Product_Id = @Product_Id WHERE Id = @Id;";
                    command = new SqlCommand(query, cnn);

                    command.Parameters.Add("@Status", SqlDbType.VarChar).Value = "Awaiting";
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = product.Id_item;
                    command.Parameters.Add("@Product_Id", SqlDbType.Int).Value = 0;

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