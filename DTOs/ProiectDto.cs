namespace AtelierTamplarie.DTOs
{
    public class CreateProiectDto
    {
        public string NumeProiect { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public string? Descriere { get; set; }
        public string Status { get; set; } = "planificat";
        public decimal? PretEstimat { get; set; }
        public decimal? PretFinal { get; set; }
        public DateTime? DataStart { get; set; }
        public DateTime? DataFinalizare { get; set; }
        public string? Observatii { get; set; }
    }

    public class UpdateProiectDto
    {
        public string NumeProiect { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public string? Descriere { get; set; }
        public string Status { get; set; } = "planificat";
        public decimal? PretEstimat { get; set; }
        public decimal? PretFinal { get; set; }
        public DateTime? DataStart { get; set; }
        public DateTime? DataFinalizare { get; set; }
        public string? Observatii { get; set; }
    }

    public class ProiectDto
    {
        public int Id { get; set; }
        public string NumeProiect { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public string ClientNume { get; set; } = string.Empty;
        public string? Descriere { get; set; }
        public string Status { get; set; } = "planificat";
        public decimal? PretEstimat { get; set; }
        public decimal? PretFinal { get; set; }
        public DateTime? DataStart { get; set; }
        public DateTime? DataFinalizare { get; set; }
        public string? Observatii { get; set; }
        public DateTime DataCreare { get; set; }
        public decimal TotalCheltuieli { get; set; }
        public decimal ProfitEstimat { get; set; }
    }
}