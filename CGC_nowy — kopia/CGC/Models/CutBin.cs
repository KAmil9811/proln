using Sharp3DBinPacking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class CutBin
    {
        public BinPackParameter Parameter { get; set; }
        public Package package { get; set; }
        public BinPackResult result { get; set; }
    }
}
