using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class Glass_Id
    {
        public string Id { get; set; }
        public bool Used { get; set; }
        public bool Removed { get; set; }
        public string Cut_id { get; set; }
        public List<Piece> Pieces { get; set; }
    }
}
