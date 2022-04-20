namespace SaatStoreProject.Models
{
    public class WatchImage
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public bool isMain { get; set; }
        public int WatchId { get; set; }
        public Watch Watch { get; set; }
    }
}
