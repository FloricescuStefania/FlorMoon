using FlorMoon.Models;
using FlorMoon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlorMoon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        public IActionResult Index()
        {
            var products = _productService.GetAllProducts().Take(3).ToList();
            return View(products);
        }

        [Route("Home/About")]
        public IActionResult About() => View();

        [Route("Home/Contact")]
        public IActionResult Contact() => View();

        [Route("Home/FAQ")]
        public IActionResult FAQ() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}