using System.Collections.Generic;
using System.Threading.Tasks;
using CGC.Models;
using Microsoft.AspNetCore.Mvc;
using CGC.Funkcje.MagazineFuncFolder;
using CGC.Helpers;

namespace CGC.Controllers
{
    [Route("api/[controller]")]
    public sealed class MagazineController : Controller
    {
        private static MagazineController m_oInstance = null;
        private static readonly object m_oPadLock = new object();
        private MagazineFunc magazineFunc = new MagazineFunc();
        public static MagazineController Instace
        {
            get
            {
                lock (m_oPadLock)
                {
                    if (m_oInstance == null)
                    {
                        m_oInstance = new MagazineController();
                    }
                    return m_oInstance;
                }
            }
        }

        //OrderController orderController = new OrderController();

       

        //public void SetOrderStan()
        //{
        //    int count_x;
        //    int count_y;
        //    int count_z;

        //    foreach (Order order in orderController.GetOrders())
        //    {
        //        string stan = "";
        //        count_y = orderController.Item_To_Cut(order);
        //        count_x = orderController.Avaible_Cut(order);
        //        count_x = count_y - count_x;
        //        count_z = orderController.Item_To_In_Cut(order);

        //        stan = count_x + "/" + count_y + "/" + count_z;

        //        string query = "UPDATE dbo.[Order] SET Stan = @stan WHERE Id_Order = @Id_Order";
        //        SqlCommand command = new SqlCommand(query, cnn);
        //        command.Parameters.Add("@Id_Order", SqlDbType.VarChar, 40).Value = order.Id_Order;
        //        command.Parameters.Add("@Stan", SqlDbType.VarChar, 40).Value = stan;

        //    }
        //}

        
        [HttpPost("Return_All_Colors")]
        public async Task<List<string>> Return_All_Colors([FromBody] Receiver receiver)
        {
            return magazineFunc.Return_All_Colors(receiver);
        }

        [HttpPost("Return_All_Glass")]
        public async Task<List<Glass_Receiver>> Return_All_Glass([FromBody] Receiver receiver)
        {
            return magazineFunc.Return_All_Glass(receiver);
        }

        [HttpPost("Return_All_Type")]
        public async Task<List<string>> Return_All_Type([FromBody] Receiver receiver)
        {
            return magazineFunc.Return_All_Type(receiver);
        }

        [HttpPost("Return_Magazine_History")]
        public async Task<List<Magazine_History>> Return_Magazine_History([FromBody] Receiver receiver)
        {
            return magazineFunc.Return_Magazine_History(receiver);
        }

        [Authorize]
        [HttpPost("Add_Glass")]
        public async Task<List<Glass>> Add_Glass([FromBody] Receiver receiver)
        {
            return magazineFunc.Add_Glass(receiver);
        }

        [Authorize]
        [HttpPost("Edit_Glass")]
        public async Task<List<Glass>> Edit_Glass([FromBody] Receiver receiver)
        {
            return magazineFunc.Edit_Glass(receiver);
        }

        [Authorize]
        [HttpPost("Remove_Glass")]
        public async Task<List<Glass>> Remove_Glass([FromBody] Receiver receiver)
        {
            return magazineFunc.Remove_Glass(receiver);
        }

        [Authorize]
        [HttpPost("Restore_Glass")]
        public async Task<List<Glass>> Restore_Glass([FromBody] Receiver receiver)
        {
            return magazineFunc.Restore_Glass(receiver);
        }

        [Authorize]
        [HttpPost("Add_type_Admin")]
        public async Task<List<string>> Add_Type_Admin([FromBody] Receiver receiver)
        {
            return magazineFunc.Add_Type_Admin(receiver);
        }

        [Authorize]
        [HttpPost("Add_Color_Admin")]
        public async Task<List<string>> Add_Color_Admin([FromBody] Receiver receiver)
        {
            return magazineFunc.Add_Color_Admin(receiver);
        }

        [Authorize]
        [HttpPost("Change_type_Admin")]
        public async Task<List<string>> Change_type_Admin([FromBody] Receiver receiver)
        {
            return magazineFunc.Change_Type_Admin(receiver);
        }

        [Authorize]
        [HttpPost("Change_Color_Admin")]
        public async Task<List<string>> Change_Color_Admin([FromBody] Receiver receiver)
        {
            return magazineFunc.Change_Color_Admin(receiver);
        }
    }
}