using Microsoft.AspNetCore.Mvc;

namespace WebbenVNext.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
