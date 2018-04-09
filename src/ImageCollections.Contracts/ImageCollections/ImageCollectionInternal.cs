using System.Collections.Generic;

namespace ImageCollections.Contracts.ImageCollections
{
    public class ImageCollectionInternal
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<long> Images { get; set; }
    }
}