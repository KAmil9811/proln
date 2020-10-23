using Microsoft.IdentityModel.Tokens.Saml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class Item
    {
        //Zalezne od shape
        
        public double Thickness { get; set; }
        public double Length { get; set; }


        //Ogólne
        public double Width { get; set; }
        public string Type { get; set; }
        public bool Can_Be_Createad { get; set; }
        public string Color { get; set; }
        public int Id { get; set; }
        public string Status { get; set; }
        public string Shape { get; set; }
        public string Sub_Shape { get; set; }
        public string Desk { get; set; }
        public int Amount { get; set; }
    }
}
