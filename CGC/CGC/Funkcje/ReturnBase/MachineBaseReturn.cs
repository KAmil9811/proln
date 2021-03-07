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

        public List<Machines> GetMachines(string company)
        {
            List<Machines> temp = new List<Machines>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines] WHERE Company = @Company;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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

        public List<Machines> GetMachine(string cut_id, string company)
        {
            List<Machines> temp = new List<Machines>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines] WHERE Last_Cut_id = @Last_Cut_id AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Last_Cut_id", SqlDbType.VarChar, 40).Value = cut_id;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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

        public List<Machines> GetMachines(string status, bool stan, string company)
        {
            List<Machines> temp = new List<Machines>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines] WHERE Status = @Status AND Stan = @Stan AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = status;
            command.Parameters.Add("@Stan", SqlDbType.Bit).Value = stan;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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

        public List<Machines_History_All> GetMachinesHistoryAll(string company)
        {
            List<Machines_History_All> machines_History_Alls = new List<Machines_History_All>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines_History_All] WHERE Company = @Company;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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

        public List<Machines_History> GetMachinesHistory(string No, string company)
        {
            List<Machines_History> machines_Historys = new List<Machines_History>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines_History] WHERE No = @No AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@No", SqlDbType.VarChar, 40).Value = No;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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

        public List<string> Get_Types(string company)
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines_Type] WHERE Company = @Company;", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

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

        public List<Machines> GetLastGlobalIdMachine(string company)
        {
            List<Machines> temp = new List<Machines>();
            SqlCommand command = new SqlCommand("Select TOP(1) Global_id From [Machines] WHERE Company = @Company ORDER BY convert(int, Global_id) DESC ", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Machines machines = new Machines();
                machines.Global_Id = Convert.ToInt32(sqlDataReader["Global_id"]) + 1;

                temp.Add(machines);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<string> GetLastGlobalIdTypes(string company)
        {
            List<string> temp = new List<string>();
            SqlCommand command = new SqlCommand("Select TOP(1) Global_id From [Machines_Type] WHERE Company = @Company ORDER BY convert(int, Global_id) DESC", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string Global_id = sqlDataReader["Global_id"].ToString() + 1;

                temp.Add(Global_id);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<Machines_History> GetLastGlobalIdMachineHistory(string company)
        {
            List<Machines_History> temp = new List<Machines_History>();
            SqlCommand command = new SqlCommand("Select TOP(1) Global_id From [Machines_History] WHERE Company = @Company ORDER BY convert(int, Global_id) DESC", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Machines_History machines_History = new Machines_History();
                machines_History.Global_Id = Convert.ToInt32(sqlDataReader["Global_id"]) + 1;

                temp.Add(machines_History);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }

        public List<Machines_History_All> GetLastGlobalIdMachineHistoryAll(string company)
        {
            List<Machines_History_All> temp = new List<Machines_History_All>();
            SqlCommand command = new SqlCommand("Select TOP(1) Global_id From [Machines_History_All] WHERE Company = @Company ORDER BY convert(int, Global_id) DESC", connect.cnn);
            connect.cnn.Open();

            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = company;

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Machines_History_All machines_History_All = new Machines_History_All();
                machines_History_All.Global_Id = Convert.ToInt32(sqlDataReader["Global_id"]) + 1;

                temp.Add(machines_History_All);
            }
            sqlDataReader.Close();
            command.Dispose();
            connect.cnn.Close();

            return temp;
        }
    }
}
