using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCKfinalProject.Models;

namespace TCKfinalProject.Controllers
{
    public class CartController : Controller
    {
        dbfinalProjectASPDataContext db = new dbfinalProjectASPDataContext();
        public List<Cart> getCart()
        {
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart == null)
            {
                lstCart = new List<Cart>();
                Session["Cart"] = lstCart;
            }
            return lstCart;
        }
        public ActionResult AddCart(int id, string strURL)
        {
            List<Cart> lstCart = getCart();
            Cart product = lstCart.Find(n => n.food_id == id);
            if (product == null)
            {
                product = new Cart(id);
                lstCart.Add(product);
                return Redirect(strURL);
            }
            else
            {
                product.iquantity++;
                return Redirect(strURL);
            }
        }
        private int sumQuantity()
        {
            int sum = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                sum = lstCart.Sum(n => n.iquantity);
            }
            return sum;
        }
        private int sumProductQuantity()
        {
            int count = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                count = lstCart.Count;
            }
            return count;
        }
        private double Total()
        {
            double tt = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                tt = lstCart.Sum(n => n.Total);
            }
            return tt;
        }
        public ActionResult Cart()
        {
            List<Cart> lstCart = getCart();
            ViewBag.sumQuantity = sumQuantity();
            ViewBag.Total = Total();
            ViewBag.sumProductQuantity = sumProductQuantity();
            return View(lstCart);
        }
        public ActionResult CartPartial()
        {
            ViewBag.sumQuantity = sumQuantity();
            ViewBag.Total = Total();
            ViewBag.sumProductQuantity = sumProductQuantity();
            return PartialView();
        }
        public ActionResult CartDelete(int id)
        {
            List<Cart> lstCart = getCart();
            Cart product = lstCart.SingleOrDefault(n => n.food_id == id);
            if (product != null)
            {
                lstCart.RemoveAll(n => n.food_id == id);
                return RedirectToAction("Cart");
            }
            return RedirectToAction("Cart");
        }
        [HttpPost]
        public ActionResult CartUpdate(int id, FormCollection collection)
        {
            List<Cart> lstCard = getCart();
            Cart product = lstCard.SingleOrDefault(n => n.food_id == id);
            if (product != null)
            {
                product.iquantity = int.Parse(collection["txtQuantity"].ToString());
            }
            return RedirectToAction("Cart");
        }
        [HttpGet]
        public ActionResult AllCartDelete()
        {
            List<Cart> lstCart = getCart();
            lstCart.Clear();
            return RedirectToAction("Cart");
        }
        public ActionResult PlaceOrder()
        {
            if (Session["User"] == null || Session["User"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Book");
            }

            List<Cart> lstCart = getCart();
            ViewData["sumQuantity"] = sumQuantity();
            ViewBag.Total = Total();
            ViewBag.sumProductQuantity = sumProductQuantity();
            return View(lstCart);
        }
        [HttpPost]
        public ActionResult PlaceOrder(FormCollection collection)
        {
            order dh = new order();
            customer kh = (customer)Session["User"];
            food s = new food();

            List<Cart> gh = getCart();
            var delivery_date = String.Format("{0:PM/dd/yyyy}", collection["delivery_date"]);

            dh.customer_id = kh.customer_id;
            dh.order_date = DateTime.Now;
            dh.delivery_date = DateTime.Parse(delivery_date);
            dh.isship = false;
            dh.ispayment = false;

            db.orders.InsertOnSubmit(dh);
            db.SubmitChanges();

            foreach (var item in gh)
            {
                orderdetail ctdh = new orderdetail();

                ctdh.order_id = dh.order_id;
                ctdh.food_id = item.food_id;
                ctdh.quantity = item.iquantity;
                ctdh.price = (decimal)item.price;
                s = db.foods.SingleOrDefault(n => n.food_id == item.food_id);
                s.quantity_instock = ctdh.quantity;
                db.SubmitChanges();
                db.orderdetails.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["Cart"] = null;
            return RedirectToAction("ConfirmOrder", "Cart");
        }
        public ActionResult ConfirmOrder()
        {
            return View();
        }
    }
}