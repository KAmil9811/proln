using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class Glass_Receiver
    {
        public double Hight { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public int Count { get; set; }

        public string Type { get; set; }
        public string Color { get; set; }
        public string Owner { get; set; }
        public string Desk { get; set; }

        public int Glass_Id { get; set; }
        public bool Used { get; set; }
        public bool Destroyed { get; set; }
        public bool Removed { get; set; }

    }
}
