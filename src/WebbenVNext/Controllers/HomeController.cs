using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebbenVNext.Models;
using WebbenVNext.Storage;

namespace WebbenVNext.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlobs _blobs;

        public HomeController(IBlobs blobs)
        {
            _blobs = blobs;
        }

        public async Task<IActionResult> Index()
        {
            var blobUrls = await _blobs.GetAllBlobUrls();
            var viewModel = new HomeViewModel(blobUrls);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ICollection<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    await _blobs.Save(file.FileName, file.OpenReadStream());
                }
            }

            return RedirectToAction("Index");
        }
    }
}

