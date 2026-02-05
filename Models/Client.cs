using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtelierTamplarie.Models
{
    [Table("clienti")]
    public class Client
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nume")]
        [MaxLength(255)]
        public string Nume { get; set; } = string.Empty;

        [Column("telefon")]
        [MaxLength(50)]
        public string? Telefon { get; set; }

        [Column("email")]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Column("adresa")]
        public string? Adresa { get; set; }

        [Column("observatii")]
        public string? Observatii { get; set; }

        [Column("data_creare")]
        public DateTime DataCreare { get; set; } = DateTime.Now;

        // Relație cu Proiecte
        public ICollection<Proiect>? Proiecte { get; set; }
    }
}