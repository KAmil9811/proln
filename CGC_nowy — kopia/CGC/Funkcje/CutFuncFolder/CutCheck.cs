using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.CutFuncFolder
{
    public class CutCheck
    {
        private static CutCheck m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static CutCheck Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new CutCheck();
                    }
                    return m_oInstance;
                }
            }
        }
    }
}
