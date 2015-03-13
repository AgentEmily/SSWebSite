using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SStest.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace SStest.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        [Authorize]
        public ActionResult Index()
        {
            return View();
            
        }
        int MemberID;
        public ActionResult GetEvent()
        {
            //讀出貨產品 + 日期 
            
            using (SmartShoppingEntities db=new SmartShoppingEntities())
            {
                if (User.Identity.IsAuthenticated)
            {
                string aspid = User.Identity.GetUserId();
                MemberID = db.Members.Where(m => m.Id == aspid).Select(m=>m.Member_ID).First();
            }
                var list = (from p in db.PList
                            where p.Member_ID == MemberID
                            select p.PList_ID).ToList();

                //如果沒有PID  就讀CID
                var q = from s in db.ShoppingList.Include("ShoppingListInside").Where(c=> list.Contains(c.PList_ID))
                            select new { title = s.Products.ProductName??s.ListName, start = s.NextDate, id=s.List_ID};
                var list2 = q.ToList();  
                        

                return Json(q.ToList(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult Dropped(string title, DateTime date, int id)
        {
            //存進List Update
            using (SmartShoppingEntities db=new SmartShoppingEntities())
            {
                ShoppingList shoppinglist = db.ShoppingList.Find(id);
           
            shoppinglist.NextDate = date;
            db.Entry(shoppinglist).State = EntityState.Modified;
            db.SaveChanges();
                //修改訂單資料如果是需要出貨的

            return RedirectToAction("Index");
            }
           
        }

        [HttpPost]
        public ActionResult Added(string title, DateTime date)
        {

            using (SmartShoppingEntities db = new SmartShoppingEntities())
            {
                ShoppingList shoppinglist = new ShoppingList();
                //categoryname?
                shoppinglist.NextDate = date;
                db.Entry(shoppinglist).State = EntityState.Modified;
                db.SaveChanges();
                //修改訂單資料如果是需要出貨的

                return RedirectToAction("Index");
            }
        }

        
    }
}