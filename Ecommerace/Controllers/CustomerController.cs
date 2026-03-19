using Ecommerace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerace.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Catagory> category = _context.tbl_Catagory.ToList();
            ViewData["category"] = category;

            List<Product> products = _context.tbl_Product.ToList();
            ViewData["products"] = products;
            ViewBag.checksession = HttpContext.Session.GetString("customerSession");

            return View();
        }

        public IActionResult CustomerLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CustomerLogin(string customer_username, string customer_password)
        {

            var customer = _context.Customers.FirstOrDefault(c => c.customer_email == customer_username);
            if (customer != null && customer.customer_password == customer_password)
            {
                HttpContext.Session.SetString("customerSession",
                    customer.customer_id.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.message = "Incorrect Username or Password";
                return View();
            }
        }

        public IActionResult CustomerRegistration()
        {
            return View();

        }

        [HttpPost]
        public IActionResult CustomerRegistration(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return RedirectToAction("CustomerLogin");
        }

        public IActionResult CustomerLogout()
        {
            HttpContext.Session.Remove("customerSession");
            return RedirectToAction("index");
        }

        public IActionResult CustomerProfile()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerSession")))
            {
                return RedirectToAction("CustomerLogin");
            }
            else
            {
                List<Catagory> category = _context.tbl_Catagory.ToList();
                ViewData["category"] = category;
                var customerid = HttpContext.Session.GetString("customerSession");
                var row = _context.Customers.Where(a => a.customer_id == int.Parse(customerid)).ToList();


                return View(row);
            }

        }

        public IActionResult feedback()
        {
            List<Catagory> category = _context.tbl_Catagory.ToList();
            ViewData["category"] = category;
            return View();
        }

        [HttpPost]
        public IActionResult feedback(Feedback feedback)
        {

            TempData["message"] = "Thank You For Your Feedback !";
            _context.tbl_Feedback.Add(feedback);
            _context.SaveChanges();
            return RedirectToAction("feedback");
        }

        public IActionResult ProductDetail(int id)
        {
            List<Catagory> category = _context.tbl_Catagory.ToList();
            ViewData["category"] = category;
            var product = _context.tbl_Product.Where(p => p.product_id == id).ToList();
            return View(product);
        }

        public IActionResult fetchAllProducts()
        {
            List<Catagory> category = _context.tbl_Catagory.ToList();
            ViewData["category"] = category;

            List<Product> products = _context.tbl_Product.ToList();
            ViewData["product"] = products;

            return View();
        }

        public IActionResult AddToCart(int prod_id, Carts cart)
        {

            string isLogin = HttpContext.Session.GetString("customerSession");
            if (isLogin != null)
            {
                 int custId = int.Parse(isLogin);
                cart.prod_id = prod_id;
                cart.cust_id = int.Parse(isLogin);
                cart.product_quantity = 1;
                cart.cart_status = 1;

                _context.tbl_Carts.Add(cart);
                _context.SaveChanges();

                TempData["message"] = "Product Successfully Added in Cart";
                return RedirectToAction("fetchAllProducts");
            }
            else
            {
                return RedirectToAction("CustomerLogin");
            }
        }

        public IActionResult fetchChart()
        {

            List<Catagory> category = _context.tbl_Catagory.ToList();
            ViewData["category"] = category;

            string customerId = HttpContext.Session.GetString("customerSession");
            if (customerId != null)
            {
                var cart = _context.tbl_Carts.Where(c => c.cust_id == int.Parse(customerId)).Include(c => c.products).ToList();
                return View(cart);
            }
            else
            {
                return RedirectToAction("customerLogin");
            }
        }

        public IActionResult removeProduct(int id)
        {
            var product = _context.tbl_Carts.Find(id);
            _context.tbl_Carts.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("fetchChart");
        }

        public IActionResult IncreaseQty(int id)
        {
            var cart = _context.tbl_Carts.FirstOrDefault(c => c.cart_id == id);

            if (cart != null)
            {
                cart.product_quantity += 1;
                _context.SaveChanges();
            }

            return RedirectToAction("fetchChart");
        }
        public IActionResult DecreaseQty(int id)
        {
            var cart = _context.tbl_Carts.FirstOrDefault(c => c.cart_id == id);

            if (cart != null)
            {
                // Quantity 1 se kam nahi hone deni
                if (cart.product_quantity > 1)
                {
                    cart.product_quantity -= 1;
                    _context.SaveChanges();
                }
                // Agar already 1 hai to kuch nahi karein (no delete)
            }

            return RedirectToAction("fetchChart");
        }
       
        public IActionResult AboutUs()
        {
            List<Catagory> category = _context.tbl_Catagory.ToList();
            ViewData["category"] = category;
            return View();
        }

        // GET - Checkout page dikhao with cart data
        [HttpPost]
        [HttpPost]
        public IActionResult Checkoutinformation()
        {

            List<Catagory> category = _context.tbl_Catagory.ToList();
            ViewData["category"] = category;
            string customerId = HttpContext.Session.GetString("customerSession");
            if (customerId == null)
            {
                return RedirectToAction("customerLogin");
            }

            int custId = int.Parse(customerId);

            // Get customer details
            var customer = _context.Customers.FirstOrDefault(c => c.customer_id == custId);

            // Get cart items
            var cartItems = _context.tbl_Carts
                .Where(c => c.cust_id == custId)
                .Include(c => c.products)
                .ToList();

            if (cartItems.Count == 0)
            {
                TempData["error"] = "Your cart is empty!";
                return RedirectToAction("fetchChart");
            }

            // Calculate total amount safely
            var totalAmount = cartItems.Sum(c =>
            {
                decimal price = 0m;
                decimal.TryParse(c.products.product_price, out price);
                return price * c.product_quantity;
            });

            // Create ViewModel to pass data to view
            var viewModel = new CheckoutViewModel
            {
                Customer = customer,
                CartItems = cartItems,
                TotalAmount = totalAmount,
                OrderDate = DateTime.Now
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult PlaceOrder(CheckoutViewModel model)
        {

            List<Catagory> category = _context.tbl_Catagory.ToList();
            ViewData["category"] = category;
            string customerId = HttpContext.Session.GetString("customerSession");
            if (customerId == null)
            {
                return RedirectToAction("customerLogin");
            }

            int custId = int.Parse(customerId);

            // Get cart items
            var cartItems = _context.tbl_Carts
                .Where(c => c.cust_id == custId)
                .Include(c => c.products)
                .ToList();

            if (cartItems.Count == 0)
            {
                TempData["error"] = "Your cart is empty!";
                return RedirectToAction("fetchChart");
            }

            // Calculate total amount
            decimal totalAmount = cartItems.Sum(c =>
            {
                decimal price = 0;
                decimal.TryParse(c.products.product_price, out price); // safe conversion
                return price * c.product_quantity;
            });
            // Create Order
            Order order = new Order
            {
                customer_id = custId,
                order_date = DateTime.Now,
                order_status = "Pending",
                shipping_address = model.ShippingAddress,
                total_amount = totalAmount
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Create Order Details for each cart item
            foreach (var item in cartItems)
            {
                OrderDetails orderDetail = new OrderDetails
                {
                    order_id = order.order_id,
                    product_id = item.prod_id,
                    quantity = item.product_quantity,
                    price = decimal.Parse(item.products.product_price),
                    sub_total = decimal.Parse(item.products.product_price) * item.product_quantity
            };

                _context.OrderDetails.Add(orderDetail);
            }

            // Clear the cart
            _context.tbl_Carts.RemoveRange(cartItems);

            _context.SaveChanges();

            TempData["success"] = "Order placed successfully! Order ID: " + order.order_id;
            return RedirectToAction("OrderConfirmation", new { id = order.order_id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            var order = _context.Orders
                .Include(o => o.customer)
                .Include(o => o.orderDetails)
                    .ThenInclude(od => od.product)
                .FirstOrDefault(o => o.order_id == id);

            if (order == null)
            {
                return RedirectToAction("fetchChart");
            }

            return View(order);
        }
    }
}



