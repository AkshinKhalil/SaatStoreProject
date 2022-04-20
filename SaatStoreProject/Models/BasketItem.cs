namespace SaatStoreProject.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int WatchId { get; set; }
        public string AppUserId { get; set; }
        public int Count { get; set; }
        public Watch Watch { get; set; }
        public AppUser AppUser { get; set; }
    }
}
