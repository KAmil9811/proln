using CGC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.MagazineFuncFolder
{
    public class MagazineCheck
    {
        private static MagazineCheck m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static MagazineCheck Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new MagazineCheck();
                    }
                    return m_oInstance;
                }
            }
        }

        public List<Glass> Set_Filter(List<Glass> temporary)
        {
            List<Glass> temp = new List<Glass>();

            foreach (Glass glass in temporary)
            {
                List<Glass_Id> temp2 = new List<Glass_Id>();
                foreach (Glass_Id glass_Id in glass.Glass_info)
                {
                    if (glass_Id.Destroyed == false && glass_Id.Used == false && glass_Id.Removed == false && glass_Id.Cut_id == 0)
                    {
                        temp2.Add(glass_Id);
                    }
                }
                if (temp2.Count != 0)
                {
                    glass.Glass_info = temp2;

                    temp.Add(glass);
                }
            }

            foreach (Glass tmp in temp)
            {
                foreach (Glass_Id glass_Id in tmp.Glass_info)
                {
                    tmp.Glass_id.Add(glass_Id.Id);
                }
            }

            return temp;
        }
    }
}
