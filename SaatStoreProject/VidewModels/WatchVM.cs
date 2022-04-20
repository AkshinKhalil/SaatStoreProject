using SaatStoreProject.Models;
using System.Collections.Generic;

namespace SaatStoreProject.VidewModels
{
    public class WatchVM
    {
        public List<Watch> Watches { get; set; }
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
        public List<WatchCategory> WatchCategories { get; set; }
        public List<WatchImage> WatchImages { get; set; }
    }
}
