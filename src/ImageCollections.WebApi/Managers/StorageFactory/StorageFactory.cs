using System;
using ImageCollections.WebApi.Repositories;
using ImageCollections.WebApi.Repositories.FileSystemStorage;

namespace ImageCollections.WebApi.Managers.StorageFactory
{
    internal class StorageFactory : IStorageFactory
    {
        private readonly IFileSystemStorage _fileSystemStorage;

        public StorageFactory(IFileSystemStorage fileSystemStorage)
        {
            _fileSystemStorage = fileSystemStorage;
        }

        public IStorage Create(StorageType type)
        {
            switch (type)
            {
                case StorageType.FileSystem:
                    return _fileSystemStorage;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}