using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIUsuarios.Models;

namespace APIUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly UsuariosDBContext _context;

        public RolesController(UsuariosDBContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolT>>> GetRolesT()
        {
            return await _context.RolesT.ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolT>> GetRolT(int id)
        {
            var rolT = await _context.RolesT.FindAsync(id);

            if (rolT == null)
            {
                return NotFound();
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
