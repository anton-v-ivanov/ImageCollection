using System.Collections.Generic;

namespace ImageCollections.Contracts.ImageCollections
{
    public class ImageCollection
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<long> Images { get; set; }
    }
}