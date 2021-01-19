using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.OrderFuncFolder
{
    public class OrderCheck
    {
        private static OrderCheck m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static OrderCheck Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new OrderCheck();
                    }
                    return m_oInstance;
                }
            }
        }
    }
}
