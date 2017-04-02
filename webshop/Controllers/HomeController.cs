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
            var user = new UserViewModel();
            return View(user);
        }

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
                   Merchandise_id = item.Id, Product = item, Count = 1 
                };

                if (this.Session["cart"] != null)
                    cart = this.Session["cart"] as List<CartModel>;

                    cart.Add(cartItem);         
                this.Session["cart"] = cart;

                return Json(cart, JsonRequestBehavior.AllowGet);
            }

            //string url = this.Request.UrlReferrer.AbsolutePath;
            

        }

        public ActionResult Checkout ()
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
                //var getId = "SELECT @@IDENTITY";
                ////var getId = "SELECT LAST_INSERT_ID()";
                //var id = connection.Execute(getId);
                ////take user_id and add a cartdb whith it
                //var insertCart = "INSERT INTO Cart (User_id) values (@User_id)";
                //var userIdValue = new { User_id = id };
                //connection.Execute(insertCart, userIdValue);
                //var getCartId = "SELECT @@IDENTITY";
                //var cartId = connection.Execute(getCartId);
                ////add cart id and [cart] to Cart_products
                //foreach (var product in this.Session["cart"] as List<CartModel>)
                //{
                //    var insertProduct = "INSERT INTO Cart_products (Name, Dscription, Price, Productimg, Category, Count, Cart_id) " +
                //        "values (@Name, @Dscription, @Price, @Productimg, @Category, @Count, @Cart_id)";
                //    var productValues = new
                //    {
                //        Name = product.Product.Name,
                //        Dscription = product.Product.Dscription,
                //        Price = product.Product.Price,
                //        Productimg = product.Product.Price,
                //        Category = product.Product.Category,
                //        Count = product.Count,
                //        Cart_id = cartId
                //    };

                //    connection.Execute(insertProduct, productValues);
                //}
                //add cart id to orderdb
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