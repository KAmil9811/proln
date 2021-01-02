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
        
        public double Length { get; set; } //x
        public double Width { get; set; } //y


        //Ogólne
        public double Thickness { get; set; } //z
        public string Type { get; set; }
        public bool Can_Be_Createad { get; set; }
        public string Color { get; set; }
        public int Id { get; set; }
        public int Product_Id { get; set; }
        public string Status { get; set; }
        public string Shape { get; set; }
        public string Order_id { get; set; }
        public string Desk { get; set; }
        public int Amount { get; set; }

        //Ciecie
        public double Area { get; set; }
        public int Fit_pos { get; set; }
        public int Cut_id { get; set; }


        public string Error_Messege { get; set; }
    }
}