using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SStest.Models
{
    public class ProductModels
    {
        [Key]
        public int Product_ID { get; set; }

        [Required]
        [StringLength(15)]
        public string ProductName { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [Column(TypeName = "image")]
        public byte[] Picture { get; set; }
        
        [Column(TypeName = "ntext1")]
        public decimal Stock { get; set; }

        [Column(TypeName = "price")]
        public decimal Price { get; set; }

 
    }


    }
   
