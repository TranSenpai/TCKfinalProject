using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCKfinalProject.Models
{
    public class Order
    {
        public int orderID { get; set; }
        public string customerName { get; set; }
        public bool isShip { get; set; }
        public bool isPayent { get; set; }
        public DateTime deliverydate { get; set; }
        public DateTime orderDate { get; set; }
        public decimal? total { get; set; }
    }
}