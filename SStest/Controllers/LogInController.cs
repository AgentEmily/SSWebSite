using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SStest.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace SmartShoppingWebsite.Controllers
{
    
    public class LogInController : Controller
    {
        // GET: LogIn
        SmartShoppingEntities db = new SmartShoppingEntities();
        int MemberID;
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Members member)
        {
            //先做一個salt再存Password
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] buf = new byte[15];
            rng.GetBytes(buf);
            string salt = Convert.ToBase64String(buf);
          
            string Password = member.Password;
            Password = FormsAuthentication.HashPasswordForStoringInConfigFile(Password + salt, "sha1");
            member.Password = Password;
            
            db.Members.Add(member);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
            
            return View();
        }

        public ActionResult test()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.message= User.Identity.GetUserId();
            }
            return View();
        }

        public ActionResult RoleSelect()
        {
            
            return View(db.Roles.ToList());
        }
        //被點的RoleID
        [HttpPost]
        public ActionResult RoleSelected(List<int> id)
        {
            //List<int> SelectedRole = new List<int>();
            //if (!SelectedRole.Contains(id))
            //{
            //    SelectedRole.Add(id);
            //}
            //else
            //{
            //    SelectedRole.Remove(id);
            //}
           //id get categories list function
            List<Categories> categories = SStest.Models.RoleSelect.Operations.GetCategories(id);
            var q = from p in categories
                    select new { categoryid = p.Category_ID, categoryname = p.CategoryName };

            return Json(q.ToList(),JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CategorySelected(List<int> id)
        {

            //if (System.Web.HttpContext.Current.Session["CategorySelected"]==null)
            //{
            //    System.Web.HttpContext.Current.Session["CategorySelected"] = id;
            //}

            //幫她存起來
            if (User.Identity.IsAuthenticated)
            {
                string aspid = User.Identity.GetUserId();
                MemberID = db.Members.Where(m => m.Id == aspid).Select(m => m.Member_ID).First();
            }
            //先做一個PList
            PList pl = new PList();
            pl.PList_ID = db.PList.Max(p => p.PList_ID) + 1;
            pl.ListName = "";

                //寫到這裡
            foreach (var item in id)
            {
                ShoppingList sl = new ShoppingList();
            }
            
            return Content("Success");

        }

    }
}