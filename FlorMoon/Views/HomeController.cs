using Microsoft.AspNetCore.Mvc;

namespace FlorMoon.Views
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
