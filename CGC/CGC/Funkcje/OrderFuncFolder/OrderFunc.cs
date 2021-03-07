using CGC.Funkcje.OrderFuncFolder.OrderBase;
using CGC.Funkcje.UserFuncFolder;
using CGC.Funkcje.UserFuncFolder.UserReturn;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.OrderFuncFolder
{
    public class OrderFunc
    {
        private static OrderFunc m_oInstance = null;
        private static readonly object m_oPadLock = new object();
        private UserBaseReturn userBaseReturn = new UserBaseReturn();
        private OrderBaseReturn orderBaseReturn = new OrderBaseReturn();
        private OrderBaseModify orderBaseModify = new OrderBaseModify();
        private OrderCheck orderCheck = new OrderCheck();

        public static OrderFunc Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new OrderFunc();
                    }
                    return m_oInstance;
                }
            }
        }

        public List<Order> Return_All_Orders(Receiver receiver)
        {
            return orderBaseReturn.GetOrders(false, false, receiver.user.Company);
        }

        public List<Item> Return_All_Items(Receiver receiver)
        {            
            return orderBaseReturn.GetItems(receiver.order, receiver.user.Company);
        }
        
        public List<Order_History> Return_Order_History(Receiver receiver)
        {
            return orderBaseReturn.Return_Order_History(receiver.user.Company);
        }

        public List<Order> Add_Order(Receiver receiver)
        {
            List<Order> temp = new List<Order>();
            List<Order> temp2 = new List<Order>();
            List<Item> itemstemp = new List<Item>();

            Order order = receiver.order;
            order.items = new List<Item>();
            User user = receiver.user;
            int code, last_free_id;
            string code2;

            try
            {
                last_free_id = orderBaseReturn.GetLastItem(user.Company).Last().sort + 1;
            }
            catch
            {
                last_free_id = 1;
            }

            for (int i = 0; i <= receiver.iteme.Count; i += 6)
            {
                Item item = new Item { Width = receiver.iteme[0], Length = receiver.iteme[1], Thickness = receiver.iteme[2], Color = receiver.iteme[3], Amount = receiver.iteme[4], Type = receiver.iteme[5] };
                order.items.Add(item);
            }

            try
            {
                temp2 = orderBaseReturn.GetOrders(user.Company);

                foreach (Order orderer in temp2)
                {
                    orderer.Priority = orderer.Id_Order;
                }
                code = orderBaseReturn.GetLastOrder(user.Company).Last().sort + 1;
            }
            catch
            {
                code = 1;
            }


            order.Id_Order = code.ToString();

            foreach (Item item in order.items)
            {
                if (Convert.ToInt32(item.Amount) == 1)
                {
                    item.Status = "Awaiting";
                    item.Can_Be_Createad = false;
                    item.Id = last_free_id.ToString();
                    last_free_id++;
                    item.Amount = (0).ToString();
                }
                else if (Convert.ToInt32(item.Amount) > 1)
                {
                    item.Status = "Awaiting";
                    item.Can_Be_Createad = false;

                    item.Id = last_free_id.ToString();
                    last_free_id++;

                    while (Convert.ToInt32(item.Amount) > 1)
                    {
                        Item new_item = new Item();
                        new_item.Width = item.Width;
                        new_item.Length = item.Length;
                        new_item.Can_Be_Createad = false;
                        new_item.Color = item.Color;
                        new_item.Desk = item.Desk;
                        new_item.Order_id = item.Order_id;
                        new_item.Thickness = item.Thickness;
                        new_item.Type = item.Type;

                        new_item.Id = last_free_id.ToString();
                        last_free_id++;
                        new_item.Amount = (0).ToString();
                        itemstemp.Add(new_item);
                        item.Amount = (Convert.ToInt32(item.Amount) - 1).ToString();
                    }
                }
            }

            foreach (Item item1 in itemstemp)
            {
                order.items.Add(item1);
            }

            if (order.Priority.ToString() == null)
            {
                order.Priority = "5";
            }


            foreach (User usere in userBaseReturn.GetUser(user.Login,false, user.Company))
            {
                if (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Order_management == true)
                {
                    string LastGlobalIdOrder = orderBaseReturn.GetLastGlobalIdOrder(user.Company).Last().Global_Id.ToString();
                    string LastGlobalIdItem = orderBaseReturn.GetLastGlobalIdItem(user.Company).Last().Global_Id.ToString();
                    return orderBaseModify.Add_Order(usere, order, LastGlobalIdOrder, LastGlobalIdItem);
                }
            }

            order.Error_Messege = "User not exist";
            temp.Add(order);
            return temp;
        }

        public List<Order> Edit_Order(Receiver receiver)
        {
            List<Order> temp = new List<Order>();

            Order order = receiver.order;
            User user = receiver.user;

            foreach (User usere in userBaseReturn.GetUser(user.Login, user.Company))
            {
                if (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Order_management == true)
                {
                    return orderBaseModify.Edit_Order(usere, order);
                }
            }
            order.Error_Messege = "User not exist";
            temp.Add(order);
            return temp;
        }
       
        public List<Order> Edit_Order_Items(Receiver receiver)
        {
            List<Order> temp = new List<Order>();

            Order order = receiver.order;
            User user = receiver.user;
            Item items = receiver.item;

            foreach (User usere in userBaseReturn.GetUser(user.Login, false, user.Company))
            {
                if (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Order_management == true)
                {
                    foreach (Item item in orderBaseReturn.GetItems(order, user.Company))
                    {
                        if (item.Id == items.Id)
                        {
                            orderBaseModify.Edit_Order_Items(usere, order, items);
                        }
                    }
                    temp.Add(order);
                    return temp;
                }
                order.Error_Messege = "User no permission";
                temp.Add(order);
                return temp;
            }
            order.Error_Messege = "User not exist";
            temp.Add(order);
            return temp;
        }

        public List<Order> Set_Stan(Receiver receiver)
        {
            List<Order> temp = new List<Order>();

            Order order = receiver.order;
            User user = receiver.user;
            string name = receiver.new_name;

            foreach (User usere in userBaseReturn.GetUser(user.Login, false, user.Company))
            {
                if (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Order_management == true)
                {
                    foreach (Order ord in orderBaseReturn.GetOrder(order.Id_Order, user.Company))
                    {
                        return orderBaseModify.Set_stan(usere, ord, name);
                    }
                }
                order.Error_Messege = "User no permission";
                temp.Add(order);
                return temp;
            }

            order.Error_Messege = "User not exist";
            temp.Add(order);
            return temp;
        }

        public List<Order> ReleasedOrder(User user, Order order)
        {
            List<Order> temp = new List<Order>();

            if(orderBaseReturn.GetOrder(order.Id_Order, user.Company).First().Status != "Ready")
            {
                order.Error_Messege = "Order is not ready";
                temp.Add(order);
                return temp;
            }

            foreach (User use in userBaseReturn.GetUser(user.Login, false, user.Company))
            {
                if (use.Manager == true || use.Super_Admin == true || use.Admin == true || use.Order_management == true)
                {
                    return orderBaseModify.ReleasedOrder(use, order, orderBaseReturn.GetItems(order, user.Company));
                }
                order.Error_Messege = "User no permission";
                temp.Add(order);
                return temp;
            }
            order.Error_Messege = "User not exist";
            temp.Add(order);
            return temp;
        }

        public List<Item> ReleasedItems(User user, List<Item> items)
        {
            List<Item> temp = new List<Item>();
            Item temp_item = new Item();
            Order order = new Order { Id_Order = items.First().Order_id };
            List<Item> temp2 = orderBaseReturn.GetItems(order, user.Company);


            foreach (Item item in orderBaseReturn.GetItems(order, user.Company))
            {
                foreach (Item itm in items)
                {
                    if (item.Id == itm.Id)
                    {
                        if (item.Status != "Ready")
                        {
                            temp_item.Error_Messege = "Items are not ready";
                            temp.Add(temp_item);
                            return temp;
                        }
                        break;
                    }
                }
            }

            foreach (User use in userBaseReturn.GetUser(user.Login, false, user.Company))
            {
                if (use.Manager == true || use.Super_Admin == true || use.Admin || use.Order_management == true)
                {
                    return orderBaseModify.ReleasedItems(use, items);
                }
                temp_item.Error_Messege = "User no permission";
                temp.Add(temp_item);
                return temp;
            }
            temp_item.Error_Messege = "User not exist";
            temp.Add(temp_item);
            return temp;
        }

        public List<Item> Remove_Item(Receiver receiver)
        {
            List<string> tempo = receiver.item_Id;
            List<int> items = new List<int>();

            foreach(string t in tempo)
            {
                items.Add(Convert.ToInt32(t));
            }

            List<Item> temp = new List<Item>();
            User user = receiver.user;

            foreach (Item item in orderBaseReturn.GetItems(receiver.order, user.Company))
            {
                foreach (int it in items)
                {
                    if (it.ToString() == item.Id)
                    {
                        if (item.Status == "Deleted")
                        {
                            item.Error_Messege = "Items have been already deleted";
                            temp.Add(item);
                            return temp;
                        }
                        break;
                    }
                }
            }

            foreach (User usere in userBaseReturn.GetUser(user.Login, false, user.Company))
            {
                if (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Order_management == true)
                {
                    return orderBaseModify.Remove_Item(usere, receiver.order, items, orderBaseReturn.GetItems(receiver.order, user.Company));
                }
                Item iteme2 = new Item();
                iteme2.Error_Messege = "User no permission";
                temp.Add(iteme2);
            }

            Item iteme = new Item();
            iteme.Error_Messege = "User not found";
            temp.Add(iteme);

            return temp;
        }
    }
}
