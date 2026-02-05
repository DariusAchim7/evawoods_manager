using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtelierTamplarie.Data;
using AtelierTamplarie.Models;
using AtelierTamplarie.DTOs;

namespace AtelierTamplarie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MiscariStocController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MiscariStocController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MiscariStoc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MiscareStocDto>>> GetMiscariStoc()
        {
            var miscari = await _context.MiscariStoc
                .Include(m => m.Stoc)
                .Include(m => m.Proiect)
                .OrderByDescending(m => m.DataMiscare)
                .Select(m => new MiscareStocDto
                {
                    Id = m.Id,
                    StocId = m.StocId,
                    NumeProdus = m.Stoc!.NumeProdus,
                    TipMiscare = m.TipMiscare,
                    Cantitate = m.Cantitate,
                    UnitateMasura = m.Stoc.UnitateMasura,
                    ProiectId = m.ProiectId,
                    ProiectNume = m.Proiect != null ? m.Proiect.NumeProiect : null,
                    Motiv = m.Motiv,
                    DataMiscare = m.DataMiscare
                })
                .ToListAsync();

            return Ok(miscari);
        }

        // GET: api/MiscariStoc/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MiscareStocDto>> GetMiscareStoc(int id)
        {
            var miscare = await _context.MiscariStoc
                .Include(m => m.Stoc)
                .Include(m => m.Proiect)
                .Where(m => m.Id == id)
                .Select(m => new MiscareStocDto
                {
                    Id = m.Id,
                    StocId = m.StocId,
                    NumeProdus = m.Stoc!.NumeProdus,
                    TipMiscare = m.TipMiscare,
                    Cantitate = m.Cantitate,
                    UnitateMasura = m.Stoc.UnitateMasura,
                    ProiectId = m.ProiectId,
                    ProiectNume = m.Proiect != null ? m.Proiect.NumeProiect : null,
                    Motiv = m.Motiv,
                    DataMiscare = m.DataMiscare
                })
                .FirstOrDefaultAsync();

            if (miscare == null)
            {
                return NotFound(new { message = "Mișcarea nu a fost găsită" });
            }

            return Ok(miscare);
        }

        // GET: api/MiscariStoc/Produs/5
        [HttpGet("Produs/{stocId}")]
        public async Task<ActionResult<object>> GetMiscariByProdus(int stocId)
        {
            var produs = await _context.Stocuri.FindAsync(stocId);
            if (produs == null)
            {
                return NotFound(new { message = "Produsul nu a fost găsit" });
            }

            var miscari = await _context.MiscariStoc
                .Include(m => m.Proiect)
                .Where(m => m.StocId == stocId)
                .OrderByDescending(m => m.DataMiscare)
                .Select(m => new MiscareStocDto
                {
                    Id = m.Id,
                    StocId = m.StocId,
                    NumeProdus = m.Stoc!.NumeProdus,
                    TipMiscare = m.TipMiscare,
                    Cantitate = m.Cantitate,
                    UnitateMasura = m.Stoc.UnitateMasura,
                    ProiectId = m.ProiectId,
                    ProiectNume = m.Proiect != null ? m.Proiect.NumeProiect : null,
                    Motiv = m.Motiv,
                    DataMiscare = m.DataMiscare
                })
                .ToListAsync();

            var totalIntrari = miscari.Where(m => m.TipMiscare == "intrare").Sum(m => m.Cantitate);
            var totalIesiri = miscari.Where(m => m.TipMiscare == "iesire").Sum(m => m.Cantitate);

            return Ok(new
            {
                Produs = produs.NumeProdus,
                CantitateActuala = produs.Cantitate,
                TotalIntrari = totalIntrari,
                TotalIesiri = totalIesiri,
                Miscari = miscari
            });
        }

        // POST: api/MiscariStoc
        [HttpPost]
        public async Task<ActionResult<MiscareStocDto>> PostMiscareStoc(CreateMiscareStocDto dto)
        {
            // Verifică dacă produsul există
            var stoc = await _context.Stocuri.FindAsync(dto.StocId);
            if (stoc == null)
            {
                return BadRequest(new { message = "Produsul specificat nu există" });
            }

            // Verifică dacă proiectul există (dacă e specificat)
            if (dto.ProiectId.HasValue)
            {
                var proiect = await _context.Proiecte.FindAsync(dto.ProiectId.Value);
                if (proiect == null)
                {
                    return BadRequest(new { message = "Proiectul specificat nu există" });
                }
            }

            // Actualizează cantitatea în stoc
            if (dto.TipMiscare == "intrare")
            {
                stoc.Cantitate += dto.Cantitate;
            }
            else if (dto.TipMiscare == "iesire")
            {
                if (stoc.Cantitate < dto.Cantitate)
                {
                    return BadRequest(new { message = "Cantitate insuficientă în stoc" });
                }
                stoc.Cantitate -= dto.Cantitate;
            }
            else
            {
                return BadRequest(new { message = "Tip mișcare invalid. Folosește 'intrare' sau 'iesire'" });
            }

            stoc.DataActualizare = DateTime.UtcNow;

            var miscare = new MiscareStoc
            {
                StocId = dto.StocId,
                TipMiscare = dto.TipMiscare,
                Cantitate = dto.Cantitate,
                ProiectId = dto.ProiectId,
                Motiv = dto.Motiv
            };

            _context.MiscariStoc.Add(miscare);
            await _context.SaveChangesAsync();

            // Încarcă datele pentru DTO
            var proiectNume = dto.ProiectId.HasValue 
                ? (await _context.Proiecte.FindAsync(dto.ProiectId.Value))?.NumeProiect 
                : null;

            var miscareDto = new MiscareStocDto
            {
                Id = miscare.Id,
                StocId = miscare.StocId,
                NumeProdus = stoc.NumeProdus,
                TipMiscare = miscare.TipMiscare,
                Cantitate = miscare.Cantitate,
                UnitateMasura = stoc.UnitateMasura,
                ProiectId = miscare.ProiectId,
                ProiectNume = proiectNume,
                Motiv = miscare.Motiv,
                DataMiscare = miscare.DataMiscare
            };

            return CreatedAtAction(nameof(GetMiscareStoc), new { id = miscare.Id }, miscareDto);
        }

        // DELETE: api/MiscariStoc/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMiscareStoc(int id)
        {
            var miscare = await _context.MiscariStoc.FindAsync(id);
            if (miscare == null)
            {
                return NotFound(new { message = "Mișcarea nu a fost găsită" });
            }

            _context.MiscariStoc.Remove(miscare);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mișcare ștearsă cu succes", id = id });
        }
    }
}