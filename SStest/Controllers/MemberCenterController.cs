using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SStest.Models;
using System.Data.Entity;


namespace SStest.Controllers
{
    public class MemberCenterController : Controller
    {
        SmartShoppingEntities db = new SmartShoppingEntities();
        // GET: MemberCenter
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CancelEmailAd(bool Confirmed)
        {
            ViewBag.Confirmed = Confirmed;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelAd(Members member)
        {
            //get member with the email, save adok to false
            if (ModelState.IsValid)
            {
                var user = db.Members.Where(c=>c.Email==member.Email).First();
                if (user!=null)
                {
                    user.AdOK = false;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
            }
            return RedirectToAction("CancelEmailAd", new { Confirmed=true});
        }
    }
}