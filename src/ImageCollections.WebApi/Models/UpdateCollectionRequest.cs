using System.ComponentModel.DataAnnotations;

namespace ImageCollections.WebApi.Models
{
    public class UpdateCollectionRequest
    {
        [Required]
        public string Name { get; set; }
    }
}