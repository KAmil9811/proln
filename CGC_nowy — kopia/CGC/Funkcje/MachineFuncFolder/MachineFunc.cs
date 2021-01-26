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

        public List<string> Return_All_Type()
        {
            return machineBaseReturn.Get_Types();
        }
        public List<Machines> Return_All_Machines()
        {
            return machineBaseReturn.GetMachines();
        }

        public List<Machines_History> Return_Machines_History(Receiver receiver)
        {
            return machineBaseReturn.GetMachinesHistory(receiver.machines.No);
        }

        public List<Machines_History_All> Return_All_Machines_History()
        {
            return machineBaseReturn.GetMachinesHistoryAll();
        }

        public List<Machines> Add_Machine(Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;
            int temper;

            try
            {
                temper = machineBaseReturn.GetMachines().OrderBy(mach => mach.No).Last().No + 1;
            }
            catch (Exception e)
            {
                temper = 1;
            }

            machines.No = temper;

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    return machineBaseModify.Add_Machine(usere, machines);
                }
            }
            machines.Error_Message = "Nie znaleziono uzytkownika";
            temp.Add(machines);
            return temp;
        }
       
        public List<Machines> Change_Status_Machine(Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;

            if (machines.Status == "Gotowa")
            {
                machines.Status = "Uszkodzona";
            }
            else if (machines.Status == "Uszkodzona")
            {
                machines.Status = "Gotowa";
            }

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    foreach (Machines edit_machines in machineBaseReturn.GetMachines())
                    {
                        if (edit_machines.No == machines.No)
                        {
                            return machineBaseModify.Change_Status_Machine(usere, machines);
                        }
                    }
                    machines.Error_Message = "Nie znaleziono maszyny";
                    temp.Add(machines);
                    return temp;
                }
            }
            machines.Error_Message = "Nie znaleziono uzytkownika";
            temp.Add(machines);
            return temp;
        }
       
        public List<Machines> Change_Type_Machine(Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    return machineBaseModify.Change_Type_Machine(usere, machines);
                }
            }
            machines.Error_Message = "Nie znaleziono uzytkownika";
            temp.Add(machines);
            return temp;
        }
        public List<Machines> Remove_Machine(Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    return machineBaseModify.Remove_Machine(usere, machines);
                }
            }
            
            machines.Error_Message = "Nie znaleziono uzytkownika";
            temp.Add(machines);
            return temp;
        }
        public List<Machines> Restore_Machine(Receiver receiver)
        {
            List<Machines> temp = new List<Machines>();

            Machines machines = receiver.machines;
            User user = receiver.user;

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    return machineBaseModify.Restore_Machine(usere, machines);
                }
            }
            machines.Error_Message = "Nie znaleziono uzytkownika";
            temp.Add(machines);
            return temp;
        }

        public List<string> Add_Type_Admin(Receiver receiver)
        {
            User user = receiver.user;
            string type = receiver.type;
            List<string> temp = new List<string>();

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (user.Login == usere.Login && usere.Password == user.Password && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    foreach (string types in machineBaseReturn.Get_Types())
                    {
                        if (types == type)
                        {
                            temp.Add("Typ juz istnieje");
                            return temp;
                        }
                    }

                    return machineBaseModify.Add_Type_Admin(usere, type);
                }
            }
            temp.Add("Nie znaleziono uzytkownika lub zle haslo");
            return temp;
        }
        public List<string> Change_Type_Admin(Receiver receiver)
        {
            User user = receiver.user;
            string new_type = receiver.new_type;
            string old_type = receiver.old_type;
            List<string> temp = new List<string>();

            foreach (string type in machineBaseReturn.Get_Types())
            {
                if (type == new_type)
                {
                    temp.Add("Typ juz istnieje");
                    return temp;
                }
            }

            foreach (User usere in userBaseReturn.GetUsers())
            {
                if (usere.Login == user.Login && usere.Password == user.Password && (usere.Manager == true || usere.Super_Admin == true || usere.Admin || usere.Machine_management == true))
                {
                    foreach (string type in machineBaseReturn.Get_Types())
                    {
                        if (type == old_type)
                        {
                            return machineBaseModify.Change_Type_Admin(user, old_type, new_type);
                        }
                    }
                    temp.Add("Typ nie istnieje");
                    return temp;
                }
            }
            temp.Add("Nie znaleziono uzytkownika lub zle haslo");
            return temp;
        }
    }
}
