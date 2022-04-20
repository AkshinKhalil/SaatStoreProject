using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaatStoreProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 70)]
        public string Name { get; set; }
        public List<WatchCategory> WatchCategory { get; set; }
    }
}
