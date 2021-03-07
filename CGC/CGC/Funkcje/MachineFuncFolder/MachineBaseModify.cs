using CGC.Funkcje.History;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.MachineFuncFolder.MachineBase
{
    public class MachineBaseModify
    {
        private Connect connect = new Connect();
        private InsertHistory insertHistory = new InsertHistory();
        private MachineBaseReturn machineBaseReturn = new MachineBaseReturn();

        private static MachineBaseModify m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static MachineBaseModify Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new MachineBaseModify();
                    }
                    return m_oInstance;
                }
            }
        }

        public List<Machines> Add_Machine(User user, Machines machines, string LastGlobalIdMachine)
        {
            List<Machines> temp = new List<Machines>();

            string query = "INSERT INTO dbo.Machines(Global_id, No, Status, Type, Stan, Company) VALUES(@Global_id, @No, @Status, @Type, @Stan, @Company)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalIdMachine;
            command.Parameters.Add("@No", SqlDbType.VarChar, 40).Value = machines.No;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";
            command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = machines.Type;
            command.Parameters.Add("@Stan", SqlDbType.Bit).Value = false;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();

            command.ExecuteNonQuery();

            command.Dispose();

            connect.cnn.Close();

            string userhistory = "You added machine " + machines.No;
            string machinehistoryall = "Machine has been added";

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Machine_History_All(machines.No, user.Login, machinehistoryall, user.Company);

            temp.Add(machines);
            return temp;
        }

        public List<Machines> Change_Status_Machine(User user, Machines machines)
        {
            List<Machines> temp = new List<Machines>();

            string query = "UPDATE  Machines SET Status = @Status  WHERE No = @No AND Company = @Company;";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@No", SqlDbType.VarChar, 40).Value = machines.No;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = machines.Status;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();

            command.ExecuteNonQuery();

            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You changed machine status from " + machines.No + " to " + machines.Status;
            string machinehistoryall = "Machine status has been changed to " + machines.Status;

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Machine_History_All(machines.No, user.Login, machinehistoryall, user.Company);

            temp.Add(machines);
            return temp;
        }

        public List<Machines> Change_Type_Machine(User user, Machines machines)
        {
            List<Machines> temp = new List<Machines>();

            string query = "UPDATE Machines SET Typ = @Typ  WHERE No = @No AND Company = @Company;";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@No", SqlDbType.VarChar, 40).Value = machines.No;
            command.Parameters.Add("@Typ", SqlDbType.VarChar, 40).Value = machines.Type;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();

            command.ExecuteNonQuery();

            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You changed machine type from " + machines.No + " to " + machines.Type;
            string machinehistoryall = "Machine type has been changed to " + machines.Type;

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Machine_History_All(machines.No, user.Login, machinehistoryall, user.Company);

            temp.Add(machines);
            return temp;
        }

        public List<Machines> Remove_Machine(User user, Machines machines)
        {
            List<Machines> temp = new List<Machines>();

            string query = "UPDATE dbo.[Machines] SET Stan = @Stan WHERE No = @No AND Company = @Company;";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@No", SqlDbType.VarChar, 40).Value = machines.No;
            command.Parameters.Add("@Stan", SqlDbType.Bit).Value = true;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();

            command.ExecuteNonQuery();

            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You deleted machine " + machines.No;
            string machinehistoryall = "Machine has been deleted";

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Machine_History_All(machines.No, user.Login, machinehistoryall, user.Company);

            temp.Add(machines);
            return temp;
        }

        public List<Machines> Restore_Machine(User user, Machines machines)
        {
            List<Machines> temp = new List<Machines>();

            string query = "UPDATE dbo.[Machines] SET Stan = @Stan WHERE No = @No AND Company = @Company;";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@No", SqlDbType.VarChar, 40).Value = machines.No;
            command.Parameters.Add("@Stan", SqlDbType.Bit).Value = false;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();

            command.ExecuteNonQuery();

            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You restored machine " + machines.No;
            string machinehistoryall = "Machine has been restored";

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Machine_History_All(machines.No, user.Login, machinehistoryall, user.Company);

            temp.Add(machines);
            return temp;
        }

        public List<string> Add_Type_Admin(User user, string type, string LastGlobalIdType)
        {
            List<string> temp = new List<string>();

            string query = "INSERT INTO dbo.Machines_Type(Global_id, Type, Company) VALUES(@Global_id, @Type, @Company)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Global_id", SqlDbType.VarChar, 40).Value = LastGlobalIdType;
            command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = type;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();

            command.ExecuteNonQuery();

            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You added new machine type: " + type;
            string machinehistoryall = type + " has been added";

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Machine_History_All(user.Login, machinehistoryall, user.Company);

            temp.Add(type);
            return temp;
        }

        public List<string> Change_Type_Admin(User user, string old_type, string new_type)
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("UPDATE dbo.Machines_Type SET Type = @new_type WHERE Type = @old_type AND Company = @Company;", connect.cnn);

            command.Parameters.Add("@new_type", SqlDbType.VarChar, 40).Value = new_type;
            command.Parameters.Add("@old_type", SqlDbType.VarChar, 40).Value = old_type;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            foreach (Machines machines in machineBaseReturn.GetMachines(user.Company))
            {
                if (machines.Type == old_type)
                {
                    command = new SqlCommand("UPDATE dbo.Machines SET Type = @new_type WHERE Type = @old_type AND Company = @Company;", connect.cnn);

                    command.Parameters.Add("@new_type", SqlDbType.VarChar, 40).Value = new_type;
                    command.Parameters.Add("@old_type", SqlDbType.VarChar, 40).Value = old_type;
                    command.Parameters.Add("@Company", SqlDbType.VarChar, 40).Value = user.Company;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();
                }
            }

            string userhistory = "You changed machine type from " + old_type + " to " + new_type;
            string machinehistoryall = old_type + " has been changed to " + new_type;

            insertHistory.Insert_User_History(userhistory, user.Login, user.Company);
            insertHistory.Insert_Machine_History_All(user.Login, machinehistoryall, user.Company);

            temp.Add(new_type);
            return temp;
        }
    }
}
