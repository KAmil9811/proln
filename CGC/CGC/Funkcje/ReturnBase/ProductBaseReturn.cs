using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.ProductFuncFolder.ProductBase
{
    public class ProductBaseReturn
    {
        private static ProductBaseReturn m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static ProductBaseReturn Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new ProductBaseReturn();
                    }
                    return m_oInstance;
                }
            }
        }

        private Connect connect = new Connect();

        public List<Product_History> GetProductHistory(int Id, string company)
        {
            List<Product_History> product_Histories = new List<Product_History>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Product_History] WHERE Id = @Id AND Company = @Company;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Id", SqlDbType.VarChar,40).Value = Id;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Product_History product_History = new Product_History();
                product_History.Id = sqlDataReader["Id"].ToString();
                product_History.Date = sqlDataReader["Data"].ToString();
                product_History.Description = sqlDataReader["Description"].ToString();

                product_Histories.Add(product_History);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return product_Histories;
        }

        public List<Product> GetProduct(string id, string company)
        {
            List<Product> temp = new List<Product>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Product] WHERE Id = @Id AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = id;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Product product = new Product();
                product.Id = sqlDataReader["Id"].ToString();
                product.Owner = sqlDataReader["Owner"].ToString();
                product.Status = sqlDataReader["Status"].ToString();
                product.Desk = sqlDataReader["Desk"].ToString();
                product.Id_item = sqlDataReader["Id_item"].ToString();
                product.Id_order = sqlDataReader["Id_order"].ToString();

                temp.Add(product);
            }

            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<Product> GetProducts(string company)
        {
            List<Product> temp = new List<Product>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Product] AND Company = @Company;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Product product = new Product();
                product.Id = sqlDataReader["Id"].ToString();
                product.Owner = sqlDataReader["Owner"].ToString();
                product.Status = sqlDataReader["Status"].ToString();
                product.Desk = sqlDataReader["Desk"].ToString();
                product.Id_item = sqlDataReader["Id_item"].ToString();
                product.Id_order = sqlDataReader["Id_order"].ToString();

                temp.Add(product);
            }

            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<Product> GetProducts(string status, string company)
        {
            List<Product> temp = new List<Product>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Product] Where Status = @Status AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = status;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Product product = new Product();
                product.Id = sqlDataReader["Id"].ToString();
                product.Owner = sqlDataReader["Owner"].ToString();
                product.Status = sqlDataReader["Status"].ToString();
                product.Desk = sqlDataReader["Desk"].ToString();
                product.Id_item = sqlDataReader["Id_item"].ToString();
                product.Id_order = sqlDataReader["Id_order"].ToString();

                temp.Add(product);
            }

            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<Product> GetLastProduct(string company)
        {
            List<Product> temp = new List<Product>();
            SqlCommand command = new SqlCommand("Select TOP(1) Id From [Product] WHERE Company = @Company ORDER BY convert(int, Id) DESC", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Product product = new Product();
                product.sort = Convert.ToInt32(sqlDataReader["Id"]);

                temp.Add(product);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            //temp.Sort((item1, item2) => (item1.Id.CompareTo(item2.Id)));

            return temp;
        }

        public List<Product> GetLastGlobalIdProduct(string company)
        {
            List<Product> temp = new List<Product>();
            SqlCommand command = new SqlCommand("Select TOP(1) Global_id From [Product] WHERE Company = @Company ORDER BY convert(int, Global_id) DESC", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Product product = new Product();
                product.Global_Id = Convert.ToInt32(sqlDataReader["Global_id"]) + 1;

                temp.Add(product);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }


        public List<Product_History> GetLastGlobalIdProductHistory(string company)
        {
            List<Product_History> temp = new List<Product_History>();
            SqlCommand command = new SqlCommand("Select TOP(1) Global_id From [Product_History] WHERE Company = @Company ORDER BY convert(int, Global_id) DESC", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Product_History product_History = new Product_History();
                product_History.Global_Id = Convert.ToInt32(sqlDataReader["Global_id"]) + 1;

                temp.Add(product_History);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }
    }  
}
