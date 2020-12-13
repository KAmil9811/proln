using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CGC.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    public class CutController : Controller
    {
        static MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        {
            Server = "projekt-inz.database.windows.net",
            Database = "projekt-inz",
            UserID = "Michal",
            Password = "lemES98naw141",
            //SslMode = MySqlSslMode.Required,
        };


        readonly SqlConnection cnn = new SqlConnection(builder.ConnectionString);
        private static CutController m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static CutController Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new CutController();
                    }
                    return m_oInstance;
                }
            }
        }

        OrderController orderController = new OrderController();
        MachineController machineController = new MachineController();
        MagazineController magazineController = new MagazineController();
        UsersController usersController = new UsersController();

        [HttpGet("Return_Orders_To_Cut")]
        public async Task<List<Order>> Return_Orders_To_Cut()
        {
            List<Order> orders = new List<Order>();
            List<Order> temp = new List<Order>();

            DateTime datetime;


            string help;
            string help2;
            int year = 0;
            int month = 0;
            int day = 0;
            int counter;

            int data_y = 0;
            int data_m = 0;
            int data_d = 0;

            foreach (Order order in orderController.GetOrders())
            {
                if (order.Status == "awaiting" || order.Status == "stopped")
                {
                    if (orderController.Avaible_Cut(order) > 0)
                    {
                        orders.Add(order);
                    }
                }
            }

            foreach (Order order in orders)
            {
                order.Deadline2 = Convert.ToDateTime(order.Deadline);
            }

            orders.Sort((order1,  order2) => order1.Deadline2.CompareTo(order2.Deadline2));

                //foreach(Order order in orders)
                //{
                //    counter = 0;
                //    help2 = "";
                //    help = order.Deadline;

                //    datetime = Convert.ToDateTime(help);

                //    foreach(char i in help)
                //    {
                //        if(counter < 4)
                //        {
                //            help2 = help2 + i;
                //            counter++;

                //            if (counter == 4)
                //            {
                //                year = Convert.ToInt32(help2);
                //                help2 = "";
                //            }

                //        }
                //        else if(counter < 7)
                //        {
                //            help2 = help2 + i;
                //            counter++;

                //            if (counter == 7)
                //            {
                //                month = Convert.ToInt32(help2);
                //                help2 = "";
                //            }
                //        }                
                //        else if (counter < 10)
                //        {
                //            help2 = help2 + i;
                //            counter++;

                //            if (counter == 10)
                //            {
                //                day = Convert.ToInt32(help2);
                //            }
                //        }
                //    }
                //  }

            return orders;
        }


        [HttpPost("Return_Package_To_Cut")]
        public async Task<List<Package>> Return_Package_To_Cut([FromBody]Receiver receiver)
        {
            Order order = receiver.order;
            List<Package> temp = new List<Package>();
            bool kontrol;



            foreach (Item item in orderController.GetItems(order))
            {
                kontrol = false;

                if(temp.Count != 0)
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

            return temp;
        }

        [HttpGet("Return_Glass_To_Cut")]
        public async Task<List<Glass>> Return_Glass_To_Cut(Package package)
        {
            List<Glass> glasses = new List<Glass>();

            foreach (Glass glasse in magazineController.Set_Filter(magazineController.Getglass()))
            {
                if (glasse.Type == package.Type && glasse.Color == package.Color && glasse.Hight == package.Thickness)
                {
                    glasses.Add(glasse);
                }
            }
            return glasses;
        }

        [HttpGet("Return_Machine_To_Cut")]
        public async Task<List<Machines>> Return_Machine_To_Cut()
        {
            List<Machines> machines = new List<Machines>();

            foreach (Machines mach in machineController.GetMachines())
            {
                if (mach.Stan == false && mach.Status == "Ready")
                {
                    machines.Add(mach);
                }
            }
            return machines;
        }

        public void Return_Area(Package package)
        {
            foreach(Item item in package.Item)
            {
                item.Area = item.Length * item.Width;
            }
        }

        public void Set_Package(Package package)
        {
            double temp;

            foreach(Item item in package.Item)
            {
                if(item.Width > item.Length)
                {
                    temp = item.Length;
                    item.Length = item.Width;
                    item.Width = temp;
                }
            }
        }

        public void Sort_Package(Package package)
        {
            package.Item.Sort((item1, item2) => (item1.Width.CompareTo(item2.Width)) * (-1));

            package.Item.Sort((item1,  item2) => (item1.Length.CompareTo(item2.Length)) * (-1));

            package.Item.Sort((item1, item2) => (item1.Area.CompareTo(item2.Area)) * (-1));

            package.Item.Sort((item1, item2) => (item1.Fit_pos.CompareTo(item2.Fit_pos)) * (-1));
        }

        public List<Piece> Package_Piece(double x, double y, Package package)
        {
            List<Piece> wynik = new List<Piece>();
            double glass_lenght = x;
            double glass_widht = y;
            int Fit;

            foreach(Item item in package.Item)
            {
                Fit = 0;
                if (glass_lenght < item.Length || glass_widht < item.Width)
                {
                    Fit = 0;
                }
                else
                {
                    Fit++;
                    if (glass_lenght == item.Length)
                    {
                        Fit++;
                    }
                    if (glass_widht == item.Width)
                    {
                        Fit++;
                    }
                }
                item.Fit_pos = Fit;
            }

            Sort_Package(package);

            //if (package.Item.First().Fit_pos > 0)
            //{

            //}

            return wynik;
        }

        [HttpGet("Magic")]
        public async Task<List<Glass>> Magic(Receiver receiver)
        { 
            List<Glass> glasses = receiver.glasses;
            List<Glass> wynik = new List<Glass>();
            User user = receiver.user;

            Machines machines = receiver.machines;
            
            Package packages = receiver.package;



            Return_Area(packages);
            Set_Package(packages);
            Sort_Package(packages);

            foreach (User usere in usersController.GetUsers())
            {
                if(usere.Login == user.Login)
                {
                    foreach (Glass glass in glasses)
                    {
                        if (packages.Item.Count > 0)
                        {
                            Glass tmp = new Glass();

                            tmp.Glass_id = glass.Glass_id;

                            tmp.Pieces = Package_Piece(glass.Length, glass.Width, packages);

                            wynik.Add(tmp);
                        }
                    }

                    return wynik;
                }            
            }
           
            //błąd nie ma takiego usera

            return wynik;
        }
    }
}