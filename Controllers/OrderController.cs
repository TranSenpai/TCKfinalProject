using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCKfinalProject.Models;

namespace TCKfinalProject.Controllers
{
    public class OrderController : Controller
    {
        dbfinalProjectASPDataContext db = new dbfinalProjectASPDataContext();
        public ActionResult Orders()
        {
            List<Order> lst = (from a in db.orders
                               join b in db.orderdetails on a.order_id equals b.order_id
                               group b by b.order_id into g
                               select new Order
                               {
                                   orderID = g.First().order.order_id,
                                   customerName = g.First().order.customer.customer_name,
                                   isShip = g.First().order.isship.Value,
                                   isPayent = g.First().order.ispayment.Value,
                                   deliverydate = g.First().order.delivery_date.Value,
                                   orderDate = g.First().order.order_date.Value,
                                   total = g.Sum(x => x.price)
                               }).ToList();
            return View(lst);
        }
    }
}