using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class Package
    {
        public string Type { get; set; }
        public string Color { get; set; }
        public string Owner { get; set; }
        public string Id_Order { get; set; }
        public double Thickness { get; set; }

        public List<Item> Item { get; set; }
    }
}
