using System.ComponentModel.DataAnnotations;

namespace SaatStoreProject.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 150)]
        public string Logo { get; set; }
        [Required]
        public string SearchIcon { get; set; }
        [Required]
        public string NumberIcon { get; set; }
        [Required]
        public string BasketIcon { get; set; }
        public string LoginIcon { get; set; }
        [StringLength(maximumLength: 150)]
        public int Connectnumber { get; set; }
        [StringLength(maximumLength: 150)]
        public string TwitIcon { get; set; }
        [StringLength(maximumLength: 150)]
        public string FacebookIcon { get; set; }
        [StringLength(maximumLength: 150)]
        public string InstagramIcon { get; set; }
        [StringLength(maximumLength: 250)]
        public string VideoURL { get; set; }

        //info.html
        [StringLength(maximumLength: 350)]
        public string InfoTxt1 { get; set; }
        [StringLength(maximumLength: 350)]
        public string InfoTxt2 { get; set; }
        [StringLength(maximumLength: 350)]
        public string InfoTxt3 { get; set; }
        [StringLength(maximumLength: 250)]
        public string InfoVideoURL { get; set; }

    }
}
