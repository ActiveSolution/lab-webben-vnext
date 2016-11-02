using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace WebbenVNext.Storage
{
    public class LocalBlobs : IBlobs
    {
        private const string FilesRootDirectoryName = "uploads";
        private readonly IHostingEnvironment _environment;

        public LocalBlobs(IHostingEnvironment environment)
        {
            this._environment = environment;
        }

        public async Task Save(string name, Stream file)
        {
            var filesFolder = Path.Combine(_environment.WebRootPath, FilesRootDirectoryName);
            if (!Directory.Exists(filesFolder))
            {
                Directory.CreateDirectory(filesFolder);
            }

            using (var fileStream = new FileStream(Path.Combine(filesFolder, name), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        public Task<IEnumerable<string>> GetAllBlobUrls()
        {
            var filesFolder = Path.Combine(_environment.WebRootPath, FilesRootDirectoryName);
            if (!Directory.Exists(filesFolder))
            {
                var empty = new string[] { };
                return Task.FromResult(empty.AsEnumerable());
            }

            var directoryInfo = new DirectoryInfo(filesFolder);
            var blobUrls = directoryInfo.GetFiles()
                .OrderByDescending(p => p.CreationTime)
                .Select(x => x.Name)
                .Select(GetUrl);

            return Task.FromResult(blobUrls);
        }

        private string GetUrl(string name)
        {
            var path = Path.Combine("/", FilesRootDirectoryName, name);
            var url = path.Replace(@"\", "/");

            return url;
        }
    }
}