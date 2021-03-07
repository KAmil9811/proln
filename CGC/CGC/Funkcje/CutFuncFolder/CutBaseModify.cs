using CGC.Funkcje.History;
using CGC.Funkcje.MagazineFuncFolder.MagazineBase;
using CGC.Funkcje.OrderFuncFolder.OrderBase;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.CutFuncFolder.CutBase
{
    public class CutBaseModify
    {
        private Connect connect = new Connect();
        private OrderBaseReturn orderBaseReturn = new OrderBaseReturn();
        private MagazineBaseReturn magazineBaseReturn = new MagazineBaseReturn();
        private InsertHistory insertHistory = new InsertHistory();
        private static CutBaseModify m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static CutBaseModify Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new CutBaseModify();
                    }
                    return m_oInstance;
                }
            }
        }

        public int Save_Project(User user, Order order, int code, List<Glass> glasses, List<Piece> pieces, string LastGlobalIdProject)
        {
            try
            {
                string query = "INSERT INTO dbo.[Cut_Project](Global_id, Cut_id, Order_id, Status, Company) VALUES(@Global_id, @Cut_id,@Order_id, @Status, @Company)";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalIdProject;
                command.Parameters.Add("@Cut_id", SqlDbType.VarChar, 40).Value = code.ToString();
                command.Parameters.Add("@Order_id", SqlDbType.VarChar, 40).Value = order.Id_Order;
                command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Saved";
                command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                foreach (Glass glass in glasses)
                {
                    if (glass.Error_Messege == null)
                    {
                        query = "UPDATE dbo.[Glass] SET Cut_id = @Cut_id WHERE Glass_Id = @Glass_Id AND Company = @Company";
                        command = new SqlCommand(query, connect.cnn);

                        command.Parameters.Add("@Cut_id", SqlDbType.VarChar, 40).Value = code.ToString();
                        command.Parameters.Add("@Glass_Id", SqlDbType.VarChar, 40).Value = glass.Ids;
                        command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                        connect.cnn.Open();
                        command.ExecuteNonQuery();
                        command.Dispose();
                        connect.cnn.Close();


                        foreach (Item item in orderBaseReturn.GetItems(order, user.Company))
                        {
                            foreach (Piece piece in pieces)
                            {
                                if (piece.Id == item.Id)
                                {
                                    query = "UPDATE dbo.[Item] SET Cut_id = @Cut_id WHERE Id = @Id AND Company = @Company";
                                    command = new SqlCommand(query, connect.cnn);

                                    command.Parameters.Add("@Cut_id", SqlDbType.VarChar, 40).Value = code.ToString();
                                    command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = item.Id;
                                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                                    connect.cnn.Open();
                                    command.ExecuteNonQuery();
                                    command.Dispose();
                                    connect.cnn.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                return 0;
            }

            string userhistory = "You saved project: " + code;

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);

            return code;
        }

        public List<Cut_Project> Remove_Project(Cut_Project cut_Project, Order order, User user)
        {
            List<Cut_Project> cut_Projects = new List<Cut_Project>();

            string query = "UPDATE dbo.[Cut_Project] SET Status = @Status WHERE Cut_id = @Cut_id AND Company = @Company;)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Cut_id", SqlDbType.VarChar, 40).Value = cut_Project.Cut_id;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Deleted";
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();


            query = "UPDATE dbo.[Glass] SET Cut_id = @Cut_id2 WHERE Cut_id = @Cut_id AND Company = @Company";
            command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Cut_id2", SqlDbType.VarChar, 40).Value = "0";
            command.Parameters.Add("@Cut_id", SqlDbType.VarChar, 40).Value = cut_Project.Cut_id;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();



            query = "UPDATE dbo.[Item] SET Cut_id = @Cut_id2 WHERE Cut_id = @Cut_id AND Company = @Company";
            command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Cut_id2", SqlDbType.Int).Value = "0";
            command.Parameters.Add("@Cut_id", SqlDbType.Int).Value = cut_Project.Cut_id;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You deleted project: " + cut_Project.Cut_id;

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);

            cut_Projects.Add(cut_Project);
            return cut_Projects;
        }

        public void Post_Production(User user, Order ord, Item item, int code, string LastGlobalIdProduct)
        {
            try
            {
                string query = "INSERT INTO dbo.[Product](Global_id, Id,Owner,Desk,Status,Id_item,Id_order, Company) VALUES(@Global_id, @Id,@Owner,@Desk,@Status,@Id_item,@Id_order, @Company)";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalIdProduct;
                command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = code.ToString();
                command.Parameters.Add("@Owner", SqlDbType.VarChar, 40).Value = ord.Owner;
                command.Parameters.Add("@Desk", SqlDbType.VarChar, 40).Value = "";
                command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";
                command.Parameters.Add("@Id_item", SqlDbType.VarChar, 40).Value = item.Id;
                command.Parameters.Add("@Id_order", SqlDbType.VarChar, 40).Value = ord.Id_Order;
                command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                query = "UPDATE dbo.[Item] SET Product_id = @Product_id, Status = @Status WHERE Id = @Id AND Company = @Company";
                command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = item.Id;
                command.Parameters.Add("@Product_id", SqlDbType.VarChar, 40).Value = code.ToString();
                command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";
                command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();

                string producthistory = "Product has been cutted";
                string orderhistory = item.Id + " has been cutted";

                insertHistory.InsertProductHistory(code.ToString(), user.Login, producthistory, user.Company);
                insertHistory.Insert_Order_History(orderhistory, user.Login, ord.Id_Order, user.Company);
            }
            catch(Exception e)
            {
                e.ToString();
            }

        }

        public string Post_Production_step2(User user, Machines machines, Cut_Project cut_Project)
        {
            string query2 = "UPDATE dbo.[Glass] SET Used = @Used WHERE Cut_id = @Cut_id AND Company = @Company";
            SqlCommand command2 = new SqlCommand(query2, connect.cnn);

            command2.Parameters.Add("@Cut_id", SqlDbType.VarChar, 40).Value = cut_Project.Cut_id;
            command2.Parameters.Add("@Used", SqlDbType.Bit).Value = 1;
            command2.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();
            command2.ExecuteNonQuery();
            command2.Dispose();
            connect.cnn.Close();

            foreach(Glass gl in magazineBaseReturn.Getglass(user.Company))
            {
                foreach(Glass_Id glass_Id in gl.Glass_info)
                {
                    if(glass_Id.Cut_id == cut_Project.Cut_id)
                    {
                        string magazinehistory = "Glass " + glass_Id.Id + " has been used";
                        insertHistory.Insert_Magazine_History(magazinehistory, user.Login, user.Company);
                    }
                }
            }


            string query3 = "UPDATE dbo.[Cut_Project] SET Status = @Status WHERE Cut_id = @Cut_id AND Company = @Company";
            SqlCommand command3 = new SqlCommand(query3, connect.cnn);

            command3.Parameters.Add("@Cut_id", SqlDbType.VarChar, 40).Value = cut_Project.Cut_id;
            command3.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";
            command3.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();
            command3.ExecuteNonQuery();
            command3.Dispose();
            connect.cnn.Close();

            string query4 = "Update dbo.[Machines] SET Status = @Status WHERE No = @No AND Company = @Company";
            SqlCommand command4 = new SqlCommand(query4, connect.cnn);

            command4.Parameters.Add("@No", SqlDbType.VarChar, 40).Value = machines.No;
            command4.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";
            command4.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;


            connect.cnn.Open();
            command4.ExecuteNonQuery();
            command4.Dispose();
            connect.cnn.Close();


            string userhistory = "You cutted project " + cut_Project.Cut_id;
            string machinehistory = "Project " + cut_Project.Cut_id + " has been cutted";

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Machine_History(cut_Project.Cut_id, user.Login, machinehistory, machines.No, user.Company);

            return "Ready";
        }

        public string Start_Production(Machines machines, Cut_Project cut_Project, User user)
        {
            string query = "Update dbo.[Machines] SET Status = @Status, Last_Cut_id = @Last_Cut_id WHERE No = @No AND Company = @Company";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@No", SqlDbType.VarChar, 40).Value = machines.No;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "In use";
            command.Parameters.Add("@Last_Cut_id", SqlDbType.VarChar, 40).Value = cut_Project.Cut_id;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;


            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            query = "UPDATE dbo.[Cut_Project] SET Status = @Status WHERE Cut_id = @Cut_id AND Company = @Company";
            command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Cut_id", SqlDbType.VarChar).Value = cut_Project.Cut_id;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "On production";
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();


            return "Rozpoczeto ciecie";
        }
    }
}
