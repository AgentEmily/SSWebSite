using SStest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SStest.Controllers
{
    public class MapController : Controller
    {

        private SmartShoppingEntities db = new SmartShoppingEntities();
        //private IQueryable<ShoppingList> ShoppingLists;

        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize]
        public ActionResult Map(int M_id = 1)
        {
            var MemberLists = from a in db.ShoppingList
                              where a.PList.Member_ID == M_id
                              select a;

            return PartialView(MemberLists);
        }

        public ActionResult AddrListsByM_ID(int M_id = 1)
        {
            var List = from a in db.ShoppingList
                       where a.PList.Member_ID == M_id
                       select new { addr = a.Address, text = a.ListName, id = a.ListName };

            return Json(List.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddrListsByPL_ID(int PL_ID = 1)
        {
            var List = from a in db.ShoppingList
                       where a.PList.Member_ID == PL_ID
                       select new { addr = a.Address, text = a.ListName, id = a.ListName };

            return Json(List.ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}