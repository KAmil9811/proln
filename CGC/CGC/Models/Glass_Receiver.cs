﻿using System;
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
        public string Count { get; set; }

        public string Type { get; set; }
        public string Color { get; set; }
        public string Owner { get; set; }
        public string Desk { get; set; }

        public string Glass_Id { get; set; }
        public bool Used { get; set; }
        public bool Removed { get; set; }
        public string Cut_id { get; set; }
        public int Global_Id { get; set; }
    }
}
