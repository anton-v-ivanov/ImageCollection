using System.Collections.Generic;

namespace ImageCollections.WebApi.Models
{
    public class ImageCollection
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<string> Images { get; set; }

        public ImageCollection()
        {
            Images = new List<string>();
        }

        public ImageCollection(Contracts.ImageCollections.ImageCollection internaCollection)
        {
            Id = internaCollection.Id;
            Name = internaCollection.Name;
            Images = new List<string>();
        }
    }
}