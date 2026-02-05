using System.ComponentModel.DataAnnotations;

namespace AtelierTamplarie.DTOs
{
    // DTO pentru creare calcul cant
    public class CreateCalculCantDto
    {
        [Required(ErrorMessage = "Proiect ID este obligatoriu")]
        public int ProiectId { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu")]
        [StringLength(200, ErrorMessage = "Numele nu poate depăși 200 caractere")]
        public string Nume { get; set; } = string.Empty;

        [Required(ErrorMessage = "Total cant este obligatoriu")]
        [Range(0, double.MaxValue, ErrorMessage = "Total cant trebuie să fie pozitiv")]
        public decimal TotalCant { get; set; }

        [Required(ErrorMessage = "Detaliile sunt obligatorii")]
        public string Detalii { get; set; } = string.Empty; // JSON

        public DateTime? DataCalcul { get; set; }
    }

    // DTO pentru actualizare calcul cant
    public class UpdateCalculCantDto
    {
        [StringLength(200, ErrorMessage = "Numele nu poate depăși 200 caractere")]
        public string? Nume { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total cant trebuie să fie pozitiv")]
        public decimal? TotalCant { get; set; }

        public string? Detalii { get; set; }
    }

    // DTO pentru returnare calcul cant
    public class CalculCantDto
    {
        public int Id { get; set; }
        public int ProiectId { get; set; }
        public string Nume { get; set; } = string.Empty;
        public decimal TotalCant { get; set; }
        public string Detalii { get; set; } = string.Empty;
        public DateTime DataCalcul { get; set; }
    }
}