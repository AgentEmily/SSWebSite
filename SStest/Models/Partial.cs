using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SStest.Models
{
    public class Partial
    {
    }

    //定義 Models.Order的部分類別
    public partial class Orders
    {
        //取得訂單中的使用者暱稱
        public string GetUserName()
        {
            //使用 Order 類別中的 UserId 到 AspNetUsers 資料表中搜尋出 UserName
            using(Models.SmartShoppingEntities ss = new SmartShoppingEntities())
            {
                var result = (from m in ss.Members
                              where m.Member_ID == this.Member_ID
                              select m.MemberName).FirstOrDefault();

                return result;
            }
        }
    }
}