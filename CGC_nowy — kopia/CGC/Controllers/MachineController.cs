using CGC.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    public class MachineController : Controller
    {
        static MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        {
            Server = "projekt-inz.database.windows.net",
            Database = "projekt-inz",
            UserID = "Michal",
            Password = "lemES98naw141",
            //SslMode = MySqlSslMode.Required,
        };


        SqlConnection cnn = new SqlConnection(builder.ConnectionString);

        private static MachineController m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static MachineController Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new MachineController();
                    }
                    return m_oInstance;
                }
            }
        }

        //do usuniecia po dodaniu bazydanych
        public UsersController usersController = new UsersController();

        public List<Machines> GetMachines()
        {
            List<Machines> temp = new List<Machines>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines];", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Machines machines = new Machines();
                machines.No = Convert.ToInt32(sqlDataReader["No"]);
                machines.Status = sqlDataReader["Status"].ToString();
                machines.Type = sqlDataReader["Type"].ToString();
                machines.Stan = Convert.ToBoolean(sqlDataReader["Stan"]);

                temp.Add(machines);
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();
            return temp;
        }

        public void Insert_Machine_History_All(int No, string Login, string Description)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.Machines_History_All(Date, Login, Description) VALUES(@data, @No, @Login, @Description)";
            SqlCommand command = new SqlCommand(query, cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@No", SqlDbType.Int).Value = No;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();
        }

        public void Insert_Machine_History_All(string Login, string Description)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.Machines_History_All(Date, Login, Description) VALUES(@data, @Login, @Description)";
            SqlCommand command = new SqlCommand(query, cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();
        }

        public void Insert_Machine_History(int Cut_id, string Login, string Description, int No)
        {
            string data = DateTime.Today.ToString("d");
            string query = "INSERT INTO dbo.Machines_History_All(Date, Cut_Id, Login, Description, No) VALUES(@data, @Cut_Id, @Login, @Description, @No)";
            SqlCommand command = new SqlCommand(query, cnn);

            command.Parameters.Add("@data", SqlDbType.VarChar, 40).Value = data;
            command.Parameters.Add("@Cut_Id", SqlDbType.Int).Value = Cut_id;
            command.Parameters.Add("@Login", SqlDbType.VarChar, 40).Value = Login;
            command.Parameters.Add("@Description", SqlDbType.VarChar, 40).Value = Description;
            command.Parameters.Add("@No", SqlDbType.Int).Value = No;

            cnn.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();
        }

        public List<string> Get_Types()
        {
            List<string> temp = new List<string>();

            SqlCommand command = new SqlCommand("SELECT * FROM [Machines_Type];", cnn);
            cnn.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string type = sqlDataReader["Type"].ToString();
                temp.Add(type);
            }
            sqlDataReader.Close();
            command.Dispose();
            cnn.Close();
            return temp;
        }

        [HttpGet("Return_All_Type")]
        public async Task<List<string>> Return_All_Type()
        {
            return Get_Types();
        }

        [HttpGet("Return_All_Machines")]
        public async Task<List<Machines>> Return_All_Machines()
        {
            return GetMachines();
        }

        [HttpPost("Add_Machine")]
        public async Task<List<Machines>> Add_Machine([FromBody] Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;
            int temper;

            if (GetMachines().Last() != null)
            {
                temper = GetMachines().Last().No + 1;
            }
            else
            {
                temper = 1;
            }

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {

                    string query = "INSERT INTO dbo.Machines(No, Status, Type, Stan) VALUES(@No, @Status, @Type, @Stan)";
                    SqlCommand command = new SqlCommand(query, cnn);

                    command.Parameters.Add("@No", SqlDbType.Int).Value = temper;
                    command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = "Ready";
                    command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = machines.Type;
                    command.Parameters.Add("@Stan", SqlDbType.Bit).Value = false;

                    cnn.Open();

                    command.ExecuteNonQuery();

                    command.Dispose();

                    cnn.Close();

                    string userhistory = "You added new machine " + machines.No;
                    string machinehistoryall = "Machine has been added";

                    usersController.Insert_User_History(userhistory, user.Login);
                    Insert_Machine_History_All(machines.No, user.Login, machinehistoryall);

                    temp.Add(machines);
                    return temp;
                }
            }
            machines.Error_Message = "User_no_exist";
            temp.Add(machines);
            return temp;
        }

        [HttpPost("Change_Status_Machine")]
        public async Task<List<Machines>> Change_Status_Machine([FromBody] Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;

            if(machines.Status == "Ready")
            {
                machines.Status = "Broken";
            }
            else if(machines.Status == "Broken")
            {
                machines.Status = "Ready";
            }

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (Machines edit_machines in GetMachines())
                    {
                        if (edit_machines.No == machines.No)
                        {
                            string query = "UPDATE  Machines SET Status = @Status  WHERE No = @No;";
                            SqlCommand command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@No", SqlDbType.Int).Value = machines.No;
                            command.Parameters.Add("@Status", SqlDbType.VarChar, 40).Value = machines.Status;

                            cnn.Open();

                            command.ExecuteNonQuery();

                            command.Dispose();
                            cnn.Close();

                            string userhistory = "You changed status " + machines.No + " to " + machines.Status;
                            string machinehistoryall = "machine status changed to " + machines.Status;

                            usersController.Insert_User_History(userhistory, user.Login);
                            Insert_Machine_History_All(machines.No, user.Login, machinehistoryall);

                            temp.Add(machines);
                            return temp;
                        }
                    }
                    machines.Error_Message = "Id_no_exist";
                    temp.Add(machines);
                    return temp;
                }
            }
            machines.Error_Message = "User_no_exist";
            temp.Add(machines);
            return temp;
        }

        [HttpPost("Change_Type_Machine")]
        public async Task<List<Machines>> Change_Type_Machine([FromBody] Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (Machines edit_machines in GetMachines())
                    {
                        if (edit_machines.No == machines.No)
                        {
                            string query = "UPDATE  Machines SET Typ = @Typ  WHERE No = @No;";
                            SqlCommand command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@No", SqlDbType.Int).Value = machines.No;
                            command.Parameters.Add("@Typ", SqlDbType.VarChar, 40).Value = machines.Type;

                            cnn.Open();

                            command.ExecuteNonQuery();

                            command.Dispose();
                            cnn.Close();

                            string userhistory = "You changed type " + machines.No + " to " + machines.Type;
                            string machinehistoryall = "machine type changed to " + machines.Type;

                            usersController.Insert_User_History(userhistory, user.Login);
                            Insert_Machine_History_All(machines.No, user.Login, machinehistoryall);

                            temp.Add(machines);
                            return temp;
                        }
                    }
                    machines.Error_Message = "Id_no_exist";
                    temp.Add(machines);
                    return temp;
                }
            }
            machines.Error_Message = "User_no_exist";
            temp.Add(machines);
            return temp;
        }

        [HttpPost("Remove_Machine")]
        public async Task<List<Machines>> Remove_Machine([FromBody] Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (Machines edit_machines in GetMachines())
                    {
                        if (edit_machines.No == machines.No)
                        {
                            string query = "UPDATE dbo.[Machines] SET Stan = @Stan WHERE No = @No;";
                            SqlCommand command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@No", SqlDbType.Int).Value = machines.No;
                            command.Parameters.Add("@Stan", SqlDbType.Bit).Value = true;

                            cnn.Open();

                            command.ExecuteNonQuery();

                            command.Dispose();
                            cnn.Close();

                            string userhistory = "You deleted machine " + edit_machines.No;
                            string machinehistoryall = "Machine has been deleted";

                            usersController.Insert_User_History(userhistory, user.Login);
                            Insert_Machine_History_All(machines.No, user.Login, machinehistoryall);

                            temp.Add(machines);
                            return temp;
                        }
                    }
                    machines.Error_Message = "Id_no_exist";
                    temp.Add(machines);
                    return temp;
                }
            }
            machines.Error_Message = "User_no_exist";
            temp.Add(machines);
            return temp;
        }

        [HttpPost("Restore_Machine")]
        public async Task<List<Machines>> Restore_Machine([FromBody] Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (Machines edit_machines in GetMachines())
                    {
                        if (edit_machines.No == machines.No)
                        {
                            string query = "UPDATE dbo.[Machines] SET Stan = @Stan WHERE No = @No;";
                            SqlCommand command = new SqlCommand(query, cnn);

                            command.Parameters.Add("@No", SqlDbType.Int).Value = machines.No;
                            command.Parameters.Add("@Stan", SqlDbType.Bit).Value = false;

                            cnn.Open();

                            command.ExecuteNonQuery();

                            command.Dispose();
                            cnn.Close();

                            string userhistory = "You resoted machine " + edit_machines.No;
                            string machinehistoryall = "Machine has been restored";

                            usersController.Insert_User_History(userhistory, user.Login);
                            Insert_Machine_History_All(machines.No, user.Login, machinehistoryall);

                            temp.Add(machines);
                            return temp;
                        }
                    }
                    machines.Error_Message = "Id_no_exist";
                    temp.Add(machines);
                    return temp;
                }
            }
            machines.Error_Message = "User_no_exist";
            temp.Add(machines);
            return temp;
        }

        [HttpPost("Add_Type_Admin")]
        public async Task<List<string>> Add_Type_Admin([FromBody] Receiver receiver)
        {
            User user = receiver.user;
            string type = receiver.type;
            List<string> temp = new List<string>();

            foreach (User usere in usersController.GetUsers())
            {
                if (user.Login == usere.Login)
                {
                    foreach (string types in Get_Types())
                    {
                        if (types == type)
                        {
                            temp.Add("Type_alredy_exist");
                            return temp;
                        }
                    }

                    string query = "INSERT INTO dbo.Machines_Type(Type) VALUES(@Type)";
                    SqlCommand command = new SqlCommand(query, cnn);

                    command.Parameters.Add("@Type", SqlDbType.VarChar, 40).Value = type;

                    cnn.Open();

                    command.ExecuteNonQuery();

                    command.Dispose();
                    cnn.Close();

                    string userhistory = "You added new machine type " + type;
                    string machinehistoryall = type + " has been added";

                    usersController.Insert_User_History(userhistory, user.Login);
                    Insert_Machine_History_All(user.Login, machinehistoryall);

                    temp.Add(type);
                    return temp;
                }
            }
            temp.Add("Admin_no_exist");
            return temp;
        }

        [HttpPost("Change_Type_Admin")]
        public async Task<List<string>> Change_Type_Admin([FromBody] Receiver receiver)
        {
            User user = receiver.user;
            string new_type = receiver.new_type;
            string old_type = receiver.old_type;
            List<string> temp = new List<string>();

            foreach (string type in Get_Types())
            {
                if (type == new_type)
                {
                    temp.Add("New_type_already_exist");
                    return temp;
                }
            }

            foreach (User usere in usersController.GetUsers())
            {
                if (usere.Login == user.Login)
                {
                    foreach (string type in Get_Types())
                    {
                        if (type == old_type)
                        {
                            SqlCommand command = new SqlCommand("UPDATE dbo.Machines_Type SET Type = @new_type WHERE Type = @old_type;", cnn);

                            command.Parameters.Add("@new_type", SqlDbType.VarChar, 40).Value = new_type;
                            command.Parameters.Add("@old_type", SqlDbType.VarChar, 40).Value = old_type;

                            cnn.Open();
                            command.ExecuteNonQuery();
                            command.Dispose();
                            cnn.Close();

                            foreach (Machines machines in GetMachines())
                            {
                                if (machines.Type == old_type)
                                {
                                    command = new SqlCommand("UPDATE dbo.Machines SET Type = @new_type WHERE Type = @old_type;", cnn);

                                    command.Parameters.Add("@new_type", SqlDbType.VarChar, 40).Value = new_type;
                                    command.Parameters.Add("@old_type", SqlDbType.VarChar, 40).Value = old_type;

                                    cnn.Open();
                                    command.ExecuteNonQuery();
                                    command.Dispose();
                                    cnn.Close();
                                }
                            }

                            string userhistory = "You changed type " + old_type + " to " + new_type;
                            string machinehistoryall = old_type + " has been changed to " + new_type;

                            usersController.Insert_User_History(userhistory, user.Login);
                            Insert_Machine_History_All(user.Login, machinehistoryall);


                            temp.Add(new_type);
                            return temp;
                        }
                    }
                    temp.Add("Type_dont_exist");
                    return temp;
                }
            }
            temp.Add("Admin_no_exist");
            return temp;
        }

    }
}