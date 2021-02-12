using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CGC.Funkcje.OrderFuncFolder;
using CGC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public sealed class OrderController : Controller
    {
        private static OrderController m_oInstance = null;
        private static readonly object m_oPadLock = new object();

        public static OrderController Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new OrderController();
                    }
                    return m_oInstance;
                }
            }
        }

        OrderFunc orderFunc = new OrderFunc();

        
        [HttpGet("Return_All_Orders")]
        public async Task<List<Order>> Return_All_Orders()
        {
            return orderFunc.Return_All_Orders();
        }

        [HttpPost("Return_All_Items")]
        public async Task<List<Item>> Return_All_Items([FromBody] Receiver receiver)
        {
            return orderFunc.Return_All_Items(receiver);
        }

        [HttpPost("Return_Order_History")]
        public async Task<List<Order_History>> Return_Order_History([FromBody] Receiver receiver)
        {
            return orderFunc.Return_Order_History(receiver);
        }

        [HttpPost("Add_Order")]
        public async Task<List<Order>> Add_Order([FromBody] Receiver receiver)
        {
            return orderFunc.Add_Order(receiver);
        }

        [HttpPost("Edit_Order")]
        public async Task<List<Order>> Edit_Order([FromBody] Receiver receiver)
        {
            return orderFunc.Edit_Order(receiver);
        }

        [HttpPost("Edit_Order_Items")]
        public async Task<List<Order>> Edit_Order_Items([FromBody] Receiver receiver)
        {
            return orderFunc.Edit_Order_Items(receiver);
        }

        [HttpPost("Set_Stan")]
        public async Task<List<Order>> Set_Stan([FromBody] Receiver receiver)
        {
            return orderFunc.Set_Stan(receiver);
        }

        [HttpPost("Released_Order")]
        public async Task<List<Order>> Released_Order([FromBody] Receiver receiver)
        {
            return orderFunc.ReleasedOrder(receiver.user, receiver.order);
        }

        [HttpPost("Released_Item")]
        public async Task<List<Item>> Released_Item([FromBody] Receiver receiver)
        {
            return orderFunc.ReleasedItems(receiver.user, receiver.items);
        }

        [HttpPost("Remove_Item")]
        public async Task<List<Item>> Remove_Item([FromBody] Receiver receiver)
        {
            return orderFunc.Remove_Item(receiver);
        }
    }
}