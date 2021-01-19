using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class Cut_Project
    {
        public int Cut_id { get; set; }
        public string Order_id {  get; set;  }
        public string Status { get; set; }
        public string Owner { get; set; }
        public string Deadline { get; set; }
        public int Priority { get; set; }
        public List<Item> Items = new List<Item>();
        public List<Glass> Glasses = new List<Glass>();
    }
}
