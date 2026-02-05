using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtelierTamplarie.Data;
using AtelierTamplarie.Models;
using AtelierTamplarie.DTOs;

namespace AtelierTamplarie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProiecteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProiecteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Proiecte
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProiectDto>>> GetProiecte()
        {
            var proiecte = await _context.Proiecte
                .Include(p => p.Client)
                .Include(p => p.Cheltuieli)
                .Select(p => new ProiectDto
                {
                    Id = p.Id,
                    NumeProiect = p.NumeProiect,
                    ClientId = p.ClientId,
                    ClientNume = p.Client!.Nume,
                    Descriere = p.Descriere,
                    Status = p.Status,
                    PretEstimat = p.PretEstimat,
                    PretFinal = p.PretFinal,
                    DataStart = p.DataStart,
                    DataFinalizare = p.DataFinalizare,
                    Observatii = p.Observatii,
                    DataCreare = p.DataCreare,
                    TotalCheltuieli = p.Cheltuieli != null ? p.Cheltuieli.Sum(c => c.Suma) : 0,
                    ProfitEstimat = (p.PretEstimat ?? 0) - (p.Cheltuieli != null ? p.Cheltuieli.Sum(c => c.Suma) : 0)
                })
                .ToListAsync();

            return Ok(proiecte);
        }

        // GET: api/Proiecte/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetProiect(int id)
        {
            var proiect = await _context.Proiecte
                .Include(p => p.Client)
                .Include(p => p.Cheltuieli)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.NumeProiect,
                    p.ClientId,
                    ClientNume = p.Client!.Nume,
                    ClientTelefon = p.Client.Telefon,
                    ClientEmail = p.Client.Email,
                    ClientAdresa = p.Client.Adresa,
                    p.Descriere,
                    p.Status,
                    p.PretEstimat,
                    p.PretFinal,
                    p.DataStart,
                    p.DataFinalizare,
                    p.Observatii,
                    p.DataCreare,
                    Cheltuieli = p.Cheltuieli,
                    TotalCheltuieli = p.Cheltuieli != null ? p.Cheltuieli.Sum(c => c.Suma) : 0,
                    ProfitEstimat = (p.PretEstimat ?? 0) - (p.Cheltuieli != null ? p.Cheltuieli.Sum(c => c.Suma) : 0)
                })
                .FirstOrDefaultAsync();

            if (proiect == null)
            {
                return NotFound(new { message = "Proiectul nu a fost găsit" });
            }

            return Ok(proiect);
        }

        // GET: api/Proiecte/Status/in_lucru
        [HttpGet("Status/{status}")]
        public async Task<ActionResult<IEnumerable<ProiectDto>>> GetProiecteByStatus(string status)
        {
            var proiecte = await _context.Proiecte
                .Include(p => p.Client)
                .Include(p => p.Cheltuieli)
                .Where(p => p.Status == status)
                .Select(p => new ProiectDto
                {
                    Id = p.Id,
                    NumeProiect = p.NumeProiect,
                    ClientId = p.ClientId,
                    ClientNume = p.Client!.Nume,
                    Descriere = p.Descriere,
                    Status = p.Status,
                    PretEstimat = p.PretEstimat,
                    PretFinal = p.PretFinal,
                    DataStart = p.DataStart,
                    DataFinalizare = p.DataFinalizare,
                    Observatii = p.Observatii,
                    DataCreare = p.DataCreare,
                    TotalCheltuieli = p.Cheltuieli != null ? p.Cheltuieli.Sum(c => c.Suma) : 0,
                    ProfitEstimat = (p.PretEstimat ?? 0) - (p.Cheltuieli != null ? p.Cheltuieli.Sum(c => c.Suma) : 0)
                })
                .ToListAsync();

            return Ok(proiecte);
        }

        // GET: api/Proiecte/Statistici
        [HttpGet("Statistici")]
        public async Task<ActionResult<object>> GetStatistici()
        {
            var totalProiecte = await _context.Proiecte.CountAsync();
            var proiectePlanificate = await _context.Proiecte.CountAsync(p => p.Status == "planificat");
            var proiecteInLucru = await _context.Proiecte.CountAsync(p => p.Status == "in_lucru");
            var proiecteFinalizate = await _context.Proiecte.CountAsync(p => p.Status == "finalizat");
            
            var valoareTotalaEstimata = await _context.Proiecte.SumAsync(p => p.PretEstimat ?? 0);
            var valoareTotalaCheltuieli = await _context.Cheltuieli.SumAsync(c => c.Suma);

            return Ok(new
            {
                TotalProiecte = totalProiecte,
                ProiectePlanificate = proiectePlanificate,
                ProiecteInLucru = proiecteInLucru,
                ProiecteFinalizate = proiecteFinalizate,
                ValoareTotalaEstimata = valoareTotalaEstimata,
                ValoareTotalaCheltuieli = valoareTotalaCheltuieli,
                ProfitEstimatTotal = valoareTotalaEstimata - valoareTotalaCheltuieli
            });
        }

        // POST: api/Proiecte
        [HttpPost]
        public async Task<ActionResult<ProiectDto>> PostProiect(CreateProiectDto dto)
        {
            // Verifică dacă clientul există
            var client = await _context.Clienti.FindAsync(dto.ClientId);
            if (client == null)
            {
                return BadRequest(new { message = "Clientul specificat nu există" });
            }

            var proiect = new Proiect
            {
                NumeProiect = dto.NumeProiect,
                ClientId = dto.ClientId,
                Descriere = dto.Descriere,
                Status = dto.Status,
                PretEstimat = dto.PretEstimat,
                PretFinal = dto.PretFinal,
                DataStart = dto.DataStart,
                DataFinalizare = dto.DataFinalizare,
                Observatii = dto.Observatii
            };

            _context.Proiecte.Add(proiect);
            await _context.SaveChangesAsync();

            var proiectDto = new ProiectDto
            {
                Id = proiect.Id,
                NumeProiect = proiect.NumeProiect,
                ClientId = proiect.ClientId,
                ClientNume = client.Nume,
                Descriere = proiect.Descriere,
                Status = proiect.Status,
                PretEstimat = proiect.PretEstimat,
                PretFinal = proiect.PretFinal,
                DataStart = proiect.DataStart,
                DataFinalizare = proiect.DataFinalizare,
                Observatii = proiect.Observatii,
                DataCreare = proiect.DataCreare,
                TotalCheltuieli = 0,
                ProfitEstimat = proiect.PretEstimat ?? 0
            };

            return CreatedAtAction(nameof(GetProiect), new { id = proiect.Id }, proiectDto);
        }

        // PUT: api/Proiecte/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ProiectDto>> PutProiect(int id, UpdateProiectDto dto)
        {
            var proiect = await _context.Proiecte.FindAsync(id);
            
            if (proiect == null)
            {
                return NotFound(new { message = "Proiectul nu există" });
            }

            var client = await _context.Clienti.FindAsync(dto.ClientId);
            if (client == null)
            {
                return BadRequest(new { message = "Clientul specificat nu există" });
            }

            proiect.NumeProiect = dto.NumeProiect;
            proiect.ClientId = dto.ClientId;
            proiect.Descriere = dto.Descriere;
            proiect.Status = dto.Status;
            proiect.PretEstimat = dto.PretEstimat;
            proiect.PretFinal = dto.PretFinal;
            proiect.DataStart = dto.DataStart;
            proiect.DataFinalizare = dto.DataFinalizare;
            proiect.Observatii = dto.Observatii;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProiectExists(id))
                {
                    return NotFound(new { message = "Proiectul nu există" });
                }
                throw;
            }

            var totalCheltuieli = await _context.Cheltuieli
                .Where(ch => ch.ProiectId == id)
                .SumAsync(ch => ch.Suma);

            var proiectDto = new ProiectDto
            {
                Id = proiect.Id,
                NumeProiect = proiect.NumeProiect,
                ClientId = proiect.ClientId,
                ClientNume = client.Nume,
                Descriere = proiect.Descriere,
                Status = proiect.Status,
                PretEstimat = proiect.PretEstimat,
                PretFinal = proiect.PretFinal,
                DataStart = proiect.DataStart,
                DataFinalizare = proiect.DataFinalizare,
                Observatii = proiect.Observatii,
                DataCreare = proiect.DataCreare,
                TotalCheltuieli = totalCheltuieli,
                ProfitEstimat = (proiect.PretEstimat ?? 0) - totalCheltuieli
            };

            return Ok(proiectDto);
        }

        // DELETE: api/Proiecte/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProiect(int id)
        {
            var proiect = await _context.Proiecte.FindAsync(id);
            if (proiect == null)
            {
                return NotFound(new { message = "Proiectul nu a fost găsit" });
            }

            _context.Proiecte.Remove(proiect);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Proiect șters cu succes", id = id });
        }

        private bool ProiectExists(int id)
        {
            return _context.Proiecte.Any(e => e.Id == id);
        }
    }
}