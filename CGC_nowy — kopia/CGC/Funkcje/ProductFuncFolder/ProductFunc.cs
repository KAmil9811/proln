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
            return productBaseReturn.GetProductsUser();
        }

        public List<Product_History> Get_Product_History(Receiver receiver)
        {
            return productBaseReturn.GetProductHistory(receiver.id);
        }

        public List<Product> Released_Product(User user, List<int> product_id)
        {
            List<Product> temp = new List<Product>();
            List<Product> products_to_change = new List<Product>();
            Product product = new Product();

            foreach (Product pro in productBaseReturn.GetProducts())
            {
                foreach (int pro2 in product_id)
                {
                    if (pro2 == pro.Id)
                    {
                        products_to_change.Add(pro);
                    }
                }
            }

            foreach (Product pro in products_to_change)
            {
                if (pro.Status != "Gotowy")
                {
                    product.Error_Messege = "Produkty nie są gotowe";
                    temp.Add(product);
                    return temp;
                }
            }

            foreach (User use in userBaseReturn.GetUsers())
            {
                if (use.Login == user.Login)
                {
                    return productBaseModify.Released_Product(user, products_to_change);
                }
            }

            product.Error_Messege = "Nie znaleziono użytkownika";
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

            foreach (Product pro in productBaseReturn.GetProducts())
            {
                foreach (int pro2 in product_id)
                {
                    if (pro2 == pro.Id)
                    {
                        products_to_delete.Add(pro);
                    }
                }
            }

            foreach (Product pro in products_to_delete)
            {
                if (pro.Status != "Usunięty")
                {
                    producted.Error_Messege = "Produkty nie są usunięte";
                    temp.Add(producted);
                    return temp;
                }
            }

            foreach (User use in userBaseReturn.GetUsers())
            {
                if (use.Login == user.Login)
                {
                    foreach (Product product in productBaseReturn.GetProducts())
                    {
                        return productBaseModify.Delete_Product(user, products_to_delete);
                    }
                    return temp;
                }
            }
            producted.Error_Messege = "User not found";
            temp.Add(producted);
            return temp;
        }

    }
}
