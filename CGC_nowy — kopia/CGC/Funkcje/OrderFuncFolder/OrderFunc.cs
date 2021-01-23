using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.OrderFuncFolder
{
    public class OrderFunc
    {
        private static OrderFunc m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static OrderFunc Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new OrderFunc();
                    }
                    return m_oInstance;
                }
            }
        }
    }
}
