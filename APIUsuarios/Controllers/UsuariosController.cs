using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIUsuarios.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace APIUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosDBContext _context;
        private readonly IConnectionMultiplexer _redis;

        public UsuariosController(UsuariosDBContext context, IConnectionMultiplexer? redis = null)
        {
            _context = context;
            _redis = redis;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioT>>> GetUsuariosT()
        {

            var db = _redis.GetDatabase();
            string cacheKey = "usuarioList";
            var permisoCache = await db.StringGetAsync(cacheKey);
            if (!permisoCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<List<UsuarioT>>(permisoCache);
            }
            var usuarios = await _context.UsuariosT.ToListAsync();
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(usuarios), TimeSpan.FromMinutes(10));
            return usuarios;
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioT>> GetUsuarioT(int id)
        {
            if (_redis != null)
            {
                var db = _redis.GetDatabase();
                string cacheKey = "usuario_" + id.ToString();
                var usuarioCache = await db.StringGetAsync(cacheKey);
                if (!usuarioCache.IsNullOrEmpty)
                {
                    return JsonSerializer.Deserialize<UsuarioT>(usuarioCache);
                }
            }
            var usuarioT = await _context.UsuariosT.FindAsync(id);

            if (usuarioT == null)
            {
                return NotFound();
            }

            // Si Redis está configurado, almacenar en caché el permiso
            if (_redis != null)
            {
                var db = _redis.GetDatabase();
                await db.StringSetAsync("usuario_" + id.ToString(), JsonSerializer.Serialize(usuarioT), TimeSpan.FromMinutes(10));
            }

            return usuarioT;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarioT(int id, UsuarioT usuarioT)
        {
            if (id != usuarioT.UsuarioId)
            {
                return BadRequest();
            }

            _context.Entry(usuarioT).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioTExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuarioT>> PostUsuarioT(UsuarioT usuarioT)
        {
            _context.UsuariosT.Add(usuarioT);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuarioT", new { id = usuarioT.UsuarioId }, usuarioT);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarioT(int id)
        {
            var usuarioT = await _context.UsuariosT.FindAsync(id);
            if (usuarioT == null)
            {
                return NotFound();
            }

            _context.UsuariosT.Remove(usuarioT);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioTExists(int id)
        {
            return _context.UsuariosT.Any(e => e.UsuarioId == id);
        }
    }
}
