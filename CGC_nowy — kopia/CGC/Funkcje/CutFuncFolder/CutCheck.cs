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

        public void Return_Area(Package package)
        {
            foreach (Item item in package.Item)
            {
                item.Area = item.Length * item.Width;
            }
        }

        public void Set_Package(Package package)
        {
            double temp;

            foreach (Item item in package.Item)
            {
                if (item.Width > item.Length)
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

        public void Set_Fit(Package package, double Pw, double Pl)
        {
            int Fit;

            foreach (Item item in package.Item)
            {
                Fit = 0;
                if (Pl < item.Length || Pw < item.Width)
                {
                    Fit = 0;
                }
                else
                {
                    Fit++;
                    if (Pl == item.Length)
                    {
                        Fit++;
                    }
                    if (Pw == item.Width)
                    {
                        Fit++;
                    }
                }
                item.Fit_pos = Fit;
            }
        }

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
            List<Order> temp = new List<Order>();

            foreach (Order order in orderBaseReturn.GetOrders())
            {
                if (order.Status == "Oczekujący" || order.Status == "Zatrzymany")
                {
                    if (orderCheck.Avaible_Cut(order) > 0)
                    {
                        orders.Add(order);
                    }
                }
            }

            foreach (Order order in orders)
            {
                order.Deadline2 = Convert.ToDateTime(order.Deadline);
            }

            orders.OrderBy(orderer => orderer.Deadline2);

            return orders;
        }

        public List<Package> Return_Package_To_Cut(Receiver receiver)
        {
            Order order = receiver.order;
            List<Package> temp = new List<Package>();
            bool kontrol;
            bool kontrol2;

            foreach (Item item in orderBaseReturn.GetItems(order))
            {
                kontrol = false;
                kontrol2 = false;

                foreach (Glass glass in magazineBaseReturn.Getglass())
                {
                    if (glass.Type == item.Type && glass.Color == item.Color && item.Thickness == glass.Hight)
                    {
                        foreach (Glass_Id glass_Id in glass.Glass_info)
                        {
                            if (glass_Id.Used == false && glass_Id.Destroyed == false && glass_Id.Removed == false && glass_Id.Cut_id == 0)
                            {
                                if (item.Length <= glass.Length && item.Width <= glass.Width)
                                {
                                    kontrol2 = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (item.Status == "Oczekujący" && item.Cut_id == 0 && kontrol2 == true)
                {
                    kontrol = false;
                    if (temp.Count != 0)
                    {
                        foreach (Package package in temp)
                        {
                            if (package.Color == item.Color && package.Type == item.Type && item.Thickness == package.Thickness)
                            {
                                package.Item.Add(item);
                                kontrol = true;
                            }
                        }
                    }

                    if (kontrol == false)
                    {
                        Package package = new Package { Color = item.Color, Type = item.Type, Id_Order = order.Id_Order, Thickness = item.Thickness, Item = new List<Item>(), Owner = order.Owner };
                        package.Item.Add(item);

                        temp.Add(package);
                    }
                }
            }
            return temp;
        }

        public List<Glass> Return_Glass_To_Cut(Receiver receiver)
        {
            Package package = receiver.package;
            List<Glass> glasses = new List<Glass>();

            Sort_Package(package);

            foreach (Glass glasse in magazineCheck.Set_Filter(magazineBaseReturn.Getglass()))
            {
                if (glasse.Type == package.Type && glasse.Color == package.Color && glasse.Hight == package.Thickness)
                {
                    if (package.Item.First().Length <= glasse.Length && package.Item.First().Width <= glasse.Width)
                    {
                        glasses.Add(glasse);
                    }
                }
            }
            return glasses;
        }

        public List<Machines> Return_Machine_To_Cut()
        {
            List<Machines> machines = new List<Machines>();

            foreach (Machines mach in machineBaseReturn.GetMachines())
            {
                if (mach.Stan == false && mach.Status == "Ready")
                {
                    machines.Add(mach);
                }
            }
            return machines;
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
                            if (itm.Width == Convert.ToDouble(cuboid.Width) && itm.Length == Convert.ToDouble(cuboid.Height))
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
    }
}
