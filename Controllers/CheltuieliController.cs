using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtelierTamplarie.Data;
using AtelierTamplarie.Models;
using AtelierTamplarie.DTOs;

namespace AtelierTamplarie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheltuieliController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CheltuieliController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cheltuieli
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CheltuialaDto>>> GetCheltuieli()
        {
            var cheltuieli = await _context.Cheltuieli
                .Include(ch => ch.Proiect)
                .OrderByDescending(ch => ch.DataCheltuiala)
                .Select(ch => new CheltuialaDto
                {
                    Id = ch.Id,
                    ProiectId = ch.ProiectId,
                    ProiectNume = ch.Proiect!.NumeProiect,
                    TipCheltuiala = ch.TipCheltuiala,
                    Descriere = ch.Descriere,
                    Suma = ch.Suma,
                    DataCheltuiala = ch.DataCheltuiala,
                    Observatii = ch.Observatii,
                    DataCreare = ch.DataCreare,
                    CategorieMaterial = ch.CategorieMaterial,
                    SubcategorieMaterial = ch.SubcategorieMaterial,
                    Cantitate = ch.Cantitate,
                    UnitateMasura = ch.UnitateMasura
                })
                .ToListAsync();

            return Ok(cheltuieli);
        }

        // GET: api/Cheltuieli/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CheltuialaDto>> GetCheltuiala(int id)
        {
            var cheltuiala = await _context.Cheltuieli
                .Include(ch => ch.Proiect)
                .Where(ch => ch.Id == id)
                .Select(ch => new CheltuialaDto
                {
                    Id = ch.Id,
                    ProiectId = ch.ProiectId,
                    ProiectNume = ch.Proiect!.NumeProiect,
                    TipCheltuiala = ch.TipCheltuiala,
                    Descriere = ch.Descriere,
                    Suma = ch.Suma,
                    DataCheltuiala = ch.DataCheltuiala,
                    Observatii = ch.Observatii,
                    DataCreare = ch.DataCreare,
                    CategorieMaterial = ch.CategorieMaterial,
                    SubcategorieMaterial = ch.SubcategorieMaterial,
                    Cantitate = ch.Cantitate,
                    UnitateMasura = ch.UnitateMasura
                })
                .FirstOrDefaultAsync();

            if (cheltuiala == null)
            {
                return NotFound(new { message = "Cheltuiala nu a fost găsită" });
            }

            return Ok(cheltuiala);
        }

        // GET: api/Cheltuieli/Proiect/5
        [HttpGet("Proiect/{proiectId}")]
        public async Task<ActionResult<object>> GetCheltuieliByProiect(int proiectId)
        {
            var proiect = await _context.Proiecte.FindAsync(proiectId);
            if (proiect == null)
            {
                return NotFound(new { message = "Proiectul nu a fost găsit" });
            }

            var cheltuieli = await _context.Cheltuieli
                .Where(ch => ch.ProiectId == proiectId)
                .OrderBy(ch => ch.DataCheltuiala)
                .ToListAsync();

            var totalCheltuieli = cheltuieli.Sum(ch => ch.Suma);

            var cheltuieliPeTip = cheltuieli
                .GroupBy(ch => ch.TipCheltuiala)
                .Select(g => new
                {
                    TipCheltuiala = g.Key,
                    Total = g.Sum(ch => ch.Suma),
                    Numar = g.Count()
                })
                .ToList();

            return Ok(new
            {
                ProiectId = proiectId,
                ProiectNume = proiect.NumeProiect,
                PretEstimat = proiect.PretEstimat,
                Cheltuieli = cheltuieli,
                TotalCheltuieli = totalCheltuieli,
                CheltuieliPeTip = cheltuieliPeTip,
                BugetRamas = proiect.PretEstimat - totalCheltuieli,
                ProcentConsumat = proiect.PretEstimat > 0 
                    ? (totalCheltuieli / proiect.PretEstimat * 100) 
                    : 0
            });
        }

        // GET: api/Cheltuieli/Tip/materiale
        [HttpGet("Tip/{tip}")]
        public async Task<ActionResult<IEnumerable<CheltuialaDto>>> GetCheltuieliByTip(string tip)
        {
            var cheltuieli = await _context.Cheltuieli
                .Include(ch => ch.Proiect)
                .Where(ch => ch.TipCheltuiala == tip)
                .OrderByDescending(ch => ch.DataCheltuiala)
                .Select(ch => new CheltuialaDto
                {
                    Id = ch.Id,
                    ProiectId = ch.ProiectId,
                    ProiectNume = ch.Proiect!.NumeProiect,
                    TipCheltuiala = ch.TipCheltuiala,
                    Descriere = ch.Descriere,
                    Suma = ch.Suma,
                    DataCheltuiala = ch.DataCheltuiala,
                    Observatii = ch.Observatii,
                    DataCreare = ch.DataCreare
                })
                .ToListAsync();

            return Ok(cheltuieli);
        }

        // POST: api/Cheltuieli
        [HttpPost]
        public async Task<ActionResult<CheltuialaDto>> PostCheltuiala(CreateCheltuialaDto dto)
        {
            // Verifică dacă proiectul există
            var proiect = await _context.Proiecte.FindAsync(dto.ProiectId);
            if (proiect == null)
            {
                return BadRequest(new { message = "Proiectul specificat nu există" });
            }

            var cheltuiala = new Cheltuiala
            {
                ProiectId = dto.ProiectId,
                TipCheltuiala = dto.TipCheltuiala,
                Descriere = dto.Descriere,
                Suma = dto.Suma,
                DataCheltuiala = dto.DataCheltuiala ?? DateTime.UtcNow,
                Observatii = dto.Observatii,
                CategorieMaterial = dto.CategorieMaterial,
                SubcategorieMaterial = dto.SubcategorieMaterial,
                Cantitate = dto.Cantitate,
                UnitateMasura = dto.UnitateMasura
            };

            _context.Cheltuieli.Add(cheltuiala);
            await _context.SaveChangesAsync();

            var cheltuialaDto = new CheltuialaDto
            {
                Id = cheltuiala.Id,
                ProiectId = cheltuiala.ProiectId,
                ProiectNume = proiect.NumeProiect,
                TipCheltuiala = cheltuiala.TipCheltuiala,
                Descriere = cheltuiala.Descriere,
                Suma = cheltuiala.Suma,
                DataCheltuiala = cheltuiala.DataCheltuiala,
                Observatii = cheltuiala.Observatii,
                DataCreare = cheltuiala.DataCreare,
                CategorieMaterial = cheltuiala.CategorieMaterial,
                SubcategorieMaterial = cheltuiala.SubcategorieMaterial,
                Cantitate = cheltuiala.Cantitate,
                UnitateMasura = cheltuiala.UnitateMasura
            };

            return CreatedAtAction(nameof(GetCheltuiala), new { id = cheltuiala.Id }, cheltuialaDto);
        }

        // PUT: api/Cheltuieli/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CheltuialaDto>> PutCheltuiala(int id, UpdateCheltuialaDto dto)
        {
            var cheltuiala = await _context.Cheltuieli.FindAsync(id);
            
            if (cheltuiala == null)
            {
                return NotFound(new { message = "Cheltuiala nu există" });
            }

            var proiect = await _context.Proiecte.FindAsync(dto.ProiectId);
            if (proiect == null)
            {
                return BadRequest(new { message = "Proiectul specificat nu există" });
            }

            cheltuiala.ProiectId = dto.ProiectId;
            cheltuiala.TipCheltuiala = dto.TipCheltuiala;
            cheltuiala.Descriere = dto.Descriere;
            cheltuiala.Suma = dto.Suma;
            cheltuiala.DataCheltuiala = dto.DataCheltuiala ?? cheltuiala.DataCheltuiala;
            cheltuiala.Observatii = dto.Observatii;
            cheltuiala.CategorieMaterial = dto.CategorieMaterial;
            cheltuiala.SubcategorieMaterial = dto.SubcategorieMaterial;
            cheltuiala.Cantitate = dto.Cantitate;
            cheltuiala.UnitateMasura = dto.UnitateMasura;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheltuialaExists(id))
                {
                    return NotFound(new { message = "Cheltuiala nu există" });
                }
                throw;
            }

            var cheltuialaDto = new CheltuialaDto
            {
                Id = cheltuiala.Id,
                ProiectId = cheltuiala.ProiectId,
                ProiectNume = proiect.NumeProiect,
                TipCheltuiala = cheltuiala.TipCheltuiala,
                Descriere = cheltuiala.Descriere,
                Suma = cheltuiala.Suma,
                DataCheltuiala = cheltuiala.DataCheltuiala,
                Observatii = cheltuiala.Observatii,
                DataCreare = cheltuiala.DataCreare,
                CategorieMaterial = cheltuiala.CategorieMaterial,
                SubcategorieMaterial = cheltuiala.SubcategorieMaterial,
                Cantitate = cheltuiala.Cantitate,
                UnitateMasura = cheltuiala.UnitateMasura
            };

            return Ok(cheltuialaDto);
        }

        // DELETE: api/Cheltuieli/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCheltuiala(int id)
        {
            var cheltuiala = await _context.Cheltuieli.FindAsync(id);
            if (cheltuiala == null)
            {
                return NotFound(new { message = "Cheltuiala nu a fost găsită" });
            }

            _context.Cheltuieli.Remove(cheltuiala);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cheltuială ștearsă cu succes", id = id });
        }

        private bool CheltuialaExists(int id)
        {
            return _context.Cheltuieli.Any(e => e.Id == id);
        }
    }
}