using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webshop.Models
{
    public class MerchandiseModel
    {
        public int Id { get; set; }
        public string Name { get; set;  }
        public string Dscription { get; set;  }
        public int Price { get; set; }
        public string Productimg { get;  set; }
        public string Category { get; set; }
        private List<string> str = new List<string>();
    }

    public enum Category
    {
        Sport
    }
}