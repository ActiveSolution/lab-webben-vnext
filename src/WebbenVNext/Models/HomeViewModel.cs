using System.Collections.Generic;

namespace WebbenVNext.Models
{
    public class HomeViewModel
    {
        public HomeViewModel(IEnumerable<string> blobUrls)
        {
            BlobUrls = blobUrls;
        }

        public IEnumerable<string> BlobUrls { get; }
    }
}
