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

        public void Insert_User_History(string Description, string Login)
        {
            string data = DateTime.Today.ToString("g");

            string query = "INSERT INTO dbo.User_History(Data, Description, Login) VALUES(@data, @Description, @Login)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();
        }

        public void Insert_Machine_History_All(int No, string Login, string Description)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.Machines_History_All(Date,No, Login, Description) VALUES(@Date, @No, @Login, @Description)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Date", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@No", SqlDbType.Int).Value = No;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();
        }

        public void Insert_Machine_History_All(string Login, string Description)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.Machines_History_All(Date, Login, Description) VALUES(@data, @Login, @Description)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();
        }

        public void Insert_Machine_History(int Cut_id, string Login, string Description, int No)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.Machines_History_All(Date, Cut_Id, Login, Description, No) VALUES(@data, @Cut_Id, @Login, @Description, @No)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Cut_Id", SqlDbType.Int).Value = Cut_id;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@No", SqlDbType.Int).Value = No;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();
        }

        public void Insert_Magazine_History(string Description, string Login)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.Magazine_History(Data, Login, Description) VALUES(@data, @Login, @Description)";

            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();
        }

        public void Insert_Order_History(string Description, string Login, string Id_Order)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.[Order_History](Date, Login, Description, Id_Order) VALUES(@data, @Login, @Description, @Id_Order)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = Id_Order;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();
        }
    }
}
