using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGC.Funkcje.CutFuncFolder;
using CGC.Funkcje.MachineFuncFolder;
using CGC.Funkcje.MagazineFuncFolder;
using CGC.Funkcje.OrderFuncFolder;
using CGC.Funkcje.ProductFuncFolder;
using CGC.Funkcje.UserFuncFolder;

namespace CGC.Funkcje
{
    public class AllFunc
    {
        private static AllFunc m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static AllFunc Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new AllFunc();
                    }
                    return m_oInstance;
                }
            }
        }

        public AllFunc()
        {
            UserFunc userFunc = this.userFunc;
            ProductFunc productFunc = this.productFunc;
            OrderFunc orderFunc = this.orderFunc;
            MagazineFunc magazineFunc = this.magazineFunc;
            MachineFunc machineFunc = this.machineFunc;
            CutFunc cutFunc = this.cutFunc;
        }

        UserFunc userFunc { get; }
        ProductFunc productFunc { get; }
        OrderFunc orderFunc  { get; }
        MagazineFunc magazineFunc { get; }
        MachineFunc machineFunc { get; }
        CutFunc cutFunc { get; }

    }
}
