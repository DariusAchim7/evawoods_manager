using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtelierTamplarie.Models
{
    [Table("stoc")]
    public class Stoc
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nume_produs")]
        [MaxLength(255)]
        public string NumeProdus { get; set; } = string.Empty;

        [Column("categorie")]
        [MaxLength(100)]
        public string? Categorie { get; set; }

        [Column("unitate_masura")]
        [MaxLength(20)]
        public string UnitateMasura { get; set; } = "buc";

        [Column("cantitate")]
        [Precision(10, 2)]
        public decimal Cantitate { get; set; } = 0;

        [Column("pret_unitar")]
        [Precision(10, 2)]
        public decimal? PretUnitar { get; set; }

        [Column("furnizor")]
        [MaxLength(255)]
        public string? Furnizor { get; set; }

        [Column("locatie_depozit")]
        [MaxLength(255)]
        public string? LocatieDepozit { get; set; }

        [Column("observatii")]
        public string? Observatii { get; set; }

        [Column("data_creare")]
        public DateTime DataCreare { get; set; } = DateTime.UtcNow;

        [Column("data_actualizare")]
        public DateTime DataActualizare { get; set; } = DateTime.UtcNow;

        // Relație
        public ICollection<MiscareStoc>? MiscariStoc { get; set; }
    }
}