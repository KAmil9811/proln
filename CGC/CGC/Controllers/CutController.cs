using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CGC.Funkcje.CutFuncFolder;
using CGC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Sharp3DBinPacking;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public sealed class CutController : Controller
    {
        private CutFunc cutFunc = new CutFunc();
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

        [HttpGet("Return_Orders_To_Cut")]
        public async Task<List<Order>> Return_Orders_To_Cut()
        {
            return cutFunc.Return_Orders_To_Cut();
        }

        [HttpPost("Return_Package_To_Cut")]
        public async Task<List<Package>> Return_Package_To_Cut([FromBody]Receiver receiver)
        {
            return cutFunc.Return_Package_To_Cut(receiver);
        }

        [HttpGet("Return_Glass_To_Cut")]
        public async Task<List<Glass>> Return_Glass_To_Cut([FromBody] Receiver receiver)
        {
            return cutFunc.Return_Glass_To_Cut(receiver);
        }

        [HttpGet("Return_Machine_To_Cut")]
        public async Task<List<Machines>> Return_Machine_To_Cut()
        {
            return cutFunc.Return_Machine_To_Cut();
        }

        [HttpGet("Return_All_Project")]
        public async Task<List<Cut_Project>> Return_All_Project()
        {
            return cutFunc.Return_Cut_Project();
        }

        [HttpPost("Return_Porject")]
        public async Task<List<Glass>> Return_Porject([FromBody] Receiver receiver)
        {
            return cutFunc.Return_Porject(receiver);
        }

        /*[HttpPost("Magic")]
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
        */

        [HttpPost("Save_Project")]
        public async Task<int> Save_Project([FromBody] Receiver receiver)
        {
            return cutFunc.Save_Project(receiver);
        }

        [HttpPost("Remove_Project")]
        public async Task<List<Cut_Project>> Remove_Project([FromBody] Receiver receiver)
        {
            return cutFunc.Remove_Project(receiver);
        }

        [HttpPost("Post_Production")]
        public async Task<string> Post_Production([FromBody] Receiver receiver)
        {
            return cutFunc.Post_Production(receiver);
        }

        [HttpPost("Start_Production")]
        public async Task<string> Start_Production([FromBody] Receiver receiver)
        {
            return cutFunc.Start_Production(receiver);
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

        [HttpPost("Magic")]
        public async Task<List<Glass>> Magic([FromBody] Receiver receiver)
        {
            return cutFunc.Magic(receiver);
        }

        [HttpPost("CreatePdf")]
        public async Task<string> CreatePdf([FromBody] Receiver receiver)
        {
            return cutFunc.CreatePdf(receiver);
        }
    }
}