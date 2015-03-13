using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SStest.Controllers
{
    public class MyShoppingList
    {
        public int id { get; set; }
        public string listname { get; set; }
        public string productname { get; set; }
         public int? repeatno { get; set; }
         public int? repeatby { get; set; }
         public DateTime lastdate { get; set; }
         public DateTime? nextdate { get; set; }
         public int? quantity { get; set; }


    }
}
