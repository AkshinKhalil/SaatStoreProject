using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaatStoreProject.Models
{
    public class Watch
    {    
        public int Id { get; set; }
        [Required]
        public double Price { get; set; }
        [StringLength(maximumLength: 90)]
        public string Glass { get; set; }
        [StringLength(maximumLength: 90)]
        public string Mechanism { get; set; }
        [StringLength(maximumLength: 90)]
        public string WaterProtection { get; set; }
        [StringLength(maximumLength: 90)]
        public string CaseThickness { get; set; }
        [StringLength(maximumLength: 90)]
        public string WatchModel { get; set; }
        public bool InStock { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public List<WatchCategory> WatchCategory { get; set; }
        public List<WatchImage> WatchImages{ get; set; }
        [NotMapped]
        public List<IFormFile> ImageFiles { get; set; }
        [NotMapped]
        public List<int> ImageIds { get; set; }
        [NotMapped]
        public List<int> CategoryIds { get; set; }
        [NotMapped]
        public List<int> BrandIds { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
