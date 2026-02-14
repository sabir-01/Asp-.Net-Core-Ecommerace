using Microsoft.AspNetCore.Mvc;

namespace Ecommerace.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
