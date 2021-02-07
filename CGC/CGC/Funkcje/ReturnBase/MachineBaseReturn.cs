using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.MachineFuncFolder.MachineBase
{
    public class MachineBaseReturn
    {
        private Connect connect = new Connect();

        private static MachineBaseReturn m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static MachineBaseReturn Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new MachineBaseReturn();
                    }
                    return m_oInstance;
                }
            }
        }

        public List<Machines> GetMachines()
        {
            List<Machines> temp = new List<Machines>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines];", connect.cnn);
            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Machines machines = new Machines();
                machines.No = sqlDataReader["No"].ToString();
                machines.Status = sqlDataReader["Status"].ToString();
                machines.Type = sqlDataReader["Type"].ToString();
                machines.Stan = Convert.ToBoolean(sqlDataReader["Stan"]);

                try
                {
                    machines.Last_Cut_id = sqlDataReader["Last_Cut_id"].ToString();
                }
                catch (Exception e)
                {
                    machines.Last_Cut_id = "0";
                }

                temp.Add(machines);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<Machines> GetMachines(string status, bool stan)
        {
            List<Machines> temp = new List<Machines>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines] WHERE Status = @Status AND Stan = @Stan;", connect.cnn);

            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = status;
            command.Parameters.Add("@Stan", SqlDbType.Bit).Value = stan;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Machines machines = new Machines();
                machines.No = sqlDataReader["No"].ToString();
                machines.Status = sqlDataReader["Status"].ToString();
                machines.Type = sqlDataReader["Type"].ToString();
                machines.Stan = Convert.ToBoolean(sqlDataReader["Stan"]);

                try
                {
                    machines.Last_Cut_id = sqlDataReader["Last_Cut_id"].ToString();
                }
                catch (Exception e)
                {
                    machines.Last_Cut_id = "0";
                }

                temp.Add(machines);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }

        public List<Machines_History_All> GetMachinesHistoryAll()
        {
            List<Machines_History_All> machines_History_Alls = new List<Machines_History_All>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines_History_All];", connect.cnn);
            connect.cnn.Open();

      
            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Machines_History_All machines_History_All = new Machines_History_All();

                machines_History_All.Login = sqlDataReader["Login"].ToString();
                machines_History_All.Date = sqlDataReader["Date"].ToString();
                machines_History_All.Description = sqlDataReader["Description"].ToString();

                try
                {
                    machines_History_All.No = sqlDataReader["No"].ToString();
                }
                catch (Exception e)
                {
                    e.ToString();
                }

                machines_History_Alls.Add(machines_History_All);

            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            

            return machines_History_Alls;
        }

        public List<Machines_History> GetMachinesHistory(string No)
        {
            List<Machines_History> machines_Historys = new List<Machines_History>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines_History] WHERE No = @No;", connect.cnn);

            command.Parameters.Add("@No", SqlDbType.VarChar, 40).Value = No;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Machines_History machines_History = new Machines_History();
                machines_History.No = sqlDataReader["No"].ToString();
                machines_History.Cut_Id = sqlDataReader["Cut_Id"].ToString();
                machines_History.Login = sqlDataReader["Login"].ToString();
                machines_History.Date = sqlDataReader["Date"].ToString();
                machines_History.Description = sqlDataReader["Description"].ToString();

                machines_Historys.Add(machines_History);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return machines_Historys;
        }

        public List<string> Get_Types()
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines_Type];", connect.cnn);
            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string type = sqlDataReader["Type"].ToString();
                temp.Add(type);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();
            return temp;
        }
    }
}
