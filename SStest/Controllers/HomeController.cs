using SStest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SStest.Controllers
{
  [RequireHttps]
    public class HomeController : Controller
    {
      private SmartShoppingEntities db = new SmartShoppingEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AdSlider()
        {
            var q = from a in db.AdSlider
                          select a;

            return PartialView("_AdSliderPartial", q);
        }

        public ActionResult HotItems()
        {
            IQueryable<Products> productQuery = db.Promotions
          .Where(c => c.PromotionName == "熱銷商品")
          .SelectMany(c => c.Products);
            return PartialView("_HotItemsPartial", productQuery);
        }

        public ActionResult FeaturesItems()
        {
            IQueryable<Products> productQuery = db.Promotions
          .Where(c => c.PromotionName == "每日好康")
          .SelectMany(c => c.Products);
            return PartialView("_FeaturesItemsPartial", productQuery);
        }

        public ActionResult RecommendedItems()
        {
            IQueryable<Products> productQuery = db.Promotions
          .Where(c => c.PromotionName == "站長推薦")
          .SelectMany(c => c.Products);
            return PartialView("_RecommendedItemsPartial", productQuery);
        }

        public ActionResult GetImageByte(int id = 1)
        {
            AdSlider photo = db.AdSlider.Find(id);
            byte[] img = photo.Picture;
            return File(img, "image/jpeg");
        }

        public ActionResult GetProductPhoto(int id = 1)
        {
            var PP_Id = db.ProductPictures.FirstOrDefault(p=>p.Product_ID==id).ProductPicture_ID;
            byte[] img = db.ProductPictures.Find(PP_Id).Picture;
            return File(img, "image/jpeg");
        }

    }
}