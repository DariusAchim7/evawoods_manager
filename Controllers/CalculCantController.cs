using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtelierTamplarie.Data;
using AtelierTamplarie.Models;
using AtelierTamplarie.DTOs;

namespace AtelierTamplarie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculCantController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CalculCantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CalculCant/proiect/5
        [HttpGet("proiect/{proiectId}")]
        public async Task<ActionResult<IEnumerable<CalculCantDto>>> GetCalculeByProiect(int proiectId)
        {
            var calcule = await _context.CalculeCant
                .Where(c => c.ProiectId == proiectId)
                .OrderByDescending(c => c.DataCalcul)
                .Select(c => new CalculCantDto
                {
                    Id = c.Id,
                    ProiectId = c.ProiectId,
                    Nume = c.Nume,
                    TotalCant = c.TotalCant,
                    Detalii = c.Detalii,
                    DataCalcul = c.DataCalcul
                })
                .ToListAsync();

            return Ok(calcule);
        }

        // GET: api/CalculCant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CalculCantDto>> GetCalculCant(int id)
        {
            var calcul = await _context.CalculeCant.FindAsync(id);

            if (calcul == null)
            {
                return NotFound(new { message = "Calcul cant nu a fost găsit" });
            }

            var dto = new CalculCantDto
            {
                Id = calcul.Id,
                ProiectId = calcul.ProiectId,
                Nume = calcul.Nume,
                TotalCant = calcul.TotalCant,
                Detalii = calcul.Detalii,
                DataCalcul = calcul.DataCalcul
            };

            return Ok(dto);
        }

        // POST: api/CalculCant
        [HttpPost]
        public async Task<ActionResult<CalculCantDto>> PostCalculCant(CreateCalculCantDto dto)
        {
            // Verifică dacă proiectul există
            var proiectExists = await _context.Proiecte.AnyAsync(p => p.Id == dto.ProiectId);
            if (!proiectExists)
            {
                return BadRequest(new { message = "Proiectul nu există" });
            }

            var calcul = new CalculCant
            {
                ProiectId = dto.ProiectId,
                Nume = dto.Nume,
                TotalCant = dto.TotalCant,
                Detalii = dto.Detalii,
                DataCalcul = dto.DataCalcul ?? DateTime.UtcNow
            };

            _context.CalculeCant.Add(calcul);
            await _context.SaveChangesAsync();

            var resultDto = new CalculCantDto
            {
                Id = calcul.Id,
                ProiectId = calcul.ProiectId,
                Nume = calcul.Nume,
                TotalCant = calcul.TotalCant,
                Detalii = calcul.Detalii,
                DataCalcul = calcul.DataCalcul
            };

            return CreatedAtAction(nameof(GetCalculCant), new { id = calcul.Id }, resultDto);
        }

        // PUT: api/CalculCant/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CalculCantDto>> PutCalculCant(int id, UpdateCalculCantDto dto)
        {
            var calcul = await _context.CalculeCant.FindAsync(id);
            if (calcul == null)
            {
                return NotFound(new { message = "Calcul cant nu a fost găsit" });
            }

            // Actualizează doar câmpurile care nu sunt null în DTO
            if (dto.Nume != null) calcul.Nume = dto.Nume;
            if (dto.TotalCant.HasValue) calcul.TotalCant = dto.TotalCant.Value;
            if (dto.Detalii != null) calcul.Detalii = dto.Detalii;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalculCantExists(id))
                {
                    return NotFound(new { message = "Calcul cant nu mai există" });
                }
                throw;
            }

            var resultDto = new CalculCantDto
            {
                Id = calcul.Id,
                ProiectId = calcul.ProiectId,
                Nume = calcul.Nume,
                TotalCant = calcul.TotalCant,
                Detalii = calcul.Detalii,
                DataCalcul = calcul.DataCalcul
            };

            return Ok(resultDto);
        }

        // DELETE: api/CalculCant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalculCant(int id)
        {
            var calcul = await _context.CalculeCant.FindAsync(id);
            if (calcul == null)
            {
                return NotFound(new { message = "Calcul cant nu a fost găsit" });
            }

            _context.CalculeCant.Remove(calcul);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Calcul cant șters cu succes", id = id });
        }

        private bool CalculCantExists(int id)
        {
            return _context.CalculeCant.Any(e => e.Id == id);
        }
    }
}