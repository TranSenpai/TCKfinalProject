using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCKfinalProject.Models
{
    public class Cart
    {
        dbfinalProjectASPDataContext db = new dbfinalProjectASPDataContext();
        public int food_id { get; set; }
        [Display(Name = "Book name")]
        public string food_name { get; set; }
        [Display(Name = "Cover image")]
        public string image { get; set; }
        [Display(Name = "Price")]
        public Double price { get; set; }
        [Display(Name = "Quantity")]
        public int iquantity { get; set; }
        [Display(Name = "Total")]
        public Double Total
        {
            get { return iquantity * price; }
        }
        public Cart(int id)
        {
            food_id = id;
            food f = db.foods.Single(n => n.food_id == food_id);
            food_name = f.food_name;
            image = f.image;
            price = Double.Parse(f.price.ToString());
            iquantity = 1;
        }
    }
}