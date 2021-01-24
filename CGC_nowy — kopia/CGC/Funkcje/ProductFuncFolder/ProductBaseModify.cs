using CGC.Funkcje.History;
using CGC.Funkcje.OrderFuncFolder.OrderBase;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.ProductFuncFolder
{
    public class ProductBaseModify
    {

        private static ProductBaseModify m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static ProductBaseModify Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new ProductBaseModify();
                    }
                    return m_oInstance;
                }
            }
        }

        private Connect connect = new Connect();
        private InsertHistory insertHistory = new InsertHistory();
        private OrderBaseReturn orderBaseReturn = new OrderBaseReturn();

        public List<Product> Released_Product(User user, List<Product> products_to_change)
        {
            List<string> Ordr_ids = new List<string>();
            List<Product> wynik = new List<Product>();

            foreach (Product pro in products_to_change)
            {
                string query = "UPDATE dbo.[Product] SET Status = @Status WHERE Id = @Id;";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Wydany";
                command.Parameters.Add("@Id", SqlDbType.Int).Value = pro.Id;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "Wydaleś produkt " + pro.Id;
                string producthistory = "Produkt zostal wydany";

                insertHistory.Insert_User_History(userhistory, user.Login);
                insertHistory.InsertProductHistory(pro.Id, producthistory, user.Login);

                query = "UPDATE dbo.[Item] SET Status = @Status WHERE Id = @Id;";
                command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Wydany";
                command.Parameters.Add("@Id", SqlDbType.Int).Value = pro.Id_item;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string orderhistory = "Pozycja zamowienia zostala wydana " + pro.Id_item;
                insertHistory.Insert_Order_History(orderhistory, user.Login, pro.Id_order);

                wynik.Add(pro);
                Ordr_ids.Add(pro.Id_order);
            }

            foreach (string Ord_Id in Ordr_ids.Distinct())
            {
                bool check = true;
                Order order = new Order { Id_Order = Ord_Id };

                foreach (Item item in orderBaseReturn.GetItems(order))
                {
                    if (item.Status != "Wydany" && item.Status != "Usuniety")
                    {
                        check = false;
                        break;
                    }
                }

                if (check == true)
                {
                    string query = "UPDATE dbo.[Order] SET Released = @Released WHERE Order_Id = @Order_Id;";
                    SqlCommand command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Released", SqlDbType.Bit).Value = true;
                    command.Parameters.Add("@Order_Id", SqlDbType.VarChar, 40).Value = order.Id_Order;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    string userhistory = "Wydaleś zamowienie " + order.Id_Order;
                    string orderhistory = "Zamowienie zostalo wydane";

                    insertHistory.Insert_User_History(userhistory, user.Login);
                    insertHistory.Insert_Order_History(orderhistory, user.Login, order.Id_Order);
                }
            }
            return wynik;
        }

        public List<Product> Delete_Product(User user, List<Product> products_to_change)
        {
            string userhistory;
            string Orderhistory;
            string Producthistory;

            foreach (Product product in products_to_change)
            {
                string query = "UPDATE dbo.[Product] SET Status = @Status WHERE Id = @Id;";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Status", SqlDbType.VarChar).Value = "Usuniety";
                command.Parameters.Add("@Id", SqlDbType.Int).Value = product.Id;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                userhistory = "Usunaleś produkt " + product.Id.ToString();
                Producthistory = "Produkt zostal usuniety";

                insertHistory.InsertProductHistory(product.Id, user.Login, Producthistory);
                insertHistory.Insert_User_History(userhistory, user.Login);


                query = "UPDATE dbo.[Item] SET Status = @Status, Product_Id = @Product_Id WHERE Id = @Id;";
                command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Status", SqlDbType.VarChar).Value = "Oczekujacy";
                command.Parameters.Add("@Id", SqlDbType.Int).Value = product.Id_item;
                command.Parameters.Add("@Product_Id", SqlDbType.Int).Value = 0;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string orderHistory = "";
                //insertHistory.Insert_Order_History(orderHistory, user.Login,);

            }
            return products_to_change;
        }    
    }  
}
