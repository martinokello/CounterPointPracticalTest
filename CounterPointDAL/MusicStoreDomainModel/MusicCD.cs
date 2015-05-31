using System.ComponentModel.DataAnnotations;

namespace CounterPointDAL.MusicStoreDomainModel
{
    public class MusicCD
    {
        [Required(ErrorMessage = "Catalogue required")]
        public string CatalogueNumber { get; set; }
        [Required(ErrorMessage = "Album Title required")]
        public string AlbumTitle { get; set; }
        [Required(ErrorMessage = "Artist required")]
        public string Artist { get; set; }
        public int ProductId { get; set; }
    }
}
