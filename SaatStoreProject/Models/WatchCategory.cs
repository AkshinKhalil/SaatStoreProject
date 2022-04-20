namespace SaatStoreProject.Models
{
    public class WatchCategory
    {
        public int Id { get; set; }
        public int WatchId { get; set; }
        public int CategoryId { get; set; }
        public Watch Watch { get; set; }
        public Category Category { get; set; }
    }
}
