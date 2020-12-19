using System.Collections.Generic;

namespace CGC.Models
{
    public class Receiver
    {
        public User admin { get; set; }
        public User user { get; set; }
        public Order order { get; set; }
        public Glass glass { get; set; }
        public Machines machines { get; set; }
        public Product product { get; set; }
        public Item item { get; set; }
        public Package package { get; set; }
        public Cut_Project cut_Project { get; set; }

        public List<Glass> glasses = new List<Glass>();
        public List<Item> items = new List<Item>();
        public List<Glass_Id> glass_Ids = new List<Glass_Id>();
        public List<int> glass_Id = new List<int>();
        public List<Package> packages = new List<Package>();


        public string type { get; set; }
        public string new_type { get; set; }
        public string old_type { get; set; }
        public string color { get; set; }
        public string status { get; set; }
        public string email { get; set; }
        public string new_password { get; set; }
        public string new_password_check { get; set; }
        public string new_color { get; set; }
        public string old_color { get; set; }
        public string new_name { get; set; }
        public string new_surname { get; set; }
        public string perm { get; set; }
        public string id_glass { get; set; }
    }
}