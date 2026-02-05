namespace AtelierTamplarie.DTOs
{
    public class CreateStocDto
    {
        public string NumeProdus { get; set; } = string.Empty;
        public string? Categorie { get; set; }
        public string UnitateMasura { get; set; } = "buc";
        public decimal Cantitate { get; set; }
        public decimal? PretUnitar { get; set; }
        public string? Furnizor { get; set; }
        public string? LocatieDepozit { get; set; }
        public string? Observatii { get; set; }
    }

    public class UpdateStocDto
    {
        public string NumeProdus { get; set; } = string.Empty;
        public string? Categorie { get; set; }
        public string UnitateMasura { get; set; } = "buc";
        public decimal Cantitate { get; set; }
        public decimal? PretUnitar { get; set; }
        public string? Furnizor { get; set; }
        public string? LocatieDepozit { get; set; }
        public string? Observatii { get; set; }
    }

    public class StocDto
    {
        public int Id { get; set; }
        public string NumeProdus { get; set; } = string.Empty;
        public string? Categorie { get; set; }
        public string UnitateMasura { get; set; } = "buc";
        public decimal Cantitate { get; set; }
        public decimal? PretUnitar { get; set; }
        public decimal ValoareTotala { get; set; }
        public string? Furnizor { get; set; }
        public string? LocatieDepozit { get; set; }
        public string? Observatii { get; set; }
        public DateTime DataCreare { get; set; }
        public DateTime DataActualizare { get; set; }
    }

    public class CreateMiscareStocDto
    {
        public int StocId { get; set; }
        public string TipMiscare { get; set; } = string.Empty;
        public decimal Cantitate { get; set; }
        public int? ProiectId { get; set; }
        public string? Motiv { get; set; }
    }

    public class MiscareStocDto
    {
        public int Id { get; set; }
        public int StocId { get; set; }
        public string NumeProdus { get; set; } = string.Empty;
        public string TipMiscare { get; set; } = string.Empty;
        public decimal Cantitate { get; set; }
        public string UnitateMasura { get; set; } = string.Empty;
        public int? ProiectId { get; set; }
        public string? ProiectNume { get; set; }
        public string? Motiv { get; set; }
        public DateTime DataMiscare { get; set; }
    }
}