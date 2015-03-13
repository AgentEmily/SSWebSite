using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SStest.Models.Carts
{
    [Serializable]
    public class Cart : IEnumerable<CartItem>   //購物車類別
    {
        //建構值
        public Cart()
        {
            this.cartItems = new List<CartItem>();
        }

        //儲存所有商品
        private List<CartItem> cartItems;


        ///<summary>
        /// 取得購物車內商品的總數量
        ///</summary>
        public int Count
        {
            get
            {
                return this.cartItems.Count;
            }
        }


        //取得商品總價
        public decimal TotalAmount
        {
            get
            {
                decimal totalAmount = 0.0m;
                foreach (var cartItem in this.cartItems)
                {
                    totalAmount = totalAmount + cartItem.Amount;
                }
                return totalAmount;
            }
        }


        //新增一筆Product，使用ProductId
        public bool AddProduct(int Product_ID)
        {
            var findItem = this.cartItems
                           .Where(s => s.Product_ID == Product_ID)
                           .Select(s => s)
                           .FirstOrDefault();

            //判斷相同Id的CartItem是否已經存在購物車內
            if (findItem == default(Models.Carts.CartItem))
            {
                //不存在購物車內，則新增一筆
                using (Models.SmartShoppingEntities ss = new SmartShoppingEntities())
                {
                    var product = (from p in ss.Products
                                   where p.Product_ID == Product_ID
                                   select p).FirstOrDefault();
                    if (product != default(Models.Products))
                    {
                        this.AddProduct(product);
                    }
                }
            }
            else
            {
                //存在購物車內，則將商品數量增加
                findItem.Stock += 1;
            }

            return true;
        }

        //新增一筆 Product，使用 Product 物件
        private bool AddProduct(Products product)
        {
            //將 Product 轉為 CartItem
            var cartItem = new Models.Carts.CartItem()
            {
                Product_ID = product.Product_ID,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                Stock = 1
            };

            //加入CartItem至購物車
            this.cartItems.Add(cartItem);
            return true;
        }


        //移除一筆Product，使用ProductId (要移除購物車內的商品)
        public bool RemoveProduct(int Product_ID)
        {
            var findItem = this.cartItems
                           .Where(s => s.Product_ID == Product_ID)
                           .Select(s => s)
                           .FirstOrDefault();

            //判斷相同 Id 的 CartItem 是否已經存在購物車內
            if (findItem == default(Models.Carts.CartItem))
            {
                //不存在購物車內，不需要做任何動作
            }
            else
            {
                //存在購物車內，將商品移除
                this.cartItems.Remove(findItem);
            }

            return true;
        }


        //清空購物車
        public bool ClearCart()
        {
            this.cartItems.Clear();
            return true;
        }


        //將購物車中的商品轉成OrderDetail的List
        public List<Models.OrderDetail> ToOrderDetailList(int orderId)
        {
            var result = new List<Models.OrderDetail>();
            foreach (var cartItem in this.cartItems)
            {
                result.Add(new Models.OrderDetail()
                {
                            
                     Order_ID = orderId,         
                     Product_ID =  cartItem.Product_ID,
                     UnitPrice = cartItem.UnitPrice,
                     Quantity = cartItem.Stock

                });
            }
            return result;
        }


        #region IEnumerator (明確實作介面)
        IEnumerator<CartItem> IEnumerable<CartItem>.GetEnumerator()
        {
            return this.cartItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.cartItems.GetEnumerator();
        }

        #endregion
    }
}