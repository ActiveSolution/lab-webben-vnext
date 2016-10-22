using Microsoft.AspNetCore.Mvc;

namespace WebbenVNext.Controllers
{
    public class FilesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
