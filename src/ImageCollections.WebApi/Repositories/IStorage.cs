using System.Threading.Tasks;

namespace ImageCollections.WebApi.Repositories
{
    public interface IStorage
    {
        Task<byte[]> Get(string contentHash);
        Task<string> Save(byte[] content, string contentHash);
    }
}