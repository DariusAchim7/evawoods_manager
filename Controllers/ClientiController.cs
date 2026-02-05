using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtelierTamplarie.Data;
using AtelierTamplarie.Models;
using AtelierTamplarie.DTOs;

namespace AtelierTamplarie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Clienti
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClienti()
        {
            var clienti = await _context.Clienti
                .Include(c => c.Proiecte)
                .Select(c => new ClientDto
                {
                    Id = c.Id,
                    Nume = c.Nume,
                    Telefon = c.Telefon,
                    Email = c.Email,
                    Adresa = c.Adresa,
                    Observatii = c.Observatii,
                    DataCreare = c.DataCreare,
                    NumarProiecte = c.Proiecte != null ? c.Proiecte.Count : 0
                })
                .ToListAsync();

            return Ok(clienti);
        }

        // GET: api/Clienti/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetClient(int id)
        {
            var client = await _context.Clienti
                .Include(c => c.Proiecte)
                .Where(c => c.Id == id)
                .Select(c => new ClientDto
                {
                    Id = c.Id,
                    Nume = c.Nume,
                    Telefon = c.Telefon,
                    Email = c.Email,
                    Adresa = c.Adresa,
                    Observatii = c.Observatii,
                    DataCreare = c.DataCreare,
                    NumarProiecte = c.Proiecte != null ? c.Proiecte.Count : 0
                })
                .FirstOrDefaultAsync();

            if (client == null)
            {
                return NotFound(new { message = "Clientul nu a fost găsit" });
            }

            return Ok(client);
        }

        // POST: api/Clienti
        [HttpPost]
        public async Task<ActionResult<ClientDto>> PostClient(CreateClientDto dto)
        {
            var client = new Client
            {
                Nume = dto.Nume,
                Telefon = dto.Telefon,
                Email = dto.Email,
                Adresa = dto.Adresa,
                Observatii = dto.Observatii
            };

            _context.Clienti.Add(client);
            await _context.SaveChangesAsync();

            var clientDto = new ClientDto
            {
                Id = client.Id,
                Nume = client.Nume,
                Telefon = client.Telefon,
                Email = client.Email,
                Adresa = client.Adresa,
                Observatii = client.Observatii,
                DataCreare = client.DataCreare,
                NumarProiecte = 0
            };

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, clientDto);
        }

        // PUT: api/Clienti/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ClientDto>> PutClient(int id, UpdateClientDto dto)
        {
            var client = await _context.Clienti.FindAsync(id);
            
            if (client == null)
            {
                return NotFound(new { message = "Clientul nu există" });
            }

            client.Nume = dto.Nume;
            client.Telefon = dto.Telefon;
            client.Email = dto.Email;
            client.Adresa = dto.Adresa;
            client.Observatii = dto.Observatii;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound(new { message = "Clientul nu există" });
                }
                throw;
            }

            var clientDto = new ClientDto
            {
                Id = client.Id,
                Nume = client.Nume,
                Telefon = client.Telefon,
                Email = client.Email,
                Adresa = client.Adresa,
                Observatii = client.Observatii,
                DataCreare = client.DataCreare,
                NumarProiecte = await _context.Proiecte.CountAsync(p => p.ClientId == id)
            };

            return Ok(clientDto);
        }

        // DELETE: api/Clienti/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClient(int id)
        {
            var client = await _context.Clienti.FindAsync(id);
            if (client == null)
            {
                return NotFound(new { message = "Clientul nu a fost găsit" });
            }

            _context.Clienti.Remove(client);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Client șters cu succes", id = id });
        }

        private bool ClientExists(int id)
        {
            return _context.Clienti.Any(e => e.Id == id);
        }
    }
}