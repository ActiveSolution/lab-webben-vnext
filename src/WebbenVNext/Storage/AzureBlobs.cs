using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WebbenVNext.Storage
{
    public class AzureBlobsOptions
    {
        public string StorageConnectionString { get; set; }
    }

    public class AzureBlobs : IBlobs
    {
        private readonly CloudBlobContainer _container;

        public AzureBlobs(IOptions<AzureBlobsOptions> options)
        {
            var storageAccount = CloudStorageAccount.Parse(options.Value.StorageConnectionString);
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