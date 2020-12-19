using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGC.Models
{
    public class Machines
    {
        public int No { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public bool Stan { get; set; }

        public string Error_Message;

    }
}