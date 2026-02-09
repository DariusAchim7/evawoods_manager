namespace AtelierTamplarie.DTOs
{
    // DTO pentru upload imagine
    public class UploadImaginiDto
    {
        public int ProiectId { get; set; }
        public string TipImagine { get; set; } = string.Empty; // schita, randare, final
        public string? Descriere { get; set; }
        public int Ordine { get; set; } = 0;
    }

    // DTO pentru returnarea unei imagini
    public class ImaginiProiectDto
    {
        public int Id { get; set; }
        public int ProiectId { get; set; }
        public string TipImagine { get; set; } = string.Empty;
        public string NumeFisier { get; set; } = string.Empty;
        public string UrlImagine { get; set; } = string.Empty; // URL pentru afișare
        public string? Descriere { get; set; }
        public DateTime DataUpload { get; set; }
        public int Ordine { get; set; }
    }

    // DTO pentru actualizare imagine (fără fișier)
    public class UpdateImaginiDto
    {
        public string? TipImagine { get; set; }
        public string? Descriere { get; set; }
        public int? Ordine { get; set; }
    }
}