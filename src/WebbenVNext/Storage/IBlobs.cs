using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WebbenVNext.Storage
{
    public interface IBlobs
    {
        Task Save(string name, Stream fileStream);
        Task<IEnumerable<string>> GetAllBlobUrls();
    }

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

    public class AzureBlobs : IBlobs
    {
        private readonly CloudBlobContainer _container;

        public AzureBlobs(CloudStorageAccount storageAccount)
        {
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference("uploads");
        }

        public async Task Save(string name, Stream file)
        {
            await _container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, null, null);
            var blob = _container.GetBlockBlobReference(name);
            await blob.UploadFromStreamAsync(file);
        }

        public async Task<IEnumerable<string>> GetAllBlobUrls()
        {
            await _container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, null, null);
            var blobs = await _container.ListBlobsSegmentedAsync(null);
            return blobs.Results.Select(x => x.Uri.AbsoluteUri);
        }
    }
}
