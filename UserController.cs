using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shoppng.Models.Entity;
using Online_Shoppng.Models.Repository;
using Online_Shoppng.Models.Services;
using Online_Shoppng.Models.ViewModels;
using System.Drawing;

namespace Online_Shoppng.Controllers
{
    public class UserController : Controller
    {
        private IfCategory _ifCategory;
        private IfProducts _ifProducts;
        private IfOrder _ifOrder;
        private IfCart _ifCart;
        private Iflogin _iflogin;
        public UserController(IfCategory ifCategory, IfProducts ifProducts, IfOrder ifOrder, IfCart ifCart, Iflogin iflogin)
        {
            _ifCategory = ifCategory;
            _ifProducts = ifProducts;
            _ifOrder = ifOrder;
            _ifCart = ifCart;
            _iflogin = iflogin;
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Categories()
        {
            return View();
        }
        public IActionResult BuyProduct() 
        {
            return View();
        }
        public IActionResult AddtoCart()
        {
            return View();
        }
        public IActionResult BuyNow()
        {
            return View();
        }
        public IActionResult Order()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult SetCategory(int categoryId)
        {
            TempData["SelectedCategoryId"] = categoryId; // Or use Session if needed
            return Ok();
        }
        public IActionResult Products()
        {
            var selectedCategoryId = TempData["SelectedCategoryId"];
            ViewBag.SelectedCategoryId = selectedCategoryId;
            return View();
        }

        [HttpGet]
        public JsonResult ShowCategories()
        {
            var categories = _ifCategory.GetAllCategories().Select(c => new
            {
                c.CategoryId,
                c.CategoryName,
                c.Categoryimage,  // Assuming image is stored as a path (string)
                c.IsActive
            }).ToList();

            return Json(categories);
        }
        [HttpGet]
        public JsonResult ShowProducts(int? categoryId)
        {
            var products = _ifProducts.GetProducts().AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            var result = products.Select(p => new {
                p.ProductName,
                p.ProductPrice,
                p.ProductDescription,
                p.ProductDiscount,
                p.BrandName,
                p.Productimage1,
                p.Productimage2,
                p.Productimage3,
                p.ProductId
            }).ToList();

            return Json(result);
        }
        [HttpGet]
        public JsonResult GetProductDetails(int productId)
        {
            var product = _ifProducts.SearchProduct(productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }
            var category = _ifCategory.GetAllCategories().FirstOrDefault(c => c.CategoryId == product.CategoryId);
            var categoryName = category?.CategoryName ?? "Unknown";

            return Json(new
            {
                success = true,
                data = new
                {

                    product.ProductId,
                    product.ProductName,
                    product.ProductDescription,
                    product.ProductPrice,
                    product.ProductDiscount,
                    product.BrandName,
                    product.Productimage1,
                    product.Productimage2,
                    product.Productimage3,
                    product.CategoryId,
                    CategoryName = categoryName   


                }
            });
        }
        [HttpGet]
        public IActionResult PlaceOrder(int productId)
        {
            var product = _ifProducts.SearchProduct(productId);

            if (product == null)
            {
                return RedirectToAction("Products");
            }

            Order order = new Order
            {
                ProductId = product.ProductId,
                Product = product,
                Price = (decimal)product.ProductPrice,
                Quantity = 1,
                TotalAmount = (decimal)product.ProductPrice,
                OrderDate = DateTime.Now
            };

            return View("Order", order); // Order.cshtml
        }
        [HttpPost]
        public JsonResult ConfirmOrder(Order order)
        {
            // 🔑 Username from session
            order.Username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(order.Username))
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            order.OrderDate = DateTime.Now;
            order.OrderStatus = "Placed";
            order.PaymentStatus = "Pending";
            order.TrackingNumber = "TRK" + DateTime.Now.Ticks;
            order.TotalAmount = order.Price * order.Quantity;

            int result = _ifOrder.AddOrder(order);

            if (result > 0)
                return Json(new { success = true });

            return Json(new { success = false, message = "Order failed" });
        }
        [HttpPost]
        public JsonResult ConfirmCartOrder(string ShippingAddress, string PhoneNumber, string PaymentMethod)
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
                return Json(new { redirect = "/Login/Login" });

