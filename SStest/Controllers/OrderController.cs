using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace SStest.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        int MemberID;
        [HttpPost]
        public ActionResult Index(Models.OrderModel.Ship postback)
        {
            if (this.ModelState.IsValid)
            {
                //取得目前購物車
                var currentcart = Models.Carts.Operation.GetCurrentCart();

                using (Models.SmartShoppingEntities ss = new Models.SmartShoppingEntities())
                {
                    //取得目前登入使用者Id
                    if (User.Identity.IsAuthenticated)
                    {
                        string aspid = User.Identity.GetUserId();
                        MemberID = ss.Members.Where(m => m.Id == aspid).Select(m => m.Member_ID).First();
                    }
                    
                    //建立 Order 物件            
                    int T = (int)currentcart.TotalAmount;
                    var order = new Models.Orders()
                    {
                        //Order_ID
                        Member_ID = MemberID,
                        OrderDate=DateTime.Now,
                        SubTotal=T,
                        ValueAddTax=System.Convert.ToInt32(T*0.05),
                        ShippingFee=50,
                        Amount=System.Convert.ToInt32(T*1.05+50),
                        Consignee = postback.Consignee,
                        ShipAddress = postback.ShipAddress,
                        ShipMethod_ID=1,
                        PaymentMethod_ID=1,
                        Status_ID=1,
                        Comment = postback.ConsigneePhone
                    };

                    //將其加入 Orders資料表後，儲存變更
                    ss.Orders.Add(order);
                    ss.SaveChanges();

                    //取得購物車中OrderDetail物件
                    var orderDetails = currentcart.ToOrderDetailList(order.Order_ID);
           
                    //將其加入 OrderDetails 資料表後，儲存變更
                    ss.OrderDetail.AddRange(orderDetails);
                    ss.SaveChanges();
                }
                return Content("訂購成功");
            }
            return View();
        }


        //
        public ActionResult MyOrder()
        {
            using (Models.SmartShoppingEntities ss = new Models.SmartShoppingEntities())
            {
                //取得目前登入使用者Id
                if (User.Identity.IsAuthenticated)
                {
                    string aspid = User.Identity.GetUserId();
                    MemberID = ss.Members.Where(m => m.Id == aspid).Select(m => m.Member_ID).First();
                }


                var result = (from o in ss.Orders
                              where o.Member_ID == MemberID
                              select o).ToList();

                return View(result);
            }
           
        }


        public ActionResult MyOrderDetail(int id)
        {
            using(Models.SmartShoppingEntities ss = new Models.SmartShoppingEntities())
            {
                var result = (from o in ss.OrderDetail
                              where o.Order_ID == id
                              select o).ToList();

                if(result.Count == 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(result);
                }
            }
        }
    }
}