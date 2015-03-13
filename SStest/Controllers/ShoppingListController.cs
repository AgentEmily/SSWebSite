using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SStest.Models;
using System.Data.Entity;

namespace SStest.Controllers
{
    public class ShoppingListController : Controller
    {
        // GET: ShoppingList
        int MemberID;
        int firstid;
        [Authorize]
        public ActionResult ShoppingList()
        {
            //using (SmartShoppingEntities db = new SmartShoppingEntities())
            //{
            //    if (User.Identity.IsAuthenticated)
            //    {
            //        string aspid = User.Identity.GetUserId();
            //        MemberID = db.Members.Where(m => m.Id == aspid).Select(m => m.Member_ID).First();
            //    }
            //    firstid = db.PList.Where(m => m.Member_ID == MemberID).Select(m => m.PList_ID).First();
            //    var q = db.ShoppingList.Where(s => s.PList_ID == firstid);
            //    return View(q.ToList());
            //}
            return View();
        }
        public ActionResult ShoppingListP(int id)
        {
            //點到的PListID存起來
            System.Web.HttpContext.Current.Session["PListID"] = id;
            return RedirectToAction("ShoppingList");

            //using (SmartShoppingEntities db = new SmartShoppingEntities())
            //{
            //    if (User.Identity.IsAuthenticated)
            //    {
            //        string aspid = User.Identity.GetUserId();
            //        MemberID = db.Members.Where(m => m.Id == aspid).Select(m => m.Member_ID).First();
            //    }

            //    var q = db.ShoppingList.Where(s => s.PList_ID == id);
            //    return RedirectToAction("ShoppingList", q.ToList());//改了這個
            //}
        }

        public ActionResult ShoppingCategoryPartial()
        {
            //讀取Member的父類別
            using (SmartShoppingEntities db = new SmartShoppingEntities())
            {

                if (User.Identity.IsAuthenticated)
                {
                    string aspid = User.Identity.GetUserId();
                    MemberID = db.Members.Where(m => m.Id == aspid).Select(m => m.Member_ID).First();
                }

                var q = from s in db.PList
                        where s.Member_ID == MemberID
                        select s;


                return PartialView("ShoppingCategoryPartial", q.ToList());

            }

        }

        public ActionResult CategoryInsert(string item)
        {
            using (SmartShoppingEntities db = new SmartShoppingEntities())
            {
                if (User.Identity.IsAuthenticated)
                {
                    string aspid = User.Identity.GetUserId();
                    MemberID = db.Members.Where(m => m.Id == aspid).Select(m => m.Member_ID).First();
                }

                PList plist = new PList();
                plist.ListName = item;
                plist.Member_ID = MemberID;
                db.Entry(plist).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("ShoppingList");
            }

        }

        public ActionResult AddItem(string item)
        {
            using (SmartShoppingEntities db = new SmartShoppingEntities())
            {
                if (User.Identity.IsAuthenticated)
                {
                    string aspid = User.Identity.GetUserId();
                    MemberID = db.Members.Where(m => m.Id == aspid).Select(m => m.Member_ID).First();
                }

                ShoppingList slist = new Models.ShoppingList();
                if (System.Web.HttpContext.Current.Session["PListID"] == null)
                {
                    slist.PList_ID = db.PList.Where(m => m.Member_ID == MemberID).Select(m => m.PList_ID).First(); ;
                }
                else
                {
                    slist.PList_ID = (int)System.Web.HttpContext.Current.Session["PListID"];
                }

                slist.ListName = item;
                slist.Active = true;

                db.Entry(slist).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("ShoppingListP", new { id = slist.PList_ID });
            }
        }

        public ActionResult GetItem(int id = 1)
        {
            using (SmartShoppingEntities db = new SmartShoppingEntities())
            {
                if (User.Identity.IsAuthenticated)
                {
                    string aspid = User.Identity.GetUserId();
                    MemberID = db.Members.Where(m => m.Id == aspid).Select(m => m.Member_ID).First();
                }

                var q = db.ShoppingList.Where(s => s.PList_ID == id);
                return PartialView("ListItemPartial", q.ToList());
            }

        }

        [Authorize]
        public ActionResult ListItemPartial()
        {
            int id;
            
            using (SmartShoppingEntities db = new SmartShoppingEntities())
            {
               
                if (User.Identity.IsAuthenticated)
                {
                    string aspid = User.Identity.GetUserId();
                    MemberID = db.Members.Where(m => m.Id == aspid).Select(m => m.Member_ID).First();
                }
            if (System.Web.HttpContext.Current.Session["PListID"] == null)
            {
                id = db.PList.Where(m => m.Member_ID == MemberID).Select(m => m.PList_ID).First(); ;
            }
            else
            {
                id = (int)System.Web.HttpContext.Current.Session["PListID"];
            }


                var q = db.ShoppingList.Where(s => s.PList_ID == id);
                return PartialView(q.ToList());//改了這個
            }
        }

        public ActionResult Detail(int id)//Get ListID 
        {
            ViewBag.Clicked = true;

            using (SmartShoppingEntities db = new SmartShoppingEntities())
            {
                var sl = from s in db.ShoppingList.Include("ShoppingListInside")
                         where s.List_ID == id
                         select new MyShoppingList{id=s.List_ID, listname=s.ListName, productname=s.Products.ProductName??"瀏覽相關產品", repeatno=s.RepeatNumber??0, repeatby=s.RepeatBy??1, lastdate= s.LastDate??DateTime.Today, nextdate=s.NextDate??DateTime.Today, quantity=s.Quantity??1};
               
                return View("ShoppingList", sl.First());
            }
        }

