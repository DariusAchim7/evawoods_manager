namespace AtelierTamplarie.DTOs
{
    // DTO pentru crearea unei cheltuieli noi
    public class CreateCheltuialaDto
    {
        public int ProiectId { get; set; }
        public string TipCheltuiala { get; set; } = string.Empty;
        public string Descriere { get; set; } = string.Empty;
        public decimal Suma { get; set; }
        public DateTime? DataCheltuiala { get; set; }
        public string? Observatii { get; set; }
        
        // Câmpuri noi
        public string? CategorieMaterial { get; set; }
        public string? SubcategorieMaterial { get; set; }
        public decimal? Cantitate { get; set; }
        public string? UnitateMasura { get; set; }
    }

    // DTO pentru actualizarea unei cheltuieli
    public class UpdateCheltuialaDto
    {
        public int ProiectId { get; set; }
        public string TipCheltuiala { get; set; } = string.Empty;
        public string Descriere { get; set; } = string.Empty;
        public decimal Suma { get; set; }
        public DateTime? DataCheltuiala { get; set; }
        public string? Observatii { get; set; }
        
        // Câmpuri noi
        public string? CategorieMaterial { get; set; }
        public string? SubcategorieMaterial { get; set; }
        public decimal? Cantitate { get; set; }
        public string? UnitateMasura { get; set; }
    }

    // DTO pentru returnarea datelor cheltuielii
    public class CheltuialaDto
    {
        public int Id { get; set; }
        public int ProiectId { get; set; }
        public string ProiectNume { get; set; } = string.Empty;
        public string TipCheltuiala { get; set; } = string.Empty;
        public string Descriere { get; set; } = string.Empty;
        public decimal Suma { get; set; }
        public DateTime DataCheltuiala { get; set; }
        public string? Observatii { get; set; }
        public DateTime DataCreare { get; set; }
        
        // Câmpuri noi
        public string? CategorieMaterial { get; set; }
        public string? SubcategorieMaterial { get; set; }
        public decimal? Cantitate { get; set; }
        public string? UnitateMasura { get; set; }
    }
}