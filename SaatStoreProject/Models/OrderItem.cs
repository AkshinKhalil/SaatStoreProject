namespace SaatStoreProject.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int? WatchId { get; set; }
        public string AppUserId { get; set; }
        public int OrderId { get; set; }
        public Watch Watch { get; set; }
        public AppUser AppUser { get; set; }
        public Order Order { get; set; }
    }
}
