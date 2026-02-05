using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtelierTamplarie.Data;
using AtelierTamplarie.Models;
using AtelierTamplarie.DTOs;

namespace AtelierTamplarie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StocController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Stoc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StocDto>>> GetStoc()
        {
            var stoc = await _context.Stocuri
                .Select(s => new StocDto
                {
                    Id = s.Id,
                    NumeProdus = s.NumeProdus,
                    Categorie = s.Categorie,
                    UnitateMasura = s.UnitateMasura,
                    Cantitate = s.Cantitate,
                    PretUnitar = s.PretUnitar,
                    ValoareTotala = s.Cantitate * (s.PretUnitar ?? 0),
                    Furnizor = s.Furnizor,
                    LocatieDepozit = s.LocatieDepozit,
                    Observatii = s.Observatii,
                    DataCreare = s.DataCreare,
                    DataActualizare = s.DataActualizare
                })
                .OrderBy(s => s.Categorie)
                .ThenBy(s => s.NumeProdus)
                .ToListAsync();

            return Ok(stoc);
        }

        // GET: api/Stoc/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetStocItem(int id)
        {
            var stoc = await _context.Stocuri
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    Stoc = new StocDto
                    {
                        Id = s.Id,
                        NumeProdus = s.NumeProdus,
                        Categorie = s.Categorie,
                        UnitateMasura = s.UnitateMasura,
                        Cantitate = s.Cantitate,
                        PretUnitar = s.PretUnitar,
                        ValoareTotala = s.Cantitate * (s.PretUnitar ?? 0),
                        Furnizor = s.Furnizor,
                        LocatieDepozit = s.LocatieDepozit,
                        Observatii = s.Observatii,
                        DataCreare = s.DataCreare,
                        DataActualizare = s.DataActualizare
                    }
                })
                .FirstOrDefaultAsync();

            if (stoc == null)
            {
                return NotFound(new { message = "Produsul nu a fost găsit" });
            }

            var miscariRecente = await _context.MiscariStoc
                .Include(m => m.Proiect)
                .Where(m => m.StocId == id)
                .OrderByDescending(m => m.DataMiscare)
                .Take(10)
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

            return Ok(new
            {
                stoc.Stoc,
                MiscariRecente = miscariRecente
            });
        }

        // GET: api/Stoc/Categorie/lemn
        [HttpGet("Categorie/{categorie}")]
        public async Task<ActionResult<IEnumerable<StocDto>>> GetStocByCategorie(string categorie)
        {
            var stoc = await _context.Stocuri
                .Where(s => s.Categorie == categorie)
                .Select(s => new StocDto
                {
                    Id = s.Id,
                    NumeProdus = s.NumeProdus,
                    Categorie = s.Categorie,
                    UnitateMasura = s.UnitateMasura,
                    Cantitate = s.Cantitate,
                    PretUnitar = s.PretUnitar,
                    ValoareTotala = s.Cantitate * (s.PretUnitar ?? 0),
                    Furnizor = s.Furnizor,
                    LocatieDepozit = s.LocatieDepozit,
                    Observatii = s.Observatii,
                    DataCreare = s.DataCreare,
                    DataActualizare = s.DataActualizare
                })
                .OrderBy(s => s.NumeProdus)
                .ToListAsync();

            return Ok(stoc);
        }

        // GET: api/Stoc/Valoare
        [HttpGet("Valoare")]
        public async Task<ActionResult<object>> GetValoareStoc()
        {
            var valoareTotala = await _context.Stocuri
                .SumAsync(s => s.Cantitate * (s.PretUnitar ?? 0));

            var valoarePeCategorie = await _context.Stocuri
                .GroupBy(s => s.Categorie)
                .Select(g => new
                {
                    Categorie = g.Key ?? "Necategorizat",
                    NumarProduse = g.Count(),
                    ValoareTotala = g.Sum(s => s.Cantitate * (s.PretUnitar ?? 0))
                })
                .OrderByDescending(x => x.ValoareTotala)
                .ToListAsync();

            return Ok(new
            {
                ValoareTotala = valoareTotala,
                ValoarePeCategorie = valoarePeCategorie
            });
        }

        // GET: api/Stoc/StocScazut
        [HttpGet("StocScazut")]
        public async Task<ActionResult<IEnumerable<object>>> GetStocScazut()
        {
            var stocScazut = await _context.Stocuri
                .Where(s => s.Cantitate < 10)
                .Select(s => new
                {
                    s.Id,
                    s.NumeProdus,
                    s.Categorie,
                    s.Cantitate,
                    s.UnitateMasura,
                    s.Furnizor
                })
                .OrderBy(s => s.Cantitate)
                .ToListAsync();

            return Ok(stocScazut);
        }

        // POST: api/Stoc
        [HttpPost]
        public async Task<ActionResult<StocDto>> PostStoc(CreateStocDto dto)
        {
            var stoc = new Stoc
            {
                NumeProdus = dto.NumeProdus,
                Categorie = dto.Categorie,
                UnitateMasura = dto.UnitateMasura,
                Cantitate = dto.Cantitate,
                PretUnitar = dto.PretUnitar,
                Furnizor = dto.Furnizor,
                LocatieDepozit = dto.LocatieDepozit,
                Observatii = dto.Observatii
            };

            _context.Stocuri.Add(stoc);
            await _context.SaveChangesAsync();

            var stocDto = new StocDto
            {
                Id = stoc.Id,
                NumeProdus = stoc.NumeProdus,
                Categorie = stoc.Categorie,
                UnitateMasura = stoc.UnitateMasura,
                Cantitate = stoc.Cantitate,
                PretUnitar = stoc.PretUnitar,
                ValoareTotala = stoc.Cantitate * (stoc.PretUnitar ?? 0),
                Furnizor = stoc.Furnizor,
                LocatieDepozit = stoc.LocatieDepozit,
                Observatii = stoc.Observatii,
                DataCreare = stoc.DataCreare,
                DataActualizare = stoc.DataActualizare
            };

            return CreatedAtAction(nameof(GetStocItem), new { id = stoc.Id }, stocDto);
        }

        // PUT: api/Stoc/5
        [HttpPut("{id}")]
        public async Task<ActionResult<StocDto>> PutStoc(int id, UpdateStocDto dto)
        {
            var stoc = await _context.Stocuri.FindAsync(id);
            
            if (stoc == null)
            {
                return NotFound(new { message = "Produsul nu există" });
            }

            stoc.NumeProdus = dto.NumeProdus;
            stoc.Categorie = dto.Categorie;
            stoc.UnitateMasura = dto.UnitateMasura;
            stoc.Cantitate = dto.Cantitate;
            stoc.PretUnitar = dto.PretUnitar;
            stoc.Furnizor = dto.Furnizor;
            stoc.LocatieDepozit = dto.LocatieDepozit;
            stoc.Observatii = dto.Observatii;
            stoc.DataActualizare = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StocExists(id))
                {
                    return NotFound(new { message = "Produsul nu există" });
                }
                throw;
            }

            var stocDto = new StocDto
            {
                Id = stoc.Id,
                NumeProdus = stoc.NumeProdus,
                Categorie = stoc.Categorie,
                UnitateMasura = stoc.UnitateMasura,
                Cantitate = stoc.Cantitate,
                PretUnitar = stoc.PretUnitar,
                ValoareTotala = stoc.Cantitate * (stoc.PretUnitar ?? 0),
                Furnizor = stoc.Furnizor,
                LocatieDepozit = stoc.LocatieDepozit,
                Observatii = stoc.Observatii,
                DataCreare = stoc.DataCreare,
                DataActualizare = stoc.DataActualizare
            };

            return Ok(stocDto);
        }

        // DELETE: api/Stoc/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStoc(int id)
        {
            var stoc = await _context.Stocuri.FindAsync(id);
            if (stoc == null)
            {
                return NotFound(new { message = "Produsul nu a fost găsit" });
            }

            _context.Stocuri.Remove(stoc);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Produs șters cu succes", id = id });
        }

        private bool StocExists(int id)
        {
            return _context.Stocuri.Any(e => e.Id == id);
        }
    }
}