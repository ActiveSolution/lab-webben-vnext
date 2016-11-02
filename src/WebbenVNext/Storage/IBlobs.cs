using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WebbenVNext.Storage
{
    public interface IBlobs
    {
        Task Save(string name, Stream fileStream);
        Task<IEnumerable<string>> GetAllBlobUrls();
    }
}
