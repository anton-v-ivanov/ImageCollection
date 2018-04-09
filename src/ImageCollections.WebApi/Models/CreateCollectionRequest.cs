using System.ComponentModel.DataAnnotations;

namespace ImageCollections.WebApi.Models
{
    public class CreateCollectionRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
