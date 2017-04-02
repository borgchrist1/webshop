using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webshop.Models
{
    public class CartModel
    {
       // public int Id { get; set; }
        public int Merchandise_id { get; set; }
        public MerchandiseModel Product { get; set; }
        public int Count { get; set; }
        
        
    }
}