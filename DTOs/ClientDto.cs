namespace AtelierTamplarie.DTOs
{
    // DTO pentru crearea unui client nou
    public class CreateClientDto
    {
        public string Nume { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public string? Email { get; set; }
        public string? Adresa { get; set; }
        public string? Observatii { get; set; }
    }

    // DTO pentru actualizarea unui client
    public class UpdateClientDto
    {
        public string Nume { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public string? Email { get; set; }
        public string? Adresa { get; set; }
        public string? Observatii { get; set; }
    }

    // DTO pentru returnarea datelor clientului
    public class ClientDto
    {
        public int Id { get; set; }
        public string Nume { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public string? Email { get; set; }
        public string? Adresa { get; set; }
        public string? Observatii { get; set; }
        public DateTime DataCreare { get; set; }
        public int NumarProiecte { get; set; }
    }
}