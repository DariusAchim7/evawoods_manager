namespace AtelierTamplarie.DTOs
{
    // DTO pentru datele din Excel
    public class LinieExcelCant
    {
        public decimal Lungime { get; set; }      // mm
        public decimal Latime { get; set; }       // mm
        public int CantLungime { get; set; }      // 0, 1, sau 2
        public int CantLatime { get; set; }       // 0, 1, sau 2
    }

    // DTO pentru rezultatul calculului
    public class RezultatCalculCant
    {
        public decimal TotalCantCm { get; set; }
        public decimal TotalCantMetri { get; set; }
        public List<DetaliuLinieCant> Detalii { get; set; } = new();
    }

    public class DetaliuLinieCant
    {
        public decimal Lungime { get; set; }
        public decimal Latime { get; set; }
        public int CantLungime { get; set; }
        public int CantLatime { get; set; }
        public decimal LungimeCantata { get; set; }  // cm
        public decimal LatimeCantata { get; set; }    // cm
        public decimal TotalLinie { get; set; }       // cm
    }

    // DTO pentru crearea unui calcul nou
    public class CreateCalculCantDto
    {
        public int ProiectId { get; set; }
        public string NumeFisier { get; set; } = string.Empty;
        public List<LinieExcelCant> Linii { get; set; } = new();
        public string? Observatii { get; set; }
    }

    // DTO pentru returnarea unui calcul salvat
    public class CalculCantDto
    {
        public int Id { get; set; }
        public int ProiectId { get; set; }
        public string NumeFisier { get; set; } = string.Empty;
        public DateTime DataUpload { get; set; }
        public decimal TotalCantCm { get; set; }
        public decimal TotalCantMetri { get; set; }
        public List<DetaliuLinieCant> Detalii { get; set; } = new();
        public string? Observatii { get; set; }
    }
}