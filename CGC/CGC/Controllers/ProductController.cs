using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CGC.Funkcje.ProductFuncFolder;
using CGC.Models;
using CGC.Helpers;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    public sealed class ProductController : Controller
    {
        private static ProductController m_oInstance = null;
        private static readonly object m_oPadLock = new object();
        private ProductFunc productFunc = new ProductFunc();

        public static ProductController Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new ProductController();
                    }
                    return m_oInstance;
                }
            }
        }

               
        [HttpPost("Get_Products")]
        public async Task<List<Product>> Get_Products([FromBody] Receiver receiver)
        {
            return productFunc.Get_Products(receiver);
        }
        
        [HttpPost("Get_Product_History")]
        public async Task<List<Product_History>> Get_Product_History([FromBody] Receiver receiver)
        {
            return productFunc.Get_Product_History(receiver);
        }

        [Authorize]
        [HttpPost("Released_Product")]
        public async Task<List<Product>> Released_Product([FromBody] Receiver receiver)
        {
            return productFunc.Released_Product(receiver.user, receiver.product_Id);
        }

        [Authorize]
        [HttpPost("Delete_Product")]
        public async Task<List<Product>> Delete_Product([FromBody] Receiver receiver)
        {
            return productFunc.Delete_Product(receiver);
        }
        
    }
}