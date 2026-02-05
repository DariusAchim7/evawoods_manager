using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column("nume")]
        [MaxLength(200)]
        public string Nume { get; set; } = string.Empty;

        [Required]
        [Column("total_cant",TypeName = "decimal(10,2)")]
        public decimal TotalCant { get; set; }

        [Required]
        [Column("detalii", TypeName = "text")]
        public string Detalii { get; set; } = string.Empty; // JSON cu detaliile calculului

        [Column("data_calcul")]
        public DateTime DataCalcul { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("ProiectId")]
        public virtual Proiect? Proiect { get; set; }
    }
}