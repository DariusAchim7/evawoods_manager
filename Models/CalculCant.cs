using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AtelierTamplarie.Models
{
    [Table("calcul_cant")]
    public class CalculCant
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("proiect_id")]
        public int ProiectId { get; set; }

        [Required]
        [Column("nume_fisier")]
        [MaxLength(255)]
        public string NumeFisier { get; set; } = string.Empty;

        [Column("data_upload")]
        public DateTime DataUpload { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("total_cant_cm")]
        [Precision(10, 2)]
        public decimal TotalCantCm { get; set; }

        [Required]
        [Column("total_cant_metri")]
        [Precision(10, 2)]
        public decimal TotalCantMetri { get; set; }

        // JSON cu toate detaliile calculului
        [Column("detalii_json")]
        public string? DetaliiJson { get; set; }

        [Column("observatii")]
        public string? Observatii { get; set; }

        // Relație
        [ForeignKey("ProiectId")]
        public Proiect? Proiect { get; set; }
    }

    // Clasă pentru deserializare JSON detalii
    public class DetaliuCalculCant
    {
        public decimal Lungime { get; set; }  // mm
        public decimal Latime { get; set; }   // mm
        public int CantLungime { get; set; }  // 0, 1, sau 2
        public int CantLatime { get; set; }   // 0, 1, sau 2
        public decimal TotalLinieCm { get; set; }
    }
}