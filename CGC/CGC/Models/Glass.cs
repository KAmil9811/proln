using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGC.Models
{
    public class Glass
    {
        public string Hight { get; set; } //z
        public string Width { get; set; } //y
        public string Length { get; set; } //x
        public string Count { get; set; }

        public string Type { get; set; }
        public string Color { get; set; }

        public string Id { get; set; }
        public string Owner { get; set; }
        public string Desk { get; set; }

        public List<Glass_Id> Glass_info { get; set; }
        public string Error_Messege { get; set; }

        public List<int> Glass_id { get; set; }


    }
}