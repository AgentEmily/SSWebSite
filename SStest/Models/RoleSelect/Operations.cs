using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SStest.Models;

namespace SStest.Models.RoleSelect
{
    public static class Operations
    {
        public static List<Categories> GetCategories(List<int> RolesID)//用角色抓出推薦產品category的函式
        {
            List<Categories> Categories = new List<Categories>();
            if (RolesID != null)
            {
                using (SmartShoppingEntities db = new SmartShoppingEntities())
                {
                    for (int i = 0; i < RolesID.Count; i++)
                    {

                        int id = RolesID[i];
                        //List<RolesDetail> rolesd = db.RolesDetail.Where(p => p.Roles_ID == RolesID[i]);
                        var q = from r in db.RolesDetail
                                where r.Roles_ID == id
                                select r;
                        foreach (var item in q)
                        {

                            Categories category = db.Categories.Where(c => c.Category_ID == item.Categories_ID).First();
                            if (!Categories.Contains(category))
                            {
                                Categories.Add(category);
                            }

                        }

                    }


                }

            }
            

            return Categories;
        }

        //public static List<Categories> GetList() { }
    }
}