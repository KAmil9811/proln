using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class Cut_Project
    {
        public string Order_id;
        public List<Item> Items = new List<Item>();
        public List<Glass> Glasses = new List<Glass>();
    }
}
