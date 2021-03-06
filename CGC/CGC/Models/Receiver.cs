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

        public List<Glass> glasses { get; set; }
        public List<Item> items { get; set; }
        public List<Glass_Id> glass_Ids { get; set; }
        public List<string> glass_Id { get; set; }
        public List<int> product_Id { get; set; }
        public List<int> item_Id { get; set; }
        public List<Package> packages { get; set; }
        public List<Product> products { get; set; }
        public List<Piece> pieces { get; set; }
        public List<string> iteme { get; set; }

        public string id { get; set; }
        public double thickness { get; set; }
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
        public string glass_count { get; set; }
    }
}