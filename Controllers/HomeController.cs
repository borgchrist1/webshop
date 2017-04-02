using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Configuration;
using webshop.Models;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace webshop.Controllers
{
    public class HomeController : Controller
    {
        

        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public ActionResult Index()
        {
            
            List<MerchandiseModel> merchandise;
            //List<MerchandiseModel> sport;
            using (var connection = new SqlConnection(this.connectionString))
            {
                var query = "SELECT * FROM merchandise WHERE Category = 'Sport'";
                merchandise = connection.Query<MerchandiseModel>(query).ToList();
                //sport = merchandise.Where(i => i.Category == "Sport").ToList();
            }
            return View(merchandise);
        }


        public ActionResult Details(int id)
        {
            var item = new MerchandiseModel();
            using (var connection = new SqlConnection(this.connectionString))
            {
                var query = "SELECT * FROM merchandise WHERE Id = @Id";
                item = connection.QueryFirstOrDefault<MerchandiseModel>(query, new { id });
            }

            return View(item);
        }

        public ActionResult CartView ()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddToCart(int id)
        {
            
            var cart = new List<CartModel>();
            using (var connection = new SqlConnection(this.connectionString))
            {
                var query = "SELECT * FROM merchandise WHERE Id = @Id";
                var item = connection.QueryFirstOrDefault<MerchandiseModel>(query, new { id });
                var cartItem = new CartModel
                {
                    Merchandise_id = id, Product = item, count = 1
                };
                if (this.Session["cart"] != null)
                    cart = this.Session["cart"] as List<CartModel>;

                cart.Add(cartItem);
                this.Session["cart"] = cart;
                //string bCart = JsonConvert.SerializeObject(nCart);
                //var bb = new List<CartModel>();
                //var nn = JsonConvert.DeserializeObject(bCart);
                return Json(cartItem, JsonRequestBehavior.AllowGet);
            }

            
            

        }

        public ActionResult GetCart ()
        {

            return Json(this.Session["cart"], JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}