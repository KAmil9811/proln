using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class Order
    {
        public string Id_Order { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; } //oczekujacy(expectant), wykonany(done), wstrzymany(stopped) 
        public int Priority { get; set; }
        public string Deadline { get; set; }
        public DateTime Deadline2 { get; set; }
        public string Stan { get; set; }
        public bool Deletead { get; set; }
        public bool Frozen { get; set; }
        public bool Released { get; set; }
        
        public List<Order_History> order_Histories = new List<Order_History>();

        public List<Item> items = new List<Item>();

        public string Error_Messege { get; set; }
    }
}
