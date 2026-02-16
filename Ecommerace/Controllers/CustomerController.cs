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
            ViewBag.checksession = HttpContext.Session.GetString("customerSession");

            return View();
        }

        public IActionResult CustomerLogin()
        {           
            return View();
        }
        [HttpPost]
        public IActionResult CustomerLogin(string customer_username ,string customer_password)
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
    }

    
}
