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
                machines.No = Convert.ToInt32(sqlDataReader["No"]);
                machines.Status = sqlDataReader["Status"].ToString();
                machines.Type = sqlDataReader["Type"].ToString();
                machines.Stan = Convert.ToBoolean(sqlDataReader["Stan"]);

                try
                {
                    machines.Last_Cut_id = Convert.ToInt32(sqlDataReader["Last_Cut_id"]);
                }
                catch (Exception e)
                {
                    machines.Last_Cut_id = 0;
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
                machines_History_All.No = Convert.ToInt32(sqlDataReader["No"]);
                machines_History_All.Login = sqlDataReader["Login"].ToString();
                machines_History_All.Date = sqlDataReader["Date"].ToString();
                machines_History_All.Description = sqlDataReader["Description"].ToString();

                machines_History_Alls.Add(machines_History_All);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return machines_History_Alls;
        }

        public List<Machines_History> GetMachinesHistory(int No)
        {
            List<Machines_History> machines_Historys = new List<Machines_History>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines_History] WHERE No = @No;", connect.cnn);

            command.Parameters.Add("@No", SqlDbType.Int).Value = No;

            connect.cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Machines_History machines_History = new Machines_History();
                machines_History.No = Convert.ToInt32(sqlDataReader["No"]);
                machines_History.Cut_Id = Convert.ToInt32(sqlDataReader["Cut_Id"]);
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
