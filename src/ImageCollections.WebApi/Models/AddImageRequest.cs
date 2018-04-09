﻿using System.ComponentModel.DataAnnotations;

namespace ImageCollections.WebApi.Models
{
    public class AddImageToCollectionRequest
    {
        [Required]
        public long ImageId { get; set; }
    }
}