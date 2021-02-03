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

        public List<Order> Return_All_Orders()
        {
            return orderBaseReturn.GetOrdersUser();
        }

        public List<Item> Return_All_Items(Receiver receiver)
        {            
            return orderBaseReturn.GetItems(receiver.order);
        }
        
        public List<Order_History> Return_Order_History(Receiver receiver)
        {
            return orderBaseReturn.Return_Order_History(receiver.order.Id_Order);
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
                last_free_id = Convert.ToInt32(orderBaseReturn.GetAllItems().Last().Id) + 1;
            }
            catch (Exception e)
            {
                last_free_id = 1;
            }

            for (int i = 0; i <= receiver.iteme.Count; i= i +6)
            {
                Item item = new Item { Width = Convert.ToDouble(receiver.iteme[0]), Length = Convert.ToDouble(receiver.iteme[1]), Thickness = Convert.ToDouble(receiver.iteme[2]), Color = receiver.iteme[3], Amount = receiver.iteme[4], Type = receiver.iteme[5] };
                order.items.Add(item);
            }

            //foreach (Item item in receiver.iteme)
            //{
            //    order.items.Add(item);
            //}

            try
            {
                temp2 = orderBaseReturn.GetOrders();

                foreach (Order orderer in temp2)
                {
                    orderer.Priority = orderer.Id_Order;
                }
                code2 = temp2.OrderBy(ord => ord.Priority).Last().Id_Order;
                code = Int32.Parse(code2) + 1;
            }
            catch (Exception e)
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


            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Order_management == true))
                {
                    return orderBaseModify.Add_Order(usere, order);
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

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Order_management == true))
                {
                    orderBaseModify.Edit_Order(usere, order);
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

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Order_management == true))
                {
                    foreach (Item item in orderBaseReturn.GetItems(order))
                    {
                        if (item.Id == items.Id)
                        {
                            return orderBaseModify.Edit_Order_Items(usere, order, items);
                        }
                    }
                    order.Error_Messege = "Item not exist";
                    temp.Add(order);
                    return temp;
                }
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

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Order_management == true))
                {
                    foreach (Order ord in orderBaseReturn.GetOrders())
                    {
                        if (ord.Id_Order == order.Id_Order)
                        {
                            orderBaseModify.Set_stan(usere, ord, name);
                        }
                    }
                }
            }

            order.Error_Messege = "User not exist";
            temp.Add(order);
            return temp;
        }

        public List<Order> ReleasedOrder(User user, Order order)
        {
            List<Order> temp = new List<Order>();

            foreach (User use in userBaseReturn.GetUsers())
            {
                if (use.Login == user.Login && (use.Manager == true || use.Super_Admin == true || use.Admin || use.Order_management == true))
                {
                    if (order.Status == "Ready")
                    {
                        orderBaseModify.ReleasedOrder(use, order, orderBaseReturn.GetItems(order));
                    }
                    order.Error_Messege = "Order is not ready";
                    temp.Add(order);
                    return temp;
                }
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
            List<Item> temp2 = orderBaseReturn.GetItems(order);

            foreach (Item itm in items) 
            {
                foreach (Item item in orderBaseReturn.GetItems(order))
                {
                    if (item.Status != "Ready" && item.Id == itm.Id)
                    {
                        temp_item.Error_Messege = "Items are not ready";
                        temp.Add(temp_item);
                        return temp;
                    }
                }
            }

            foreach (User use in userBaseReturn.GetUsers())
            {
                if (use.Login == user.Login && (use.Manager == true || use.Super_Admin == true || use.Admin || use.Order_management == true))
                {
                    orderBaseModify.ReleasedItems(use, items);
                }
            }
            temp_item.Error_Messege = "User not exist";
            temp.Add(temp_item);
            return temp;
        }

        public List<Item> Remove_Item(Receiver receiver)
        {
            List<int> items = receiver.item_Id;
            List<Item> temp = new List<Item>();
            User user = receiver.user;

            foreach (Item item in orderBaseReturn.GetItems(receiver.order))
            {
                foreach (int it in items)
                {
                    if (it.ToString() == item.Id && item.Status == "Deleted")
                    {         
                        item.Error_Messege = "Items have been already deleted";
                        temp.Add(item);
                        return temp;                   
                    }
                }
            }

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Order_management == true))
                {
                    orderBaseModify.Remove_Item(usere, receiver.order, items, orderBaseReturn.GetItems(receiver.order));
                }
            }

            Item iteme = new Item();
            iteme.Error_Messege = "User not found";
            temp.Add(iteme);

            return temp;
        }
    }
}
