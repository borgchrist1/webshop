using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webshop.Models;
using Dapper;
using System.Configuration;
using System.IO;

namespace webshop.Controllers
{
    public class AdminController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        [HttpGet]
        public ActionResult Index ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(MerchandiseModel item, HttpPostedFileBase productimg)
        {
            if (productimg != null)
            {
                var imgPath = Path.Combine(Server.MapPath("~/Content/img"), productimg.FileName);
                productimg.SaveAs(imgPath);
            }
            using (var connection = new SqlConnection(this.connectionString))
            {
                var query = "INSERT INTO Merchandise (Name, Dscription, Price, Productimg, Category) values (@Name, @Dscription, @Price, @Productimg, @Category)";
                var value = new { Name = item.Name, Dscription = item.Dscription, price = item.Price, Productimg = productimg.FileName, category = item.Category };
                connection.Execute(query, value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit (int id)
        {
            var item = new MerchandiseModel();
            using (var connection = new SqlConnection(this.connectionString))
            {
                var query = "SELECT * FROM merchandise WHERE Id = @Id";
                item = connection.QueryFirstOrDefault<MerchandiseModel>(query, new { id });
            }

            return View(item);
            
        }

        [HttpPost]
            public ActionResult Edit(MerchandiseModel item, HttpPostedFileBase productimg)
        {
            var imgPath = Path.Combine(Server.MapPath("~/Content/img"), productimg.FileName);
            productimg.SaveAs(imgPath);
            using (var connection = new SqlConnection(this.connectionString))
            {
                var query = "UPDATE Merchandise SET Name = @Name, Dscription = @Dscription, Price = @Price, Productimg = @Productimg WHERE Id = @Id";
                var value = new { Name = item.Name, Dscription = item.Dscription, price = item.Price, Productimg = productimg.FileName, category = item.Category, Id = item.Id };
                connection.Execute(query, value);
            }
            return View();
        }
    }
    }
