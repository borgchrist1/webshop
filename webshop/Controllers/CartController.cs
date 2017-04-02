using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webshop.Models;
using Dapper;
using System.Configuration;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace webshop.Controllers
{
    public class CartController : Controller
    {
        public ActionResult CartView()
        {
            var user = new UserViewModel();
            return View(user);
        }

        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        [HttpGet]
        public ActionResult GetCart()
        {
            return Json(this.Session["cart"], JsonRequestBehavior.AllowGet);
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
                    Merchandise_id = item.Id,
                    Product = item,
                    Count = 1
                };

                if (this.Session["cart"] != null)
                    cart = this.Session["cart"] as List<CartModel>;

                cart.Add(cartItem);
                this.Session["cart"] = cart;

                return Json(cart, JsonRequestBehavior.AllowGet);
            }

            //string url = this.Request.UrlReferrer.AbsolutePath;


        }

        public ActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThankYou(UserViewModel user)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                //add user
                var userQuery = "INSERT INTO Users (Email, Name, Street, Zip, City) values (@Email, @Name, @Street, @Zip, @City)";
                var userValues = new
                {
                    Email = user.Email,
                    Name = user.Name,
                    Street = user.Street,
                    Zip = user.Zip,
                    City = user.City
                };
                connection.Execute(userQuery, userValues);
                
                var uq = "SELECT * FROM Users WHERE Email = @Email";
                var uv = new { Email = user.Email };
                var lastUser = new UserViewModel();
                lastUser = connection.QueryFirstOrDefault<UserViewModel>(uq, uv);
                var userCart = this.Session["cart"] as List<CartModel>;
                string cartAsString = JsonConvert.SerializeObject(userCart);
                var createOrder = "Insert INTO Orders (Cart, User_id) values (@Cart, @User_id)";
                var orderValue = new { Cart = cartAsString, User_id = lastUser.Id };
                connection.Execute(createOrder, orderValue);
                this.Session["cart"] = null;
                return View(lastUser);
            }


        }
    }
}