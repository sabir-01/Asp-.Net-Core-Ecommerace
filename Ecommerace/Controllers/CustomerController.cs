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
                cart.prod_id = prod_id;
                cart.cust_id = int.Parse(isLogin);
                cart.product_quantity = 1;
                cart.cart_status = 0;

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
        public IActionResult OrderSuccess()
        {
            List<Catagory> category = _context.tbl_Catagory.ToList();
            ViewData["category"] = category;
            return View();
        }

        public IActionResult Checkoutinformation()
        {
         
            return View();
        }   
        // Step 2: Confirm order (POST)
        [HttpPost]
        public IActionResult Checkoutinformation(int customerId)
        {
            var cartItems = _context.tbl_Carts
                .Where(c => c.cust_id == customerId && c.cart_status == 1)
                .Include(c => c.products)
                .Include(c => c.customers)
                .ToList();

            if (!cartItems.Any())
                return RedirectToAction("fetchChart");

            // Create order
            Order order = new Order
            {
                customer_id = customerId,
                shipping_address = cartItems.First().customers.customer_address,
                order_status = "Confirmed",
                order_date = DateTime.Now,
                total_amount = 0
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            decimal total = 0;

            foreach (var item in cartItems)
            {
                decimal price = Convert.ToDecimal(item.products.product_price);

                OrderDetails details = new OrderDetails
                {
                    order_id = order.order_id,
                    product_id = item.prod_id,
                    quantity = item.product_quantity,
                    price = price,
                    sub_total = price * item.product_quantity
                };

                total += details.sub_total;
                _context.OrderDetails.Add(details);
            }

            order.total_amount = total;

            _context.tbl_Carts.RemoveRange(cartItems);
            _context.SaveChanges();

            TempData["message"] = "Order Placed Successfully!";
            return RedirectToAction("OrderSuccess");
        }

        // Step : Checkout page
        public IActionResult Checkout()
        {
            string customerIdStr = HttpContext.Session.GetString("customerSession");
            if (string.IsNullOrEmpty(customerIdStr))
            {
                // Not logged in
                TempData["message"] = "Please login first to checkout!";
                return RedirectToAction("CustomerLogin");
            }

            int customerId = int.Parse(customerIdStr);

            // Get all cart items
            var cartItems = _context.tbl_Carts
                .Where(c => c.cust_id == customerId && c.cart_status == 1)
                .Include(c => c.products)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["message"] = "Your cart is empty!";
                return RedirectToAction("fetchChart");
            }

            // Calculate totals
            decimal grandTotal = 0;
            foreach (var item in cartItems)
            {
                decimal price = Convert.ToDecimal(item.products.product_price);
                grandTotal += price * item.product_quantity;
            }

            ViewBag.GrandTotal = grandTotal;

            // Shipping info
            var customer = _context.Customers.FirstOrDefault(c => c.customer_id == customerId);
            ViewBag.ShippingAddress = customer.customer_address;

            return View(cartItems); // pass cart items to Checkout page
        }


    }
}



