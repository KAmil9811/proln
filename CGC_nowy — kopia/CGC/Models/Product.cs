using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGC.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Desk { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; }
        public int Id_item { get; set; }
        public string Id_order { get; set; }
        public int Cut_id { get; set; }
        public string Error_Messege;
    }
}