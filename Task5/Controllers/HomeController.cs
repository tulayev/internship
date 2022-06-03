using Microsoft.AspNetCore.Mvc;

namespace Task5.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
