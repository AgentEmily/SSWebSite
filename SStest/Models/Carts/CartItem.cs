using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SStest.Models.Carts
{
    [Serializable]  //可序列化
    public class CartItem   //購物車內單一商品類別
    {
        //商品編號
        [DisplayName("商品編號")]
        public int Product_ID { get; set; }

        //商品名稱
        [DisplayName("商品名稱")]
        public string ProductName { get; set; }

        //商品購買時價格
        [DisplayName("單價")]
        public int UnitPrice { get; set; }

        //商品購買數量
        [DisplayName("數量")]
        public int Stock { get; set; }

        //商品小計
        public decimal Amount
        {
            get
            {
                return this.UnitPrice * this.Stock;
            }
        }
    }
}