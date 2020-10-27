﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CGC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        public static string connetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\micha\Desktop\INZ V1\proln\Inz_Base\Inz_Base\DataBaseInz.mdf;Integrated Security=True;Connect Timeout=30";
        SqlConnection cnn = new SqlConnection(connetionString);
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
                    item.Sub_Shape = sqlDataReader["Sub_Shape"].ToString();
                    item.Desk = sqlDataReader["Desk"].ToString();

                    temp.Add(item);
                }
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();

            return temp;
        }

        [HttpGet("Released_Order")]
        public async Task<List<Order>> Released_Order([FromBody] Order order)
        {
            List<Order> temp = new List<Order>();

            if(order.Status == "ready")
            {
                string query = "UPDATE dbo.[Order] SET Released = @Released WHERE Order_Id = @Order_Id;";
                SqlCommand command = new SqlCommand(query, cnn);

                command.Parameters.Add("@Released", SqlDbType.Bit).Value = true;
                command.Parameters.Add("@Order_Id", SqlDbType.Int).Value = order.Id_Order;

                cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                cnn.Close();

                foreach (Item item in GetItems(order))
                {
                    query = "UPDATE dbo.[Order] SET Status = @Status WHERE Order_Id = @Order_Id;";
                    command = new SqlCommand(query, cnn);

                    command.Parameters.Add("@Status", SqlDbType.Bit).Value = "Released";
                    command.Parameters.Add("@Order_Id", SqlDbType.Int).Value = order.Id_Order;

                    cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    cnn.Close();
                }
                temp.Add(order);
                return temp;
            }

            order.Error_Messege = "Orders status is not ready";
            temp.Add(order);
            return temp;
        }

        [HttpGet("Released_Item")]
        public async Task<List<Item>> Released_Item([FromBody] Receiver receiver)
        {
            List<Item> temp = new List<Item>();
            Item temp_item = new Item();

            foreach(Item item in receiver.items)
            {
                if (item.Status == "ready")
                {
                    string query = "UPDATE dbo.[Order] SET Status = @Status WHERE Id = @Id;";
                    SqlCommand command = new SqlCommand(query, cnn);

                    command.Parameters.Add("@Status", SqlDbType.Bit).Value = "Released";
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = item.Id;

                    cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    cnn.Close();
                }
                return temp;
            }
            temp_item.Error_Messege = "No good id to change";
            temp.Add(temp_item);
            return temp;
        }

    }
}