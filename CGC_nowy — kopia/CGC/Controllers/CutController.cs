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

            foreach(Order order in orders)
            {
                counter = 0;
                help2 = "";
                help = order.Deadline;

                datetime = Convert.ToDateTime(help);

                foreach(char i in help)
                {
                    if(counter < 4)
                    {
                        help2 = help2 + i;
                        counter++;

                        if (counter == 4)
                        {
                            year = Convert.ToInt32(help2);
                            help2 = "";
                        }

                    }
                    else if(counter < 7)
                    {
                        help2 = help2 + i;
                        counter++;

                        if (counter == 7)
                        {
                            month = Convert.ToInt32(help2);
                            help2 = "";
                        }
                    }                
                    else if (counter < 10)
                    {
                        help2 = help2 + i;
                        counter++;

                        if (counter == 10)
                        {
                            day = Convert.ToInt32(help2);
                        }
                    }
                }


    
            }

            return temp;
        }


        [HttpGet("Return_Package_To_Cut")]
        public async Task<List<Package>> Return_Package_To_Cut(Receiver receiver)
        {
            Order order = receiver.order;
            List<Package> temp = new List<Package>();
            bool kontrol;

            foreach (Item item in order.items)
            {
                kontrol = false;

                foreach (Package package in temp)
                {
                    if (package.Color == item.Color && package.Type == item.Type && item.Thickness == package.Thickness)
                    {
                        package.Item.Add(item);
                        kontrol = true;
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

        [HttpGet("Return_Machine_To_Cut")]
        public async Task<List<Piece>> Magic(Receiver receiver)
        {
            List<Piece> pieces = new List<Piece>();

            List<Glass> glasses = receiver.glasses;
            User user = receiver.user;
            Machines machines = receiver.machines;
            List<Package> packages = receiver.packages;




            return pieces;
        }
    }
}