            // 🔹 STEP 1: Check Buy Now product
            int? buyNowProductId = HttpContext.Session.GetInt32("BuyNowProductId");

            // =========================
            // ⚡ BUY NOW ORDER
            // =========================
            if (buyNowProductId.HasValue)
            {
                var product = _ifProducts.SearchProduct(buyNowProductId.Value);

                if (product == null)
                    return Json(new { success = false, message = "Product not found" });

                decimal finalPrice =
                    (decimal)(product.ProductPrice -
                    (product.ProductPrice * (product.ProductDiscount / 100)));

                OrdersMaster master = new OrdersMaster
                {
                    Username = username,
                    OrderDate = DateTime.Now,
                    GrandTotal = finalPrice,
                    ShippingAddress = ShippingAddress,
                    PhoneNumber = PhoneNumber,
                    PaymentMethod = PaymentMethod,
                    PaymentStatus = "Pending",
                    OrderStatus = "Placed"
                };

                var orders = new List<Order>
        {
            new Order
            {
                Username = username,
                ProductId = product.ProductId,
                Quantity = 1,
                Price = finalPrice,
                TotalAmount = finalPrice,
                OrderDate = master.OrderDate,
                PaymentMethod = PaymentMethod,
                PaymentStatus = "Pending",
                OrderStatus = "Placed",
                ShippingAddress = ShippingAddress,
                PhoneNumber = PhoneNumber
            }
        };

                _ifOrder.AddCartOrder(master, orders);

                // 🔥 Clear Buy Now session
                HttpContext.Session.Remove("BuyNowProductId");

                return Json(new { success = true });
            }

            // =========================
            // 🛒 CART ORDER
            // =========================
            var cartItems = _ifCart.GetCartByUsername(username);

            if (!cartItems.Any())
                return Json(new { success = false, message = "Cart is empty" });

            OrdersMaster cartMaster = new OrdersMaster
            {
                Username = username,
                OrderDate = DateTime.Now,
                GrandTotal = cartItems.Sum(c => c.TotalAmount),
                ShippingAddress = ShippingAddress,
                PhoneNumber = PhoneNumber,
                PaymentMethod = PaymentMethod,
                PaymentStatus = "Pending",
                OrderStatus = "Placed"
            };

            var cartOrders = cartItems.Select(c => new Order
            {
                Username = username,
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                Price = c.Price,
                TotalAmount = c.TotalAmount,
                OrderDate = cartMaster.OrderDate,
                PaymentMethod = PaymentMethod,
                PaymentStatus = "Pending",
                OrderStatus = "Placed",
                ShippingAddress = ShippingAddress,
                PhoneNumber = PhoneNumber
            }).ToList();

            _ifOrder.AddCartOrder(cartMaster, cartOrders);

            // 🔥 Clear cart after order
            _ifCart.ClearCart(username);

            return Json(new { success = true });
        }


        [HttpPost]
        public JsonResult AddToCart(int productId, int quantity, string size)
            {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
                return Json(new { redirect = "/Login/Login" });

            var product = _ifProducts.SearchProduct(productId);

            decimal finalPrice =
                (decimal)(product.ProductPrice -
                (product.ProductPrice * (product.ProductDiscount / 100)));

            Cart cart = new Cart
            {
                Username = username,
                ProductId = product.ProductId,
                Quantity = quantity,
                Size = size,          // ✅ SAVE SIZE
                Price = finalPrice,
                TotalAmount = finalPrice * quantity,
                AddedOn = DateTime.Now
            };

            _ifCart.AddToCart(cart);

            return Json(new { success = true });
        }


        [HttpGet]
        public JsonResult GetOrderSummary()
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
                return Json(new { redirect = "/Login/Login" });

