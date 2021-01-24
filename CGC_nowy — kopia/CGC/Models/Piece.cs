using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class Piece
    {
        public double X { get; set; }
        public double Y { get; set; }

        public double Lenght { get; set; }
        public double Widht { get; set; }

        public double Id { get; set; }

        public List<int> Rgb { get; set; }
    }
}
