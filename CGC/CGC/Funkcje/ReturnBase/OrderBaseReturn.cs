using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.OrderFuncFolder.OrderBase
{
    public class OrderBaseReturn
    {

        private Connect connect = new Connect();

        private static OrderBaseReturn m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static OrderBaseReturn Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new OrderBaseReturn();
                    }
                    return m_oInstance;
                }
            }
        }

        public List<Order> GetOrder(string id_order, string company)
        {
            List<Order> temp = new List<Order>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Order] WHERE Id_Order = @Id_Order AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = id_order;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();

            try
            {
                SqlDataReader sqlDataReader = command.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    Order order = new Order();
                    order.Id_Order = sqlDataReader["Id_Order"].ToString();
                    order.Owner = sqlDataReader["Owner"].ToString();
                    order.Status = sqlDataReader["Status"].ToString();
                    order.Priority = sqlDataReader["Priority"].ToString();
                    order.Deadline = sqlDataReader["Deadline"].ToString();
                    order.Stan = sqlDataReader["Stan"].ToString();
                    order.Deleted = Convert.ToBoolean(sqlDataReader["Deletead"]);
                    order.Frozen = Convert.ToBoolean(sqlDataReader["Frozen"]);
                    order.Released = Convert.ToBoolean(sqlDataReader["Released"]);

                    temp.Add(order);
                }
                sqlDataReader.Close();
                command.Dispose();
                connect.cnn.Close();
            }
            catch (Exception e)
            {
                e.ToString();
                return temp;
            }

            return temp;
        }

        public List<Order> GetOrders(string company)
        {
            List<Order> temp = new List<Order>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Order] WHERE Company = @Company;", connect.cnn);

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;
            connect.cnn.Open();

            try
            {
                SqlDataReader sqlDataReader = command.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    Order order = new Order();
                    order.Id_Order = sqlDataReader["Id_Order"].ToString();
                    order.Owner = sqlDataReader["Owner"].ToString();
                    order.Status = sqlDataReader["Status"].ToString();
                    order.Priority = sqlDataReader["Priority"].ToString();
                    order.Deadline = sqlDataReader["Deadline"].ToString();
                    order.Stan = sqlDataReader["Stan"].ToString();
                    order.Deleted = Convert.ToBoolean(sqlDataReader["Deletead"]);
                    order.Frozen = Convert.ToBoolean(sqlDataReader["Frozen"]);
                    order.Released = Convert.ToBoolean(sqlDataReader["Released"]);
                    order.Deadline2 = Convert.ToDateTime(order.Deadline);

                    temp.Add(order);
                }
                sqlDataReader.Close();
                command.Dispose();
                connect.cnn.Close();
            }
            catch(Exception e)
            {
                e.ToString();
                return temp;
            }

            return temp;
        }

        public List<Order> GetOrders(bool deletead, bool released, string company)
        {
            List<Order> temp = new List<Order>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Order] WHERE Deletead = @Deletead and Released = @Released AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = deletead;
            command.Parameters.Add("@Released", SqlDbType.Bit).Value = released;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Order order = new Order();
                order.Id_Order = sqlDataReader["Id_Order"].ToString();
                order.Owner = sqlDataReader["Owner"].ToString();
                order.Status = sqlDataReader["Status"].ToString();
                order.Priority = sqlDataReader["Priority"].ToString();
                order.Deadline = sqlDataReader["Deadline"].ToString();
                order.Stan = sqlDataReader["Stan"].ToString();
                order.Deleted = Convert.ToBoolean(sqlDataReader["Deletead"]);
                order.Frozen = Convert.ToBoolean(sqlDataReader["Frozen"]);
                order.Released = Convert.ToBoolean(sqlDataReader["Released"]);
                order.Deadline2 = Convert.ToDateTime(order.Deadline);

                temp.Add(order);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<Order> GetOrders(string status, bool deletead, bool released, string company)
        {
            List<Order> temp = new List<Order>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Order] WHERE Status = @Status AND Deletead = @Deletead AND Released = @Released AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = status;
            command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = deletead;
            command.Parameters.Add("@Released", SqlDbType.Bit).Value = released;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Order order = new Order();
                order.Id_Order = sqlDataReader["Id_Order"].ToString();
                order.Owner = sqlDataReader["Owner"].ToString();
                order.Status = sqlDataReader["Status"].ToString();
                order.Priority = sqlDataReader["Priority"].ToString();
                order.Deadline = sqlDataReader["Deadline"].ToString();
                order.Stan = sqlDataReader["Stan"].ToString();
                order.Deleted = Convert.ToBoolean(sqlDataReader["Deletead"]);
                order.Frozen = Convert.ToBoolean(sqlDataReader["Frozen"]);
                order.Released = Convert.ToBoolean(sqlDataReader["Released"]);
                order.Deadline2 = Convert.ToDateTime(order.Deadline);

                temp.Add(order);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<Order> GetOrders(string status, string status2, bool deletead, bool released, string company)
        {
            List<Order> temp = new List<Order>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Order] WHERE (Status = @Status OR Status = @Status2) AND Deletead = @Deletead AND Released = @Released AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = status;
            command.Parameters.Add("@Status2", SqlDbType.VarChar, 40).Value = status2;
            command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = deletead;
            command.Parameters.Add("@Released", SqlDbType.Bit).Value = released;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Order order = new Order();
                order.Id_Order = sqlDataReader["Id_Order"].ToString();
                order.Owner = sqlDataReader["Owner"].ToString();
                order.Status = sqlDataReader["Status"].ToString();
                order.Priority = sqlDataReader["Priority"].ToString();
                order.Deadline = sqlDataReader["Deadline"].ToString();
                order.Stan = sqlDataReader["Stan"].ToString();
                order.Deleted = Convert.ToBoolean(sqlDataReader["Deletead"]);
                order.Frozen = Convert.ToBoolean(sqlDataReader["Frozen"]);
                order.Released = Convert.ToBoolean(sqlDataReader["Released"]);
                order.Deadline2 = Convert.ToDateTime(order.Deadline);

                temp.Add(order);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<Item> GetItems(Order order, string company)
        {
            List<Item> temp = new List<Item>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Item] Where Order_id = @Order_id AND Company = @Company", connect.cnn);

            command.Parameters.Add("@Order_id", SqlDbType.VarChar, 40).Value = order.Id_Order;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Item item = new Item();
                item.Id = sqlDataReader["Id"].ToString();
                item.Thickness = sqlDataReader["Height"].ToString();
                item.Width = sqlDataReader["Weight"].ToString();
                item.Length = sqlDataReader["Lenght"].ToString();
                item.Type = sqlDataReader["Glass_Type"].ToString();
                item.Color = sqlDataReader["Color"].ToString();
                item.Status = sqlDataReader["Status"].ToString();
                item.Order_id = sqlDataReader["Order_id"].ToString();
                item.Desk = sqlDataReader["Desk"].ToString();
                item.WidthSort = Convert.ToDouble(item.Width);
                item.LengthSort = Convert.ToDouble(item.Length);

                try
                {
                    item.Cut_id = sqlDataReader["Cut_id"].ToString();
                }
                catch
                {
                    item.Cut_id = "0";
                }

                try
                {
                    item.Product_Id = sqlDataReader["Product_Id"].ToString();
                }
                catch
                {
                    item.Product_Id = "0";
                }

                temp.Add(item);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<Item> GetItems(Order order, string cut_id, string company)
        {
            List<Item> temp = new List<Item>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Item] Where Order_id = @Order_id AND Cut_id = @Cut_id AND Company = @Company", connect.cnn);

            command.Parameters.Add("@Order_id", SqlDbType.VarChar, 40).Value = order.Id_Order;
            command.Parameters.Add("@Cut_id", SqlDbType.VarChar, 40).Value = cut_id;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Item item = new Item();
                item.Id = sqlDataReader["Id"].ToString();
                item.Thickness = sqlDataReader["Height"].ToString();
                item.Width = sqlDataReader["Weight"].ToString();
                item.Length = sqlDataReader["Lenght"].ToString();
                item.Type = sqlDataReader["Glass_Type"].ToString();
                item.Color = sqlDataReader["Color"].ToString();
                item.Status = sqlDataReader["Status"].ToString();
                item.Order_id = sqlDataReader["Order_id"].ToString();
                item.Desk = sqlDataReader["Desk"].ToString();
                item.WidthSort = Convert.ToDouble(item.Width);
                item.LengthSort = Convert.ToDouble(item.Length);

                try
                {
                    item.Cut_id = sqlDataReader["Cut_id"].ToString();
                }
                catch
                {
                    item.Cut_id = "0";
                }

                try
                {
                    item.Product_Id = sqlDataReader["Product_Id"].ToString();
                }
                catch
                {
                    item.Product_Id = "0";
                }

                temp.Add(item);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<Item> GetItems(Order order, Item example, string company)
        {
            List<Item> temp = new List<Item>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Item] Where Order_id = @Order_id AND Height = @Height AND Color = @Color AND Glass_Type = @Glass_Type AND Status = @Status AND Cut_id = @Cut_id AND Company = @Company", connect.cnn);

            command.Parameters.Add("@Order_id", SqlDbType.VarChar, 40).Value = order.Id_Order;
            command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = example.Color;
            command.Parameters.Add("@Glass_Type", SqlDbType.VarChar, 40).Value = example.Type;
            command.Parameters.Add("@Height", SqlDbType.Float).Value = Convert.ToDouble(example.Thickness);
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = example.Status;
            command.Parameters.Add("@Cut_id", SqlDbType.VarChar, 40).Value = example.Cut_id;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Item item = new Item();
                item.Id = sqlDataReader["Id"].ToString();
                item.Thickness = sqlDataReader["Height"].ToString();
                item.Width = sqlDataReader["Weight"].ToString();
                item.Length = sqlDataReader["Lenght"].ToString();
                item.Type = sqlDataReader["Glass_Type"].ToString();
                item.Color = sqlDataReader["Color"].ToString();
                item.Status = sqlDataReader["Status"].ToString();
                item.Order_id = sqlDataReader["Order_id"].ToString();
                item.Desk = sqlDataReader["Desk"].ToString();
                item.WidthSort = Convert.ToDouble(item.Width);
                item.LengthSort = Convert.ToDouble(item.Length);

                try
                {
                    item.Cut_id = sqlDataReader["Cut_id"].ToString();
                }
                catch
                {
                    item.Cut_id = "0";
                }

                try
                {
                    item.Product_Id = sqlDataReader["Product_Id"].ToString();
                }
                catch
                {
                    item.Product_Id = "0";
                }

                temp.Add(item);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<Item> GetAllItems(string company)
        {
            List<Item> temp = new List<Item>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Item] WHERE Company = @Company", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Item item = new Item();
                item.Id = sqlDataReader["Id"].ToString();
                item.Thickness = sqlDataReader["Height"].ToString();
                item.Width = sqlDataReader["Weight"].ToString();
                item.Length = sqlDataReader["Lenght"].ToString();
                item.Type = sqlDataReader["Glass_Type"].ToString();
                item.Color = sqlDataReader["Color"].ToString();
                item.Status = sqlDataReader["Status"].ToString();
                item.Order_id = sqlDataReader["Order_id"].ToString();
                item.Desk = sqlDataReader["Desk"].ToString();

                try
                {
                    item.Cut_id = sqlDataReader["Cut_id"].ToString();
                }
                catch (Exception e)
                {
                    item.Cut_id = "0";
                }

                try
                {
                    item.Product_Id = sqlDataReader["Product_Id"].ToString();
                }
                catch (Exception e)
                {
                    item.Product_Id = "0";
                }

                temp.Add(item);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            temp.Sort((item1, item2) => (item1.Id.CompareTo(item2.Id)));

            return temp;
        }

        public List<Item> GetLastItem(string company)
        {
            List<Item> temp = new List<Item>();
            SqlCommand command = new SqlCommand("Select TOP(1) Id From [Item] ORDER BY convert(int, Id) DESC WHERE Company = @Company", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Item item = new Item();
                item.sort = Convert.ToInt32(sqlDataReader["Id"]);

                temp.Add(item);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            //temp.Sort((item1, item2) => (item1.Id.CompareTo(item2.Id)));

            return temp;
        }

        public List<Order> GetLastOrder(string company)
        {
            List<Order> temp = new List<Order>();
            SqlCommand command = new SqlCommand("Select TOP(1) Id_Order From [Order] ORDER BY convert(int, Id_Order) DESC WHERE Company = @Company", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Order order = new Order();
                order.sort = Convert.ToInt32(sqlDataReader["Id_Order"]);

                temp.Add(order);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            //temp.Sort((item1, item2) => (item1.Id.CompareTo(item2.Id)));

            return temp;
        }

        public List<Order_History> Return_Order_History(string company)
        {
            List<Order_History> order_Histories = new List<Order_History>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Order_History] Where Company = @Company;", connect.cnn);

            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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
            connect.cnn.Close();

            return order_Histories;
        }
    }
}
