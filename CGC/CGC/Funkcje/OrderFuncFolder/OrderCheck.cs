using CGC.Funkcje.MagazineFuncFolder.MagazineBase;
using CGC.Funkcje.OrderFuncFolder.OrderBase;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.OrderFuncFolder
{
    public class OrderCheck
    {
        private static OrderCheck m_oInstance = null;
        private static readonly object m_oPadLock = new object();
        private OrderBaseReturn orderBaseReturn = new OrderBaseReturn();
        private MagazineBaseReturn magazineBaseReturn = new MagazineBaseReturn();

        public static OrderCheck Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new OrderCheck();
                    }
                    return m_oInstance;
                }
            }
        }

        public int Avaible_Cut(Order order, User user)
        {
            int count = 0;
            foreach (Item item in orderBaseReturn.GetItems(order, user.Company))
            {
                if (item.Status == "Awaiting" && item.Cut_id == "0")
                {
                    foreach (Glass glass in magazineBaseReturn.Getglass(user.Company))
                    {
                        if (Convert.ToDouble(item.Width) <= Convert.ToDouble(glass.Width) && Convert.ToDouble(item.Thickness) == Convert.ToDouble(glass.Hight) && Convert.ToDouble(item.Length) <= Convert.ToDouble(glass.Length) && glass.Color == item.Color && glass.Type == item.Type && (glass.Owner == order.Owner || glass.Owner == ""))
                        {
                            foreach (Glass_Id glass_Id in glass.Glass_info)
                            {
                                if (glass_Id.Used == false && glass_Id.Removed == false && glass_Id.Cut_id == "0")
                                {
                                    count++;
                                }
                            }
                        }
                    }
                }
            }
            return count;
        }

        public bool Avaible_Cut_Check(Order order, User user)
        {
            List<Glass> temp;
            List<Item> temp2 = orderBaseReturn.GetItems(order, user.Company);

            temp2.OrderBy(it => it.WidthSort).ThenBy(it => it.LengthSort);

            foreach (Item item in temp2)
            {
                if (item.Status == "Awaiting" && (item.Cut_id == "0" || item.Cut_id == ""))
                {
                    Glass glasse = new Glass { Color = item.Color, Type = item.Type, Hight = item.Thickness, Owner = order.Owner, Length = item.Length, Width = item.Width};
                    temp = magazineBaseReturn.Getglass(glasse, user.Company);
                    temp.OrderBy(gl => gl.WidthSort).ThenBy(gl => gl.LengthSort);
                    

                    foreach (Glass glass in temp)
                    {
                        if (Convert.ToDouble(item.Width) <= Convert.ToDouble(glass.Width) && Convert.ToDouble(item.Length) <= Convert.ToDouble(glass.Length))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public int Item_To_Cut(Order order)
        {
            int count = 0;
            foreach (Item item in order.items)
            {
                if (item.Status == "Awaiting")
                {
                    count++;
                }
            }
            return count;
        }

        public int Item_To_In_Cut(Order order)
        {
            int count = 0;
            foreach (Item item in order.items)
            {
                if (item.Status == "In use")
                {
                    count++;
                }
            }
            return count;
        }

    }
}
