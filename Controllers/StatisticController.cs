using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCKfinalProject.Models;
namespace TCKfinalProject.Controllers
{
    public class StatisticController : Controller
    {
        dbfinalProjectASPDataContext db = new dbfinalProjectASPDataContext();
        public ActionResult StatisticFood()
        {
            List<SumSaleQuantity> lst = (
                from a in db.foods
                join b in db.orderdetails on a.food_id equals b.food_id
                group b by b.food_id into g
                select new SumSaleQuantity
                {
                    foodName = g.First().food.food_name,
                    SumQuantity = g.Sum(x => x.quantity)
                }
            ).ToList();
            return View(lst);
        }
    }
}