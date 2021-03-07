using CGC.Funkcje.MachineFuncFolder.MachineBase;
using CGC.Funkcje.UserFuncFolder.UserReturn;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.MachineFuncFolder
{
    public class MachineFunc
    {
        private static MachineFunc m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static MachineFunc Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new MachineFunc();
                    }
                    return m_oInstance;
                }
            }
        }

        private MachineBaseModify machineBaseModify = new MachineBaseModify();
        private MachineBaseReturn machineBaseReturn = new MachineBaseReturn();
        private UserBaseReturn userBaseReturn = new UserBaseReturn();

        public List<string> Return_All_Type(Receiver receiver)
        {
            return machineBaseReturn.Get_Types(receiver.user.Company);
        }
        public List<Machines> Return_All_Machines(Receiver receiver)
        {
            return machineBaseReturn.GetMachines(receiver.user.Company);
        }

        public List<Machines_History> Return_Machines_History(Receiver receiver)
        {
            return machineBaseReturn.GetMachinesHistory(receiver.machines.No, receiver.user.Company);
        }

        public List<Machines_History_All> Return_All_Machines_History(Receiver receiver)
        {
            return machineBaseReturn.GetMachinesHistoryAll(receiver.user.Company);
        }

        public List<Machines> Add_Machine(Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;
            int temper;

            try
            {
                temper = Convert.ToInt32(machineBaseReturn.GetMachines(user.Company).OrderBy(mach => mach.No).Last().No) + 1;
            }
            catch (Exception e)
            {
                temper = 1;
            }

            machines.No = temper.ToString();

            foreach (User usere in userBaseReturn.GetUsers(user.Company))
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    string LastGlobalIdMachine = machineBaseReturn.GetLastGlobalIdMachine(user.Company).Last().Global_Id.ToString();
                    return machineBaseModify.Add_Machine(usere, machines, LastGlobalIdMachine);
                }
            }
            machines.Error_Message = "User not found";
            temp.Add(machines);
            return temp;
        }
       
        public List<Machines> Change_Status_Machine(Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;

            if (machines.Status == "Ready")
            {
                machines.Status = "Broken";
            }
            else if (machines.Status == "Broken")
            {
                machines.Status = "Ready";
            }

            foreach (User usere in userBaseReturn.GetUsers(user.Company))
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    foreach (Machines edit_machines in machineBaseReturn.GetMachines(user.Company))
                    {
                        if (edit_machines.No == machines.No)
                        {
                            return machineBaseModify.Change_Status_Machine(usere, machines);
                        }
                    }
                    machines.Error_Message = "Machine not found";
                    temp.Add(machines);
                    return temp;
                }
            }
            machines.Error_Message = "User not found";
            temp.Add(machines);
            return temp;
        }
       
        public List<Machines> Change_Type_Machine(Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;

            foreach (User usere in userBaseReturn.GetUsers(user.Company))
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    return machineBaseModify.Change_Type_Machine(usere, machines);
                }
            }
            machines.Error_Message = "User not found";
            temp.Add(machines);
            return temp;
        }
        public List<Machines> Remove_Machine(Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;

            foreach (User usere in userBaseReturn.GetUsers(user.Company))
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    return machineBaseModify.Remove_Machine(usere, machines);
                }
            }
            
            machines.Error_Message = "User not found";
            temp.Add(machines);
            return temp;
        }
        public List<Machines> Restore_Machine(Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;

            foreach (User usere in userBaseReturn.GetUsers(user.Company))
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    return machineBaseModify.Restore_Machine(usere, machines);
                }
            }
            machines.Error_Message = "User not found";
            temp.Add(machines);
            return temp;
        }

        public List<string> Add_Type_Admin(Receiver receiver)
        {
            User user = receiver.user;
            string type = receiver.type;
            List<string> temp = new List<string>();

            foreach (User usere in userBaseReturn.GetUsers(user.Company))
            {
                if (user.Login == usere.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    foreach (string types in machineBaseReturn.Get_Types(user.Company))
                    {
                        if (types == type)
                        {
                            temp.Add("Type already exist");
                            return temp;
                        }
                    }
                    string LastGlobalIdType = machineBaseReturn.GetLastGlobalIdTypes(user.Company).Last();
                    return machineBaseModify.Add_Type_Admin(usere, type, LastGlobalIdType);
                }
            }
            temp.Add("User not found, or password is wrong");
            return temp;
        }
        public List<string> Change_Type_Admin(Receiver receiver)
        {
            User user = receiver.user;
            string new_type = receiver.new_type;
            string old_type = receiver.old_type;
            List<string> temp = new List<string>();

            foreach (string type in machineBaseReturn.Get_Types(user.Company))
            {
                if (type == new_type)
                {
                    temp.Add("Type already exist");
                    return temp;
                }
            }

            foreach (User usere in userBaseReturn.GetUsers(user.Company))
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    foreach (string type in machineBaseReturn.Get_Types(user.Company))
                    {
                        if (type == old_type)
                        {
                            return machineBaseModify.Change_Type_Admin(user, old_type, new_type);
                        }
                    }
                    temp.Add("Type already exist");
                    return temp;
                }
            }
            temp.Add("User not found, or password is wrong");
            return temp;
        }
    }
}
