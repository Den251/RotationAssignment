using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RotationAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotationAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotationController : ControllerBase
    {
        private readonly RotationContext _context;

        public RotationController(RotationContext context)
        {
            _context = context;
            //adding terminals
            context.Terminals.AddRange(
                new Terminal() { Id = "0175d159-f61f-40b9-aaf7-967dbe461349", Name = "FirstTerminal", ATB = DateTime.Now, ATD = DateTime.Now },
                new Terminal() { Id = "02ec54f7-8a50-454f-9503-62eeb2f3f4d0", Name = "SecondTerminal", ATB = DateTime.Now, ATD = DateTime.Now },
                new Terminal() { Id = "035d8512-1093-4e40-bc90-d677d059a380", Name = "ThirdTerminal", ATB = DateTime.Now, ATD = DateTime.Now }
                );
            _context.SaveChangesAsync();
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cargo>>> GetCargoes()
        {
            return await _context.Cargoes.ToListAsync();
        }
        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cargo>> GetCargo(string id)
        {
            var cargo = await _context.Cargoes.FindAsync(id);
        
            if (cargo == null)
            {
                return NotFound();
            }
        
            return cargo; 
        }

        
        [Route("getrotation")]
        public async Task<ActionResult<IEnumerable<Terminal>>> GetRotation()
        {
            return await _context.Terminals.ToListAsync();
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<Cargo>> PostCargo(Cargo cargo)
        {
            if (CargoExists(cargo.Id))
                return BadRequest("Cargo already used in this rotation");
            _context.Cargoes.Add(cargo);
            await _context.SaveChangesAsync();
        
            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetCargo), new { id = cargo.Id }, cargo);
        }

        [Route("postcargolist")]
        public async Task<ActionResult<Cargo>> PostCargoList(List<Cargo> cargo)
        {
            _context.Cargoes.AddRange(cargo);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetCargo), new { id = cargo.Last().Id }, cargo); ;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(string id, Cargo cargo)
        {
            if (id != cargo.Id)
            {
                return BadRequest();
            }
        
            _context.Entry(cargo).State = EntityState.Modified;
        
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CargoExists(id))
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
        private bool CargoExists(string id)
        {
            return _context.Cargoes.Any(c => c.Id == id);
        }
    }
}
