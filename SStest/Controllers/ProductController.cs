using SStest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace SStest.Controllers
{
    public class ProductController : Controller
    {
        private SmartShoppingEntities db = new SmartShoppingEntities();
        ProductModels a = new ProductModels();//韶怡的join Product & Picture Model
        int MemberID;

        // GET: Product
        public ActionResult Index()
        {
            List<ProductPictures> c = new List<ProductPictures>();
            List<ProductModels> product = new List<ProductModels>();


            foreach (var item in db.Products)
            {
                ProductModels _product = new ProductModels();

                _product.Product_ID = item.Product_ID;
                _product.ProductName = item.ProductName;
                _product.Stock = Convert.ToDecimal(item.Stock);

                //var Picture = item.ProductPictures;             
                _product.Price = Convert.ToDecimal(item.UnitPrice);
                _product.Description = item.Description;

                product.Add(_product);
            }
            foreach (var p in db.ProductPictures)
            {
                ProductPictures b = new ProductPictures();
                b.Picture = a.Picture;
                c.Add(b);
            }

            return View(product.ToList());

        }

        public ActionResult GetImageByte(int id = 1)
        {

            ProductPictures product1 = db.ProductPictures.Find(id);
            byte[] img = product1.Picture;
            return File(img, "image/jpeg");
        }

        public ActionResult Details(int id = 1)
        {
            if (System.Web.HttpContext.Current.Session["currentPID"] == null)
            {
                System.Web.HttpContext.Current.Session["currentPID"] = id;
            }
            return PartialView(db.Products.Find(id));
        }

        public ActionResult CategoryParent()
        {
            var q = db.Categories.Where(c => c.PCategory_ID == null && c.Source_ID == 1);

            return PartialView("_CategoryParent", q.ToList());
        }

        public ActionResult CategoryKids(int id)
        {
            var q = db.Categories.Where(c => c.PCategory_ID == id && c.Source_ID == 1);

            return PartialView("_CategoryKids", q.ToList());
        }

        public ActionResult Brands()
        {


            return PartialView("_Brands", db.Brands.ToList());
        }

        public ActionResult Search(string SearchText, int? id)
        {
            if (id != null && SearchText == "瀏覽相關產品")
            {
                SearchText = db.ShoppingList.Where(s => s.List_ID == id).First().ListName;
                //變更List中
                System.Web.HttpContext.Current.Session["switchingid"] = id;
            }

            List<ProductModels> product = new List<ProductModels>();
            foreach (var item in db.Products.Where(p => p.ProductName.Contains(SearchText)))
            {
                ProductModels _product = new ProductModels();

                _product.Product_ID = item.Product_ID;
                _product.ProductName = item.ProductName;
                _product.Stock = Convert.ToDecimal(item.Stock);

                //var Picture = item.ProductPictures;             
                _product.Price = Convert.ToDecimal(item.UnitPrice);
                _product.Description = item.Description;

                product.Add(_product);
            }

            return View("Index", product.ToList());

        }

        public ActionResult CategorySearch(int cid)
        {
            List<ProductModels> product = new List<ProductModels>();
            foreach (var item in db.Products.Where(p => p.Category_ID == cid))
            {
                ProductModels _product = new ProductModels();

                _product.Product_ID = item.Product_ID;
                _product.ProductName = item.ProductName;
                _product.Stock = Convert.ToDecimal(item.Stock);

                //var Picture = item.ProductPictures;             
                _product.Price = Convert.ToDecimal(item.UnitPrice);
                _product.Description = item.Description;

                product.Add(_product);
            }

            return View("Index", product.ToList());
        }

        [HttpPost]  //限定使用POST
        [Authorize] //登入會員才可留言
        public ActionResult AddComment(int Product_ID, string Content)
        {

            //取得目前登入使用者Id

            if (User.Identity.IsAuthenticated)
            {
                string k = User.Identity.GetUserId();
                MemberID = db.Members.Where(m => m.Id == k).Select(m => m.Member_ID).First();
            }



            var currentDateTime = DateTime.Now;

            var comment = new Models.Comment()
            {
                Product_ID = Product_ID,
                Comment1 = Content,

                Member_ID = MemberID,
                Date = currentDateTime
            };

            using (Models.SmartShoppingEntities db = new Models.SmartShoppingEntities())
            {
                db.Comment.Add(comment);
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = Product_ID });
        }

        public ActionResult AddComment()
        {
            return RedirectToAction("Details", new { id = System.Web.HttpContext.Current.Session["currentPID"] });
        }

        public ActionResult RecommendationChart(int? P_ID)
        {
            if (P_ID == null)//判斷P_ID是否為null
            {
                return RedirectToAction("Home", "RecommendedItems", new { id = P_ID });
            }
            else
            {
                IQueryable<int?> p_idQuery = db.RecommendationChart.Where(c => c.PID == P_ID).Select(c=>c.NO);
                if(p_idQuery==null)//判斷P_ID的商品是否有關連商品
                {
                    return RedirectToAction("Home", "RecommendedItems", new { id = P_ID });
                }
                else
                {
                    IQueryable<Products> productQuery = db.Products.Where(t => p_idQuery.Contains(t.Product_ID)).Take(4);
                    return PartialView("_RecommendationChartPartial", productQuery);
                }
            }
        }

        public ActionResult GetPhoto(int id = 1)
        {
            var PP_Id = db.ProductPictures.FirstOrDefault(p => p.Product_ID == id).ProductPicture_ID;
            byte[] img = db.ProductPictures.Find(PP_Id).Picture;
            return File(img, "image/jpeg");
        }
    }
}
