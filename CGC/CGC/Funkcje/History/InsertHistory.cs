using CGC.Funkcje.CutFuncFolder.CutBase;
using CGC.Funkcje.MachineFuncFolder.MachineBase;
using CGC.Funkcje.MagazineFuncFolder.MagazineBase;
using CGC.Funkcje.OrderFuncFolder.OrderBase;
using CGC.Funkcje.ProductFuncFolder.ProductBase;
using CGC.Funkcje.UserFuncFolder.UserReturn;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.History
{
    public class InsertHistory
    {
        private static InsertHistory m_oInstance = null;
        private static readonly object m_oPadLock = new object();
        private CutBaseReturn cutBaseReturn = new CutBaseReturn();
        private OrderBaseReturn orderBaseReturn = new OrderBaseReturn();
        private MagazineBaseReturn magazineBaseReturn = new MagazineBaseReturn();
        private UserBaseReturn userBaseReturn = new UserBaseReturn();
        private ProductBaseReturn productBaseReturn = new ProductBaseReturn();
        private MachineBaseReturn machineBaseReturn = new MachineBaseReturn();

        public static InsertHistory Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new InsertHistory();
                    }
                    return m_oInstance;
                }
            }
        }

        Connect connect = new Connect();

        //Wprowadzanie historii

        public void Insert_User_History(string Description, string Login, string company)
        {
            string data = DateTime.Now.ToString("g");
            string LastGlobalId = userBaseReturn.GetLastGlobalIdUserHistory(company).Last().Global_Id.ToString();

            string query = "INSERT INTO dbo.User_History(Global_id, Data, Description, Login, Company) VALUES(@Global_id, @data, @Description, @Login, @Company)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalId;
            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();
        }

        public void Insert_Machine_History_All(string No, string Login, string Description, string company)
        {
            string data = DateTime.Now.ToString("g");
            string LastGlobalId = machineBaseReturn.GetLastGlobalIdMachineHistoryAll(company).Last().Global_Id.ToString();

            string query = "INSERT INTO dbo.Machines_History_All(Global_id, Date,No, Login, Description, Company) VALUES(@Global_id, @Date, @No, @Login, @Description, @Company)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalId;
            command.Parameters.Add("@Date", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@No", SqlDbType.VarChar, 40).Value = No;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();
        }

        public void Insert_Machine_History_All(string Login, string Description, string company)
        {
            try
            {
                string data = DateTime.Now.ToString("g");
                string LastGlobalId = machineBaseReturn.GetLastGlobalIdMachineHistoryAll(company).Last().Global_Id.ToString();

                string query = "INSERT INTO dbo.Machines_History_All(Global_id, Date, Login, Description, Company) VALUES(@Global_id, @data, @Login, @Description, @Company)";
                SqlCommand command = new SqlCommand(query, connect.cnn);

                command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalId;
                command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
                command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
                command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
                command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

                connect.cnn.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connect.cnn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Insert_Machine_History(string Cut_id, string Login, string Description, string No, string company)
        {
            string data = DateTime.Now.ToString("g");
            string LastGlobalId = machineBaseReturn.GetLastGlobalIdMachineHistory(company).Last().Global_Id.ToString();

            string query = "INSERT INTO dbo.Machines_History(Global_id, Date, Cut_Id, Login, Description, No, Company) VALUES(@Global_id, @data, @Cut_Id, @Login, @Description, @No, @Company)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalId;
            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Cut_Id", SqlDbType.VarChar, 40).Value = Cut_id;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@No", SqlDbType.VarChar, 40).Value = No;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();
        }

        public void Insert_Magazine_History(string Description, string Login, string company)
        {
            string data = DateTime.Now.ToString("g");
            string LastGlobalId = magazineBaseReturn.GetLastGlobalIdMagazineHistory(company).Last().Global_Id.ToString();

            string query = "INSERT INTO dbo.Magazine_History(Global_id, Data, Login, Description, Company) VALUES(@Global_id, @data, @Login, @Description, @Company)";

            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalId;
            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();
        }

        public void Insert_Order_History(string Description, string Login, string Id_Order, string company)
        {
            string data = DateTime.Now.ToString("g");
            string LastGlobalId = orderBaseReturn.GetLastGlobalIdOrderHistory(company).Last().Global_Id.ToString();

            string query = "INSERT INTO dbo.[Order_History](Global_id, Date, Login, Description, Id_Order, Company) VALUES(@Global_id, @data, @Login, @Description, @Id_Order, @Company)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalId;
            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = Id_Order;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();
        }

        public void InsertProductHistory(string Id, string Login, string Description, string company)
        {
            string data = DateTime.Now.ToString("g");
            string LastGlobalId = productBaseReturn.GetLastGlobalIdProductHistory(company).Last().Global_Id.ToString();

            string query = "INSERT INTO dbo.[Product_History](Global_id, Data, Login, Description, Id, Company) VALUES(@Global_id, @data, @Login, @Description, @Id, @Company)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalId;
            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@Id", SqlDbType.VarChar, 40).Value = Id;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();
        }

    }
}
