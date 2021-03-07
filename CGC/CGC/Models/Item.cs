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
        
        public string Length { get; set; } //x
        public string Width { get; set; } //y


        //Ogólne
        public string Thickness { get; set; } //z
        public string Type { get; set; }
        public bool Can_Be_Createad { get; set; }
        public string Color { get; set; }
        public string Id { get; set; }
        public string Product_Id { get; set; }
        public string Status { get; set; }
        public string Shape { get; set; }
        public string Order_id { get; set; }
        public string Desk { get; set; }
        public string Amount { get; set; }

        //Ciecie
        public double Area { get; set; }
        public int Fit_pos { get; set; }
        public string Cut_id { get; set; }


        public int sort { get; set; }

        public double WidthSort { get; set; }
        public double LengthSort { get; set; }
        public string Error_Messege { get; set; }
        public int Global_Id { get; set; }
    }
}