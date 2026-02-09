using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtelierTamplarie.Models
{
    [Table("imagini_proiect")]
    public class ImaginiProiect
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("proiect_id")]
        public int ProiectId { get; set; }

        [Required]
        [Column("tip_imagine")]
        [MaxLength(50)]
        public string TipImagine { get; set; } = string.Empty; // schita, randare, final

        [Required]
        [Column("nume_fisier")]
        [MaxLength(255)]
        public string NumeFisier { get; set; } = string.Empty;

        [Required]
        [Column("cale_fisier")]
        public string CaleFisier { get; set; } = string.Empty;

        [Column("descriere")]
        public string? Descriere { get; set; }

        [Column("data_upload")]
        public DateTime DataUpload { get; set; } = DateTime.UtcNow;

        [Column("ordine")]
        public int Ordine { get; set; } = 0; // Pentru sortare

        // Relație
        [ForeignKey("ProiectId")]
        public Proiect? Proiect { get; set; }
    }
}