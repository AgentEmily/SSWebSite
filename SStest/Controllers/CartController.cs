using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SStest.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }


        //取得購物車頁面
        public ActionResult GetCart()
        {
            return PartialView("_CartPartial");
        }

        //以id 加入Product 至購物車，並回傳購物車頁面
        public ActionResult AddToCart(int id)
        {
            var currentCart = Models.Carts.Operation.GetCurrentCart();
            currentCart.AddProduct(id);
            return PartialView("_CartPartial");
        }


        //將商品從購物車中移除 (要去呼叫 Models 資料夾裡的 Cart.cs 的 RemoveProduct()方法)
        public ActionResult RemoveFromCart(int id)
        {
            var currentCart = Models.Carts.Operation.GetCurrentCart();
            currentCart.RemoveProduct(id);
            return PartialView("_CartPartial");
        }


        //清空購物車 (要去呼叫 Models 資料夾裡的 Cart.cs 的 ClearCart() 方法)
        public ActionResult ClearCart()
        {
            var currentCart = Models.Carts.Operation.GetCurrentCart();
            currentCart.ClearCart();
            return PartialView("_CartPartial");
        }
    }
}