using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtelierTamplarie.Data;
using AtelierTamplarie.Models;
using AtelierTamplarie.DTOs;

namespace AtelierTamplarie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImaginiProiectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ImaginiProiectController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/ImaginiProiect/Proiect/5
        [HttpGet("Proiect/{proiectId}")]
        public async Task<ActionResult<IEnumerable<ImaginiProiectDto>>> GetImaginiByProiect(int proiectId)
        {
            var imagini = await _context.ImaginiProiect
                .Where(i => i.ProiectId == proiectId)
                .OrderBy(i => i.Ordine)
                .ThenByDescending(i => i.DataUpload)
                .ToListAsync();

            var imaginiDto = imagini.Select(i => new ImaginiProiectDto
            {
                Id = i.Id,
                ProiectId = i.ProiectId,
                TipImagine = i.TipImagine,
                NumeFisier = i.NumeFisier,
                UrlImagine = $"/uploads/proiecte/{i.CaleFisier}",
                Descriere = i.Descriere,
                DataUpload = i.DataUpload,
                Ordine = i.Ordine
            }).ToList();

            return Ok(imaginiDto);
        }

        // GET: api/ImaginiProiect/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImaginiProiectDto>> GetImagine(int id)
        {
            var imagine = await _context.ImaginiProiect.FindAsync(id);

            if (imagine == null)
            {
                return NotFound(new { message = "Imaginea nu a fost găsită" });
            }

            var imagineDto = new ImaginiProiectDto
            {
                Id = imagine.Id,
                ProiectId = imagine.ProiectId,
                TipImagine = imagine.TipImagine,
                NumeFisier = imagine.NumeFisier,
                UrlImagine = $"/uploads/proiecte/{imagine.CaleFisier}",
                Descriere = imagine.Descriere,
                DataUpload = imagine.DataUpload,
                Ordine = imagine.Ordine
            };

            return Ok(imagineDto);
        }

        // POST: api/ImaginiProiect/Upload
        [HttpPost("Upload")]
        public async Task<ActionResult<ImaginiProiectDto>> UploadImagine([FromForm] IFormFile file, [FromForm] UploadImaginiDto dto)
        {
            // Validare fișier
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Niciun fișier selectat" });
            }

            // Verifică dacă proiectul există
            var proiect = await _context.Proiecte.FindAsync(dto.ProiectId);
            if (proiect == null)
            {
                return BadRequest(new { message = "Proiectul specificat nu există" });
            }

            // Verifică extensia fișierului
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { message = "Tip fișier invalid. Permise: JPG, PNG, GIF, WEBP" });
            }

            // Verifică dimensiunea (max 10MB)
            if (file.Length > 10 * 1024 * 1024)
            {
                return BadRequest(new { message = "Fișierul este prea mare. Maxim 10MB" });
            }

            // Creează directorul dacă nu există
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", "proiecte");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            // Generează nume unic pentru fișier
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            // Salvează fișierul
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Salvează în baza de date
            var imagine = new ImaginiProiect
            {
                ProiectId = dto.ProiectId,
                TipImagine = dto.TipImagine,
                NumeFisier = file.FileName,
                CaleFisier = uniqueFileName,
                Descriere = dto.Descriere,
                Ordine = dto.Ordine
            };

            _context.ImaginiProiect.Add(imagine);
            await _context.SaveChangesAsync();

            var imagineDto = new ImaginiProiectDto
            {
                Id = imagine.Id,
                ProiectId = imagine.ProiectId,
                TipImagine = imagine.TipImagine,
                NumeFisier = imagine.NumeFisier,
                UrlImagine = $"/uploads/proiecte/{imagine.CaleFisier}",
                Descriere = imagine.Descriere,
                DataUpload = imagine.DataUpload,
                Ordine = imagine.Ordine
            };

            return CreatedAtAction(nameof(GetImagine), new { id = imagine.Id }, imagineDto);
        }

        // PUT: api/ImaginiProiect/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ImaginiProiectDto>> UpdateImagine(int id, UpdateImaginiDto dto)
        {
            var imagine = await _context.ImaginiProiect.FindAsync(id);

            if (imagine == null)
            {
                return NotFound(new { message = "Imaginea nu a fost găsită" });
            }

            // Actualizează doar câmpurile furnizate
            if (dto.TipImagine != null)
                imagine.TipImagine = dto.TipImagine;
            
            if (dto.Descriere != null)
                imagine.Descriere = dto.Descriere;
            
            if (dto.Ordine.HasValue)
                imagine.Ordine = dto.Ordine.Value;

            await _context.SaveChangesAsync();

            var imagineDto = new ImaginiProiectDto
            {
                Id = imagine.Id,
                ProiectId = imagine.ProiectId,
                TipImagine = imagine.TipImagine,
                NumeFisier = imagine.NumeFisier,
                UrlImagine = $"/uploads/proiecte/{imagine.CaleFisier}",
                Descriere = imagine.Descriere,
                DataUpload = imagine.DataUpload,
                Ordine = imagine.Ordine
            };

            return Ok(imagineDto);
        }

        // DELETE: api/ImaginiProiect/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagine(int id)
        {
            var imagine = await _context.ImaginiProiect.FindAsync(id);
            
            if (imagine == null)
            {
                return NotFound(new { message = "Imaginea nu a fost găsită" });
            }

            // Șterge fișierul fizic
            var filePath = Path.Combine(_environment.WebRootPath, "uploads", "proiecte", imagine.CaleFisier);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Șterge din baza de date
            _context.ImaginiProiect.Remove(imagine);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Imagine ștearsă cu succes" });
        }
    }
}