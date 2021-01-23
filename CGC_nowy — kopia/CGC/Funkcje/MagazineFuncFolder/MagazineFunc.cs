using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.MagazineFuncFolder
{
    public class MagazineFunc
    {
        private static MagazineFunc m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static MagazineFunc Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new MagazineFunc();
                    }
                    return m_oInstance;
                }
            }
        }
    }
}
