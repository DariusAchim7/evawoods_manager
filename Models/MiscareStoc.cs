using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtelierTamplarie.Models
{
    [Table("miscari_stoc")]
    public class MiscareStoc
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("stoc_id")]
        public int StocId { get; set; }

        [Required]
        [Column("tip_miscare")]
        [MaxLength(20)]
        public string TipMiscare { get; set; } = string.Empty;

        [Required]
        [Column("cantitate")]
        [Precision(10, 2)]
        public decimal Cantitate { get; set; }

        [Column("proiect_id")]
        public int? ProiectId { get; set; }

        [Column("motiv")]
        public string? Motiv { get; set; }

        [Column("data_miscare")]
        public DateTime DataMiscare { get; set; } = DateTime.UtcNow;

        // Relații
        [ForeignKey("StocId")]
        public Stoc? Stoc { get; set; }

        [ForeignKey("ProiectId")]
        public Proiect? Proiect { get; set; }
    }
}