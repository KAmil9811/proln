using CGC.Funkcje.CutFuncFolder.CutBase;
using CGC.Funkcje.MagazineFuncFolder.MagazineBase;
using CGC.Funkcje.OrderFuncFolder.OrderBase;
using CGC.Funkcje.ProductFuncFolder.ProductBase;
using CGC.Funkcje.UserFuncFolder.UserReturn;
using CGC.Models;
using Sharp3DBinPacking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.CutFuncFolder
{
    public class CutFunc
    {
        private CutCheck cutCheck = new CutCheck();
        private CutBaseReturn cutBaseReturn = new CutBaseReturn();
        private OrderBaseReturn orderBaseReturn = new OrderBaseReturn();
        private MagazineBaseReturn magazineBaseReturn = new MagazineBaseReturn();
        private UserBaseReturn userBaseReturn = new UserBaseReturn();
        private CutBaseModify cutBaseModify = new CutBaseModify();
        private ProductBaseReturn productBaseReturn = new ProductBaseReturn();

        private static CutFunc m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static CutFunc Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new CutFunc();
                    }
                    return m_oInstance;
                }
            }
        }

        public List<Order> Return_Orders_To_Cut()
        {
            return cutCheck.Return_Orders_To_Cut();
        }

        public List<Package> Return_Package_To_Cut(Receiver receiver)
        {
            return cutCheck.Return_Package_To_Cut(receiver);
        }

        public List<Glass> Return_Glass_To_Cut(Receiver receiver)
        {
            return cutCheck.Return_Glass_To_Cut(receiver);
        }

        public List<Machines> Return_Machine_To_Cut()
        {
            return cutCheck.Return_Machine_To_Cut();
        }

        public List<Cut_Project> Return_Cut_Project()
        {
            return cutBaseReturn.Get_Cut_Project_User();
        }

        public List<Glass> Return_Porject(Receiver receiver)
        {
            List<Glass> wynik = new List<Glass>();
            int Cut_id = receiver.id;
            Order order = receiver.order;
            int kontrol;
            List<Glass> glasses = new List<Glass>();
            bool kon = false;

            Package packages = new Package();
            Package backup = new Package();

            foreach (Item item in orderBaseReturn.GetItems(order))
            {
                if (item.Cut_id == Cut_id)
                {
                    packages.Item.Add(item);
                    backup.Item.Add(item);
                }
            }

            kontrol = packages.Item.Count;

            cutCheck.Return_Area(packages);
            cutCheck.Set_Package(packages);
            cutCheck.Sort_Package(packages);

            foreach (Glass glass in magazineBaseReturn.Getglass())
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

                    cutCheck.Set_Fit(package, Pw, Pl);

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

                                cutCheck.Set_Fit(package, W_2, L_2);
                                package.Item = package.Item.OrderByDescending(item1 => item1.Fit_pos).ThenByDescending(item1 => item1.Area).ThenByDescending(item1 => item1.Length).ThenByDescending(item1 => item1.Width).ToList();

                                Item item = cutCheck.Find_Area(package);

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

                                cutCheck.Set_Fit(package, W_1, L_1);
                                package.Item = package.Item.OrderByDescending(item1 => item1.Fit_pos).ThenByDescending(item1 => item1.Area).ThenByDescending(item1 => item1.Length).ThenByDescending(item1 => item1.Width).ToList();

                                Item item = cutCheck.Find_Area(package);

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

        public List<Glass> Magic2(Receiver receiver)
        {
            List<Glass> wynik = new List<Glass>();
            User user = receiver.user;
            Order order = receiver.order;
            Item item1 = receiver.item;
            List<Glass> glasses = new List<Glass>();
            int kontrol;

            Package packages = new Package();
            Package backup = new Package();

            foreach (Item item in orderBaseReturn.GetItems(order))
            {
                if (item.Color == item1.Color && item.Type == item1.Type && item1.Thickness == item.Thickness && item.Status == "Awaiting")
                {
                    packages.Item.Add(item);
                    backup.Item.Add(item);
                }
            }

            kontrol = packages.Item.Count;

            cutCheck.Return_Area(packages);
            cutCheck.Set_Package(packages);
            cutCheck.Sort_Package(packages);

            List<Glass> tempo = magazineBaseReturn.Getglass();

            foreach (Glass glass in magazineBaseReturn.Getglass())
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

            foreach (User usere in userBaseReturn.GetUsers())
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

                               cutCheck.Set_Pieces(glass_id.Pieces);

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

        public int Save_Project(Receiver receiver)
        {
            List<Glass> glasses = receiver.glasses;
            Order order = receiver.order;
            int code;

            try
            {
                code = cutBaseReturn.GetCut_Project().OrderBy(cutid => cutid.Cut_id).Last().Cut_id + 1;
            }
            catch (Exception e)
            {
                code = 1;
            }

            foreach(Glass glass in magazineBaseReturn.Getglass())
            {
                foreach (Glass_Id glass_Id in glass.Glass_info)
                {
                    foreach (Glass gla in glasses)
                    {
                        if (glass_Id.Id == gla.Glass_info.First().Id && glass_Id.Cut_id != 0)
                        {
                            return 0;
                        }

                        foreach (Item item in orderBaseReturn.GetItems(order))
                        {
                            foreach (Piece piece in gla.Glass_info.First().Pieces)
                            {
                                if (piece.id == item.Id && item.Cut_id != 0)
                                {
                                    return 0;
                                }
                            }
                        }
                    }                 
                }
            }

            return cutBaseModify.Save_Project(receiver.user, order, code, glasses);
        }

        public List<Cut_Project> Remove_Project(Receiver receiver)
        {
            List<Cut_Project> cut_Projects = new List<Cut_Project>();
            Order order = new Order();
            User user = receiver.user;

            Cut_Project cut_Project = receiver.cut_Project;
            order.Id_Order = cut_Project.Order_id;

            return cutBaseModify.Remove_Project(cut_Project, order, user);
        }

        public string Post_Production(Receiver receiver)
        {
            User user = receiver.user;
            Machines machines = receiver.machines;
            Cut_Project cut_Project = receiver.cut_Project;

            foreach (Cut_Project cut in cutBaseReturn.GetCut_Project())
            {
                if (cut.Cut_id == cut_Project.Cut_id)
                {
                    foreach (Order ord in orderBaseReturn.GetOrders())
                    {
                        if (ord.Id_Order == cut.Order_id)
                        {
                            foreach (Item item in orderBaseReturn.GetItems(ord))
                            {
                                if (item.Cut_id == cut.Cut_id)
                                {
                                    int code;
                                    try
                                    {
                                        code = productBaseReturn.GetProducts().OrderBy(pro => pro.Id).Last().Id + 1;
                                    }
                                    catch (Exception e)
                                    {
                                        code = 1;
                                    }

                                    cutBaseModify.Post_Production(user, ord, item, code);
                                }
                            }
                        }
                    }
                }
            }

            return cutBaseModify.Post_Production_step2(user, machines, cut_Project);
        }

        public string Start_Production(Receiver receiver)
        {
            return cutBaseModify.Start_Production(receiver.machines, receiver.cut_Project);
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
        

        public List<Glass> Magic(Receiver receiver)
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

            foreach (Item item in orderBaseReturn.GetItems(order))
            {
                if (item.Cut_id == 0 && item.Color == item1.Color && item.Type == item1.Type && item1.Thickness == item.Thickness && item.Status == "Awaiting")
                {
                    packages.Item.Add(item);
                    backup.Item.Add(item);
                }
            }

            kontrol = packages.Item.Count;

            cutCheck.Return_Area(packages);
            cutCheck.Set_Package(packages);
            cutCheck.Sort_Package(packages);

            List<Glass> tempo = magazineBaseReturn.Getglass();

            foreach (Glass glass in magazineBaseReturn.Getglass())
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

            foreach (User usere in userBaseReturn.GetUsers())
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
                                        else if (itm.Width == Convert.ToDouble(cub.Height) && itm.Length == Convert.ToDouble(cub.Width))
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
            }

            //błąd nie ma takiego usera
            return wynik;
        }

    }
}
