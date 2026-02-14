using Ecommerace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerace.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private IWebHostEnvironment _env;
        public AdminController(ApplicationDbContext context , IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            string admin_session = HttpContext.Session.GetString("admin_session");
            if (admin_session != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string adminEmail ,string adminPassword)
        {

            var row = _context.tbl_admin.FirstOrDefault(a => a.admin_email == adminEmail);
            if (row != null && row.admin_password == adminPassword)
            {
                HttpContext.Session.SetString("admin_session", row.admin_id.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.message = "Incorrect Username or Password";
                return View();
            }
        }

        public IActionResult logout()
        {
            HttpContext.Session.Remove("admin_session");
            return RedirectToAction("login");
        }

        public IActionResult Profile()
        {
            var adminId = HttpContext.Session.GetString("admin_session");
            var row = _context.tbl_admin.FirstOrDefault(a => a.admin_id == int.Parse(adminId));
            return View(row);
        }

        [HttpPost]
        public IActionResult Profile(Admin admin)
        {
            _context.tbl_admin.Update(admin);
            _context.SaveChanges();
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult changeProfile(IFormFile admin_image, Admin admin)
        {

            string ImagePath = Path.Combine(_env.WebRootPath, "admin_image", admin_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            admin_image.CopyTo(fs);
            admin.admin_image = admin_image.FileName;
            _context.tbl_admin.Update(admin);
            _context.SaveChanges();
            return RedirectToAction("Profile");
        }


        public IActionResult fetchCustomer()
        {

            return View(_context.Customers.ToList());
        }

        public IActionResult Detailcustomer(int id)
        {

            return View(_context.Customers.FirstOrDefault(c => c.customer_id == id));
        }  
        
        public IActionResult Updatecustomer(int id)                
        {

            return View(_context.Customers.Find(id));
        }
        [HttpPost]
        public IActionResult Updatecustomer(Customer customer, IFormFile customer_image)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "customer_images", customer_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            customer_image.CopyTo(fs);
            customer.customer_image = customer_image.FileName;
            _context.Customers.Update(customer);
            _context.SaveChanges();
            return RedirectToAction("fetchCustomer");
        }

        public IActionResult Deletecustomer(int id)                
        {
            var customer = _context.Customers.Find(id);
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return RedirectToAction("fetchCustomer");
        }
        public IActionResult deletePermission(int id)
        {
            return View(_context.Customers.FirstOrDefault(c => c.customer_id == id));
        }

        public IActionResult fetchCategory()
        {
            return View(_context.tbl_Catagory.ToList());
        }

        public IActionResult addCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult addCategory(Catagory cat)
        {
            _context.tbl_Catagory.Add(cat);
            _context.SaveChanges();
            return RedirectToAction("fetchCategory");
        }

        public IActionResult updateCategory(int id)
        {
            var category = _context.tbl_Catagory.Find(id);
            return View(category); 
        }

        [HttpPost]
        public IActionResult updateCategory(Catagory cat)
        {
            _context.tbl_Catagory.Update(cat);
            _context.SaveChanges();
            return RedirectToAction("fetchCategory");
        }


        public IActionResult deletePermissionCategory(int id)
        {
            return View(_context.tbl_Catagory.FirstOrDefault(c => c.category_id == id));
        }
        public IActionResult deletecategory(int id)
        {
            var category = _context.tbl_Catagory.Find(id);
            _context.tbl_Catagory.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("fetchCategory");
        }

        public IActionResult fetchProduct()
        {
            return View(_context.tbl_Product.ToList());
        }

        public IActionResult addProduct()
        {
            List<Catagory> catagories = _context.tbl_Catagory.ToList();
            ViewData["category"] = catagories;
            return View();
        }

        [HttpPost]
        public IActionResult addProduct(Product prod ,IFormFile product_image)
        {
            string imageName = Path.GetFileName(product_image.FileName);
            string imagePath = Path.Combine(_env.WebRootPath, "product_images", imageName);
            FileStream fs = new FileStream(imagePath, FileMode.Create);
            product_image.CopyTo(fs);
            prod.product_image = imageName;
            _context.tbl_Product.Add(prod);
            _context.SaveChanges();
            return RedirectToAction("fetchproduct");
        }

        public IActionResult productDetails(int id)
        {

            return View(_context.tbl_Product.Include(p => p.catagory).FirstOrDefault(p => p.product_id == id));
        }

        public IActionResult deletePermissionProduct(int id)
        {
            return View(_context.tbl_Product.FirstOrDefault(c => c.product_id == id));
        }
        public IActionResult deleteproduct(int id)
        {
            var product = _context.tbl_Product.Find(id);
            _context.tbl_Product.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("fetchproduct");
        }
        public IActionResult updateproduct(int id)
        {
            List<Catagory> catagories = _context.tbl_Catagory.ToList();
            ViewData["category"] = catagories;
            var product = _context.tbl_Product.Find(id);
            ViewBag.selectedCategoryId = product.cat_id; 
            return View(product);
        }

        [HttpPost]
        public IActionResult updateproduct(Product prod)
        {
            _context.tbl_Product.Update(prod);
            _context.SaveChanges();
            return RedirectToAction("fetchproduct");
        }

        [HttpPost]
        public IActionResult changeProductImage(IFormFile product_image, Product product)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "product_images", product_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            product_image.CopyTo(fs);
            product.product_image = product_image.FileName;
            _context.tbl_Product.Update(product);
            _context.SaveChanges();
            return RedirectToAction("fetchproduct");
        }

    }
}
