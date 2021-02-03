using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class Cut_Project
    {
        public string Cut_id { get; set; }
        public string Order_id {  get; set;  }
        public string Status { get; set; }
        public string Owner { get; set; }
        public string Deadline { get; set; }
        public string Priority { get; set; }
        public List<Item> Items { get; set; }
        public List<Glass> Glasses { get; set; }
    }
}
