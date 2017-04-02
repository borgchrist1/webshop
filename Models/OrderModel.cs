using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webshop.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string Products { get; set; }
        public int OrderNumber { get; set; }
    }
}