using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class Machines_History
    {
        public string Date { get; set; }
        public int No { get; set; }
        public int Cut_Id { get; set; }     
        public string Login { get; set; }
        public string Description { get; set; }
    }
}
