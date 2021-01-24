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

        public List<Order> GetOrders()
        {
            List<Order> temp = new List<Order>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Order];", connect.cnn);
            connect.cnn.Open();

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
            connect.cnn.Close();

            return temp;
        }

        public List<Order> GetOrdersUser()
        {
            List<Order> temp = new List<Order>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Order] WHERE Deletead = @Deletead and Released = @Released;", connect.cnn);

            command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = false;
            command.Parameters.Add("@Released", SqlDbType.Bit).Value = false;

            connect.cnn.Open();

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
            connect.cnn.Close();

            return temp;
        }

        public List<Order> GetDeletedOrdersUser()
        {
            List<Order> temp = new List<Order>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Order] WHERE Deletead = @Deletead;", connect.cnn);

            command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = true;

            connect.cnn.Open();

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
            connect.cnn.Close();

            return temp;
        }

        public List<Order> GetReleasedOrdersUser()
        {
            List<Order> temp = new List<Order>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Order] WHERE Released = @Released;", connect.cnn);

            command.Parameters.Add("@Released", SqlDbType.VarChar ,40).Value = true;

            connect.cnn.Open();

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
            connect.cnn.Close();

            return temp;
        }

        public List<Item> GetItems(Order order)
        {
            List<Item> temp = new List<Item>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Item] Where Order_id = @Order_id", connect.cnn);

            command.Parameters.Add("@Order_id", SqlDbType.VarChar, 40).Value = order.Id_Order;

            connect.cnn.Open();

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
            connect.cnn.Close();

            return temp;
        }

        public List<Item> GetAllItems()
        {
            List<Item> temp = new List<Item>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Item]", connect.cnn);
            connect.cnn.Open();

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
            connect.cnn.Close();

            temp.Sort((item1, item2) => (item1.Id.CompareTo(item2.Id)));

            return temp;
        }

        public List<Order_History> Return_Order_History(string order_id)
        {
            List<Order_History> order_Histories = new List<Order_History>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Order_History] WHERE Id_Order = @Id_Order;", connect.cnn);

            command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order_id;

            connect.cnn.Open();

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
