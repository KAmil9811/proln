using CGC.Funkcje.History;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.OrderFuncFolder
{
    public class OrderBaseModify
    {
        private static OrderBaseModify m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static OrderBaseModify Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new OrderBaseModify();
                    }
                    return m_oInstance;
                }
            }
        }

        private Connect connect = new Connect();
        private InsertHistory insertHistory = new InsertHistory();
        private OrderCheck orderCheck = new OrderCheck();

        public List<Order> Add_Order(User user, Order order)
        {
            List<Order> temp = new List<Order>();

            string query;
            SqlCommand command;
            foreach (Item item in order.items)
            {
                try
                {
                    query = "INSERT INTO dbo.[Item](Id, Weight, Height, Lenght, Glass_Type, Color, Status,Desk, Order_id, Product_Id) VALUES(@Id, @Weight,@Height, @Lenght, @Glass_Type, @Color, @Status, @Desk, @Order_id, @Product_Id)";
                    command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = item.Id;
                    command.Parameters.Add("@Weight", SqlDbType.Float).Value = item.Width;
                    command.Parameters.Add("@Height", SqlDbType.Float).Value = item.Thickness;
                    command.Parameters.Add("@Lenght", SqlDbType.Float).Value = item.Length;
                    command.Parameters.Add("@Glass_Type", SqlDbType.VarChar, 40).Value = item.Type;
                    command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = item.Color;
                    command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Oczekujacy";
                    command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = "";
                    command.Parameters.Add("@Order_id", SqlDbType.VarChar, 40).Value = order.Id_Order;
                    command.Parameters.Add("@Product_Id", SqlDbType.Int).Value = 0;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            query = "INSERT INTO dbo.[Order](Id_Order,Owner,Status,Priority,Deadline,Stan,Deletead,Frozen,Released) VALUES(@Id_Order, @Owner, @Status, @Priority, @Deadline, @Stan, @Deletead, @Frozen, @Released)";
            command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;
            command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = order.Owner;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Oczekujace";
            command.Parameters.Add("@Priority", SqlDbType.Int).Value = order.Priority;
            command.Parameters.Add("@Deadline", SqlDbType.VarChar, 40).Value = order.Deadline;
            command.Parameters.Add("@Stan", SqlDbType.VarChar, 40).Value = orderCheck.Avaible_Cut(order).ToString() + "/" + order.items.Count + "/" + "0";
            command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = false;
            command.Parameters.Add("@Frozen", SqlDbType.Bit).Value = false;
            command.Parameters.Add("@Released", SqlDbType.Bit).Value = false;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = "Dodales zamowienie " + order.Id_Order;
            string orderhistory = "Zamowienie zostalo dodane";

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Order_History(orderhistory, user.Login, order.Id_Order);

            temp.Add(order);

            return temp;
        }

        public List<Order> Edit_Order(User user, Order order)
        {
            List<Order> temp = new List<Order>();

            string query = "UPDATE dbo.[Order] SET Deadline = @Deadline, Owner = @Owner, Priority = @Priority WHERE Id_Order = @Id_Order;";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Deadline", SqlDbType.VarChar, 40).Value = order.Deadline;
            command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = order.Owner;
            command.Parameters.Add("@Priority", SqlDbType.Decimal).Value = order.Priority;

            command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = "Zedytowales zamowienie " + order.Id_Order;
            string orderhistory = "Zamowienie zostalo zedytowane";

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Order_History(orderhistory, user.Login, order.Id_Order);

            temp.Add(order);
            return temp;
        }

        public List<Order> Edit_Order_Items(User user, Order order, Item item)
        {
            List<Order> temp = new List<Order>();

            string query = "UPDATE dbo.[Item] SET Height = @Height, Lenght = @Lenght, Weight = @Weight, Glass_Type = @Glass_Type, Color = @Color, Status = @Status, Desk = @Desk WHERE Id = @Id;";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Height", SqlDbType.Decimal).Value = item.Thickness;
            command.Parameters.Add("@Lenght", SqlDbType.Decimal).Value = item.Length;
            command.Parameters.Add("@Weight", SqlDbType.Decimal).Value = item.Width;
            command.Parameters.Add("@Glass_Type", SqlDbType.VarChar, 40).Value = item.Type;
            command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = item.Color;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = item.Status;
            command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = item.Desk;

            command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = item.Id;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = "Zedytowales pozycje zamowienia " + item.Id + " z zamowienia " + order.Id_Order;
            string orderhistory = "Pozycja zamowienia zostala zedytowana " + item.Id;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Order_History(orderhistory, user.Login, order.Id_Order);
            

            temp.Add(order);
            return temp;
        }

        public List<Order> Set_stan(User user, Order ord, string name)
        {
            List<Order> temp = new List<Order>();

            if (name == "Deletead" && ord.Deletead == false && ord.Released == false)
            {
                string query = "UPDATE dbo.[Order] SET Deletead = @Deletead WHERE Id_Order = @Id_Order;";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = true;

                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = ord.Id_Order;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "Usunales zamowienie " + ord.Id_Order;
                string orderhistory = "Zamowienie zostalo usuniete";

                insertHistory.Insert_User_History(userhistory, user.Login);
                insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order);

                temp.Add(ord);
                return temp;
            }
            else if (name == "Deletead" && ord.Deletead == true && ord.Released == false && ord.Frozen == false)
            {
                string query = "UPDATE dbo.[Order] SET Deletead = @Deletead WHERE Id_Order = @Id_Order;";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = false;

                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = ord.Id_Order;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "Przywrociles zamowienie " + ord.Id_Order;
                string orderhistory = "Zamoienie zostalo przywrocone";

                insertHistory.Insert_User_History(userhistory, user.Login);
                insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order);

                temp.Add(ord);
                return temp;
            }
            if (name == "Frozen" && ord.Deletead == false && ord.Released == false && ord.Frozen == false)
            {
                string query = "UPDATE dbo.[Order] SET Frozen = @Frozen WHERE Id_Order = @Id_Order;";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Frozen", SqlDbType.Bit).Value = true;

                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = ord.Id_Order;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "Zamroziles zamowienie " + ord.Id_Order;
                string orderhistory = "Zamoienie zostalo zamrozone";

                insertHistory.Insert_User_History(userhistory, user.Login);
                insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order);

                temp.Add(ord);
                return temp;
            }
            else if (name == "Frozen" && ord.Deletead == false && ord.Released == false && ord.Frozen == true)
            {
                string query = "UPDATE dbo.[Order] SET Frozen = @Frozen WHERE Id_Order = @Id_Order;";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Frozen", SqlDbType.Bit).Value = false;

                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = ord.Id_Order;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "Odmroziles zamowienie " + ord.Id_Order;
                string orderhistory = "Zamowienie zostalo odmrozone";

                insertHistory.Insert_User_History(userhistory, user.Login);
                insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order);

                temp.Add(ord);
                return temp;
            }
            if (name == "Released" && ord.Status == "Done" && ord.Deletead == false && ord.Released == false)
            {
                string query = "UPDATE dbo.[Order] SET Released = @Released WHERE Id_Order = @Id_Order;";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Released", SqlDbType.Bit).Value = true;

                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = ord.Id_Order;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "Zmieniles status zamowienia " + ord.Id_Order + " na dostarczony";
                string orderhistory = "Zamowienie zostalo dostarczone";

                insertHistory.Insert_User_History(userhistory, user.Login);
                insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order);

                temp.Add(ord);
                return temp;
            }
            else if (name == "Released" && ord.Deletead == false && ord.Released == true)
            {
                string query = "UPDATE dbo.[Order] SET Released = @Released WHERE Id_Order = @Id_Order;";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Released", SqlDbType.Bit).Value = false;

                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = ord.Id_Order;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "Zmieniles status zmowienia " + ord.Id_Order + " na gotowe";
                string orderhistory = "Status zamowienia zostal zmieniony na gotowe";

                insertHistory.Insert_User_History(userhistory, user.Login);
                insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order);

                temp.Add(ord);
                return temp;
            }

            ord.Error_Messege = "Nieporawny stan";
            temp.Add(ord);
            return temp;
        }

        public List<Order> ReleasedOrder(User user, Order order, List<Item> items)
        {
            List<Order> temp = new List<Order>();

            string query;
            SqlCommand command;

            foreach (Item item in items)
            {
                if (item.Status == "Wykonany")
                {
                    query = "UPDATE dbo.[Item] SET Status = @Status WHERE Id = @Id;";
                    command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Status", SqlDbType.Bit).Value = "Wydany";
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = item.Id;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    query = "UPDATE dbo.[Product] SET Status = @Status WHERE Id = @Product_Id;";
                    command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Status", SqlDbType.Bit).Value = "Wydany";
                    command.Parameters.Add("@Product_Id", SqlDbType.Int).Value = item.Product_Id;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    string magazinehistory = "Pozycja zamowienia " + item.Id + " z zamowienia "  + order.Id_Order + "zostala wydana";
                    string producthistory = "Produkt zostal wydany";

                    insertHistory.InsertProductHistory(item.Product_Id, user.Login, producthistory);
                    insertHistory.Insert_Magazine_History(magazinehistory, user.Login);
                }
            }

            query = "UPDATE dbo.[Order] SET Released = @Released WHERE Order_Id = @Order_Id;";
            command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Released", SqlDbType.Bit).Value = true;
            command.Parameters.Add("@Order_Id", SqlDbType.Int).Value = order.Id_Order;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            temp.Add(order);

            string userhistory = "Wydales zamowienie " + order.Id_Order;
            string orderhistory = "Zamowienie zostalo wydane";

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Order_History(orderhistory, user.Login, order.Id_Order);

            return temp;
        }

        public List<Item> ReleasedItems(User user, List<Item> temp2)
        {
            List<Item> temp = new List<Item>();
            Item temp_item = new Item();

            foreach (Item item in temp2)
            {
                string query = "UPDATE dbo.[Item] SET Status = @Status WHERE Id = @Id;";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Status", SqlDbType.Bit).Value = "Wydany";
                command.Parameters.Add("@Id", SqlDbType.Int).Value = item.Id;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();
                
                temp.Add(temp_item);

                string userhistory = "Wydales pozycje zamowienia " + item.Id + " z zamowienia " + item.Order_id;
                string orderhistory = "Pozycja zamowienia " + item.Id + " zostala wydana";

                insertHistory.Insert_User_History(userhistory, user.Login);
                insertHistory.Insert_Order_History(orderhistory, user.Login, item.Order_id);
            }
            return temp;          
        }

        public List<Item> Remove_Item(User user, Order order, List<int> items, List<Item> All_items)
        {
            List<Item> temp = new List<Item>();

            foreach (Item itm in All_items)
            {
                foreach (int it in items)
                {
                    if (it == itm.Id)
                    {
                        string query = "UPDATE dbo.[Item] SET Status = @Status WHERE Id = @Id;";
                        SqlCommand command = new SqlCommand(query, connect.cnn);

                        command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Usuniety";
                        command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = itm.Id;

                        connect.cnn.Open();
                        command.ExecuteNonQuery();
                        command.Dispose();
                        connect.cnn.Close();

                        string userhistory = "Usunoles pozycje " + itm.Id + " z zamowienia " + order.Id_Order;
                        string orderhistory = "Pozycja " + itm.Id + " zostala usunieta";

                        insertHistory.Insert_User_History(userhistory, user.Login);
                        insertHistory.Insert_Order_History(orderhistory, user.Login, order.Id_Order);

                        if (itm.Status == "Gotowy")
                        {
                            try
                            {
                                query = "UPDATE dbo.[Product] SET Status = @Status WHERE Id_item = @Id_item and Status = @Status_old;";
                                command = new SqlCommand(query, connect.cnn);

                                command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Usuniety";
                                command.Parameters.Add("@Status_old", SqlDbType.VarChar, 40).Value = "Gotowy";
                                command.Parameters.Add("@Id_item", SqlDbType.VarChar, 40).Value = itm.Id;

                                connect.cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                connect.cnn.Close();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }

                            userhistory = "Usunoles produkt " + itm.Product_Id;
                            string producthistory = "Produkt zostal usuniety";

                            insertHistory.Insert_User_History(userhistory, user.Login);
                            insertHistory.InsertProductHistory(itm.Product_Id, user.Login, producthistory);
                        }
                    }
                }
            }
            return temp;
        }
    }
}
