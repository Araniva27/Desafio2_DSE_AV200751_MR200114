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
    public class RolesController : ControllerBase
    {
        private readonly UsuariosDBContext _context;
        private readonly IConnectionMultiplexer _redis;
        public RolesController(UsuariosDBContext context, IConnectionMultiplexer? redis = null)
        {
            _context = context;
            _redis = redis;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolT>>> GetRolesT()
        {
            var db = _redis.GetDatabase();
            string cacheKey = "rolList";
            var rolCache = await db.StringGetAsync(cacheKey);
            if (!rolCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<List<RolT>>(rolCache);
            }
            var roles = await _context.RolesT.ToListAsync();
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(roles), TimeSpan.FromMinutes(10));
            return roles;
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolT>> GetRolT(int id)
        {
            if (_redis != null)
            {
                var db = _redis.GetDatabase();
                string cacheKey = "rol_" + id.ToString();
                var rolCache = await db.StringGetAsync(cacheKey);
                if (!rolCache.IsNullOrEmpty)
                {
                    return JsonSerializer.Deserialize<RolT>(rolCache);
                }

            }

            var rolT = await _context.RolesT.FindAsync(id);

            if (rolT == null)
            {
                return NotFound();
            }

            // Si Redis está configurado, almacenar en caché el permiso
            if (_redis != null)
            {
                var db = _redis.GetDatabase();
                await db.StringSetAsync("rol_" + id.ToString(), JsonSerializer.Serialize(rolT), TimeSpan.FromMinutes(10));
            }

            return rolT;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRolT(int id, RolT rolT)
        {
            if (id != rolT.RolId)
            {
                return BadRequest();
            }

            _context.Entry(rolT).State = EntityState.Modified;

            try
            {
                if(_redis != null)
                {
                    await _context.SaveChangesAsync();
                    var db = _redis.GetDatabase();
                    string cacheKeyRol = "rol_" + id.ToString();
                    string cacheKeyList = "rolList";
                    await db.KeyDeleteAsync(cacheKeyRol);
                    await db.KeyDeleteAsync(cacheKeyList);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolTExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RolT>> PostRolT(RolT rolT)
        {
            if(_redis != null)
            {
                _context.RolesT.Add(rolT);
                await _context.SaveChangesAsync();
                var db = _redis.GetDatabase();
                string cacheKeyList = "rolList";
                await db.KeyDeleteAsync(cacheKeyList);
                return CreatedAtAction("GetRolT", new { id = rolT.RolId }, rolT);
            }

            if (string.IsNullOrEmpty(rolT.Nombre) || rolT.Nombre.Length < 3 || rolT.Nombre.Length > 30)
            {
                return BadRequest("El nombre es obligatorio y debe tener entre 3 y 30 caracteres.");
            }
            _context.RolesT.Add(rolT);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRolT", new { id = rolT.RolId }, rolT);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolT(int id)
        {
            var rolT = await _context.RolesT.FindAsync(id);
            if (rolT == null)
            {
                return NotFound();
            }

            if(_redis != null)
            {
                _context.RolesT.Remove(rolT);
                await _context.SaveChangesAsync();
                var db = _redis.GetDatabase();
                string cacheKeyRol = "rol_" + id.ToString();
                string cacheKeyList = "rolList";
                await db.KeyDeleteAsync(cacheKeyRol);
                await db.KeyDeleteAsync(cacheKeyList);
                return NoContent();
            }

            _context.RolesT.Remove(rolT);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RolTExists(int id)
        {
            return _context.RolesT.Any(e => e.RolId == id);
        }
    }
}
