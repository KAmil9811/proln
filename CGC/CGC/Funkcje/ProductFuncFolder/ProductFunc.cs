using CGC.Funkcje.ProductFuncFolder.ProductBase;
using CGC.Funkcje.UserFuncFolder.UserReturn;
using CGC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Funkcje.ProductFuncFolder
{
    public class ProductFunc
    {
        private static ProductFunc m_oInstance = null;
        private static readonly object m_oPadLock = new object();
        private UserBaseReturn userBaseReturn = new UserBaseReturn();
        private ProductBaseModify productBaseModify = new ProductBaseModify();
        private ProductBaseReturn productBaseReturn = new ProductBaseReturn();

        public static ProductFunc Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new ProductFunc();
                    }
                    return m_oInstance;
                }
            }
        }

        public List<Product> Get_Products()
        {
            return productBaseReturn.GetProducts("Ready");
        }

        public List<Product_History> Get_Product_History(Receiver receiver)
        {
            return productBaseReturn.GetProductHistory(Convert.ToInt32(receiver.id));
        }

        public List<Product> Released_Product(User user, List<int> product_id)
        {
            List<Product> temp = new List<Product>();
            List<Product> products_to_change = new List<Product>();
            Product product = new Product();

            foreach (int pro2 in product_id)
            {
                foreach (Product pro in productBaseReturn.GetProduct(pro2.ToString()))
                {
                    products_to_change.Add(pro);
                }
            }

            foreach (Product pro in products_to_change)
            {
                if (pro.Status != "Ready")
                {
                    product.Error_Messege = "Products are not ready";
                    temp.Add(product);
                    return temp;
                }
            }

            foreach (User use in userBaseReturn.GetUser(user.Login, false))
            {
                if (use.Manager == true || use.Super_Admin == true || use.Admin || use.Magazine_management == true)
                {
                    return productBaseModify.Released_Product(use, products_to_change);
                }
            }

            product.Error_Messege = "User not found";
            temp.Add(product);
            return temp;
        }

        public List<Product> Delete_Product(Receiver receiver)
        {
            List<Product> temp = new List<Product>();
            List<int> product_id = receiver.product_Id;
            List<Product> products_to_delete = new List<Product>();
            User user = receiver.user;

            Product producted = new Product();

            foreach (int pro2 in product_id)
            {
                foreach (Product pro in productBaseReturn.GetProduct(pro2.ToString()))
                {
                    products_to_delete.Add(pro);
                }
            }

            foreach (Product pro in products_to_delete)
            {
                if (pro.Status != "Deleted")
                {
                    producted.Error_Messege = "Products are not ready";
                    temp.Add(producted);
                    return temp;
                }
            }

            foreach (User use in userBaseReturn.GetUser(user.Login))
            {
                if (use.Manager == true || use.Super_Admin == true || use.Admin || use.Magazine_management == true)
                {
                    return productBaseModify.Delete_Product(user, products_to_delete);
                }
            }
            producted.Error_Messege = "User not found";
            temp.Add(producted);
            return temp;
        }

    }
}
