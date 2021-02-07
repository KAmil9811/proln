using CGC.Funkcje.MachineFuncFolder.MachineBase;
using CGC.Funkcje.MagazineFuncFolder;
using CGC.Funkcje.MagazineFuncFolder.MagazineBase;
using CGC.Funkcje.OrderFuncFolder;
using CGC.Funkcje.OrderFuncFolder.OrderBase;
using CGC.Models;
using Sharp3DBinPacking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.CutFuncFolder
{
    public class CutCheck
    {
        private static CutCheck m_oInstance = null;
        private static readonly object m_oPadLock = new object();
        private OrderCheck orderCheck = new OrderCheck();
        private OrderBaseReturn orderBaseReturn = new OrderBaseReturn();
        private MagazineBaseReturn magazineBaseReturn = new MagazineBaseReturn();
        private MagazineCheck magazineCheck = new MagazineCheck();
        private MachineBaseReturn machineBaseReturn = new MachineBaseReturn();

        public static CutCheck Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new CutCheck();
                    }
                    return m_oInstance;
                }
            }
        }

        //do usunięcia
        public void Return_Area(Package package)
        {
            foreach (Item item in package.Item)
            {
                item.Area = Convert.ToDouble(item.Length) * Convert.ToDouble(item.Width);
            }
        }

        //do usunięcia
        public void Set_Package(Package package)
        {
            string temp;

            foreach (Item item in package.Item)
            {
                if (Convert.ToDouble(item.Width) > Convert.ToDouble(item.Length))
                {
                    temp = item.Length;
                    item.Length = item.Width;
                    item.Width = temp;
                }
            }
        }
        
        //do usunięcia
        public void Sort_Package(Package package)
        {
            package.Item.OrderByDescending(item => item.Fit_pos).ThenBy(item => item.Area).ThenBy(item => item.Length).ThenBy(item => item.Width);
        }
       
        //do usunięcia
        public void Set_Pieces(List<Piece> pieces)
        {
            double temp;

            foreach (Piece piece in pieces)
            {
                temp = piece.X;
                piece.X = piece.Y;
                piece.Y = temp;
            }
        }
        
        //do usunięcia
        public void Set_Fit(Package package, double Pw, double Pl)
        {
            int Fit;

            foreach (Item item in package.Item)
            {
                Fit = 0;
                if (Pl < Convert.ToDouble(item.Length) || Pw < Convert.ToDouble(item.Width))
                {
                    Fit = 0;
                }
                else
                {
                    Fit++;
                    if (Pl == Convert.ToDouble(item.Length))
                    {
                        Fit++;
                    }
                    if (Pw == Convert.ToDouble(item.Width))
                    {
                        Fit++;
                    }
                }
                item.Fit_pos = Fit;
            }
        }

        //do usunięcia
        public Item Find_Area(Package package)
        {
            double wynik;
            int temp = package.Item.Count;

            while (temp > 0)
            {
                if (package.Item.ElementAt(temp - 1).Fit_pos > 0)
                {
                    return package.Item.ElementAt(temp - 1);
                }
                temp--;
            }

            return package.Item.Last();
        }

        public List<Order> Return_Orders_To_Cut()
        {
            List<Order> orders = new List<Order>();
            List<Order> sort_orders = new List<Order>();

            foreach (Order order in orderBaseReturn.GetOrders("Awaiting", "Stopped", false, false))
            {
                if (orderCheck.Avaible_Cut_Check(order) == true)
                {
                    order.Deadline2 = Convert.ToDateTime(order.Deadline);
                    orders.Add(order);
                }           
            }

            //sort_orders = (List<Order>)orders.OrderBy(orderer => orderer.Deadline2);

            return orders; //sort_orders;
        }

        public List<Package> Return_Package_To_Cut(Receiver receiver)
        {
            Order order = receiver.order;
            List<Package> temp = new List<Package>();
            List<Package> wynik = new List<Package>();
            bool kontrol;

            foreach (Order ord in orderBaseReturn.GetOrder(order.Id_Order))
            {
                order.Owner = ord.Owner;
                break;
            }

            foreach(Item item in orderBaseReturn.GetItems(order))
            {
                if (item.Status == "Awaiting" && item.Cut_id == "0")
                {
                    kontrol = false;
                    if (temp.Count != 0)
                    {
                        foreach (Package package in temp)
                        {
                            if (package.Color == item.Color && package.Type == item.Type && Convert.ToDouble(item.Thickness) == package.Thickness)
                            {
                                kontrol = true;
                                package.Item.Add(item);
                            }
                        }
                    }

                    if (kontrol == false)
                    {
                        Package package = new Package { Color = item.Color, Type = item.Type, Id_Order = order.Id_Order, Thickness = Convert.ToDouble(item.Thickness), Item = new List<Item>(), Owner = order.Owner };
                        package.Item.Add(item);

                        temp.Add(package);
                    }
                }
            }

            foreach (Package pack in temp)
            {
                foreach (Item item in pack.Item)
                {
                    kontrol = false;
                    Glass glasss = new Glass { Type = item.Type, Color = item.Color, Hight = item.Thickness, Owner = order.Owner, Length = item.Length, Width = item.Width };

                    foreach (Glass glass in magazineBaseReturn.Getglass(glasss))
                    {
                        wynik.Add(pack);
                        kontrol = true;
                        break;
                    }               
                    if(kontrol == true)
                    {
                        break;
                    }
                }
            }
            return wynik;
        }

        public List<Glass> Return_Glass_To_Cut(Receiver receiver)
        {
            Package package = receiver.package;
            List<Glass> glasses = new List<Glass>();
            List<Item> Sort__items;

            Sort__items = (List<Item>)package.Item.OrderBy(ordere => ordere.Width).ThenBy(ordere => ordere.Length);

            Glass glasss = new Glass { Type = package.Type, Color = package.Color, Hight = package.Thickness.ToString() , Length = Sort__items.First().Length, Width = Sort__items.First().Width};

            foreach (Glass glasse in magazineBaseReturn.Getglass(glasss))
            {
                if (Convert.ToDouble(Sort__items.First().Length) <= Convert.ToDouble(glasse.Length) && Convert.ToDouble(Sort__items.First().Width) <= Convert.ToDouble(glasse.Width))
                {
                    glasses.Add(glasse);
                }
            }
            return glasses;
        }

        public List<Machines> Return_Machine_To_Cut()
        {
            return machineBaseReturn.GetMachines("Ready", false);
        }

        public CutBin Packing(CutBin cutBin)
        {
            try
            {
                List<Item> To_delete = new List<Item>();
                var binPacker = BinPacker.GetDefault(BinPackerVerifyOption.BestOnly);

                var result = binPacker.Pack(cutBin.Parameter);

                cutBin.result = result;
                bool kon;
                int count = 0;

                foreach (Item itm in cutBin.package.Item)
                {
                    Item it = new Item { Width = itm.Width, Length = itm.Length, Id = itm.Id };
                    To_delete.Add(it);
                }

                foreach (var bin in cutBin.result.BestResult)
                {
                    count++;
                    foreach (Cuboid cuboid in bin)
                    {
                        foreach (Item itm in cutBin.package.Item)
                        {
                            if (Convert.ToDecimal(itm.Width) == cuboid.Width && Convert.ToDecimal(itm.Length) == cuboid.Height)
                            {
                                kon = false;
                                foreach (Item i in To_delete)
                                {
                                    if (i.Id == itm.Id)
                                    {
                                        kon = true;
                                    }
                                }
                                if (kon == true)
                                {
                                    To_delete.RemoveAll(x => x.Id == itm.Id);
                                    kon = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                cutBin.package.Item.Clear();

                cutBin.package.Item = To_delete;
                return cutBin;
            }
            catch (Exception e)
            {
                CutBin cutBinErr = new CutBin();

                cutBin.package.Item.RemoveAt(cutBin.package.Item.Count - 1);

                List<Cuboid> temporary = new List<Cuboid>();
                foreach (Item itm in cutBin.package.Item)
                {
                    temporary.Add(new Cuboid(Convert.ToDecimal(itm.Width), Convert.ToDecimal(itm.Length), Convert.ToDecimal(itm.Thickness)));
                }

                cutBinErr.package = cutBin.package;

                cutBinErr.Parameter = new BinPackParameter(cutBin.Parameter.BinWidth, cutBin.Parameter.BinHeight, cutBin.Parameter.BinDepth, temporary);

                return Packing(cutBinErr);
            }
        }
    
        public void Los_Rgb(List<Piece> pieces)
        {
            Random random = new Random();
            if (pieces != null)
            {
                foreach (Piece piece in pieces)
                {
                    piece.Rgb.Add(random.Next(0, 255));
                    piece.Rgb.Add(random.Next(0, 255));
                    piece.Rgb.Add(random.Next(0, 255));
                }
            }
        }
    }
}