        public ActionResult SetProduct(int id)//PID 接收到他現在要換什麼但還沒確認
        {
            
            using (SmartShoppingEntities db = new SmartShoppingEntities())
            { 
             //存入shoppinglistinside
                int switching=(int)System.Web.HttpContext.Current.Session["switchingid"];

                var slist = (from s in db.ShoppingList.Include(s=>s.Products)
                             where s.List_ID == switching
                             select s).First();

                slist.List_ID =switching;
                slist.Products = db.Products.Where(p => p.Product_ID == id).First();//存整個product給他

                db.Entry(slist).State = EntityState.Modified;
                db.SaveChanges();
               

                return RedirectToAction("Detail", "ShoppingList", new { id=switching});
            }
        }

        public ActionResult DeleteItem(int listid)
        {
            using (SmartShoppingEntities db = new SmartShoppingEntities())
            { 
            //刪除shoppinglist& shoppinglistinside
                var slist = (from s in db.ShoppingList.Include(s => s.Products)
                             where s.List_ID == listid
                             select s).First();
                if (slist.Order_ID!=null)
                {
                     //刪除order
                    int orderid = (int)slist.Order_ID;
                    OrderDetail od = db.OrderDetail.Where(o => o.Order_ID == orderid).First();
                    db.Entry(od).State = EntityState.Deleted;
                    Orders order = db.Orders.Where(o => o.Order_ID == orderid).First();
                    db.Entry(order).State = EntityState.Deleted;
                }
                
                db.Entry(slist).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("ShoppingList");
            }
        }
        [HttpPost]
        public ActionResult SaveChanges(int listid, int repeatno, int repeatby, string nextdate, int quantity)
        {
            DateTime date = Convert.ToDateTime(nextdate);
            using (SmartShoppingEntities db = new SmartShoppingEntities())
            {
                var slist = (from s in db.ShoppingList.Include(s => s.Products)
                             where s.List_ID == listid
                             select s).First();
                slist.RepeatNumber = repeatno;
                slist.RepeatBy = repeatby;
                slist.NextDate = date;
                slist.Quantity = quantity;
                db.Entry(slist).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Clicked = null;

                //存到order去
                 //order跟著動 addOrUpdate
                if (slist.Products!=null)
                {
                    int pid = slist.Products.Product_ID;
                    
                    if (User.Identity.IsAuthenticated)
                {
                    string aspid = User.Identity.GetUserId();
                    MemberID = db.Members.Where(m => m.Id == aspid).Select(m => m.Member_ID).First();
                }
                
                    var order = (from o in db.OrderDetail
                             where o.Orders.Status_ID == 7 && o.Orders.Member_ID == MemberID && o.Product_ID == pid
                             select o).FirstOrDefault();
                    var originalOID = slist.Order_ID;
                if (order==null)
                {
                    //第一次設add
                    Orders neworder = new Orders();
                    neworder.ArrivalDate = date;
                    neworder.Member_ID = MemberID;
                    neworder.OrderDate = DateTime.Today;
                    neworder.SubTotal = slist.Products.UnitPrice;
                    neworder.ValueAddTax = (int)Math.Round((neworder.SubTotal) * 0.05);
                    neworder.Amount = neworder.SubTotal + neworder.ValueAddTax;
                    neworder.ShipMethod_ID = 1;
                    neworder.PaymentMethod_ID = 4;
                    neworder.Status_ID = 7;
               
                
neworder.Order_ID = db.Orders.Max(o => o.Order_ID) + 1;
 db.Entry(neworder).State = EntityState.Added;

     //存orderid到slist
slist.Order_ID = neworder.Order_ID;
db.Entry(slist).State = EntityState.Modified;
                   
                    

                    //save orderdetail
                    OrderDetail od = new OrderDetail();
                    od.Order_ID = neworder.Order_ID;
                    od.Product_ID = pid;
                    od.UnitPrice = slist.Products.UnitPrice;
                    
                    od.Quantity = quantity;
                    db.Entry(od).State = EntityState.Added;
                    db.SaveChanges();
                    
                }
                if(order==null&&originalOID!=null)//是更換要把本來的訂單刪除
                {
                    OrderDetail od = db.OrderDetail.Where(o => o.Order_ID == originalOID).First();
                    db.Entry(od).State = EntityState.Deleted;
                    Orders ord = db.Orders.Find(originalOID);
                    db.Entry(ord).State = EntityState.Deleted;

                    db.SaveChanges();

                }
                if (order!=null&&slist.Order_ID!=null)
                {
                    //修改既有訂單.表示只是修同一筆  
                    Orders ord = db.Orders.Find((int)slist.Order_ID);
                    ord.ArrivalDate = date;
                    OrderDetail od = db.OrderDetail.Where(o => o.Order_ID == (int)slist.Order_ID).First();
                    od.Quantity = quantity;
                    db.Entry(od).State = EntityState.Modified;
                    db.Entry(ord).State = EntityState.Modified;
                    db.SaveChanges();
                }

                }
                
            return Content("success");
            }
            
        }
    }
}


            

