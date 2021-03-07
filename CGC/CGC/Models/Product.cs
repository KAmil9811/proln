using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGC.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Desk { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; }
        public string Id_item { get; set; }
        public string Id_order { get; set; }
        public string Cut_id { get; set; }
        public string Error_Messege { get; set; }
        public int sort { get; set; }
        public int Global_Id { get; set; }
    }
}