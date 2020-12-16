using System;
using System.Collections.Generic;
using System.Data;
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
        ProductController productController = new ProductController();

        public bool Check_Code(int code)
        {
            foreach (Product product in productController.Get_All_Products())
            {
                if (code == product.Id)
                {
                    return false;
                }
            }
            return true;
        }

        public void Repeat(int code)
        {
            bool check;
            check = Check_Code(code);

            Random rand = new Random();

            if (check == false)
            {
                code = rand.Next(1, 900);
                Repeat(code);
            }
        }


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

                if(item.Status == "awaiting")
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
                        Package package = new Package {Color = item.Color, Type = item.Type, Id_Order = order.Id_Order, Thickness = item.Thickness, Item = new List<Item>(), Owner = order.Owner };
                        package.Item.Add(item);

                        temp.Add(package);
                    }              
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

        public List<Piece> Package_Pieces(double x, double y, Package package)
        {
            List<Piece> wynik = new List<Piece>();
            double glass_lenght = x; //Tl
            double glass_widht = y; //Tw
            int Fit = 0;
            int First_Fit = 0;
            double P_x = 0;
            double P_y = 0;
            double Pl = 0;
            double Pw =  0;
            double I_x = 0;
            double I_l = 0;
            double I_w = 0;
            bool P_ok = true;
            double Wmin = 0;
            double Lmin = 0;
            double Pos_x_1 = 0;
            double Pos_y_1 = 0;
            double L_1 = 0;
            double W_1 = 0;
            double A_1 = 0;
            double temp_area = 0;
            int i = 0;
            double Pos_x_2 = 0;
            double Pos_y_2 = 0;
            double L_2 = 0;
            double W_2 = 0;
            double A_2 = 0;
            List<Position> positions = new List<Position>();
            Position pos = new Position { X_pos = 0, Y_pos = 0, Lenght = glass_lenght, Widht = glass_widht };

            positions.Add(pos);

            while (positions.Count > 0 && package.Item.Count > 0)
            {
                positions.Sort((x1, x2) => (x1.X_pos.CompareTo(x2.X_pos)));
                positions.Sort((y1, y2) => (y1.Y_pos.CompareTo(y2.Y_pos)));

                Pl = positions.First().Lenght;
                Pw = positions.First().Widht;

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

                Sort_Package(package);

                First_Fit = package.Item.First().Fit_pos;

                if(First_Fit > 0)
                {
                    P_x = positions.First().X_pos;
                    P_y = positions.First().Y_pos;

                    I_x = package.Item.First().Id;
                    I_l = package.Item.First().Length;
                    I_w = package.Item.First().Width;

                    Piece piece = new Piece { id = I_x, Lenght = I_l, Widht = I_w, X = P_x, Y = P_y };

                    wynik.Add(piece);

                    package.Item.RemoveAt(0);

                    if(package.Item.Count > 0)
                    {
                        Sort_Package(package);

                        P_ok = true;

                        if(Pw > I_w)
                        {
                            Pos_x_2 = 0;
                            Pos_y_2 = 0;
                            L_2 = 0;
                            W_2 = 0;
                            A_2 = 0;
                            temp_area = 0;
                            i = 0;

                            P_ok = false;

                            Pos_x_2 = P_x;
                            Pos_y_2 = P_y + I_w;

                            L_2 = glass_lenght - Pos_x_2;
                            W_2 = Pw - I_w;
                            A_2 = L_2 * W_2;

                            temp_area = package.Item.Last().Area;
                            i = package.Item.Count();

                            while(temp_area <= A_2 && i > -1)
                            {
                                temp_area = package.Item.ElementAt(i).Amount;

                                Lmin = package.Item.ElementAt(i).Length;
                                Wmin = package.Item.ElementAt(i).Width;

                                if (L_2 >= Lmin && W_2 >= Wmin)
                                {
                                    Position position = new Position {X_pos = Pos_x_2, Y_pos = Pos_y_2, Lenght = L_2, Widht = W_2 };

                                    positions.Add(position);

                                    P_ok = true;
                                    i = -1;
                                }
                                i--;
                            }
                        }

                        if (Pl > I_l)
                        {
                            Pos_x_1 = 0;
                            Pos_y_1 = 0;
                            L_1 = 0;
                            W_1 = 0;
                            A_1 = 0;
                            temp_area = 0;
                            i = 0;

                            Pos_x_1 = P_x + I_l;
                            Pos_y_1 = P_y;

                            L_1 = glass_lenght - Pos_x_1;
                            W_1 = I_w;
                            A_1 = L_1 * W_1;

                            if (!P_ok)
                            {
                                W_1 = W_1 + W_2;
                                A_1 = L_1 * W_1;
                            }

                            temp_area = package.Item.Last().Area;
                            i = package.Item.Count();

                            while (temp_area <= A_1 && i > -1)
                            {
                                temp_area = package.Item.ElementAt(i).Amount;

                                Lmin = package.Item.ElementAt(i).Length;
                                Wmin = package.Item.ElementAt(i).Width;

                                if (L_1 >= Lmin && W_1 >= Wmin)
                                {
                                    Position position = new Position { X_pos = Pos_x_1, Y_pos = Pos_y_1, Lenght = L_1, Widht = W_1 };

                                    positions.Add(position);

                                    i = -1;
                                }
                                i--;
                            }
                        }
                    }
                }

                if(package.Item.Count > 0)
                {
                    positions.RemoveAt(0);
                }
            }          

            return wynik;
        }

        [HttpPost("Magic")]
        public async Task<List<Glass>> Magic([FromBody] Receiver receiver)
        { 
            List<Glass> wynik = new List<Glass>();
            User user = receiver.user;
            Order order = receiver.order;
            Item item1 = receiver.item;
            List<Glass> glasses = new List<Glass>();
            int kontrol;
            
            Package packages = new Package();
            Package backup = new Package();

            foreach (Item item in orderController.GetItems(order))
            {
                if (item.Color == item1.Color && item.Type == item1.Type && item1.Thickness == item.Thickness)
                {
                    packages.Item.Add(item);
                    backup.Item.Add(item);
                }
            }

            kontrol = packages.Item.Count;

            Return_Area(packages);
            Set_Package(packages);
            Sort_Package(packages);

            foreach (Glass glass in magazineController.Getglass())
            {
                if (glass.Type == item1.Type && glass.Color == item1.Color && item1.Thickness == glass.Width)
                {
                    glasses.Add(glass);
                }
            }

            foreach (User usere in usersController.GetUsers())
            {
                if(usere.Login == user.Login)
                {
                    foreach (Glass glass in glasses)
                    {
                        if (packages.Item.Count > 0)
                        {
                            Glass tmp = glass;

                            tmp.Pieces = Package_Pieces(glass.Length, glass.Width, packages);

                            wynik.Add(tmp);
                        }                       
                    }

                    if(wynik.Count < backup.Item.Count)
                    {
                        Glass tmp = new Glass();
                        tmp.Error_Messege = "zabraklo miejsca dla: ";

                        for (int i = 0; i < packages.Item.Count; i++)
                        {
                            tmp.Error_Messege = tmp.Error_Messege + ", " + packages.Item[i].Id;

                            wynik.Add(tmp);
                        }
                        wynik.Add(tmp);
                    }

                    return wynik;
                }            
            }
           
            //błąd nie ma takiego usera

            return wynik;
        }

        [HttpPost("Post_Production")]
        public async Task<string> Post_Production(Receiver receiver)
        {
            Order order = receiver.order;
            Package package = receiver.package;
            User user = receiver.user;
            Machines machines = receiver.machines;
            bool kontrolka;

            foreach(User usere in usersController.GetUsers())
            {
                if(usere.Login == user.Login)
                {
                    foreach(Order ord in orderController.GetOrders())
                    {
                        if(order.Id_Order == ord.Id_Order)
                        {
                            foreach(Item item in orderController.GetItems(ord))
                            {
                                foreach(Item item_cutted in package.Item)
                                {
                                    if(item.Id == item_cutted.Id)
                                    {
                                        item.Status = "ready";

                                        Random rand = new Random();
                                        var code = rand.Next(1, 9000);

                                        Repeat(code);

                                        string query = "INSERT INTO dbo.[Product](@Id,@Owner,@Desk,@Status,@Id_item)";
                                        SqlCommand command = new SqlCommand(query, cnn);

                                        command.Parameters.Add("@Id", SqlDbType.Int).Value = code;
                                        command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = ord.Owner;
                                        command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = "";
                                        command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = item.Status;
                                        command.Parameters.Add("@Id_item", SqlDbType.VarChar, 40).Value = item.Id;

                                        cnn.Open();
                                        command.ExecuteNonQuery();
                                        command.Dispose();
                                        cnn.Close();
                                    }
                                }
                            }

                            kontrolka = true;

                            foreach (Item itm in orderController.GetItems(ord))
                            {
                                if(itm.Status == "awaiting" || itm.Status == "cut")
                                {
                                    kontrolka = false;
                                }
                            }

                            string query2 = "Update dbo.[Machines](SET Status = @Status WHERE No = @No)";
                            SqlCommand command2 = new SqlCommand(query2, cnn);

                            command2.Parameters.Add("@No", SqlDbType.Int, 40).Value = machines.No;
                            command2.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "ready";


                            cnn.Open();
                            command2.ExecuteNonQuery();
                            command2.Dispose();
                            cnn.Close();



                            return "Done";
                        }
                    }
                }
            }


            return "Not Done";
        }
    }
}