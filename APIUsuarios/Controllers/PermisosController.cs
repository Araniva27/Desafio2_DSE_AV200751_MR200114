using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIUsuarios.Models;
using NuGet.Protocol.Plugins;
using StackExchange.Redis;
using System.Text.Json;

namespace APIUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly UsuariosDBContext _context;
        private readonly IConnectionMultiplexer _redis;

        public PermisosController(UsuariosDBContext context, IConnectionMultiplexer? redis = null)
        {
            _context = context;
            _redis = redis;
        }

        // GET: api/Permisos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermisoT>>> GetPermisosT()
        {
            var db = _redis.GetDatabase();
            string cacheKey = "permisoList";
            var permisoCache = await db.StringGetAsync(cacheKey);
            if (!permisoCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<List<PermisoT>>(permisoCache);
            }
            var permisos = await _context.PermisosT.ToListAsync();
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(permisos), TimeSpan.FromMinutes(10));
            return permisos;
        }

        // GET: api/Permisos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PermisoT>> GetPermisoT(int id)
        {
            // Si Redis no está configurado, omitir la parte de la caché
            if (_redis != null)
            {
                var db = _redis.GetDatabase();
                string cacheKey = "permiso_" + id.ToString();
                var permisoCache = await db.StringGetAsync(cacheKey);
                if (!permisoCache.IsNullOrEmpty)
                {
                    return JsonSerializer.Deserialize<PermisoT>(permisoCache);
                }
            }

            var permisoT = await _context.PermisosT.FindAsync(id);

            if (permisoT == null)
            {
                return NotFound();
            }

            // Si Redis está configurado, almacenar en caché el permiso
            if (_redis != null)
            {
                var db = _redis.GetDatabase();
                await db.StringSetAsync("permiso_" + id.ToString(), JsonSerializer.Serialize(permisoT), TimeSpan.FromMinutes(10));
            }

            return permisoT;
            //var db = _redis.GetDatabase();
            //string cacheKey = "permiso_" + id.ToString();
            //var permisoCache = await db.StringGetAsync(cacheKey);
            //if (!permisoCache.IsNullOrEmpty)
            //{
            //    return JsonSerializer.Deserialize<PermisoT>(permisoCache);
            //}
            //var permisoT = await _context.PermisosT.FindAsync(id);

            //if (permisoT == null)
            //{
            //    return NotFound();
            //}
            //await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(permisoT), TimeSpan.FromMinutes(10));
            //return permisoT;
        }

        // PUT: api/Permisos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermisoT(int id, PermisoT permisoT)
        {
            if (id != permisoT.PermisoId)
            {
                return BadRequest();
            }

            _context.Entry(permisoT).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermisoTExists(id))
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

        // POST: api/Permisos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PermisoT>> PostPermisoT(PermisoT permisoT)
        {
            if (string.IsNullOrEmpty(permisoT.Nombre))
            {
                return BadRequest("The Nombre field is required.");
            }

            if (permisoT.Nombre.Length < 3 || permisoT.Nombre.Length > 50)
            {
                return BadRequest("El campo Nombre debe tener entre 3 y 50 caracteres.");
            }

            _context.PermisosT.Add(permisoT);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPermisoT), new { id = permisoT.PermisoId }, permisoT);
            //return CreatedAtAction("GetPermisoT", new { id = permisoT.PermisoId }, permisoT);
        }

        // DELETE: api/Permisos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermisoT(int id)
        {
            var permisoT = await _context.PermisosT.FindAsync(id);
            if (permisoT == null)
            {
                return NotFound();
            }

            _context.PermisosT.Remove(permisoT);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PermisoTExists(int id)
        {
            return _context.PermisosT.Any(e => e.PermisoId == id);
        }
    }
}
