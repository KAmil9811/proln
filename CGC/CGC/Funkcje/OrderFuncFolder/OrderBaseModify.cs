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

            try
            {
                string query;
                SqlCommand command;
                foreach (Item item in order.items)
                {
                    query = "INSERT INTO dbo.[Item](Id, Weight, Height, Lenght, Glass_Type, Color, Status,Desk, Cut_id, Order_id, Product_Id, Company) VALUES(@Id, @Weight,@Height, @Lenght, @Glass_Type, @Color, @Status, @Desk, @Cut_id, @Order_id, @Product_Id, @Company)";
                    command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = item.Id;
                    command.Parameters.Add("@Weight", SqlDbType.Float).Value = Convert.ToDouble(item.Width);
                    command.Parameters.Add("@Height", SqlDbType.Float).Value = Convert.ToDouble(item.Thickness);
                    command.Parameters.Add("@Lenght", SqlDbType.Float).Value = Convert.ToDouble(item.Length);
                    command.Parameters.Add("@Glass_Type", SqlDbType.VarChar, 40).Value = item.Type;
                    command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = item.Color;
                    command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Awaiting";
                    command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = "";
                    command.Parameters.Add("@Cut_id", SqlDbType.VarChar, 40).Value = "0";
                    command.Parameters.Add("@Order_id", SqlDbType.VarChar, 40).Value = order.Id_Order;
                    command.Parameters.Add("@Product_Id", SqlDbType.VarChar, 40).Value = "0";
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();
                }

                query = "INSERT INTO dbo.[Order](Id_Order,Owner,Status,Priority,Deadline,Stan,Deletead,Frozen,Released, Company) VALUES(@Id_Order, @Owner, @Status, @Priority, @Deadline, @Stan, @Deletead, @Frozen, @Released, @Company)";
                command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;
                command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = order.Owner;
                command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Awaiting";
                command.Parameters.Add("@Priority", SqlDbType.VarChar, 40).Value = order.Priority;
                command.Parameters.Add("@Deadline", SqlDbType.VarChar, 40).Value = order.Deadline;
                command.Parameters.Add("@Stan", SqlDbType.VarChar, 40).Value = orderCheck.Avaible_Cut(order, user).ToString() + "/" + order.items.Count + "/" + "0";
                command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = false;
                command.Parameters.Add("@Frozen", SqlDbType.Bit).Value = false;
                command.Parameters.Add("@Released", SqlDbType.Bit).Value = false;
                command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "You added order " + order.Id_Order;
                string orderhistory = "Order has been added";

                insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                insertHistory.Insert_Order_History(orderhistory, user.Login, order.Id_Order, user.Company);

                temp.Add(order);

                return temp;
            }
            catch
            {
                return temp;
            }
        }

        public List<Order> Edit_Order(User user, Order order)
        {
            List<Order> temp = new List<Order>();

            try
            {
                string query = "UPDATE dbo.[Order] SET Deadline = @Deadline, Owner = @Owner, Priority = @Priority WHERE Id_Order = @Id_Order AND Company = @Company;";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Deadline", SqlDbType.VarChar, 40).Value = order.Deadline;
                command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = order.Owner;
                command.Parameters.Add("@Priority", SqlDbType.VarChar, 40).Value = order.Priority;

                command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;
                command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "You edited order" + order.Id_Order;
                string orderhistory = "Order has been edited";

                insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                insertHistory.Insert_Order_History(orderhistory, user.Login, order.Id_Order, user.Company);

                temp.Add(order);
                return temp;
            }
            catch
            {
                return temp;
            }
        }

        public List<Order> Edit_Order_Items(User user, Order order, Item item)
        {
            List<Order> temp = new List<Order>();
            try
            {               
                string query = "UPDATE dbo.[Item] SET Height = @Height, Lenght = @Lenght, Weight = @Weight, Glass_Type = @Glass_Type, Color = @Color, Status = @Status, Desk = @Desk WHERE Id = @Id AND Company = @Company;";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Height", SqlDbType.Float).Value = Convert.ToDouble(item.Thickness);
                command.Parameters.Add("@Lenght", SqlDbType.Float).Value = Convert.ToDouble(item.Length);
                command.Parameters.Add("@Weight", SqlDbType.Float).Value = Convert.ToDouble(item.Width);
                command.Parameters.Add("@Glass_Type", SqlDbType.VarChar, 40).Value = item.Type;
                command.Parameters.Add("@Color", SqlDbType.VarChar, 40).Value = item.Color;
                command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = item.Status;
                command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = item.Desk;

                command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = item.Id;
                command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string userhistory = "You edited Item " + item.Id + " from order " + order.Id_Order;
                string orderhistory = "Item has been edited " + item.Id;

                insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                insertHistory.Insert_Order_History(orderhistory, user.Login, order.Id_Order, user.Company);


                temp.Add(order);
                return temp;
            }
            catch
            {
                return temp;
            }
        }

        public List<Order> Set_stan(User user, Order ord, string name)
        {
            List<Order> temp = new List<Order>();

            try
            {
                if (name == "Deleted" && ord.Deleted == false && ord.Released == false)
                {
                    string query = "UPDATE dbo.[Order] SET Deletead = @Deletead WHERE Id_Order = @Id_Order AND Company = @Company;";
                    SqlCommand command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = true;

                    command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = ord.Id_Order;
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    string userhistory = "You deleted order " + ord.Id_Order;
                    string orderhistory = "Order has been deleted";

                    insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                    insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order, user.Company);

                    temp.Add(ord);
                    return temp;
                }
                else if (name == "Deletead" && ord.Deleted == true && ord.Released == false && ord.Frozen == false)
                {
                    string query = "UPDATE dbo.[Order] SET Deletead = @Deleted WHERE Id_Order = @Id_Order AND Company = @Company;";
                    SqlCommand command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Deletead", SqlDbType.Bit).Value = false;

                    command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = ord.Id_Order;
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    string userhistory = "You restored order " + ord.Id_Order;
                    string orderhistory = "Order has been restored";

                    insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                    insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order, user.Company);

                    temp.Add(ord);
                    return temp;
                }
                if (name == "Frozen" && ord.Deleted == false && ord.Released == false && ord.Frozen == false)
                {
                    string query = "UPDATE dbo.[Order] SET Frozen = @Frozen WHERE Id_Order = @Id_Order AND Company = @Company;";
                    SqlCommand command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Frozen", SqlDbType.Bit).Value = true;

                    command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = ord.Id_Order;
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    string userhistory = "You froze order " + ord.Id_Order;
                    string orderhistory = "Order has been frozen";

                    insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                    insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order, user.Company);

                    temp.Add(ord);
                    return temp;
                }
                else if (name == "Frozen" && ord.Deleted == false && ord.Released == false && ord.Frozen == true)
                {
                    string query = "UPDATE dbo.[Order] SET Frozen = @Frozen WHERE Id_Order = @Id_Order AND Company = @Company;";
                    SqlCommand command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Frozen", SqlDbType.Bit).Value = false;

                    command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = ord.Id_Order;
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    string userhistory = "You unfreezed order " + ord.Id_Order;
                    string orderhistory = "Order has been unfreezed";

                    insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                    insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order, user.Company);

                    temp.Add(ord);
                    return temp;
                }
                if (name == "Released" && ord.Status == "Done" && ord.Deleted == false && ord.Released == false)
                {
                    string query = "UPDATE dbo.[Order] SET Released = @Released WHERE Id_Order = @Id_Order AND Company = @Company;";
                    SqlCommand command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Released", SqlDbType.Bit).Value = true;

                    command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = ord.Id_Order;
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    string userhistory = "You change order status " + ord.Id_Order + " to released";
                    string orderhistory = "Order has been released";

                    insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                    insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order, user.Company);

                    temp.Add(ord);
                    return temp;
                }
                else if (name == "Released" && ord.Deleted == false && ord.Released == true)
                {
                    string query = "UPDATE dbo.[Order] SET Released = @Released WHERE Id_Order = @Id_Order AND Company = @Company;";
                    SqlCommand command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Released", SqlDbType.Bit).Value = false;

                    command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = ord.Id_Order;
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    string userhistory = "You changed order status " + ord.Id_Order + " to ready";
                    string orderhistory = "Order status has been changed to ready";

                    insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                    insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order, user.Company);

                    temp.Add(ord);
                    return temp;
                }

                ord.Error_Messege = "Incorrect status";
                temp.Add(ord);
                return temp;
            }
            catch
            {
                ord.Error_Messege = "Something went wrong";
                temp.Add(ord);
                return temp;
            }
        }

        public List<Order> ReleasedOrder(User user, Order order, List<Item> items)
        {
            List<Order> temp = new List<Order>();

            string query;
            SqlCommand command;

            foreach (Item item in items)
            {
                if (item.Status == "Ready")
                {
                    return temp;
                }
            }

            try
            {

                foreach (Item item in items)
                {
                    query = "UPDATE dbo.[Item] SET Status = @Status WHERE Id = @Id AND Company = @Company;";
                    command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Status", SqlDbType.Bit).Value = "Released";
                    command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = item.Id;
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    query = "UPDATE dbo.[Product] SET Status = @Status WHERE Id = @Product_Id AND Company = @Company;";
                    command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Status", SqlDbType.Bit).Value = "Released";
                    command.Parameters.Add("@Product_Id", SqlDbType.VarChar, 40).Value = item.Product_Id;
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    string magazinehistory = "Item " + item.Id + " from order " + order.Id_Order + " has been released";
                    string producthistory = "Product has been released";

                    insertHistory.InsertProductHistory(item.Product_Id, user.Login, producthistory, user.Company);
                    insertHistory.Insert_Magazine_History(magazinehistory, user.Login, user.Company);
                }

                query = "UPDATE dbo.[Order] SET Released = @Released WHERE Order_Id = @Order_Id AND Company = @Company;";
                command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Released", SqlDbType.Bit).Value = true;
                command.Parameters.Add("@Order_Id", SqlDbType.VarChar, 40).Value = order.Id_Order;
                command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                temp.Add(order);

                string userhistory = "You changed order status " + order.Id_Order + " to released";
                string orderhistory = "Order has been released";

                insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                insertHistory.Insert_Order_History(orderhistory, user.Login, order.Id_Order, user.Company);

                return temp;
            }
            catch
            {
                return temp;
            }
        }

        public List<Item> ReleasedItems(User user, List<Item> temp2)
        {
            List<Item> temp = new List<Item>();
            Item temp_item = new Item();

            try
            {
                foreach (Item item in temp2)
                {
                    string query = "UPDATE dbo.[Item] SET Status = @Status WHERE Id = @Id AND Company = @Company;";
                    SqlCommand command = new SqlCommand(query, connect.cnn);

                    command.Parameters.Add("@Status", SqlDbType.Bit).Value = "Released";
                    command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = item.Id;
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();

                    temp.Add(temp_item);

                    string userhistory = "You changed Item: " + item.Id + "status from order " + item.Order_id + " to released";
                    string orderhistory = "Item " + item.Id + " has been released";

                    insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                    insertHistory.Insert_Order_History(orderhistory, user.Login, item.Order_id, user.Company);
                }
                return temp;
            }
            catch
            {
                return temp;
            }
        }

        public List<Item> Remove_Item(User user, Order order, List<int> items, List<Item> All_items)
        {
            List<Item> temp = new List<Item>();

            try
            {
                foreach (Item itm in All_items)
                {
                    foreach (int it in items)
                    {
                        if (it.ToString() == itm.Id)
                        {
                            string query = "UPDATE dbo.[Item] SET Status = @Status WHERE Id = @Id AND Company = @Company;";
                            SqlCommand command = new SqlCommand(query, connect.cnn);

                            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Deleted";
                            command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = itm.Id;
                            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                            connect.cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            connect.cnn.Close();

                            string userhistory = "You deleted Item " + itm.Id + " from order " + order.Id_Order;
                            string orderhistory = "Item " + itm.Id + " has been deleted";

                            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                            insertHistory.Insert_Order_History(orderhistory, user.Login, order.Id_Order, user.Company);

                            if (itm.Status == "Ready")
                            {
                                try
                                {
                                    query = "UPDATE dbo.[Product] SET Status = @Status WHERE Id_item = @Id_item and Status = @Status_old AND Company = @Company;";
                                    command = new SqlCommand(query, connect.cnn);

                                    command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Deleted";
                                    command.Parameters.Add("@Status_old", SqlDbType.VarChar, 40).Value = "Ready";
                                    command.Parameters.Add("@Id_item", SqlDbType.VarChar, 40).Value = itm.Id;
                                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                                    connect.cnn.Open();
                                    command.ExecuteNonQuery();
                                    command.Dispose();
                                    connect.cnn.Close();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.ToString());
                                }

                                userhistory = "You deleted product " + itm.Product_Id;
                                string producthistory = "Product has been deleted";

                                insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
                                insertHistory.InsertProductHistory(itm.Product_Id, user.Login, producthistory, user.Company);
                            }
                        }
                    }
                }
                return temp;
            }
            catch
            {
                return temp;
            }
        }
    }
}
