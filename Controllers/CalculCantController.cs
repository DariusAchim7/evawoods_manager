using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtelierTamplarie.Data;
using AtelierTamplarie.Models;
using AtelierTamplarie.DTOs;
using System.Text.Json;

namespace AtelierTamplarie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculCantController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private const decimal PIERDERE_PER_LATURA_CM = 0.4m; // 4mm = 0.4cm

        public CalculCantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/CalculCant
        [HttpPost]
        public async Task<ActionResult<CalculCantDto>> CreateCalcul(CreateCalculCantDto dto)
        {
            // Verifică dacă proiectul există
            var proiect = await _context.Proiecte.FindAsync(dto.ProiectId);
            if (proiect == null)
            {
                return BadRequest(new { message = "Proiectul specificat nu există" });
            }

            // Calculează totalul
            var rezultat = CalculeazaCant(dto.Linii);

            // Salvează în baza de date
            var calculCant = new CalculCant
            {
                ProiectId = dto.ProiectId,
                NumeFisier = dto.NumeFisier,
                TotalCantCm = rezultat.TotalCantCm,
                TotalCantMetri = rezultat.TotalCantMetri,
                DetaliiJson = JsonSerializer.Serialize(rezultat.Detalii),
                Observatii = dto.Observatii
            };

            _context.CalculeCant.Add(calculCant);
            await _context.SaveChangesAsync();

            // Returnează rezultatul
            var calculDto = new CalculCantDto
            {
                Id = calculCant.Id,
                ProiectId = calculCant.ProiectId,
                NumeFisier = calculCant.NumeFisier,
                DataUpload = calculCant.DataUpload,
                TotalCantCm = calculCant.TotalCantCm,
                TotalCantMetri = calculCant.TotalCantMetri,
                Detalii = rezultat.Detalii,
                Observatii = calculCant.Observatii
            };

            return CreatedAtAction(nameof(GetCalcul), new { id = calculCant.Id }, calculDto);
        }

        // GET: api/CalculCant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CalculCantDto>> GetCalcul(int id)
        {
            var calcul = await _context.CalculeCant.FindAsync(id);

            if (calcul == null)
            {
                return NotFound(new { message = "Calculul nu a fost găsit" });
            }

            var detalii = string.IsNullOrEmpty(calcul.DetaliiJson)
                ? new List<DetaliuLinieCant>()
                : JsonSerializer.Deserialize<List<DetaliuLinieCant>>(calcul.DetaliiJson) ?? new List<DetaliuLinieCant>();

            var calculDto = new CalculCantDto
            {
                Id = calcul.Id,
                ProiectId = calcul.ProiectId,
                NumeFisier = calcul.NumeFisier,
                DataUpload = calcul.DataUpload,
                TotalCantCm = calcul.TotalCantCm,
                TotalCantMetri = calcul.TotalCantMetri,
                Detalii = detalii,
                Observatii = calcul.Observatii
            };

            return Ok(calculDto);
        }

        // GET: api/CalculCant/Proiect/5
        [HttpGet("Proiect/{proiectId}")]
        public async Task<ActionResult<IEnumerable<CalculCantDto>>> GetCalculeByProiect(int proiectId)
        {
            var calcule = await _context.CalculeCant
                .Where(c => c.ProiectId == proiectId)
                .OrderByDescending(c => c.DataUpload)
                .ToListAsync();

            var calculeDto = calcule.Select(c => new CalculCantDto
            {
                Id = c.Id,
                ProiectId = c.ProiectId,
                NumeFisier = c.NumeFisier,
                DataUpload = c.DataUpload,
                TotalCantCm = c.TotalCantCm,
                TotalCantMetri = c.TotalCantMetri,
                Detalii = string.IsNullOrEmpty(c.DetaliiJson)
                    ? new List<DetaliuLinieCant>()
                    : JsonSerializer.Deserialize<List<DetaliuLinieCant>>(c.DetaliiJson) ?? new List<DetaliuLinieCant>(),
                Observatii = c.Observatii
            }).ToList();

            return Ok(calculeDto);
        }

        // DELETE: api/CalculCant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalcul(int id)
        {
            var calcul = await _context.CalculeCant.FindAsync(id);
            if (calcul == null)
            {
                return NotFound(new { message = "Calculul nu a fost găsit" });
            }

            _context.CalculeCant.Remove(calcul);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Calcul șters cu succes" });
        }

        // Funcția de calcul cant
        private RezultatCalculCant CalculeazaCant(List<LinieExcelCant> linii)
        {
            var rezultat = new RezultatCalculCant();
            decimal totalCm = 0;

            foreach (var linie in linii)
            {
                var detaliu = new DetaliuLinieCant
                {
                    Lungime = linie.Lungime,
                    Latime = linie.Latime,
                    CantLungime = linie.CantLungime,
                    CantLatime = linie.CantLatime
                };

                // Convertește mm în cm și calculează cantul
                // Lungime: mm → cm, apoi adaugă pierdere tehnologică (0.4cm per latură)
                decimal lungimeMm = linie.Lungime;
                decimal latimeMm = linie.Latime;

                // Calcul cant pe lungime
                decimal lungimeCantata = 0;
                if (linie.CantLungime > 0)
                {
                    lungimeCantata = (lungimeMm / 10m) * linie.CantLungime + (PIERDERE_PER_LATURA_CM * linie.CantLungime);
                }

                // Calcul cant pe lățime
                decimal latimeCantata = 0;
                if (linie.CantLatime > 0)
                {
                    latimeCantata = (latimeMm / 10m) * linie.CantLatime + (PIERDERE_PER_LATURA_CM * linie.CantLatime);
                }

                detaliu.LungimeCantata = lungimeCantata;
                detaliu.LatimeCantata = latimeCantata;
                detaliu.TotalLinie = lungimeCantata + latimeCantata;

                totalCm += detaliu.TotalLinie;
                rezultat.Detalii.Add(detaliu);
            }

            rezultat.TotalCantCm = Math.Round(totalCm, 2);
            rezultat.TotalCantMetri = Math.Round(totalCm / 100m, 2);

            return rezultat;
        }
    }
}