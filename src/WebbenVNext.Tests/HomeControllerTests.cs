using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebbenVNext.Controllers;
using WebbenVNext.Models;
using WebbenVNext.Storage;

namespace WebbenVNext.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public async Task HomeIndexSholdReturnFilesFromBlobProvider()
        {
            var blobUrls = new[] { "/file1.png", "/file2.jpg" };
            var mockedBlob = new Mock<IBlobs>();
            mockedBlob
                .Setup(x => x.GetAllBlobUrls())
                .Returns(Task.FromResult(blobUrls.AsEnumerable()));

            var homeController = new HomeController(mockedBlob.Object);
            var homeIndexViewResult = await homeController.Index() as ViewResult;
            var homeIndexViewModel = homeIndexViewResult?.Model as HomeViewModel;
            var homeIndexViewModelBlobUrls = homeIndexViewModel?.BlobUrls.ToList();

            Assert.IsNotNull(homeIndexViewModelBlobUrls);
            Assert.AreEqual(blobUrls[0], homeIndexViewModelBlobUrls?[0]);
            Assert.AreEqual(blobUrls[1], homeIndexViewModelBlobUrls?[1]);
        }
    }
}
