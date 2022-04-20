using SaatStoreProject.Models;
using System.Collections.Generic;

namespace SaatStoreProject.VidewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Watch> Watches { get; set; }
        public List<Category> Categories { get; set; }
        public List<Setting> Settings { get; set; }
        public List<Brand> Brands { get; set; }
    }
}
