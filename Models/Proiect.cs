using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AtelierTamplarie.Models
{
    [Table("proiecte")]
    public class Proiect
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nume_proiect")]
        [MaxLength(255)]
        public string NumeProiect { get; set; } = string.Empty;

        [Required]
        [Column("client_id")]
        public int ClientId { get; set; }

        [Column("descriere")]
        public string? Descriere { get; set; }

        [Column("status")]
        [MaxLength(50)]
        public string Status { get; set; } = "planificat";

        [Column("pret_estimat")]
        [Precision(10, 2)]
        public decimal? PretEstimat { get; set; }

        [Column("pret_final")]
        [Precision(10, 2)]
        public decimal? PretFinal { get; set; }

        [Column("data_start")]
        public DateTime? DataStart { get; set; }

        [Column("data_finalizare")]
        public DateTime? DataFinalizare { get; set; }

        [Column("observatii")]
        public string? Observatii { get; set; }

        [Column("data_creare")]
        public DateTime DataCreare { get; set; } = DateTime.UtcNow;

        // Relații
        [ForeignKey("ClientId")]
        [JsonIgnore]
        public Client? Client { get; set; }

        public ICollection<Cheltuiala>? Cheltuieli { get; set; }
        public ICollection<MiscareStoc>? MiscariStoc { get; set; }
        public virtual ICollection<CalculCant> CalculeCant { get; set; } = new List<CalculCant>();
    }
}
