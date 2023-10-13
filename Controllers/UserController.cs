using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCKfinalProject.Models;

namespace TCKfinalProject.Controllers
{
    public class UserController : Controller
    {
        dbfinalProjectASPDataContext db = new dbfinalProjectASPDataContext();
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection collection, customer c)
        {
            var name = collection["customer_name"];
            var username = collection["username"];
            var password = collection["password"];
            var confirmpassword = collection["confirmpassword"];
            var email = collection["email"];
            var address = collection["address"];
            var numberphone = collection["numberphone"];
            var dob = String.Format("{0:MM/dd/yyyy}", collection["dob"]);
            if (String.IsNullOrEmpty(confirmpassword))
            {
                ViewData["enterpassword"] = "Must enter password to confirm!";
            }
            else
            {
                if (!password.Equals(confirmpassword))
                {
                    ViewData["samepassword"] = "Password and confirmmation password must be the same";
                }
                else
                {
                    c.customer_name = name;
                    c.username = username;
                    c.password = password;
                    c.email = email;
                    c.address = address;
                    c.numberphone = numberphone;
                    c.dob = DateTime.Parse(dob);
                    db.customers.InsertOnSubmit(c);
                    db.SubmitChanges();
                    return RedirectToAction("Register");
                }
            }
            return this.Register();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var username = collection["username"];
            var password = collection["password"];
            customer c = db.customers.SingleOrDefault(n => n.username == username && n.password == password);
            if (c != null)
            {
                ViewBag.Notice = "Congratulations on successful login";
                Session["User"] = c;
                return RedirectToAction("Index", "BookStore");
            }
            else
            {
                ViewBag.Notice = "Username or password is incorrect";
                return this.Login();
            }
        }
    }
}