            // 🔹 1️⃣ CHECK BUY NOW
            int? buyNowProductId = HttpContext.Session.GetInt32("BuyNowProductId");

            if (buyNowProductId.HasValue)
            {
                var product = _ifProducts.SearchProduct(buyNowProductId.Value);

                if (product == null)
                    return Json(new List<object>());

                decimal priceAfterDiscount =
                    (decimal)(product.ProductPrice -
                    (product.ProductPrice * (product.ProductDiscount / 100)));

                return Json(new[]
                {
            new
            {
                name = product.ProductName,
                quantity = 1,
                price = priceAfterDiscount,
                totalAmount = priceAfterDiscount
            }
        });
            }

            // 🔹 2️⃣ ELSE → CART ORDER
            var cartItems = _ifCart.GetCartByUsername(username)
                .Select(c => new
                {
                    name = c.Product.ProductName,
                    quantity = c.Quantity,
                    price = c.Price,
                    totalAmount = c.TotalAmount
                }).ToList();

            return Json(cartItems);
        }

        [HttpPost]
        public JsonResult RemoveFromCart(int cartId)
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return Json(new { success = false, redirect = "/Login/Login" });
            }

            _ifCart.RemoveFromCart(cartId);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult UpdateCartQuantity(int cartId, int quantity)
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return Json(new { success = false, redirect = "/Login/Login" });
            }

            _ifCart.UpdateQuantity(cartId, quantity);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult ClearCart()
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return Json(new
                {
                    success = false,
                    redirect = "/Login/Login"
                });
            }

            _ifCart.ClearCart(username);

            return Json(new { success = true });
        }
        [HttpGet]
        public IActionResult BuyNow(int productId)
        {
            HttpContext.Session.SetInt32("BuyNowProductId", productId);
            return RedirectToAction("Order");
        }
        [HttpGet]
        public JsonResult GetCartItems()
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
                return Json(new { redirect = "/Login/Login" });

            var cartItems = _ifCart.GetCartByUsername(username)
    .Select(c => new
    {
        cartId = c.CartId,
        name = c.Product.ProductName,
        image = c.Product.Productimage1,
        price = c.Price,
        quantity = c.Quantity,
        size = c.Size,            // ✅ RETURN SIZE
        totalAmount = c.TotalAmount
    }).ToList();


            return Json(cartItems);
        }
        [HttpGet]
        public JsonResult GetProfileData()
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
                return Json(new { redirect = "/Login/Login" });

            var user = _iflogin.GetUserByUsername(username);

            var orders = _ifOrder.GetOrdersByUsername(username)
                .Select(o => new
                {
                    orderId = o.OrderMasterId,
                    orderDate = o.OrderDate.ToShortDateString(),
                    total = o.GrandTotal,
                    status = o.OrderStatus
                }).ToList();

            return Json(new
            {
                username = user.Username,
                phone = user.PhoneNumber,
                email = user.Email,
                image = string.IsNullOrEmpty(user.ProfileImage)
                            ? "/images/default-user.png"
                            : user.ProfileImage,
                orders = orders
            });
        }
        [HttpPost]
        public JsonResult UpdateProfileImage(IFormFile profileImage)
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
                return Json(new { redirect = "/Login/Login" });

            if (profileImage != null && profileImage.Length > 0)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/users");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
                string path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    profileImage.CopyTo(stream);
                }

                _iflogin.UpdateProfileImage(username, "/images/users/" + fileName);
            }

            return Json(new { success = true });
        }
        [HttpGet]
        public JsonResult GetOrderItems(int orderMasterId)
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
                return Json(new { redirect = "/Login/Login" });

            var items = _ifOrder
                .GetOrderItemsByOrderMasterId(orderMasterId, username)
                .Select(o => new
                {productId = o.ProductId,
                    productName = o.Product.ProductName,
                    quantity = o.Quantity,
                    price = o.Price,
                    total = o.TotalAmount
                }).ToList();

            return Json(items);
        }



    }
}
