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

        public List<Machines> Add_Machine(User user, Machines machines)
        {
            List<Machines> temp = new List<Machines>();

            string query = "INSERT INTO dbo.Machines(No, Status, Type, Stan) VALUES(@No, @Status, @Type, @Stan)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@No", SqlDbType.Int).Value = machines.No;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";
            command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = machines.Type;
            command.Parameters.Add("@Stan", SqlDbType.Bit).Value = false;

            connect.cnn.Open();

            command.ExecuteNonQuery();

            command.Dispose();

            connect.cnn.Close();

            string userhistory = "Dodałes maszynę " + machines.No;
            string machinehistoryall = "Maszyna została stworzona";

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Machine_History_All(machines.No, user.Login, machinehistoryall);

            temp.Add(machines);
            return temp;
        }

        public List<Machines> Change_Status_Machine(User user, Machines machines)
        {
            List<Machines> temp = new List<Machines>();

            string query = "UPDATE  Machines SET Status = @Status  WHERE No = @No;";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@No", SqlDbType.Int).Value = machines.No;
            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = machines.Status;

            connect.cnn.Open();

            command.ExecuteNonQuery();

            command.Dispose();
            connect.cnn.Close();

            string userhistory = "Zmieniłeś status maszyny " + machines.No + " na " + machines.Status;
            string machinehistoryall = "Status maszyny został zmieniony na " + machines.Status;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Machine_History_All(machines.No, user.Login, machinehistoryall);

            temp.Add(machines);
            return temp;
        }

        public List<Machines> Change_Type_Machine(User user, Machines machines)
        {
            List<Machines> temp = new List<Machines>();

            string query = "UPDATE  Machines SET Typ = @Typ  WHERE No = @No;";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@No", SqlDbType.Int).Value = machines.No;
            command.Parameters.Add("@Typ", SqlDbType.VarChar, 40).Value = machines.Type;

            connect.cnn.Open();

            command.ExecuteNonQuery();

            command.Dispose();
            connect.cnn.Close();

            string userhistory = "Zmieniłeś typ maszyny " + machines.No + " na " + machines.Type;
            string machinehistoryall = "Status maszyny został zmieniony na " + machines.Type;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Machine_History_All(machines.No, user.Login, machinehistoryall);

            temp.Add(machines);
            return temp;
        }

        public List<Machines> Remove_Machine(User user, Machines machines)
        {
            List<Machines> temp = new List<Machines>();

            string query = "UPDATE dbo.[Machines] SET Stan = @Stan WHERE No = @No;";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@No", SqlDbType.Int).Value = machines.No;
            command.Parameters.Add("@Stan", SqlDbType.Bit).Value = true;

            connect.cnn.Open();

            command.ExecuteNonQuery();

            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You deleted machine " + machines.No;
            string machinehistoryall = "Machine has been deleted";

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Machine_History_All(machines.No, user.Login, machinehistoryall);

            temp.Add(machines);
            return temp;
        }

        public List<Machines> Restore_Machine(User user, Machines machines)
        {
            List<Machines> temp = new List<Machines>();

            string query = "UPDATE dbo.[Machines] SET Stan = @Stan WHERE No = @No;";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@No", SqlDbType.Int).Value = machines.No;
            command.Parameters.Add("@Stan", SqlDbType.Bit).Value = false;

            connect.cnn.Open();

            command.ExecuteNonQuery();

            command.Dispose();
            connect.cnn.Close();

            string userhistory = "You deleted machine " + machines.No;
            string machinehistoryall = "Machine has been deleted";

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Machine_History_All(machines.No, user.Login, machinehistoryall);

            temp.Add(machines);
            return temp;
        }

        public List<string> Add_Type_Admin(User user, string type)
        {
            List<string> temp = new List<string>();

            string query = "INSERT INTO dbo.Machines_Type(Type) VALUES(@Type)";
            SqlCommand command = new SqlCommand(query, connect.cnn);

            command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = type;

            connect.cnn.Open();

            command.ExecuteNonQuery();

            command.Dispose();
            connect.cnn.Close();

            string userhistory = "Dodałęś nowy typ maszyny: " + type;
            string machinehistoryall = type + " został stworzony";

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Machine_History_All(user.Login, machinehistoryall);

            temp.Add(type);
            return temp;
        }

        public List<string> Change_Type_Admin(User user, string old_type, string new_type)
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("UPDATE dbo.Machines_Type SET Type = @new_type WHERE Type = @old_type;", connect.cnn);

            command.Parameters.Add("@new_type", SqlDbType.VarChar, 40).Value = new_type;
            command.Parameters.Add("@old_type", SqlDbType.VarChar, 40).Value = old_type;

            connect.cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connect.cnn.Close();

            foreach (Machines machines in  machineBaseReturn.GetMachines())
            {
                if (machines.Type == old_type)
                {
                    command = new SqlCommand("UPDATE dbo.Machines SET Type = @new_type WHERE Type = @old_type;", connect.cnn);

                    command.Parameters.Add("@new_type", SqlDbType.VarChar, 40).Value = new_type;
                    command.Parameters.Add("@old_type", SqlDbType.VarChar, 40).Value = old_type;

                    connect.cnn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connect.cnn.Close();
                }
            }

            string userhistory = "Zmieniłeś typ maszyn " + old_type + " na " + new_type;
            string machinehistoryall = old_type + " został zmieniony na " + new_type;

            insertHistory.Insert_User_History(userhistory, user.Login);
            insertHistory.Insert_Machine_History_All(user.Login, machinehistoryall);

            temp.Add(new_type);
            return temp;
        }
    }
}
