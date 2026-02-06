using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AtelierTamplarie.Models
{
    [Table("cheltuieli")]
    public class Cheltuiala
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("proiect_id")]
        public int ProiectId { get; set; }

        [Required]
        [Column("tip_cheltuiala")]
        [MaxLength(50)]
        public string TipCheltuiala { get; set; } = string.Empty;

        [Required]
        [Column("descriere")]
        public string Descriere { get; set; } = string.Empty;

        [Required]
        [Column("suma")]
        [Precision(10, 2)]
        public decimal Suma { get; set; }

        [Column("data_cheltuiala")]
        public DateTime DataCheltuiala { get; set; } = DateTime.Now;

        [Column("observatii")]
        public string? Observatii { get; set; }

        [Column("data_creare")]
        public DateTime DataCreare { get; set; } = DateTime.Now;

        // Câmpuri noi
        [Column("categorie_material")]
        [MaxLength(100)]
        public string? CategorieMaterial { get; set; }

        [Column("subcategorie_material")]
        [MaxLength(100)]
        public string? SubcategorieMaterial { get; set; }

        [Column("cantitate")]
        [Precision(10, 2)]
        public decimal? Cantitate { get; set; }

        [Column("unitate_masura")]
        [MaxLength(20)]
        public string? UnitateMasura { get; set; }
        
        [Column("pret_unitar")]
        [Precision(10, 2)]
        public decimal? PretUnitar { get; set; }
        // Relație
        [ForeignKey("ProiectId")]
        [JsonIgnore]
        public Proiect? Proiect { get; set; }
    }
}