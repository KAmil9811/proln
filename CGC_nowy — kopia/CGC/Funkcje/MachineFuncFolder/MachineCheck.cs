using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.MachineFuncFolder
{
    public class MachineCheck
    {
        private static MachineCheck m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static MachineCheck Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new MachineCheck();
                    }
                    return m_oInstance;
                }
            }
        }
    }
}
