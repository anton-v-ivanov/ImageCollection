using System.Collections.Generic;
using ImageCollections.WebApi.Repositories;

namespace ImageCollections.WebApi.Configuration
{
    public class FileRepositorySetting
    {
        public int MaxFileLengthMb { get; set; }
        public StorageType ActiveStorage { get; set; }
        public StorageSetting[] Storages { get; set; }

        public class StorageSetting
        {
            public StorageType Type { get; set; }
            public Dictionary<string, string> Args { get; set; }
        }
    }
}
