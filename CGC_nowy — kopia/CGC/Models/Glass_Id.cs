using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class Glass_Id
    {
        public int Id { get; set; }
        public bool Used { get; set; }
        public bool Destroyed { get; set; }
        public bool Removed { get; set; }
        public int Cut_id { get; set; }
        public List<Piece> Pieces = new List<Piece>();
    }
}
