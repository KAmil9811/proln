using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CGC.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Sharp3DBinPacking;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    public sealed class CutController : Controller
    {
        static MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        {
            Server = "projekt-inz.database.windows.net",
            Database = "projekt-inz",
            UserID = "Michal",
            Password = "lemES98naw141",
            //SslMode = MySqlSslMode.Required,
        };


        SqlConnection cnn = new SqlConnection(builder.ConnectionString);
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

        public List<Cut_Project> GetCut_Project()
        {
            List<Cut_Project> temp = new List<Cut_Project>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Cut_Project];", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Cut_Project cut_Project = new Cut_Project();
                cut_Project.Cut_id = Convert.ToInt32(sqlDataReader["Cut_id"]);
                cut_Project.Order_id = sqlDataReader["Order_id"].ToString();
                cut_Project.Status = sqlDataReader["Status"].ToString();

                temp.Add(cut_Project);
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();
            return temp;
        }

        [HttpGet("Return_Orders_To_Cut")]
        public async Task<List<Order>> Return_Orders_To_Cut()
        {
            List<Order> orders = new List<Order>();
            List<Order> temp = new List<Order>();

            foreach (Order order in orderController.GetOrders())
            {
                if (order.Status == "Awaiting" || order.Status == "Stopped")
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

            orders.OrderBy(orderer => orderer.Deadline2);

            return orders;
        }

        [HttpPost("Return_Package_To_Cut")]
        public async Task<List<Package>> Return_Package_To_Cut([FromBody]Receiver receiver)
        {
            Order order = receiver.order;
            List<Package> temp = new List<Package>();
            bool kontrol;
            bool kontrol2;

            foreach (Item item in orderController.GetItems(order))
            {
                kontrol = false;
                kontrol2 = false;

                foreach (Glass glass in magazineController.Getglass())
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

                if (item.Status == "Awaiting" && item.Cut_id == 0 && kontrol2 == true)
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

        [HttpGet("Return_Glass_To_Cut")]
        public async Task<List<Glass>> Return_Glass_To_Cut(Package package)
        {
            List<Glass> glasses = new List<Glass>();

            Sort_Package(package);

            foreach (Glass glasse in magazineController.Set_Filter(magazineController.Getglass()))
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

        [HttpGet("Return_All_Project")]
        public async Task<List<Cut_Project>> Return_Cut_Project()
        {
            List<Cut_Project> cut_Projects = new List<Cut_Project>();

            foreach (Cut_Project cut_p in GetCut_Project())
            {
                if (cut_p.Status != "Ready")
                {
                    foreach(Order ord in orderController.GetOrders())
                    {
                        if(ord.Id_Order == cut_p.Order_id)
                        {
                            cut_p.Deadline = ord.Deadline;
                            cut_p.Owner = ord.Owner;
                            cut_p.Priority = ord.Priority;
                            cut_Projects.Add(cut_p);
                            break;
                        }
                    }
                }
            }
            return cut_Projects;
        }

        [HttpPost("Return_Porject")]
        public async Task<List<Glass>> Return_Porject([FromBody] Receiver receiver)
        {
            List<Glass> wynik = new List<Glass>();
            int Cut_id = receiver.id;
            Order order = receiver.order;
            int kontrol;
            List<Glass> glasses = new List<Glass>();
            bool kon = false;

            Package packages = new Package();
            Package backup = new Package();

            foreach (Item item in orderController.GetItems(order))
            {
                if (item.Cut_id == Cut_id)
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
                List<Glass_Id> temp = new List<Glass_Id>();
                bool kontrolka = false;

                foreach (Glass_Id glass_Id in glass.Glass_info)
                {
                    if (glass_Id.Used == false && glass_Id.Destroyed == false && glass_Id.Removed == false && glass_Id.Cut_id == 0)
                    {
                        kontrolka = true;
                    }
                }
                if (kontrolka == true)
                {
                    glasses.Add(glass);
                }
            }

            glasses.OrderBy(glasse => glasse.Width).ThenBy(glasses2 => glasses2.Length);

            foreach (Glass glass in glasses)
            {
                foreach (Glass_Id glass_id in glass.Glass_info)
                {
                    if (packages.Item.Count > 0)
                    {
                        Glass_Id glass_Id2 = new Glass_Id { Pieces = new List<Piece>() };
                        List<Item> Used = new List<Item>();
                        List<Cuboid> temporary = new List<Cuboid>();
                        foreach (Item itm in packages.Item)
                        {
                            temporary.Add(new Cuboid(Convert.ToDecimal(itm.Width), Convert.ToDecimal(itm.Length), Convert.ToDecimal(itm.Thickness)));
                        }

                        var parameter = new BinPackParameter(Convert.ToDecimal(glass.Length), Convert.ToDecimal(glass.Width), Convert.ToDecimal(glass.Hight), temporary);

                        Glass tmp = new Glass();

                        var binPacker = BinPacker.GetDefault(BinPackerVerifyOption.BestOnly);

                        var result = binPacker.Pack(parameter);

                        tmp.Width = Convert.ToDouble(parameter.BinWidth);
                        tmp.Hight = Convert.ToDouble(parameter.BinDepth);
                        tmp.Length = Convert.ToDouble(parameter.BinHeight);

                        foreach (var cub in result.BestResult[0])
                        {
                            foreach (Item itm in packages.Item)
                            {
                                if (itm.Width == Convert.ToDouble(cub.Width) && itm.Length == Convert.ToDouble(cub.Height))
                                {
                                    foreach (Item i in Used)
                                    {
                                        if (i.Id == itm.Id)
                                        {
                                            kon = true;
                                        }
                                    }
                                    if (kon == false)
                                    {
                                        glass_Id2.Pieces.Add(new Piece { id = itm.Id, X = Convert.ToDouble(cub.X), Y = Convert.ToDouble(cub.Y), Lenght = Convert.ToDouble(cub.Height), Widht = Convert.ToDouble(cub.Width) });
                                        Item iteme = new Item { Id = itm.Id };

                                        Used.Add(iteme);
                                        kon = false;
                                        break;
                                    }
                                    kon = false;
                                }
                            }
                        }

                        tmp.Glass_info.Add(glass_Id2);
                        wynik.Add(tmp);

                        try
                        {
                            foreach (Item itm in Used)
                            {
                                packages.Item.Remove(packages.Item.First(i => i.Id == itm.Id));
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }

            kontrol = 0;

            foreach (Glass gl in wynik)
            {
                foreach (Glass_Id gl2 in gl.Glass_info)
                {
                    kontrol += gl2.Pieces.Count;
                }
            }

            if (wynik.Count < backup.Item.Count)
            {
                List<int> Done = new List<int>();
                Glass tmp = new Glass();
                tmp.Error_Messege = "zabraklo miejsca dla: ";

                Glass_Id glass_Id = new Glass_Id();

                foreach (Item itm in backup.Item)
                {
                    foreach (Glass temp in wynik)
                    {
                        foreach (Glass_Id glass_Ids in temp.Glass_info)
                        {
                            foreach (Piece piece in glass_Ids.Pieces)
                            {
                                if (itm.Id == piece.id)
                                {
                                    Done.Add(itm.Id);
                                }
                            }
                        }
                    }
                }

                for (int i = wynik.Count - 1; i < packages.Item.Count; i++)
                {
                    Piece piece = new Piece { id = packages.Item[i].Id, Lenght = packages.Item[i].Length, Widht = packages.Item[i].Width };
                    glass_Id.Pieces.Add(piece);
                    tmp.Error_Messege = tmp.Error_Messege + ", " + packages.Item[i].Id;
                }
                tmp.Glass_info.Add(glass_Id);
                wynik.Add(tmp);
            }

            return wynik;
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
            //package.Item.Sort((item1, item2) => (item1.Width.CompareTo(item2.Width)) * (-1));

            //package.Item.Sort((item1,  item2) => (item1.Length.CompareTo(item2.Length)) * (-1));

            //package.Item.Sort((item1, item2) => (item1.Area.CompareTo(item2.Area)) * (-1));

            //package.Item.Sort((item1, item2) => (item1.Fit_pos.CompareTo(item2.Fit_pos)) * (-1));

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

                //temp = piece.Lenght;
                //piece.Lenght = piece.Widht;
                //piece.Widht = temp;
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
            double Pw = 0;
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

            try
            {
                while (positions.Count > 0 && package.Item.Count > 0)
                {
                    positions = positions.OrderBy(x1 => x1.Y_pos).ThenBy(x1 => x1.X_pos).ToList();

                    Pl = positions.First().Lenght;
                    Pw = positions.First().Widht;

                    Set_Fit(package, Pw, Pl);

                    //Sort_Package(package);

                    package.Item = package.Item.OrderByDescending(item => item.Fit_pos).ThenByDescending(item => item.Area).ThenByDescending(item => item.Length).ThenByDescending(item => item.Width).ToList();

                    First_Fit = package.Item.First().Fit_pos;

                    if (First_Fit > 0)
                    {
                        P_x = positions.First().X_pos;
                        P_y = positions.First().Y_pos;

                        I_x = package.Item.First().Id;
                        I_l = package.Item.First().Length;
                        I_w = package.Item.First().Width;

                        Piece piece = new Piece { id = I_x, Lenght = I_l, Widht = I_w, X = P_x, Y = P_y };

                        wynik.Add(piece);

                        package.Item.RemoveAt(0);

                        if (package.Item.Count > 0)
                        {
                            P_ok = true;

                            if (Pw > I_w)
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

                                Set_Fit(package, W_2, L_2);
                                package.Item = package.Item.OrderByDescending(item1 => item1.Fit_pos).ThenByDescending(item1 => item1.Area).ThenByDescending(item1 => item1.Length).ThenByDescending(item1 => item1.Width).ToList();

                                Item item = Find_Area(package);

                                temp_area = item.Area;
                                i = package.Item.IndexOf(item);

                                while (temp_area <= A_2 && i > -1)
                                {
                                    temp_area = package.Item.ElementAt(i).Amount;

                                    Lmin = package.Item.ElementAt(i).Length;
                                    Wmin = package.Item.ElementAt(i).Width;

                                    if (L_2 >= Lmin && W_2 >= Wmin)
                                    {
                                        Position position = new Position { X_pos = Pos_x_2, Y_pos = Pos_y_2, Lenght = L_2, Widht = W_2 };

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

                                Set_Fit(package, W_1, L_1);
                                package.Item = package.Item.OrderByDescending(item1 => item1.Fit_pos).ThenByDescending(item1 => item1.Area).ThenByDescending(item1 => item1.Length).ThenByDescending(item1 => item1.Width).ToList();

                                Item item = Find_Area(package);

                                temp_area = item.Area;
                                i = package.Item.IndexOf(item);

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

                    if (package.Item.Count > 0)
                    {
                        positions.RemoveAt(0);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return wynik;
        }

        //[HttpPost("Magic")]
        public async Task<List<Glass>> Magic2([FromBody] Receiver receiver)
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
                if (item.Color == item1.Color && item.Type == item1.Type && item1.Thickness == item.Thickness && item.Status == "Awaiting")
                {
                    packages.Item.Add(item);
                    backup.Item.Add(item);
                }
            }

            kontrol = packages.Item.Count;

            Return_Area(packages);
            Set_Package(packages);
            Sort_Package(packages);

            List<Glass> tempo = magazineController.Getglass();

            foreach (Glass glass in magazineController.Getglass())
            {
                if (glass.Type == item1.Type && glass.Color == item1.Color && item1.Thickness == glass.Hight)
                {
                    Glass glass1 = new Glass();

                    glass1.Length = glass.Length;
                    glass1.Width = glass.Width;
                    glass1.Length = glass.Length;


                    foreach (Glass_Id glass_Id in glass.Glass_info)
                    {
                        if (glass_Id.Destroyed == false && glass_Id.Used == false && glass_Id.Removed == false && glass_Id.Cut_id == 0)
                        {
                            glass1.Glass_info.Add(glass_Id);
                        }
                    }

                    glasses.Add(glass1);
                }
            }

            glasses.OrderBy(gla => gla.Length).ThenBy(gla2 => gla2.Width);

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (Glass glass in glasses)
                    {
                        foreach (Glass_Id glass_id in glass.Glass_info)
                        {
                            if (packages.Item.Count > 0)
                            {
                                Glass tmp = new Glass();

                                tmp.Width = glass.Width;
                                tmp.Hight = glass.Hight;
                                tmp.Length = glass.Length;

                                glass_id.Pieces = Package_Pieces(glass.Length, glass.Width, packages);

                                Set_Pieces(glass_id.Pieces);

                                tmp.Glass_info.Add(glass_id);
                                wynik.Add(tmp);
                            }
                        }
                    }

                    if (wynik.Count < backup.Item.Count)
                    {
                        Glass tmp = new Glass();
                        tmp.Error_Messege = "zabraklo miejsca dla: ";

                        Glass_Id glass_Id = new Glass_Id();

                        for (int i = wynik.Count - 1; i < packages.Item.Count; i++)
                        {
                            Piece piece = new Piece { id = packages.Item[i].Id, Lenght = packages.Item[i].Length, Widht = packages.Item[i].Width };
                            glass_Id.Pieces.Add(piece);
                            tmp.Error_Messege = tmp.Error_Messege + ", " + packages.Item[i].Id;
                        }
                        tmp.Glass_info.Add(glass_Id);
                        wynik.Add(tmp);
                    }

                    return wynik;
                }
            }

            //błąd nie ma takiego usera
            return wynik;
        }

        [HttpPost("Save_Project")]
        public async Task<int> Save_Project([FromBody] Receiver receiver)
        {
            List<Glass> glasses = receiver.glasses;
            Order order = receiver.order;
            int code;

            try
            {
                code = GetCut_Project().OrderBy(cutid => cutid.Cut_id).Last().Cut_id + 1;
            }
            catch (Exception e)
            {
                code = 1;
            }

            string query = "INSERT INTO dbo.[Cut_Project](Cut_id, Order_id, Status) VALUES(@Cut_id,@Order_id, @Status)";
            SqlCommand command = new SqlCommand(query, cnn);

            command.Parameters.Add("@Cut_id", SqlDbType.Int).Value = code;
            command.Parameters.Add("@Order_id", SqlDbType.VarChar, 40).Value = order.Id_Order;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Saved";

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();

            foreach (Glass glass in glasses)
            {
                if (glass.Error_Messege == null)
                {
                    query = "UPDATE dbo.[Glass] SET Cut_id = @Cut_id WHERE Glass_Id = @Glass_Id";
                    command = new SqlCommand(query, cnn);

                    command.Parameters.Add("@Cut_id", SqlDbType.Int).Value = code;
                    command.Parameters.Add("@Glass_Id", SqlDbType.Int).Value = glass.Glass_info.First().Id;

                    cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    cnn.Close();


                    foreach (Item item in orderController.GetItems(order))
                    {
                        foreach (Piece piece in glass.Glass_info.First().Pieces)
                        {
                            if (piece.id == item.Id)
                            {
                                query = "UPDATE dbo.[Item] SET Cut_id = @Cut_id WHERE Id = @Id";
                                command = new SqlCommand(query, cnn);

                                command.Parameters.Add("@Cut_id", SqlDbType.Int).Value = code;
                                command.Parameters.Add("@Id", SqlDbType.Int).Value = item.Id;

                                cnn.Open();
                                command.ExecuteNonQuery();
                                command.Dispose();
                                cnn.Close();
                            }
                        }
                    }
                }
            }
            return code;
        }

        [HttpPost("Remove_Project")]
        public async Task<List<Cut_Project>> Remove_Project([FromBody] Receiver receiver)
        {
            List<Cut_Project> cut_Projects = new List<Cut_Project>();
            Order order = new Order();

            Cut_Project cut_Project = receiver.cut_Project;
            order.Id_Order = cut_Project.Order_id;

            string query = "UPDATE dbo.[Cut_Project] SET Status = @Status WHERE Cut_id = @Cut_id,)";
            SqlCommand command = new SqlCommand(query, cnn);

            command.Parameters.Add("@Cut_id", SqlDbType.Int).Value = cut_Project.Cut_id;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Removed";

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();


            query = "UPDATE dbo.[Glass] SET Cut_id = @Cut_id2 WHERE Cut_id = @Cut_id";
            command = new SqlCommand(query, cnn);

            command.Parameters.Add("@Cut_id2", SqlDbType.Int).Value = null;
            command.Parameters.Add("@Cut_id", SqlDbType.Int).Value = cut_Project.Cut_id;

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();



            query = "UPDATE dbo.[Item] SET Cut_id = @Cut_id2 WHERE Cut_id = @Cut_id";
            command = new SqlCommand(query, cnn);

            command.Parameters.Add("@Cut_id2", SqlDbType.Int).Value = null;
            command.Parameters.Add("@Cut_id", SqlDbType.Int).Value = cut_Project.Cut_id;

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();


            cut_Projects.Add(cut_Project);
            return cut_Projects;
        }

        [HttpPost("Post_Production")]
        public async Task<string> Post_Production([FromBody] Receiver receiver)
        {
            User user = receiver.user;
            Machines machines = receiver.machines;
            Cut_Project cut_Project = receiver.cut_Project;

            foreach (Cut_Project cut in GetCut_Project())
            {
                if (cut.Cut_id == cut_Project.Cut_id)
                {
                    foreach (Order ord in orderController.GetOrders())
                    {
                        if (ord.Id_Order == cut.Order_id)
                        {
                            foreach (Item item in orderController.GetItems(ord))
                            {
                                if (item.Cut_id == cut.Cut_id)
                                {
                                    int code;
                                    try
                                    {
                                        code = productController.GetProducts().OrderBy(pro => pro.Id).Last().Id + 1;
                                    }
                                    catch (Exception e)
                                    {
                                        code = 1;
                                    }

                                    string query = "INSERT INTO dbo.[Product](Id,Owner,Desk,Status,Id_item,Id_order) VALUES(@Id,@Owner,@Desk,@Status,@Id_item,@Id_order)";
                                    SqlCommand command = new SqlCommand(query, cnn);

                                    command.Parameters.Add("@Id", SqlDbType.Int).Value = code;
                                    command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = ord.Owner;
                                    command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = "";
                                    command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";
                                    command.Parameters.Add("@Id_item", SqlDbType.VarChar, 40).Value = item.Id;
                                    command.Parameters.Add("@Id_order", SqlDbType.VarChar, 40).Value = ord.Id_Order;

                                    cnn.Open();
                                    command.ExecuteNonQuery();
                                    command.Dispose();
                                    cnn.Close();

                                    query = "UPDATE dbo.[Item] SET Product_id = @Product_id, Status = @Status WHERE Id = @Id";
                                    command = new SqlCommand(query, cnn);

                                    command.Parameters.Add("@Id", SqlDbType.Int).Value = item.Id;
                                    command.Parameters.Add("@Product_id", SqlDbType.Int).Value = code;
                                    command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";

                                    cnn.Open();
                                    command.ExecuteNonQuery();
                                    command.Dispose();
                                    cnn.Close();
                                }
                            }
                        }
                    }
                }
            }
            try
            {
                string query2 = "UPDATE dbo.[Glass] SET Used = @Used WHERE Cut_id = @Cut_id";
                SqlCommand command2 = new SqlCommand(query2, cnn);

                command2.Parameters.Add("@Cut_id", SqlDbType.Int).Value = cut_Project.Cut_id;
                command2.Parameters.Add("@Used", SqlDbType.Bit).Value = 1;

                cnn.Open();
                command2.ExecuteNonQuery();
                command2.Dispose();
                cnn.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            string query3 = "UPDATE dbo.[Cut_Project] SET Status = @Status WHERE Cut_id = @Cut_id";
            SqlCommand command3 = new SqlCommand(query3, cnn);

            command3.Parameters.Add("@Cut_id", SqlDbType.Int).Value = cut_Project.Cut_id;
            command3.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Wykonany";

            cnn.Open();
            command3.ExecuteNonQuery();
            command3.Dispose();
            cnn.Close();

            string query4 = "Update dbo.[Machines] SET Status = @Status WHERE No = @No";
            SqlCommand command4 = new SqlCommand(query4, cnn);

            command4.Parameters.Add("@No", SqlDbType.Int, 40).Value = machines.No;
            command4.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";


            cnn.Open();
            command4.ExecuteNonQuery();
            command4.Dispose();
            cnn.Close();

            string userhistory = "You cutted project " + cut_Project.Cut_id;
            usersController.Insert_User_History(userhistory, user.Login);

            return "Done";
        }

        [HttpPost("Start_Production")]
        public async Task<string> Start_Production([FromBody] Receiver receiver)
        {
            Machines machines = receiver.machines;
            Cut_Project cut_Project = receiver.cut_Project;

            string query = "Update dbo.[Machines] SET Status = @Status, Last_Cut_id = @Last_Cut_id WHERE No = @No";
            SqlCommand command = new SqlCommand(query, cnn);

            command.Parameters.Add("@No", SqlDbType.Int, 40).Value = machines.No;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";
            command.Parameters.Add("@Last_Cut_id", SqlDbType.Int, 40).Value = cut_Project.Cut_id;


            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();

            query = "UPDATE dbo.[Cut_Project] SET Status = @Status WHERE Cut_id = @Cut_id";
            command = new SqlCommand(query, cnn);

            command.Parameters.Add("@Cut_id", SqlDbType.Int).Value = cut_Project.Cut_id;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "W trakcie cięcia";

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();


            return "Rozpoczęto pcięcie";
        }

        /*[HttpPost("Save_and_cut")]
        public async Task<List<Glass>> Save_and_cut([FromBody] Receiver receiver)
        {
            List<Glass> glasses = receiver.glasses;
            Order order = receiver.order;
            User user = receiver.user;
            Machines machines = receiver.machines;
            int code;

            foreach(Order ord in orderController.GetOrders())
            {
                if(ord.Id_Order == order.Id_Order)
                {
                    order.Owner = ord.Owner;
                    break;
                }
            }

            try
            {
                code = GetCut_Project().OrderBy(cutid => cutid.Cut_id).Last().Cut_id + 1;
            }
            catch (Exception e)
            {
                code = 1;
            }

            string query = "INSERT INTO dbo.[Cut_Project](Cut_id, Order_id, Status) VALUES(@Cut_id,@Order_id, @Status)";
            SqlCommand command = new SqlCommand(query, cnn);

            command.Parameters.Add("@Cut_id", SqlDbType.Int).Value = code;
            command.Parameters.Add("@Order_id", SqlDbType.VarChar, 40).Value = order.Id_Order;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Wykonany";

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();

            foreach (Glass glass in glasses)
            {
                query = "UPDATE dbo.[Glass] SET Cut_id = @Cut_id, Used = @Used  WHERE Glass_Id = @Glass_Id";
                command = new SqlCommand(query, cnn);

                command.Parameters.Add("@Cut_id", SqlDbType.Int).Value = code;
                command.Parameters.Add("@Glass_Id", SqlDbType.Int).Value = glass.Glass_info.First().Id;
                command.Parameters.Add("@Used", SqlDbType.Bit).Value = 1;

                cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                cnn.Close();


                foreach (Item item in orderController.GetItems(order))
                {
                    foreach (Piece piece in glass.Glass_info.First().Pieces)
                    {
                        if (piece.id == item.Id)
                        {
                            query = "INSERT INTO dbo.[Product](Id,Owner,Desk,Status,Id_item,Id_order) VALUES(@Id,@Owner,@Desk,@Status,@Id_item,@Id_order)";
                            command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@Id", SqlDbType.Int).Value = code;
                            command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = order.Owner;
                            command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = "";
                            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";
                            command.Parameters.Add("@Id_item", SqlDbType.VarChar, 40).Value = item.Id;
                            command.Parameters.Add("@Id_order", SqlDbType.VarChar, 40).Value = order.Id_Order;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();

                            query = "UPDATE dbo.[Item] SET Product_id = @Product_id, Status = @Status, Cut_id = @Cut_id WHERE Id = @Id";
                            command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@Id", SqlDbType.Int).Value = item.Id;
                            command.Parameters.Add("@Product_id", SqlDbType.Int).Value = code;
                            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";
                            command.Parameters.Add("@Cut_id", SqlDbType.Int).Value = code;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();
                        }
                    }
                }

                string userhistory = "You cutted project " + code;
                usersController.Insert_User_History(userhistory, user.Login);
            }
            return glasses;
        }
        */
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
                                    if(i.Id == itm.Id)
                                    {
                                        kon = true;
                                    }
                                }
                                if(kon == true)
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

        [HttpPost("Magic")]
        public async Task<List<Glass>> Magic([FromBody] Receiver receiver)
        {
            List<Glass> wynik = new List<Glass>();
            User user = receiver.user;
            Order order = receiver.order;
            Item item1 = receiver.item;
            List<Glass> glasses = new List<Glass>();
            bool kon = false;
            int kontrol;

            Package packages = new Package();
            Package backup = new Package();

            foreach (Item item in orderController.GetItems(order))
            {
                if (item.Cut_id == 0 && item.Color == item1.Color && item.Type == item1.Type && item1.Thickness == item.Thickness && item.Status == "Awaiting")
                {
                    packages.Item.Add(item);
                    backup.Item.Add(item);
                }
            }

            kontrol = packages.Item.Count;

            Return_Area(packages);
            Set_Package(packages);
            Sort_Package(packages);

            List<Glass> tempo = magazineController.Getglass();

            foreach (Glass glass in magazineController.Getglass())
            {
                if (glass.Type == item1.Type && glass.Color == item1.Color && item1.Thickness == glass.Hight)
                {
                    Glass glass1 = new Glass();

                    glass1.Length = glass.Length;
                    glass1.Width = glass.Width;
                    glass1.Length = glass.Length;
                    glass1.Hight = glass.Hight;


                    foreach (Glass_Id glass_Id in glass.Glass_info)
                    {
                        if (glass_Id.Destroyed == false && glass_Id.Used == false && glass_Id.Removed == false && glass_Id.Cut_id == 0)
                        {
                            glass1.Glass_info.Add(glass_Id);
                        }
                    }

                    glasses.Add(glass1);
                }
            }

            glasses.OrderBy(gla => gla.Length).ThenBy(gla2 => gla2.Width);

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (Glass glass in glasses)
                    {
                        foreach (Glass_Id glass_id in glass.Glass_info)
                        {
                            if (packages.Item.Count > 0)
                            {
                                Glass_Id glass_Id2 = new Glass_Id { Pieces = new List<Piece>() };
                                List<Item> Used = new List<Item>();
                                List<Cuboid> temporary = new List<Cuboid>();
                                foreach (Item itm in packages.Item)
                                {
                                    temporary.Add(new Cuboid(Convert.ToDecimal(itm.Width), Convert.ToDecimal(itm.Length), Convert.ToDecimal(itm.Thickness)));
                                }

                                var parameter = new BinPackParameter(Convert.ToDecimal(glass.Length), Convert.ToDecimal(glass.Width), Convert.ToDecimal(glass.Hight), temporary);

                                Glass tmp = new Glass();

                                var binPacker = BinPacker.GetDefault(BinPackerVerifyOption.BestOnly);

                                var result = binPacker.Pack(parameter);

                                tmp.Width = Convert.ToDouble(parameter.BinWidth);
                                tmp.Hight = Convert.ToDouble(parameter.BinDepth);
                                tmp.Length = Convert.ToDouble(parameter.BinHeight);

                                foreach (var cub in result.BestResult[0])
                                {
                                    foreach (Item itm in packages.Item)
                                    {
                                        if (itm.Width == Convert.ToDouble(cub.Width) && itm.Length == Convert.ToDouble(cub.Height))
                                        {
                                            foreach (Item i in Used)
                                            {
                                                if (i.Id == itm.Id)
                                                {
                                                    kon = true;
                                                }
                                            }
                                            if (kon == false)
                                            {
                                                glass_Id2.Pieces.Add(new Piece { id = itm.Id, X = Convert.ToDouble(cub.X), Y = Convert.ToDouble(cub.Y), Lenght = Convert.ToDouble(cub.Height), Widht = Convert.ToDouble(cub.Width) });
                                                Item iteme = new Item { Id = itm.Id };

                                                Used.Add(iteme);
                                                kon = false;
                                                break;
                                            }
                                            kon = false;
                                        }
                                    }                                                                       
                                }

                                tmp.Glass_info.Add(glass_Id2);
                                wynik.Add(tmp);

                                try
                                {
                                    foreach (Item itm in Used)
                                    {
                                        packages.Item.Remove(packages.Item.First(i => i.Id == itm.Id));
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.ToString());
                                }
                            }
                        }
                    }

                   kontrol = 0;

                   foreach(Glass gl in wynik)
                    {
                        foreach(Glass_Id gl2 in gl.Glass_info)
                        {
                            kontrol += gl2.Pieces.Count;
                        }
                    }

                    if (wynik.Count < backup.Item.Count)
                    {
                        List<int> Done = new List<int>();
                        Glass tmp = new Glass();
                        tmp.Error_Messege = "zabraklo miejsca dla: ";

                        Glass_Id glass_Id = new Glass_Id();

                        foreach(Item itm in backup.Item)
                        {
                            foreach(Glass temp in wynik)
                            {
                                foreach(Glass_Id glass_Ids in temp.Glass_info)
                                {
                                    foreach(Piece piece in glass_Ids.Pieces)
                                    {
                                        if(itm.Id == piece.id)
                                        {
                                            Done.Add(itm.Id);
                                        }
                                    }
                                }
                            }
                        }

                        for (int i = wynik.Count - 1; i < packages.Item.Count; i++)
                        {
                            Piece piece = new Piece { id = packages.Item[i].Id, Lenght = packages.Item[i].Length, Widht = packages.Item[i].Width };
                            glass_Id.Pieces.Add(piece);
                            tmp.Error_Messege = tmp.Error_Messege + ", " + packages.Item[i].Id;
                        }
                        tmp.Glass_info.Add(glass_Id);
                        wynik.Add(tmp);
                    }

                    return wynik;
                }
            }

            //błąd nie ma takiego usera
            return wynik;

        }

    }
}