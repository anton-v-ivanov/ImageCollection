using ImageCollections.WebApi.Repositories;

namespace ImageCollections.WebApi.Managers.StorageFactory
{
    public interface IStorageFactory
    {
        IStorage Create(StorageType type);
    }
}
