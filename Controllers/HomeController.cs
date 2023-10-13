using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCKfinalProject.Models;
namespace TCKfinalProject.Controllers
{
    public class HomeController : Controller
    {
        dbfinalProjectASPDataContext db = new dbfinalProjectASPDataContext();
        public ActionResult Home(int? size, int? page)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "3", Value = "3" });
            items.Add(new SelectListItem { Text = "6", Value = "6" });
            items.Add(new SelectListItem { Text = "12", Value = "12" });
            items.Add(new SelectListItem { Text = "24", Value = "24" });
            items.Add(new SelectListItem { Text = "48", Value = "48" });
            foreach (var item in items)
            {
                if (item.Value == size.ToString())
                {
                    item.Selected = true;
                }
            }
            ViewBag.size = items;
            ViewBag.currentSize = size;
            if (page == null)
            {
                page = 1;
            }
            var all_book = from Book in db.foods select Book;
            int pageSize = (size ?? 3);
            int pageNum = page ?? 1;
            return View(all_book.ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult Index(int? page, string input_name)
        {
            ViewBag.Keyword = input_name;
            if (page == null)
            {
                page = 1;
            }
            var all_book = (from books in db.foods select books).OrderBy(m => m.food_id);
            if (!string.IsNullOrEmpty(input_name))
            {
                all_book = (IOrderedQueryable<food>)all_book.Where(a => String.Equals(a.food_name, input_name));
            }
            int pageSize = 3;
            int pageNum = page ?? 1;
            return View(all_book.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Detail(int? id)
        {
            var all_book = db.foods.Where(m => m.food_id == id).First();
            return View(all_book);
        }
        public ActionResult Edit(int? id)
        {
            var E_food = db.foods.First(m => m.food_id == id);
            return View(E_food);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_food = db.foods.First(m => m.food_id == id);
            var E_name = collection["food_name"];
            var E_image = collection["image"];
            var E_price = Convert.ToDecimal(collection["price"]);
            var E_updatedate = Convert.ToDateTime(collection["update_name"]);
            var E_quantity = Convert.ToInt32(collection["quantity_instock"]);
            E_food.food_id = id;
            if (string.IsNullOrEmpty(E_name))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_food.food_name = E_name;
                E_food.image = E_image;
                E_food.price = E_price;
                E_food.update_date = E_updatedate;
                E_food.quantity_instock = E_quantity;
                UpdateModel(E_food);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(food book, FormCollection collection)
        {
            var E_name = collection["food_name"];
            var E_image = collection["image"];
            var E_price = Convert.ToDecimal(collection["price"]);
            var E_updatedate = Convert.ToDateTime(collection["update_date"]);
            var E_quantity = Convert.ToInt32(collection["quantity_instock"]);
            if (string.IsNullOrEmpty(E_name))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                book.food_name = E_name.ToString();
                book.image = E_image.ToString();
                book.price = E_price;
                book.update_date = E_updatedate;
                book.quantity_instock = E_quantity;
                db.foods.InsertOnSubmit(book);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }
        public ActionResult Delete(int id)
        {
            var D_food = db.foods.First(m => m.food_id == id);
            return View(D_food);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_book = db.foods.Where(m => m.food_id == id).First();
            db.foods.DeleteOnSubmit(D_book);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}